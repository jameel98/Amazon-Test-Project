using AmazonProject.Infra;
using OpenQA.Selenium;

namespace AmazonProject.Pages{

    public class CartPage : BasePage{
        // locators
        private string ProductsNamesLocator = "//span[@class='a-truncate-cut']";
        private string CheckOutbuttonLocator = "//span[@id='sc-buy-box-ptc-button-announce']";
        public  CartPage(IWebDriver driver) : base(driver){

        }
        
        /// <summary>
        /// the function get all names in cart page and saves them in a list
        /// returns a list of the names. use it to validate the names in cart list are names
        /// of same products we added
        /// </summary>
        /// <returns></returns>
        public List<string> GetProductsNamesList(){
            List<string> namesList = new List<string>();
            var elements = _driver.FindElements(By.XPath(ProductsNamesLocator));
            foreach( var e in elements){
                string name = e.Text;
                namesList.Add(name);       
            }
            return namesList;
        }

        // click on checkout button to navigate to checkout page
        public void ClickNavigateToCheckOut(){
            IWebElement checkoutButton = WebDriverExtensions.FindElement(_driver, By.XPath(CheckOutbuttonLocator), 10);
            checkoutButton.Click();
        }
        /// <summary>
        /// Validate that all items from the file are present in the cart
        /// </summary>
        /// <param name="filePath">File path to read the items from</param>
        /// <returns>True if all items are present, otherwise false</returns>
        public bool ValidateCartItems(string filePath)
        {
            List<string> fileItemNames = File.ReadAllLines(filePath).ToList();
            List<string> cartItemNames = GetProductsNamesList();

            return fileItemNames.All(fileItem => cartItemNames.Contains(fileItem));
        }
    }
}