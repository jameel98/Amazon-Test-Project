using AmazonAutomation.Config;
using AmazonProject.Infra;
using AmazonProject.Pages;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace AmazonProject.Tests
{
    [TestFixture]
    public class AmazonTests
    {
        private BrowserWrapper _browserWrapper;
        private AmazonHomePage _amazonHomePage;
        private Navbar _navbar;
        private SignInPopUp _signInPopUp;
        private LoginPage _loginPage;
        private SearchResultsPage _searchResultsPage;
        private ConfigProvider _config;
        private int _randomNumber;

        [SetUp]
        public async Task SetUp()
        {
            _browserWrapper = new BrowserWrapper();
            await _browserWrapper.InitializeDriverAsync();

            _amazonHomePage = new AmazonHomePage(_browserWrapper.Driver);

                // Load the configuration
            _config = ConfigProvider.LoadConfig(@"C:\Users\Admin\Downloads\5 Tech\amazon project\AmazonAutomation\config.json");

            Random random = new Random();

            // Generate a random integer between 0 and 5000
            _randomNumber = random.Next(0, 5000);

        
            await Task.Delay(_randomNumber); // to avoid automate detection
            _amazonHomePage.GoToHomePage();

            _navbar = new Navbar(_browserWrapper.Driver);
            _navbar.ClickLogin();
            _loginPage = new LoginPage(_browserWrapper.Driver);
            _loginPage.LoginFlow(_config.Email, _config.Password);
            

        }

        [TearDown]
        public void TearDown()
        {
            _browserWrapper.CloseDriver();
        }

        [Test]
        public void TestAmazonAutomationTest()
        {
            _amazonHomePage = new AmazonHomePage(_browserWrapper.Driver);
            _amazonHomePage.SearchForItem(_config.SearchTerm);

            _searchResultsPage = new SearchResultsPage(_browserWrapper.Driver);

            _searchResultsPage.ApplyFilters();

        }
    }
}
