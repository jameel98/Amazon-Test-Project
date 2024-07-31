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
        private LoginPage _loginPage;
        private SearchResultsPage _searchResultsPage;
        private CartPage _cartPage;
        private ConfigProvider _config;
        private int _randomNumber;

        [SetUp]
        public async Task SetUp()
        {
            _browserWrapper = new BrowserWrapper();
            await _browserWrapper.InitializeDriverAsync();

            // Load the configuration
            _config = ConfigProvider.LoadConfig(@"C:\Users\Admin\Downloads\5 Tech\amazon project\AmazonAutomation\config.json");

            _amazonHomePage = new AmazonHomePage(_browserWrapper.Driver);

            Random random = new Random();
            // Generate a random integer between 0 and 5000
            _randomNumber = random.Next(0, 5000);

        
            await Task.Delay(_randomNumber + 5000); // to avoid automate detection
            _amazonHomePage.GoToHomePage();
            _navbar = new Navbar(_browserWrapper.Driver);
            _navbar.ClickLogin();
            _loginPage = new LoginPage(_browserWrapper.Driver);
            _loginPage.LoginFlow(_config.Email, _config.Password);

            _amazonHomePage = new AmazonHomePage(_browserWrapper.Driver);
            _amazonHomePage.SearchForItem(_config.SearchTerm);

            _searchResultsPage = new SearchResultsPage(_browserWrapper.Driver);

            _searchResultsPage.ApplyFilters();
        }

        [TearDown]
        public void TearDown()
        {   _cartPage.RefreshPage();
            _cartPage.ClickNavigateToCheckOut();
            _cartPage.TakeScreenshot();
            _browserWrapper.CloseDriver();
        }

        [Test]
        public void TestAmazonAutomationTest()
        {
           // Arrange and Act
            _searchResultsPage.CollectAndSaveProductDetails(@"C:\Users\Admin\Downloads\5 Tech\amazon project\AmazonAutomation\products.txt","bad");
            // assert
            _searchResultsPage.RefreshPage();
            _navbar = new Navbar(_browserWrapper.Driver);
            _navbar.GoToCartPage();

            _cartPage = new CartPage(_browserWrapper.Driver);

                    
            // Assert that the cart contains 10 items
            // the -1 because the locator adds sub name of laptop as a laptop
            Assert.That(_cartPage.GetProductsNamesList().Count -1 , Is.EqualTo(10));

        }
        
    }
}
