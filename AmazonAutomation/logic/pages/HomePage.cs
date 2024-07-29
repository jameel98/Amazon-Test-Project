using AmazonProject.Infra;
using OpenQA.Selenium;

namespace AmazonProject.Pages
{
    public class AmazonHomePage : BasePage  
    {

        public AmazonHomePage(IWebDriver driver) : base(driver) { }
        public void GoToHomePage()
        {
             // Load the configuration
            NavigateTo(GetConfig().BaseUrl);
            Thread.Sleep(6000);
        }

        private IWebElement SearchBox => _driver.FindElement(By.Id("twotabsearchtextbox"));
        private IWebElement SearchButton => _driver.FindElement(By.Id("nav-search-submit-button"));


        public void SearchForItem(string item)
        {
            SearchBox.Clear();
            SearchBox.SendKeys(item);
            SearchButton.Click();
        }
    }
}
