using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uk.co.edgewords.nfocus6.webdriverdemo.POMClasses;
using uk.co.edgewords.nfocus6.webdriverdemo.Utils;
using static uk.co.edgewords.nfocus6.webdriverdemo.Utils.StaticHelperLib;

namespace uk.co.edgewords.nfocus6.webdriverdemo.WebDriverTests
{
    internal class POMDemoTests : BaseTest
    {
        //A comment
        [Test]
        public void TraditionalTest()
        {
            //Mixes locators, user actions and aserts in one test method
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/";
            driver.FindElement(By.PartialLinkText("Login")).Click();
            
            driver.FindElement(By.Id("username")).SendKeys("edgewords"); 
            driver.FindElement(By.Id("password")).SendKeys("edgewords123");
            driver.FindElement(By.LinkText("Submit")).Click();

            StaticWaitForElement(driver, By.LinkText("Log Out"), 3);

            string bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.That(bodyText, Does.Contain("User is Logged in"), "Not logged in");
            Thread.Sleep(2000);
        }

        [Test]
        public void POMLoginPositive()
        {
            //Test can focus on *what* needs to be tested,
            //with the *how* handled by associated POM classes

            /*
             * Arrange
             */

            //This should make the test easer to read and maintain in the long run
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/";
            HomePagePOM home = new HomePagePOM(driver); //Should be on Home page, so init HomePagePOM and use it's methods for interactions
            home.GoLogin();

            LoginPagePOM loginPage = new LoginPagePOM(driver); //Should now be on longin page so init that

            ////Using "Low level" service methods
            //loginPage.setUsername("edgewords");
            //loginPage.setPassword("edgewords123");
            //loginPage.submitForm();

            ////If mthods reurn an instance of the class you can method chain
            //loginPage.setUsername("edgewords")
            //            .setPassword("edgewords123")
            //            .submitForm();

            ////Using a POM class as *just* an object repository (i.e. a store of objects and how to find them)
            ///is a perfectly acceptable choice, be aware that returning a IWebELement to the test could be abused
            ///as it opens the possibility of locators leaking back in to the test
            //loginPage._submitFormButton.FindElement(By.XPath("//../span")).Click();

            //Tests are still responsible for Testing (aka asserting)
            //Tests ask the POM page to get/return a value, and then assert on *that returned* value
            //You do not assert in the POM method.

            /*
             * Act
             */
            //Using higher level service method to perform the login and return if it was successful
            bool loggedIn = loginPage.LoginExpectSuccess("edgewords", "edgewords123xxxxx");
            
            /*
             * Assert
             */
            //Assert.That(loggedIn, "We did not login");
            Assert.That(loggedIn, Is.True, "We did not login"); //Slightly longer - but more readable?
            //NUnit Classic equivalent:
            //Assert.IsTrue();

            //This is a wait just for us to see the effect of the test. Synchronisation waits are best "hidden" in the POM Class - the test generally shouldn't need to know that it needs to wait at a particular point.
            Thread.Sleep(2000);
        }

        [Test]
        public void ExtremeChainingOhNo()
        {
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/";
            HomePagePOM home = new HomePagePOM(driver);
            home.GoLogin() //This method actually returns an instance of the page it navigates to
                .setUsername("edgewords") //So you *could* contine your test
                .setPassword("edgewords123") //all in this one statement
                .submitForm(); //But don't. If an action causes a page navigation it's best to stop the chain there.
        }

        [Test]
        public void NegativeLoginTest()
        {
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/";
            HomePagePOM home = new HomePagePOM(driver);
            home.GoLogin(); //Page naviagtion occurred. Now init next page.
            LoginPagePOM loginPage = new LoginPagePOM(driver); //Has check in constructor to ensure we are on the right page.
            bool failedLogin = loginPage.LoginExpectFail("edgewords", "notavalidpasswordatall");
            Assert.That(failedLogin, Is.True);
        }
    }
}
