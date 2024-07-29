using AmazonProject.Infra;
using OpenQA.Selenium;

namespace AmazonProject.Pages{


    public class ProductPage : BasePage{
        private string ReviewUSALocator = "//span[@data-hook='review-body']/div/div/span";
        private string ReviewTranslatedLocator = "//span[@data-hook='review-body']/div/div/span[2]";
        private string AddToCartLocator = "//input[@id='add-to-cart-button']";
        public ProductPage(IWebDriver driver) : base(driver){}

        public bool TopUSAReviewsDontContain(string word){
            var reviews = _driver.FindElements(By.XPath(ReviewUSALocator))
            .Take(8)
            .Select(r => r.Text);
            return reviews.All(review => !review.Contains(word, StringComparison.OrdinalIgnoreCase));
        }
         public bool TopTReviewsDontContain(string word){
            var reviews = _driver.FindElements(By.XPath(ReviewTranslatedLocator))
            .Take(2)
            .Select(r => r.Text);
            return reviews.All(review => !review.Contains(word, StringComparison.OrdinalIgnoreCase));
        }

        public void AddItemToCart(){
            IWebElement addButton = _driver.FindElement(By.XPath(AddToCartLocator));
            addButton.Click();
        }
    }
}