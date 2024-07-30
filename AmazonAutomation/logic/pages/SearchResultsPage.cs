using AmazonProject.Infra;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Newtonsoft.Json.Linq;
using AmazonProject.Entities;
using System.Diagnostics;

namespace AmazonProject.Pages
{
    public class SearchResultsPage : BasePage  
    {

        //locators
        private string SliderLocator = "//div[@id='p_36/range-slider_slider-item']/div[2]/input";
        private string FormPriceLocator = "//div[@id='priceRefinements']//div[@id='p_36/range-slider']/form";
        private string PriceSubmitLocator ="//div[@class='a-section sf-submit-range-button']";
        private string MemoryLocator = "//span[text()='16 GB']";
        private string RatingLocator = "//i[@class='a-icon a-icon-star-medium a-star-medium-4']";
        private string ProductLocator = "//div[@data-component-type='s-search-result']";
        private string ProductRatingsLocator = "//div[@data-component-type='s-search-result'][1]/div//a/span[@class='a-size-base s-underline-text']";//dynamic locator add index
        private string ProductLinkLocator = "//div[@data-component-type='s-search-result'][1]//a[@class='a-link-normal s-no-outline']"; //dynamic locator get href value
        private string ProductPriceLocator = "//div[@data-component-type='s-search-result'][1]//span[@class='a-price-whole']";
        
        private ProductPage _productPage;
        public SearchResultsPage(IWebDriver driver) : base(driver) { }

        private IWebElement MemoryFilter => _driver.FindElement(By.XPath(MemoryLocator));
        private IWebElement RatingFilter => _driver.FindElement(By.XPath(RatingLocator));
        private  IWebElement PriceFilter => _driver.FindElement(By.XPath(SliderLocator));

        private void SetRating(){
            RatingFilter.Click();
        }
        private void SetMemory(){
            MemoryFilter.Click();
        }
        /// <summary>
        /// Sets the price filter slider to the specified target price.
        /// </summary>
        /// <param name="targetPrice">The price to set the slider to, as a string in the format "$XXX".</param>
        public void SetPriceSlider(int targetPrice)
        {
            RefreshPage();
        
            var steps = GetSliderSteps();
            int sliderWidth = GetSliderWidth(PriceFilter);
            int closestStep = FindClosestStep(steps, targetPrice);
            int targetOffset = CalculateTargetOffset(steps, closestStep, sliderWidth);
            MoveSlider(PriceFilter, targetOffset);
            VerifySliderPosition(PriceFilter, targetPrice);
            ClickSubmitPrice();
        }

        /// <summary>
        /// calculate the slider steps by getting the slider form data array length
        /// </summary>
        /// <returns></returns>
        private List<int> GetSliderSteps()
        {
            string dataSliderProps = _driver.FindElement(By.XPath(FormPriceLocator)).GetAttribute("data-slider-props");
            JObject sliderProps = JObject.Parse(dataSliderProps);
            JArray stepValues = (JArray)sliderProps["stepValues"];
            return stepValues.Select(v => (int?)v).Where(v => v.HasValue).Select(v => v.Value).ToList();
        }

        private int GetSliderWidth(IWebElement slider)
        {
            return slider.Size.Width;
        }
        /// <summary>
        /// search for closest price to the target price we need 
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
        /// calculate the needed offset of the slider
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="closestStep"></param>
        /// <param name="sliderWidth"></param>
        /// <returns></returns>
        private int CalculateTargetOffset(List<int> steps, int closestStep, int sliderWidth)
        {
            int currentStep = steps.IndexOf(closestStep);
            double pixelsPerStep = (double)sliderWidth / (steps.Count - 1);
            return (int)(currentStep * pixelsPerStep - sliderWidth / 2);
        }
        
        /// <summary>
        /// move slider according to the offset value
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
        /// verify the slider is in the correct position of the target price
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
        /// after moving the slider click the submit price button to filter the results
        /// </summary>
        public void ClickSubmitPrice(){
            IWebElement SubmitButton = _driver.FindElement(By.XPath(PriceSubmitLocator));
            SubmitButton.Click();
        }
        /// <summary>
        /// apply the memory, rating and price filters
        /// </summary>
        public void ApplyFilters()
        {
            SetMemory();
            SetRating();
            SetPriceSlider(500);

        }

    /// <summary>
    /// collect the products details (url, name, price) and save them to the file
    /// </summary>
    /// <param name="filePath"></param> file path to save the details in it
    /// <param name="word"></param> the word "bad" to not be in the reviews
     public void CollectAndSaveProductDetails(string filePath, string word)
        {
            List<Product> products = CollectProductDetails(word);
            SaveProductDetailsToFile(products, filePath);
        }

        /// <summary>
        /// collect product details add them to list and save them in a file
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private List<Product> CollectProductDetails(string word)
        {
            List<Product> products = new List<Product>();

            var productElements = _driver.FindElements(By.XPath(ProductLocator))
                .Where(p => HasMoreThanTenReviews(p))
                .ToList();

            foreach (var productElement in productElements)
            {
                string link = GetProductLink(productElement);

                if (TopReviewsDoNotContainBad(productElement, word))
                {
                    string name = GetProductName(productElement);
                    string price = GetProductPrice(productElement);
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
        ///  verify product have more than 10 reviews
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private bool HasMoreThanTenReviews(IWebElement product)
        {
            int reviewCount = int.Parse(product.FindElement(By.XPath(ProductRatingsLocator)).Text.Split()[0].Replace(",", ""));
            return reviewCount > 10;
        }
        /// <summary>
        /// get product link
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private string GetProductLink(IWebElement product)
        {
            return product.FindElement(By.CssSelector(ProductLinkLocator)).GetAttribute("href");
        }
        /// <summary>
        /// get product name
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private string GetProductName(IWebElement product)
        {
            return product.FindElement(By.CssSelector("h2")).Text;
        }
        /// <summary>
        /// get product price
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private string GetProductPrice(IWebElement product)
        {
            return product.FindElement(By.XPath(ProductPriceLocator)).Text;
        }

        /// <summary>
        /// verify top 10 reviews dont contain the word bad
        /// </summary>
        /// <param name="product"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool TopReviewsDoNotContainBad(IWebElement product, string word)
        {
            string productLink = GetProductLink(product);
            _driver.Navigate().GoToUrl(productLink);

            _productPage = new ProductPage(_driver);

            if(_productPage.TopUSAReviewsDontContain(word) && _productPage.TopTReviewsDontContain(word)){
                _productPage.AddItemToCart();
                return true;
            }else{
                return false;
            }
        }

        /// <summary>
        /// save product details in a file
        /// </summary>
        /// <param name="products"></param>
        /// <param name="filePath"></param>
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

