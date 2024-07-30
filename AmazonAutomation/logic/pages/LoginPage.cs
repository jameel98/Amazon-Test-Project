using AmazonProject.Infra;
using OpenQA.Selenium;


namespace AmazonProject.Pages
{
    public class LoginPage : BasePage  
    {
        // locators
        private string EmailLocator = "//input[@id='ap_email']";
        private string PasswordLocator = "//input[@id='ap_password']";
        private string ContinueLocator = "//span[@id='continue']";
        private string SignInLocator = "//input[@id='signInSubmit']";
        public LoginPage(IWebDriver driver) : base(driver){
            
        }
        
        // lambda used instead of get. when u call the element its execute and brings the elements
        private IWebElement EmailInput => _driver.FindElement(By.XPath(EmailLocator)); 
        private IWebElement ContineButton => _driver.FindElement(By.XPath(ContinueLocator)); 
        private IWebElement PasswordInput => _driver.FindElement(By.XPath(PasswordLocator));
        private IWebElement SignInButton => _driver.FindElement(By.XPath(SignInLocator));


        private void FillEmail(string email){
            EmailInput.Clear();
            EmailInput.SendKeys(email);
        }

        private void ClickContinue(){
            ContineButton.Click();
        }
        private void FillPassword(string password){
            PasswordInput.Clear();
            PasswordInput.SendKeys(password);
        }
        private void ClickSignIn(){
            SignInButton.Click();
        }

        /// <summary>
        /// login flow gets email and password as input then login with email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public void LoginFlow(string email, string password){
            FillEmail(email);
            ClickContinue();
            FillPassword(password);
            ClickSignIn();
        }
    }
}