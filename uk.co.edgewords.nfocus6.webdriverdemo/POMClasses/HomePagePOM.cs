﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uk.co.edgewords.nfocus6.webdriverdemo.POMClasses
{
    internal class HomePagePOM
    {
        private IWebDriver _driver; //Field that will hold a driver for service methods in this test to work with

        public HomePagePOM(IWebDriver driver) //Constructor to get the driver from the test
        {
            this._driver = driver; //Assigns passed driver in to private field in this class
        }

        //Locators - finding elements on the page
        private IWebElement loginLink => _driver.FindElement(By.PartialLinkText("Login"));

        //Service methods - doing things with elements on the page
        public LoginPagePOM GoLogin()
        {
            loginLink.Click();
            return new LoginPagePOM(this._driver);
        }
        

    }
}