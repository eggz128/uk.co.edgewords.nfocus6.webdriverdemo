using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uk.co.edgewords.nfocus6.webdriverdemo.Utils
{
    internal static class StaticHelperLib
    {
        public static void StaticWaitForElement(IWebDriver driver, By locator, int timeoutInSeconds = 3) //A helper method using the IWebDriver field
        {
            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            myWait.Until(drv => drv.FindElement(locator).Enabled);
        }

    }
}
