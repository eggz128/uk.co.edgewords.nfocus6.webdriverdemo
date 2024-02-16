using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Remote;

namespace uk.co.edgewords.nfocus6.webdriverdemo.Utils
{
    internal class BaseTest
    {
        //Field to hold a WebDriver that is in scope for all methods in this class
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected IWebDriver driver; //ChromeDrivers/EdgeDrivers/FirefoxDrivers are all IWebDrivers
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [SetUp] //Runs before each and every [Test] in this class
        public void Setup()
        {
            //ToDo: set browser based on config file, command line param or env var.
            //string browser = "edge"; //Change me to instantiate different browsers

            string browser = Environment.GetEnvironmentVariable("BROWSER");
            Console.WriteLine("Browser set to: " + browser);
            if (browser == null) //Should do
            {
                browser = "edge";
                Console.WriteLine("BROWSER env not set: Setting to Edge");
            }


            //Instantiate a browser based on variable
            switch (browser)
            {
                case "edge":
                    driver = new EdgeDriver();
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "ie": //Even on Win11 IE still lives! At least in IE Mode in Edge Chromium. More setup than what you see here is needed to make it work.
                    driver = new InternetExplorerDriver();
                    break;
                case "chromemobile": //Firefox can also "emulate" mobile devices - but unless you're wanting to emulate Firefox Android I'd be worried about the validity of tests. i.e. Firefox on iOS uses WebKit, which is not related to Firefox (Gecko) desktop at all.
                    ChromeOptions optionsCrMobile = new ChromeOptions();

                    //Pick from the device list offered in Chrome
                    //optionsCrMobile.EnableMobileEmulation("Pixel 7");

                    //Or define something custom
                    ChromiumMobileEmulationDeviceSettings mobile = new ChromiumMobileEmulationDeviceSettings();
                    mobile.Width = 1000;
                    mobile.Height = 7000;
                    mobile.EnableTouchEvents = true;
                    mobile.PixelRatio = 1.5; //Essentially a zoom. A CSS pixel is not necessarily equal to a device pixel. Imagine how unuseably small UI elements could be on a modern 4k resolution mobile phone...
                    mobile.UserAgent = "Mozilla/5.0 (Linux; Android 13; StevesCustomMobileBrowser) AppleWebKit/537.36 (KHTML, like Gecko) StevesCustomBrowser/21.0 Chrome/110.0.5481.154 Mobile Safari/537.36";
                    optionsCrMobile.EnableMobileEmulation(mobile);

                    driver = new ChromeDriver(optionsCrMobile);
                    break;
                //Headless execution consumes less resources. Screnshots will still work.
                case "chromeheadless":
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--headless"); //Chrome *used* to have a seperate rendering engine for headless. https://developer.chrome.com/docs/chromium/new-headless
                    driver = new ChromeDriver(chromeOptions);
                    //You may want to set a defined window size...
                    break;
                case "firefoxheadless":
                    FirefoxOptions firefoxoptions = new FirefoxOptions();
                    firefoxoptions.AddArgument("--headless");
                    driver = new FirefoxDriver(firefoxoptions);
                    break;
                //You may find old codebases use/refer to a PhantomJS driver - this is a fork of WebKit that could run in headless mode. Since Chrome and Firefox can now both do headless the reason for PhantomJS disappeared.
                case "remote":
                    ChromeOptions remoteFx = new ChromeOptions();
                    driver = new RemoteWebDriver(new Uri("http://172.26.177.83:4444/wd/hub"), remoteFx);
                    break;
                default:
                    driver = new ChromeDriver();
                    break;
            }

            string startURL = TestContext.Parameters["WebAppURL"];
            driver.Url = startURL;
            //Implicit wait
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //Do not use in production tests - has undefined behaviour.
            //Use WebDriverWaits at key synchronisation points instead
        }

        [TearDown]
        public void Teardown()
        {
            //Determining in teardown test result
            //https://docs.nunit.org/articles/nunit/writing-tests/TestContext.html#result
            if (TestContext.CurrentContext.Result.Outcome.Status.ToString() == "Passed")
            {
                Console.WriteLine("Test passed");
                //Might be better to take a screenshot on fail
            }

            //Except for passing Assertions - results during execution are logged and can be read back in TearDown
            var assertionResults = TestContext.CurrentContext.Result.Assertions;
            foreach (var assertionResult in assertionResults)
            {
                Console.WriteLine("Status: " + assertionResult.Status.ToString());
                Console.WriteLine("Message: " + assertionResult.Message.ToString());
            }
            //driver.Close(); //Closes current tab
            driver.Quit(); //Quits browser and DriverServer and disposes of objects (although NUnit Analyser does not know this without further config)
        }
    }
}
