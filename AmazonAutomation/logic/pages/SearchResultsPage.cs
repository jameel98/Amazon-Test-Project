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

        //Todo:: make it dynamic get fileters from test to apply function
        private void SetRating(){
            RatingFilter.Click();
        }
        private void SetMemory(){
            MemoryFilter.Click();
        }
        public void SetPriceSlider(int targetPrice)
        {
            RefreshPage();
            IWebElement PriceFilter = GetPriceSliderElement();
            var steps = GetSliderSteps();
            int sliderWidth = GetSliderWidth(PriceFilter);
            int closestStep = FindClosestStep(steps, targetPrice);
            int targetOffset = CalculateTargetOffset(steps, closestStep, sliderWidth);
            MoveSlider(PriceFilter, targetOffset);
            VerifySliderPosition(PriceFilter, targetPrice);
            ClickSubmitPrice();
        }

        private IWebElement GetPriceSliderElement()
        {
            return _driver.FindElement(By.XPath(SliderLocator));
        }

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

        private int FindClosestStep(List<int> steps, int targetPrice)
        {
            return steps.OrderBy(step => Math.Abs(step - targetPrice)).First();
        }

        private int CalculateTargetOffset(List<int> steps, int closestStep, int sliderWidth)
        {
            int currentStep = steps.IndexOf(closestStep);
            double pixelsPerStep = (double)sliderWidth / (steps.Count - 1);
            return (int)(currentStep * pixelsPerStep - sliderWidth / 2);
        }

        private void MoveSlider(IWebElement slider, int targetOffset)
        {
            Actions actions = new Actions(_driver);
            actions.ClickAndHold(slider)
                   .MoveByOffset(targetOffset, 0)
                   .Release()
                   .Perform();
        }

        private void VerifySliderPosition(IWebElement slider, int targetPrice)
        {
            string ariaValueText = slider.GetAttribute("aria-valuetext");
            int newPrice = int.Parse(ariaValueText.Replace("$", "").Replace(",", "").Replace("+", ""));
            Console.WriteLine($"Slider set to: ${newPrice}");
        }

        public void ClickSubmitPrice(){
            IWebElement SubmitButton = _driver.FindElement(By.XPath(PriceSubmitLocator));
            SubmitButton.Click();
        }
        public void ApplyFilters()
        {
            SetMemory();
            SetRating();
            SetPriceSlider(500);

        }

     public void CollectAndSaveProductLinks(string filePath, string word)
        {
            List<Product> products = CollectProductLinks(word);
            SaveProductDetailsToFile(products, filePath);
        }

        private List<Product> CollectProductLinks(string word)
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

        private bool HasMoreThanTenReviews(IWebElement product)
        {
            int reviewCount = int.Parse(product.FindElement(By.XPath(ProductRatingsLocator)).Text.Split()[0].Replace(",", ""));
            return reviewCount > 10;
        }

        private string GetProductLink(IWebElement product)
        {
            return product.FindElement(By.CssSelector(ProductLinkLocator)).GetAttribute("href");
        }

        private string GetProductName(IWebElement product)
        {
            return product.FindElement(By.CssSelector("h2")).Text;
        }

        private string GetProductPrice(IWebElement product)
        {
            return product.FindElement(By.XPath(ProductPriceLocator)).Text;
        }

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

