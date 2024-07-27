using AmazonProject.Infra;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace AmazonProject.Pages
{
    public class SearchResultsPage : BasePage  
    {

        private string SliderLocator = "//div[@id='p_36/range-slider_slider-item']/div[2]/input";
        private string MemoryLocator = "//span[text()='16 GB']";
        private string RatingLocator = "//span[text()='4 Stars & Up']";
        public SearchResultsPage(IWebDriver driver) : base(driver) { }

        private IWebElement PriceFilter => _driver.FindElement(By.XPath(SliderLocator));
        private IWebElement MemoryFilter => _driver.FindElement(By.XPath(MemoryLocator));
        private IWebElement RatingFilter => _driver.FindElement(By.XPath(RatingLocator));

        //Todo:: make it dynamic get fileters from test to apply function
        private void SetRating(){
            RatingFilter.Click();
        }
        private void SetMemory(){
            MemoryFilter.Click();
        }
        public void SetPriceSlider(int maxPrice)
        {
            IWebElement slider = _driver.FindElement(By.XPath(SliderLocator));

            // Calculate the offset needed to set the slider to the desired value
            int width = slider.Size.Width;
            int offset = CalculateOffsetForPrice(maxPrice, width);

            // Use Actions class to click and drag the slider
            Actions move = new Actions(_driver);
            move.ClickAndHold(slider)
                .MoveByOffset(offset, 0)
                .Release()
                .Perform();
        }

        private int CalculateOffsetForPrice(int price, int sliderWidth)
        {
            
            int maxPrice = 5200; 
            int offset = (int)((price / (double)maxPrice) * sliderWidth);
            return offset - (sliderWidth); // Adjust to move from the right of the slider
        }

        public void ApplyFilters()
        {
            SetPriceSlider(500);
            SetMemory();
            SetRating();
        }

        public List<string> CollectProductLinks()
        {
            List<string> productLinks = new List<string>();

            var products = _driver.FindElements(By.CssSelector(".s-main-slot .s-result-item"))
                .Where(p => p.FindElements(By.CssSelector(".a-icon-star")).Count > 0)// over 10 reviews
                .Where(p => int.Parse(p.FindElement(By.CssSelector(".a-size-small .a-link-normal")).Text.Split()[0].Replace(",", "")) > 10)
                .ToList();

            foreach (var product in products)
            {
                string link = product.FindElement(By.CssSelector("a.a-link-normal")).GetAttribute("href");
                string title = product.FindElement(By.CssSelector("h2")).Text;

                if (!TitleContainsBadWord(title))
                {
                    productLinks.Add(link);
                }

                if (productLinks.Count >= 10)
                {
                    break;
                }
            }

            return productLinks;
        }

        private bool TitleContainsBadWord(string title)
        {
            // Implement logic to check if title contains "bad"
            return title.Contains("bad", StringComparison.OrdinalIgnoreCase);
        }
    }
}
