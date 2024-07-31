using OpenQA.Selenium;
using System;
using AmazonAutomation.Config;
using OpenQA.Selenium.Support.UI;
using System.Drawing.Imaging; // For ImageFormat

namespace AmazonProject.Infra
{
     public abstract class BasePage
    {
        protected IWebDriver _driver;

        private ConfigProvider _config;

        
        public BasePage(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// navigation function gets input url and navigate to the url by driver
        /// </summary>
        /// <param name="url"></param>
        public void NavigateTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public ConfigProvider GetConfig(){
            // Load the configuration
            _config = ConfigProvider.LoadConfig(@"C:\Users\Admin\Downloads\5 Tech\amazon project\AmazonAutomation\config.json");
            if (_config == null)
            {
                throw new Exception("Configuration is null.");
            }
            return _config;   
        }
         public void RefreshPage()
        {
            _driver.Navigate().Refresh();
        }

        public void TakeScreenshot()
        {
            try
            {
                // Get the screenshot
                Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();

                // Construct the directory path
                string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagesDirectory = Path.Combine(solutionDirectory, "screenshots");

                // Ensure the directory exists
                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                // Construct the file path
                string filePath = Path.Combine(imagesDirectory, $"testResult.png");

                // Save the screenshot
                screenshot.SaveAsFile(string.Format(filePath));

                Console.WriteLine($"Screenshot saved at {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving screenshot: {ex.Message}");
            }
        }
    }
}