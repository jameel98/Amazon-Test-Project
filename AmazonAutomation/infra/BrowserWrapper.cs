using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using SeleniumUndetectedChromeDriver;
using System.Threading.Tasks;

namespace AmazonProject.Infra
{
    public class BrowserWrapper
    {

        public IWebDriver Driver { get; private set; }
        /// <summary>
        /// initialize the browser, I used undetected driver to avoid automations detection by the amazon website
        /// </summary>
        /// <returns></returns>
       public async Task InitializeDriverAsync()
        {
            var driverPath = await new ChromeDriverInstaller().Auto();
            Driver = UndetectedChromeDriver.Create(driverExecutablePath: driverPath);

            ((IJavaScriptExecutor)Driver).ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})");

            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

        }

        public void CloseDriver()
        {
            if (Driver != null)
            {
                Driver.Close();
                Driver.Quit();
            }
        }

    }
}
