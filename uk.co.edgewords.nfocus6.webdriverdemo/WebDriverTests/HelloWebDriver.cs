
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uk.co.edgewords.nfocus6.webdriverdemo.Utils;
using static uk.co.edgewords.nfocus6.webdriverdemo.Utils.StaticHelperLib;

namespace uk.co.edgewords.nfocus6.webdriverdemo.WebDriverTests
{
    [TestFixture] //Optional annotation to visually quickly identify classes that contain tests
    internal class HelloWebDriver : BaseTest
    {
        

        [Test, Order(1), Category("Smoke")] //Annotation tells NUnit the following method is a test
        
        public void LoginTest()
        {
            //Tests should typically be structured like so:
            //Arrange-Act-Assert

            /*
             * Arrange - get the application in to a state ready to test the "thing" (login)
             */

            Console.WriteLine("Start test"); //Report key events in the test

            //Navigate to a page using a property
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/";
            //Alternatively using method calls. There is no actual difference in browser.
            //driver.Navigate().GoToUrl("https://www.edgewordstraining.co.uk/webdriver2/");

            //Find the login link and click it
            //(browser)-Find me an element-(how?)-By using this locator-(What then?)-Click it!
            driver.FindElement(By.LinkText("Login To Restricted Area")).Click();

            //A conditional wait to ensure we are on the next page before logging in the report
            //WebDriverWait myWait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            //myWait2.Until(drv => drv.FindElement(By.CssSelector("#username")));
            //Replace with call to helper method
            HelperLib myHelper = new HelperLib(driver); //Instantiate HelperLib class and pass the driver to the constructor
            myHelper.WaitForElement(By.CssSelector("#username"), 2);
            

            //Now on next page (- Hopefully!)
            string headingText = driver.FindElement(By.CssSelector("#right-column > h1:nth-child(1)")).Text;
            Console.WriteLine("Should be on Login page. Heading text is: " + headingText);

            //Take a screenshot -- full page
            Screenshot screenshot = driver.TakeScreenshot();
            screenshot.SaveAsFile(@"D:\Screenshots\fullpage.png");

            //Take a screenshot of a web element
            IWebElement formElm = driver.FindElement(By.Id("Login"));
            //var ssElm = formElm as ITakesScreenshot; //C#ish cast
            var ssElm = (ITakesScreenshot)formElm;
            Screenshot screenshotElm = ssElm.GetScreenshot();
            screenshotElm.SaveAsFile(@"D:\Screenshots\form.png");

            //Attach screenshots to a report
            Console.WriteLine("Attaching screenshot to report");
            TestContext.WriteLine("Also writes a line in to the report");
            TestContext.AddTestAttachment(@"D:\Screenshots\form.png", "Just the form");


            //Verification:
            //It is possible to be on the log in page and *already* be logged in
            //If we are *already* logged in, allow the test run to continue but still fail in the end

            string bodyText = driver.FindElement(By.TagName("body")).Text;

            try
            {
                //NUnit "Classic" assertion. Removed in NUnit 4+
                //Assert.<LotsOfMethods>(<captured>,<optional fail message>);
                //Assert.IsTrue(headingText.Contains("Access and authentication"), "Not on login page");

                //NUnit "Constraint model" assertion
                //Better code readability, better reporting
                //Assert.That(<captured value>,<meets some constraint>,<optional if not error message>);
                Assert.That(bodyText, Does.Contain("User is not logged in"), "Already logged in? Continue rest of test anyway...");
            }
            catch (AssertionException ex)
            {
                //Do nothing on error - Nunit will still report a fail but rest of test runs
                //Note - SpecflowLiving Doc would report the test as a pass as the exception was caught
            }

            //Newer alternatives to try/catch non fatal asserts
            //Reporter support may be spotty - need to check
            //Warn.Unless(bodyText, Does.Contain("User is not logged in"), "Already logged in? Continue rest of test anyway...");
            //Warn.If(1, Is.EqualTo(2), "Basic maths broke");
            //Assert.Warn("Fixed warning");

            Assert.Multiple(() =>
            {
                //Assert.That(1, Is.EqualTo(2));
                //Assert.That(1, Is.EqualTo(3));
            }); //Test stops here after checking *both* asserts.

            //Designed for test precondition checks - will stop and mark test as inconclusive
            //Assume.That(bodyText, Does.Contain("User is not logged in"), "Assumed not logged in but was already logged in? Continue rest of test anyway...");

            //Q: Is % applied to expected tollerance or actual value? A: Actual value can deviate from Expected by 10% of Actual
            //Assert.That(1.0, Is.EqualTo(1.1).Within(10).Percent); //NUnit warning because actual value shouldn't be a constant


            /*
             * Act - perform the login
             */
            //Fill in username and password and submit form
            driver.FindElement(By.CssSelector("#username")).Clear(); //Get rid of any pre-filled in text
            driver.FindElement(By.CssSelector("#username")).SendKeys("edgewords");
            driver.FindElement(By.CssSelector("#username")).SendKeys(Keys.Control + "a"); //These two lines also clear the text box
            driver.FindElement(By.CssSelector("#username")).SendKeys(Keys.Backspace); //but do so in a way that a "real" user would. Unlike .Clear().
            driver.FindElement(By.CssSelector("#username")).SendKeys("edgewords");

            string usernameText = driver.FindElement(By.CssSelector("#username")).Text; //input element is "empty/void" no closing tag so no inner text to capture
            Console.WriteLine("The typed username text is " + usernameText);
            string usernameValue = driver.FindElement(By.CssSelector("#username")).GetAttribute("value"); //Use .getAttribute("value"); instead
            Console.WriteLine("The typed text is actually : " + usernameValue);

            //For multiple user interactions with an element it may eb worth capturing a reference to the element...
            IWebElement usernameField = driver.FindElement(By.CssSelector("#password"));
            usernameField.Clear(); //...and re-using that element reference rather than doing 
            usernameField.SendKeys("edgewords123"); //multiple searches.

            ////But beware of elements going "stale"
            ////Provoking a stale element exception
            //driver.Navigate().Back();
            //driver.FindElement(By.LinkText("Login To Restricted Area")).Click(); //Force the login page to "reload"
            ////Without re-finding the element, reusing usernameField will die with a StaleElementException
            ////usernameField = driver.FindElement(By.CssSelector("#password"));
            //usernameField.SendKeys("edgewords123");

            //Click Submit "button"
            driver.FindElement(By.LinkText("Submit")).Click(); //(It's not actually a button)

            //Need to wait to ensure the next page has time to load
            //Explicit Unconditional wait
            //Thread.Sleep(7000); //Needs about 5 seconds. Replace with a conditional wait.
            //Explicit Conditional wait
            myHelper.WaitForElement(By.LinkText("Log Out"), 4);
            //Execution continues early if wait is satisfied


            /*
             * Assert - Are we actually logged in?
             */

            bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.That(bodyText, Does.Contain("User is logged in").IgnoreCase, "Not logged in - Test Failed");

            /*
             * If this is just a login test we're probably done here and should finish
             */


            //Now try to log out
            driver.FindElement(By.LinkText("Log Out")).Click();
            //logoutlink.Click();

            //Must handle JS alert if present.
            driver.SwitchTo().Alert().Accept();

            myHelper.WaitForElement(By.LinkText("Login"), 10);

            Console.WriteLine("Finish test"); //If we got here - congrats we're done!
        }



        [Test, Order(2), Category("Smoke")] //Another test - [SetUp] will run before to provide a browser, [TearDown] will run after to clean up.
        public void DragDropDemo()
        {
            driver.Manage().Window.Maximize(); //XBrowser way of maximising the window
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/docs/cssXPath.html";
            //Instance of HelperLib class
            //HelperLib myHelper = new HelperLib(driver);
            //myHelper.WaitForElement(By.CssSelector("#slider > a"));

            //vs.. Static method
            StaticWaitForElement(driver, By.CssSelector("#slider > a"), 2);

            IWebElement gripper = driver.FindElement(By.CssSelector("#slider > a"));

            Actions action = new Actions(driver);
            IAction dragDropAction = action //.ScrollToElement(gripper) //Sadly this scroll only works in chromium based browsers right now
                                            .ClickAndHold(gripper)
                                            .MoveByOffset(10, 0) //If the movement is too big the apple wont resize in Chrom(ium) browsers
                                            .MoveByOffset(10, 0) //So do little jumps like Fx does.
                                            .MoveByOffset(10, 0)
                                            .MoveByOffset(10, 0)
                                            .Pause(TimeSpan.FromSeconds(1)) //We can pause mid chain in C# now ! 
                                            .MoveByOffset(10, 0)
                                            .MoveByOffset(10, 0)
                                            .MoveByOffset(10, 0)
                                            .MoveByOffset(10, 0)
                                            .MoveByOffset(10, 0)
                                            .Release() //Remember to release the left mouse button
                                            .Build(); //Seems to be optional in the C# bindings
            //Scroll page to see the action in a X-Browser friendly way.
            IWebElement footer = driver.FindElement(By.Id("footer"));
            IJavaScriptExecutor? jsdriver = driver as IJavaScriptExecutor; //Historically not all drivers could execute JS, so there is a need to cast a capable drievr to a type that can run JS.
            jsdriver?.ExecuteScript("arguments[0].scrollIntoView()", footer); //footer is the 0th argument passed in


            //Perform the drag drop chain
            dragDropAction.Perform();
        }

        [Test, Order(3)]
        public void CustomWaitDemoTest()
        {
            Console.WriteLine("Start test"); //Report key events in the test

            
            driver.Url = "https://www.edgewordstraining.co.uk/webdriver2/sdocs/auth.php";
            driver.FindElement(By.Id("username")).SendKeys("edgewords");
            driver.FindElement(By.Id("password")).SendKeys("edgewords123");
            driver.FindElement(By.LinkText("Submit")).Click();
            //Wait for the log out link to:
            //a)Be in the DOM
            //b)Be visible on screen AND interactable (i.e. not covered by something else)
            //ALSO - Return the WebElement when found

            WebDriverWait myWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //Lambda expression will retry until it no longer throws NoSuchElement, or returns null or false (or until timeout reached)
            IWebElement? logout = myWait.Until(drv =>
            { 
                try
                {
                    IWebElement logOutLink = drv.FindElement(By.LinkText("Log Out")); //If this throws NoSuchElement wait will retry
                    if (logOutLink != null && logOutLink.Enabled)
                    {
                        return logOutLink; //Link found in DOM. Also enabled (therefore visible and clickable) - return the element.
                    } 
                    else
                    {
                        return null; //retry wait
                    }
                } 
                catch (StaleElementReferenceException) 
                {
                    //logOutLink was found in the DOM, but during check of visibility and interactability with .Enabled it went stale
                    return null; //retry wait
                }
            }); //That's a lot of code for something you might want to generally do often (find interactable elements) - this should probably be a general helper function


            logout?.Click();
            Thread.Sleep(4000); //You should see the confirm alert

        }

    }
}
