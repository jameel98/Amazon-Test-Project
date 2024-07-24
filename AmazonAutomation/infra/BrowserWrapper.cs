using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AmazonProject.Infra
{
    public class WebDriverManager
    {
        public IWebDriver Driver { get; private set; }

        public void InitializeDriver()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public void CloseDriver()
        {
            Driver.Quit();
        }
    }
}
