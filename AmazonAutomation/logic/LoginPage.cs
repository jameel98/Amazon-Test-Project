using AmazonProject.Infra;
using OpenQA.Selenium;


namespace AmazonProject.Pages
{
    public class LoginPage : BasePage  
    {
        private string EmailLocator = "//input[@id='ap_email']";
        private string PasswordLocator = "//input[@id='ap_password']";
        private string ContinueLocator = "//span[@id='continue']";
        private string SignInLocator = "//input[@id='signInSubmit']";
        public LoginPage(IWebDriver driver) : base(driver){
            
        }

        private IWebElement EmailInput => _driver.FindElement(By.XPath(EmailLocator)); 
        private IWebElement ContineButton => _driver.FindElement(By.XPath(ContinueLocator)); 

        public void fillEmail(string email){
            EmailInput.SendKeys(email);
        }

        public void ClickContinue(){
            ContineButton.Click();
        }
        public void fillPassword(string password){
            IWebElement PasswordInput = _driver.FindElement(By.XPath(PasswordLocator));
            PasswordInput.Clear();
            PasswordInput.SendKeys(password);
        }
        public void ClickSignIn(){
            IWebElement SignInButton = _driver.FindElement(By.XPath(SignInLocator));
            SignInButton.Click();
        }
    }
}