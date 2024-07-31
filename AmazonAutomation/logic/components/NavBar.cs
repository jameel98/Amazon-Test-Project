using AmazonProject.Infra;
using OpenQA.Selenium;


namespace AmazonProject.Pages
{
    public class Navbar : BasePage  
    {
        // locators
        private string AccountsLocator = "nav-link-accountList-nav-line-1";
        private string CartButtonLocator = "//div[@id='nav-tools']//div[@id='nav-cart-count-container']";
        public Navbar(IWebDriver driver) : base(driver){}

        // lambda used instead of get. when u call the element its execute and brings the elements
        private IWebElement AccountsButton => WebDriverExtensions.FindElement(_driver, By.Id(AccountsLocator), 10);
        private IWebElement CartButton => WebDriverExtensions.FindElement(_driver, By.XPath(CartButtonLocator), 10);

        // function click on login button
        public void ClickLogin(){
            AccountsButton.Click();
        }

        public void GoToCartPage()
        {
            CartButton.Click();
        }
    }
}