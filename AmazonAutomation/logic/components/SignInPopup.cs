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
        private IWebElement AccountsButton => _driver.FindElement(By.Id(SignInButton)); 

        // click login button
        public void ClickLogin(){
            AccountsButton.Click();
        }
    }
}