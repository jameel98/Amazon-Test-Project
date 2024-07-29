using AmazonProject.Infra;
using OpenQA.Selenium;

namespace AmazonProject.Pages{

    public class CartPage : BasePage{

        private string ProductsNamesLocator = "//span[@class='a-truncate-cut']";
        private string CheckOutbuttonLocator = "//span[@id='sc-buy-box-ptc-button-announce']";
        public  CartPage(IWebDriver driver) : base(driver){

        }

        private List<string> GetProductsNamesList(){
            List<string> namesList = new List<string>();
            var elements = _driver.FindElements(By.XPath(ProductsNamesLocator));
            foreach( var e in elements){
                string name = e.Text;
                namesList.Add(name);       
            }
            return namesList;
        }

        private void ClickNavigateToCheckOut(){
            IWebElement checkoutButton = _driver.FindElement(By.XPath(CheckOutbuttonLocator));
            checkoutButton.Click();
        }

    }
}