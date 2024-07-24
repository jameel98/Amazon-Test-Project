using OpenQA.Selenium;

namespace AmazonProject.Pages
{
    public class AmazonHomePage
    {
        private IWebDriver _driver;

        public AmazonHomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement SearchBox => _driver.FindElement(By.Id("twotabsearchtextbox"));
        private IWebElement SearchButton => _driver.FindElement(By.Id("nav-search-submit-button"));

        public void GoToHomePage()
        {
            _driver.Navigate().GoToUrl("https://www.amazon.com");
        }

        public void SearchForItem(string item)
        {
            SearchBox.SendKeys(item);
            SearchButton.Click();
        }
    }
}
