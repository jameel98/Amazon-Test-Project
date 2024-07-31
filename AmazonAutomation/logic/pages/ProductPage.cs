using AmazonProject.Infra;
using OpenQA.Selenium;

namespace AmazonProject.Pages{


    public class ProductPage : BasePage{
        // loators
        private string ReviewUSALocator = "//span[@data-hook='review-body']/div/div/span";
        private string ReviewTranslatedLocator = "//span[@data-hook='review-body']/div/div/span[2]";
        private string AddToCartLocator = "//input[@id='add-to-cart-button']";
    
        public ProductPage(IWebDriver driver) : base(driver){}

        private IWebElement addButton => WebDriverExtensions.FindElement(_driver, By.XPath(AddToCartLocator), 10);
        
        /// <summary>
        /// search for the top USA Reviews and validate they dont contain word "bad"
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool TopUSAReviewsDontContain(string word){
            var reviews = _driver.FindElements(By.XPath(ReviewUSALocator))
            .Take(8)
            .Select(r => r.Text);
            return reviews.All(review => !review.Contains(word, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// search for top reviews from other contries and valiate they dont contain word "bad"
        /// we search for the translated reviews
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
         public bool TopTReviewsDontContain(string word){
            var reviews = _driver.FindElements(By.XPath(ReviewTranslatedLocator))
            .Take(2)
            .Select(r => r.Text);
            return reviews.All(review => !review.Contains(word, StringComparison.OrdinalIgnoreCase));
        }
        public bool TopReviewsDoNotContain(string word) {
            
            return TopUSAReviewsDontContain(word) && TopTReviewsDontContain(word);
        }
        // add item to the cart
        public void AddItemToCart(){
            
            addButton.Click();
        }
    }
}