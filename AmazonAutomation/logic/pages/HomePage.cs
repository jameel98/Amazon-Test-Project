using AmazonProject.Infra;
using OpenQA.Selenium;

namespace AmazonProject.Pages
{
    public class AmazonHomePage : BasePage  
    {
        // lambda used instead of get. when u call the element its execute and brings the elements
        private IWebElement SearchBox => WebDriverExtensions.FindElement(_driver ,By.Id("twotabsearchtextbox"), 10);
        private IWebElement SearchButton => WebDriverExtensions.FindElement(_driver, By.Id("nav-search-submit-button"), 10);

        public AmazonHomePage(IWebDriver driver) : base(driver) { }

        // navigate to amazon home page
        public void GoToHomePage()
        {
             // Load the configuration
            NavigateTo(GetConfig().BaseUrl);
            Thread.Sleep(5000);
            string url = _driver.Url;
            if(url != GetConfig().BaseUrl)
            {
                Thread.Sleep(20000);
            }

        }

        // search for item, clear search input then put the item name and search it
        public void SearchForItem(string item)
        {
            SearchBox.Clear();
            SearchBox.SendKeys(item);
            SearchButton.Click();
        }
    }
}
