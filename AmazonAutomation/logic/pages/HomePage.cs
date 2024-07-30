using AmazonProject.Infra;
using OpenQA.Selenium;

namespace AmazonProject.Pages
{
    public class AmazonHomePage : BasePage  
    {
        // lambda used instead of get. when u call the element its execute and brings the elements
        private IWebElement SearchBox => _driver.FindElement(By.Id("twotabsearchtextbox"));
        private IWebElement SearchButton => _driver.FindElement(By.Id("nav-search-submit-button"));

        public AmazonHomePage(IWebDriver driver) : base(driver) { }

        // navigate to amazon home page
        public void GoToHomePage()
        {
             // Load the configuration
            NavigateTo(GetConfig().BaseUrl);
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
