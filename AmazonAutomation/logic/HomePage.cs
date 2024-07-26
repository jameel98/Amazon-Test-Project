using AmazonProject.Infra;
using OpenQA.Selenium;

namespace AmazonProject.Pages
{
    public class AmazonHomePage : BasePage  
    {

        public AmazonHomePage(IWebDriver driver) : base(driver) { }


        private IWebElement SearchBox => _driver.FindElement(By.Id("twotabsearchtextbox"));
        private IWebElement SearchButton => _driver.FindElement(By.Id("nav-search-submit-button"));

        public void GoToHomePage()
        {
            NavigateTo("https://www.amazon.com");
        }

        public void SearchForItem(string item)
        {
            SearchBox.SendKeys(item);
            SearchButton.Click();
        }
    }
}
