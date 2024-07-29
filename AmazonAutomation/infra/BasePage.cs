using OpenQA.Selenium;
using System;
using AmazonAutomation.Config;

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

        // Common methods that can be used by all pages can be added here
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
        
        public void TakeScreenshot(string filePath)
        {   
            try{

                Screenshot ss = ((ITakesScreenshot)_driver).GetScreenshot();
        //        ss.SaveAsFile(filePath, ScreenshotImageFormat.Png);

             //   screenshot.SaveAsFile(filePath, ImageFormat.Png);
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}