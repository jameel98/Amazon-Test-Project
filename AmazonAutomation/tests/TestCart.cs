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
        private WebDriverManager _webDriverManager;
        private AmazonHomePage _amazonHomePage;
        private SearchResultsPage _searchResultsPage;
        private ConfigProvider _config;
        private int _randomNumber;

        [SetUp]
        public async Task SetUp()
        {
            _webDriverManager = new WebDriverManager();
            await _webDriverManager.InitializeDriverAsync();

            _amazonHomePage = new AmazonHomePage(_webDriverManager.Driver);
            _searchResultsPage = new SearchResultsPage(_webDriverManager.Driver);

                // Load the configuration
            _config = ConfigProvider.LoadConfig(@"C:\Users\Admin\Downloads\5 Tech\amazon project\AmazonAutomation\config.json");

            Random random = new Random();

            // Generate a random integer between 0 and 100
            _randomNumber = random.Next(0, 5000);
        }

        [TearDown]
        public void TearDown()
        {
            _webDriverManager.CloseDriver();
        }

        [Test]
        public void AmazonAutomationTest()
        {
            // Arrange
            Thread.Sleep(_randomNumber); // to avoid automate detection
            _amazonHomePage.GoToHomePage();
            _amazonHomePage.SearchForItem(_config.SearchTerm);

            _searchResultsPage.ApplyFilters();

        }
    }
}
