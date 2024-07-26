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
       
        [SetUp]
        public void SetUp()
        {
            _webDriverManager = new WebDriverManager();
            _webDriverManager.InitializeDriver();

            _amazonHomePage = new AmazonHomePage(_webDriverManager.Driver);
          
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
            _amazonHomePage.GoToHomePage();
            _amazonHomePage.SearchForItem("laptop");

           
        }
    }
}
