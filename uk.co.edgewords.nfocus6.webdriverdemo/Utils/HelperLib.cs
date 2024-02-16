using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uk.co.edgewords.nfocus6.webdriverdemo.Utils
{
    internal class HelperLib
    {
        private IWebDriver _driver; //Field to work with passed driver in class methdos

        public HelperLib(IWebDriver driver) //Get the driver from the calling test
        {
            this._driver = driver; //Put it in to this classes field
        }
        public void WaitForElement(By locator, int timeoutInSeconds = 3) //A helper method using the IWebDriver field
        {
            WebDriverWait myWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            myWait.Until(drv => drv.FindElement(locator).Enabled);
        }
        //More helper methods to be added as needed
    }
}
