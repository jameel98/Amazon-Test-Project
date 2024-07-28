using AmazonProject.Infra;
using OpenQA.Selenium;


namespace AmazonProject.Pages
{
    public class SignInPopUp : BasePage  
    {
        private string SignInButton = "//a/span[@class='nav-action-inner']";
        public SignInPopUp(IWebDriver driver) : base(driver){
            
        }

        private IWebElement AccountsButton => _driver.FindElement(By.Id(SignInButton)); 

        public void ClickLogin(){
            AccountsButton.Click();
        }
    }
}