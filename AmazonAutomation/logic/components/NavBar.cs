using AmazonProject.Infra;
using OpenQA.Selenium;


namespace AmazonProject.Pages
{
    public class Navbar : BasePage  
    {
        // component
        private string AccountsLocator = "nav-link-accountList-nav-line-1";
        public Navbar(IWebDriver driver) : base(driver){
            
        }

        // lambda used instead of get. when u call the element its execute and brings the elements
        private IWebElement AccountsButton => _driver.FindElement(By.Id(AccountsLocator)); 

        // function click on login button
        public void ClickLogin(){
            AccountsButton.Click();
        }
    }
}