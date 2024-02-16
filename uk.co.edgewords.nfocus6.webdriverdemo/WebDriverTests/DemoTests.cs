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
    internal class DemoTests : BaseTest
    {
        [Test]
        public void TraditionalTest()
        {
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
        public void POMLogin()
        {
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/";
            HomePagePOM home = new HomePagePOM(driver);
            home.GoLogin();
            LoginPagePOM loginPage = new LoginPagePOM(driver);
            //loginPage.setUsername("edgewords");
            //loginPage.setPassword("edgewords123");
            //loginPage.submitForm();

            //loginPage.setUsername("edgewords")
            //            .setPassword("edgewords123")
            //            .submitForm();

            bool loggedIn = loginPage.LoginExpectSuccess("edgewords", "edgewords123xxxxx");
            Assert.That(loggedIn, "We did not login");
            //NUnit Classic equivalent:
            //Assert.IsTrue();

            //loginPage._submitFormButton.FindElement(By.XPath("//../span")).Click();
            Thread.Sleep(2000);
        }

        [Test]
        public void OhNo()
        {
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/";
            HomePagePOM home = new HomePagePOM(driver);
            home.GoLogin().setUsername("edgewords").setPassword("edgewords123").submitForm();
        }
    }
}
