using AmazonProject.Infra;
using OpenQA.Selenium;

namespace AmazonProject.Pages{

    public class CartPage : BasePage{
        // locators
        private string ProductsNamesLocator = "//span[@class='a-truncate-cut']";
        private string CheckOutbuttonLocator = "//span/input[@data-feature-id='proceed-to-checkout-action']";
        public  CartPage(IWebDriver driver) : base(driver){

        }
        
        /// <summary>
        /// the function get all names in cart page and saves them in a list
        /// returns a list of the names. use it to validate the names in cart list are names
        /// of same products we added
        /// </summary>
        /// <returns></returns>
        public List<string> GetProductsNamesList()
        {
            List<string> namesList = new List<string>();
            var elements = _driver.FindElements(By.XPath(ProductsNamesLocator));
            foreach (var e in elements)
            {
                string name = e.Text.Trim(); // Trim any extra whitespace
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
    List<string> fileItemNames = File.ReadAllLines(filePath)
                                     .Select(line => ExtractNameFromLine(line))
                                     .Select(name => name.ToLower())
                                     .ToList();
                                     
    List<string> cartItemNames = GetProductsNamesList()
                                 .Select(name => name.ToLower())
                                 .ToList();

    return fileItemNames.All(fileItem => cartItemNames.Contains(fileItem));
}

private string ExtractNameFromLine(string line)
{
    // Assuming the name is always after "Name:" in the line
    const string namePrefix = "Name:";
    int nameIndex = line.IndexOf(namePrefix, StringComparison.OrdinalIgnoreCase);
    
    if (nameIndex >= 0)
    {
        // Extract the part of the line after "Name:" and trim any leading or trailing whitespace
        string name = line.Substring(nameIndex + namePrefix.Length).Trim();
        return name;
    }

    // Return an empty string or handle the case where "Name:" is not found
    return string.Empty;
}


    }
}