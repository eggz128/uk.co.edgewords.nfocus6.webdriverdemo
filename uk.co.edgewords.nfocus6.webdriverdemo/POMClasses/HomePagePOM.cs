using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uk.co.edgewords.nfocus6.webdriverdemo.POMClasses
{
    internal class HomePagePOM
    {
        //A very simple POM page that models the webdriver2 home page

        private IWebDriver _driver; //Field that will hold a driver for service methods in this test to work with

        public HomePagePOM(IWebDriver driver) //Constructor to get the driver from the test
        {
            this._driver = driver; //Assigns passed driver in to private field in this class
        }

        //Locators - finding elements on the page
        private IWebElement _loginLink => _driver.FindElement(By.PartialLinkText("Login")); //Every time loginLink is used this find is performed again - (hopefully) avoiding stale elements

        //Service methods - doing things with elements on the page
        public LoginPagePOM GoLogin() //This method should really be void
        {
            _loginLink.Click();
            return new LoginPagePOM(this._driver); //and not return an instance of the page being navigated to. As is although this allows method chaining, debugging one long statement when things go wrong is confusing.
            //More succinctly: If an user action causes a navigation event, don't allow chaining from that point.
        }
        

    }
}
