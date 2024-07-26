using OpenQA.Selenium;
using System;

namespace AmazonProject.Infra
{
     public abstract class BasePage
    {
        protected IWebDriver _driver;

        public BasePage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Common methods that can be used by all pages can be added here
        public void NavigateTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void TakeScreenshot(string filePath)
        {
            try{

                ITakesScreenshot screenshotDriver = _driver as ITakesScreenshot;

                Screenshot screenshot = screenshotDriver.GetScreenshot();

             //   screenshot.SaveAsFile(filePath, ImageFormat.Png);
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}