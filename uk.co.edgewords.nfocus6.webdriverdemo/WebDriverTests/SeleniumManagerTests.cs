using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uk.co.edgewords.nfocus6.webdriverdemo.WebDriverTests
{
    internal class SeleniumManagerTests
    {
        //SeleniumManager is a tool bundled with WebDriver 4.6+
        //Initially it took over responsibility for getting a DriverServer that worked with the user installed browsers
        //This was a big improvement over managing the DriverServers ourselves (either completely manually, or via NuGet packages)
        //It has since added functionality for fetching Web Browsers themselves also.

        [Test]
        public void CrDebugTest()
        {
            ChromeOptions options = new ChromeOptions();
            //BrowserVersion used to be ignored when running locally - now used by SeleniumManager to fetch the browser
            options.BrowserVersion = "canary"; //stable/beta/dev/canary/num
            //options.AddArgument("--profile-directory=Default"); //Profile 1/Profile 2... - select a particular profile
            //options.AddArgument("--user-data-dir=d:\\ChromeProfile2\\Demo\\"); //Specify a custom directory for the profile


            //Instantiate the browser
            IWebDriver driver = new ChromeDriver(options);
            driver.Url = "chrome://version";
            Thread.Sleep(5000);
            driver.Quit();
        }

        [Test]
        public void FxDebugTest()
        {
            FirefoxOptions options = new FirefoxOptions();
            //Used to be ignored running locally - now used by SeleniumManager to fetch the browser
            options.BrowserVersion = "nightly"; //beta/nightly/esr/stable/num

            //Instantiate a browser
            IWebDriver driver = new FirefoxDriver(options);
            driver.Url = "about:support";
            Thread.Sleep(5000);
            driver.Quit();
        }
    }
}
