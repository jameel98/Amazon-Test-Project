using AmazonProject.Infra;
using OpenQA.Selenium;


namespace AmazonProject.Pages
{
    public class Navbar : BasePage  
    {
        private string AccountsLocator = "nav-link-accountList-nav-line-1";
        public Navbar(IWebDriver driver) : base(driver){
            
        }

        private IWebElement AccountsButton => _driver.FindElement(By.Id(AccountsLocator)); 

        public void ClickLogin(){
            AccountsButton.Click();
        }
    }
}