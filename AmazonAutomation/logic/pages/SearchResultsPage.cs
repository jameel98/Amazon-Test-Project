using AmazonProject.Infra;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Newtonsoft.Json.Linq;
using AmazonProject.Entities;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AmazonProject.Pages
{
    public class SearchResultsPage : BasePage
    {
        // Locators
        private string SliderLocator = "//div[@id='p_36/range-slider_slider-item']/div[2]/input";
        private string FormPriceLocator = "//div[@id='priceRefinements']//div[@id='p_36/range-slider']/form";
        private string PriceSubmitLocator = "//div[@class='a-section sf-submit-range-button']";
        private string MemoryLocator = "//span[text()='16 GB']";
        private string RatingLocator = "//i[@class='a-icon a-icon-star-medium a-star-medium-4']";
        private string ProductLocator = "//div[@data-component-type='s-search-result']";

        private ProductPage _productPage;
        public SearchResultsPage(IWebDriver driver) : base(driver) { }

        private IWebElement MemoryFilter => WebDriverExtensions.FindElement(_driver, By.XPath(MemoryLocator), 10);
        private IWebElement RatingFilter => WebDriverExtensions.FindElement(_driver ,By.XPath(RatingLocator),10);
        private IWebElement PriceFilter => WebDriverExtensions.FindElement(_driver ,By.XPath(SliderLocator), 10);
        private IWebElement PriceForm => WebDriverExtensions.FindElement(_driver, By.XPath(FormPriceLocator), 10);

        // Lambda functions to generate locators dynamically with an index
        private Func<int, string> ProductRatingsLocator = (index) => $"//div[@data-component-type='s-search-result'][{index}]/div//a/span[@class='a-size-base s-underline-text']";
        private Func<int, string> ProductLinkLocator = (index) => $"//div[@data-component-type='s-search-result'][{index}]//a[@class='a-link-normal s-no-outline']";
        private Func<int, string> ProductPriceLocator = (index) => $"//div[@data-component-type='s-search-result'][{index}]//span[@class='a-price-whole']";
        private Func<int, string> ProductNameLocator = (index) => $"//div[@data-component-type='s-search-result'][{index}]//h2/a/span";

        public IWebElement GetProductNameElement(int index)
        {
            return WebDriverExtensions.FindElement(_driver, By.XPath(ProductNameLocator(index)), 10);
        }

        public string GetProductName(int index)
        {
            return GetProductNameElement(index).Text;
        }

        public IWebElement GetProductRatingsElement(int index)
        {
            return WebDriverExtensions.FindElement(_driver, By.XPath(ProductRatingsLocator(index)), 10);
        }

        public IWebElement GetProductLinkElement(int index)
        {
            return WebDriverExtensions.FindElement(_driver, By.XPath(ProductLinkLocator(index)), 10);
        }

        public IWebElement GetProductPriceElement(int index)
        {
            return WebDriverExtensions.FindElement(_driver, By.XPath(ProductPriceLocator(index)), 10);
        }

        public string GetProductRatings(int index)
        {
            return GetProductRatingsElement(index).Text;
        }

        public string GetProductLink(int index)
        {
            return GetProductLinkElement(index).GetAttribute("href");
        }

        public string GetProductPrice(int index)
        {
            try
            {
                return GetProductPriceElement(index).Text;
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Product price element not found for index {index}. Exception: {ex.Message}");
                return string.Empty;
            }
        }

        private void SetRating()
        {
            RatingFilter.Click();
        }

        private void SetMemory()
        {
            MemoryFilter.Click();
        }

        /// <summary>
        /// Sets the price filter slider to the specified target price.
        /// </summary>
        /// <param name="targetPrice">The price to set the slider to, as a string in the format "$XXX".</param>
        public void SetPriceSlider(int targetPrice)
        {
            var steps = GetSliderSteps();
            int sliderWidth = GetSliderWidth(PriceFilter);
            int closestStep = FindClosestStep(steps, targetPrice);
            int targetOffset = CalculateTargetOffset(steps, closestStep, sliderWidth);
            MoveSlider(PriceFilter, targetOffset);
            VerifySliderPosition(PriceFilter, targetPrice);
            ClickSubmitPrice();
        }

        /// <summary>
        /// Calculate the slider steps by getting the slider form data array length
        /// </summary>
        /// <returns></returns>
        private List<int> GetSliderSteps()
        {
            string dataSliderProps = PriceForm.GetAttribute("data-slider-props");
            JObject sliderProps = JObject.Parse(dataSliderProps);
            JArray stepValues = (JArray)sliderProps["stepValues"];
            return stepValues.Select(v => (int?)v).Where(v => v.HasValue).Select(v => v.Value).ToList();
        }

        private int GetSliderWidth(IWebElement slider)
        {
            return slider.Size.Width;
        }

        /// <summary>
        /// Search for closest price to the target price we need 
        /// then gets the steps count
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="targetPrice"></param>
        /// <returns></returns>
        private int FindClosestStep(List<int> steps, int targetPrice)
        {
            return steps.OrderBy(step => Math.Abs(step - targetPrice)).First();
        }

        /// <summary>
        /// Calculate the needed offset of the slider
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="closestStep"></param>
        /// <param name="sliderWidth"></param>
        /// <returns></returns>
        private int CalculateTargetOffset(List<int> steps, int closestStep, int sliderWidth)
        {
            int currentStepIndex = steps.IndexOf(closestStep);
            int maxStepIndex = steps.Count - 1;
            double pixelsPerStep = (double)sliderWidth / maxStepIndex;
            return (int)((maxStepIndex - currentStepIndex) * pixelsPerStep);
        }

        /// <summary>
        /// Move slider according to the offset value
        /// </summary>
        /// <param name="slider"></param>
        /// <param name="targetOffset"></param>
        private void MoveSlider(IWebElement slider, int targetOffset)
        {
            Actions actions = new Actions(_driver);
            actions.ClickAndHold(slider)
                   .MoveByOffset(targetOffset, 0)
                   .Release()
                   .Perform();
        }

        /// <summary>
        /// Verify the slider is in the correct position of the target price
        /// </summary>
        /// <param name="slider"></param>
        /// <param name="targetPrice"></param>
        private void VerifySliderPosition(IWebElement slider, int targetPrice)
        {
            string ariaValueText = slider.GetAttribute("aria-valuetext");
            int newPrice = int.Parse(ariaValueText.Replace("$", "").Replace(",", "").Replace("+", ""));
            Console.WriteLine($"Slider set to: ${newPrice}");
        }

        /// <summary>
        /// After moving the slider click the submit price button to filter the results
        /// </summary>
        public void ClickSubmitPrice()
        {
            IWebElement SubmitButton = WebDriverExtensions.FindElement(_driver, By.XPath(PriceSubmitLocator), 10);
            SubmitButton.Click();
        }

        /// <summary>
        /// Apply the memory, rating and price filters
        /// </summary>
        public void ApplyFilters()
        {
            SetMemory();
            SetRating();
            // SetPriceSlider(500);
        }

        /// <summary>
        /// Collect the products details (url, name, price) and save them to the file
        /// </summary>
        /// <param name="filePath">File path to save the details in it</param>
        /// <param name="word">The word "bad" to not be in the reviews</param>
        public void CollectAndSaveProductDetails(string filePath, string word)
        {
            List<Product> products = CollectProductDetails(word);
            SaveProductDetailsToFile(products, filePath);
        }

        /// <summary>
        /// Collect product details and add them to a list
        /// </summary>
        /// <param name="word">Word to check in reviews</param>
        /// <returns>List of products</returns>
        private List<Product> CollectProductDetails(string word)
        {
            List<Product> products = new List<Product>();

            var productElements = _driver.FindElements(By.XPath(ProductLocator))
                .Where((p, index) => HasMoreThanTenReviews(index + 1))
                .ToList();

            for (int i = 0; i < productElements.Count; i++)
            {
                string link = GetProductLink(i + 1);

                if (TopReviewsDoNotContainBad(link, word))
                {
                    string name = GetProductName(i + 1);
                    string price = GetProductPrice(i + 1);
                    products.Add(new Product(name, price, link));
                }

                if (products.Count >= 10)
                {
                    break;
                }
            }

            return products;
        }

        /// <summary>
        /// Verify product has more than 10 reviews
        /// </summary>
        /// <param name="index">Product index</param>
        /// <returns></returns>
        private bool HasMoreThanTenReviews(int index)
        {
            int reviewCount = int.Parse(GetProductRatings(index).Split()[0].Replace(",", ""));
            return reviewCount > 10;
        }

        /// <summary>
        /// Verify top 10 reviews don't contain the word bad
        /// </summary>
        /// <param name="productLink">Product link</param>
        /// <param name="word">Word to check</param>
        /// <returns>True if the word is not found in the reviews, otherwise false</returns>
        private bool TopReviewsDoNotContainBad(string productLink, string word)
        {
            _driver.Navigate().GoToUrl(productLink);

            _productPage = new ProductPage(_driver);

            bool result = _productPage.TopReviewsDoNotContain(word);

            _driver.Navigate().Back(); // Navigate back to the search results page

            return result;
        }

        /// <summary>
        /// Save product details to a file
        /// </summary>
        /// <param name="products">List of products</param>
        /// <param name="filePath">File path to save the details</param>
        private void SaveProductDetailsToFile(List<Product> products, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var product in products)
                {
                    writer.WriteLine(product.ToString());
                }
            }
        }
    }
}
