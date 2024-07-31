using AmazonProject.Infra;
using OpenQA.Selenium;


namespace AmazonProject.Pages
{
    public class SignInPopUp : BasePage  
    {
        // locator
        private string SignInButton = "//a/span[@class='nav-action-inner']";
        public SignInPopUp(IWebDriver driver) : base(driver){
            
        }

        // lambda used instead of get. when u call the element its execute and brings the elements
        private IWebElement AccountsButton => WebDriverExtensions.FindElement(_driver, By.Id(SignInButton), 10); 

        // click login button
        public void ClickLogin(){
            AccountsButton.Click();
        }
    }
}