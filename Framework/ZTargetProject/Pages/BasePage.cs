using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading;
using AutoIt;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using Framework.Extensions;
using Framework.Utils;
using Framework.WebElements;

namespace Framework.ZTargetProject.Pages
{
    public class BasePage
    {
        protected IWebDriver Driver;
        private int pageTitleWaitSeconds;
        private const int DefaultWaitSeconds = 30;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
            pageTitleWaitSeconds = AppSettings.Current.PageTitleWaitSeconds;
        }
        protected IWebElement FindElement(By locator)
        {
            return Driver.FindElement(locator);
        }
        protected IList<IWebElement> FindElements(By locator)
        {
            return Driver.FindElements(locator);
        }
        protected SelectElement FindSelectElement(By locator)
        {
            return new SelectElement(FindElement(locator));
        }
        protected RadioButtonGroup FindRadioButtonGroup(By radioButtonGroupLocator)
        {
            return new RadioButtonGroup(FindElements(radioButtonGroupLocator));
        }
        protected WebTable FindTable(By tableLocator, string rowXPathAttribute = "")
        {
            return new WebTable(FindElement(tableLocator), rowXPathAttribute);
        }
        protected Checkbox FindCheckbox(By locator)
        {
            return new Checkbox(FindElement(locator));
        }
        protected void WaitForTitle(string title, double timeoutSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Expected page title to contain '" + title + "', Actual page title: '" + Driver.Title + "'";
            wait.Until(ExpectedConditions.TitleContains(title));
        }
        protected void WaitForTitle(string title)
        {
            WaitForTitle(title, pageTitleWaitSeconds);
        }
        protected void WaitForMatchingTitle(string titlePattern, double timeoutSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Expected page title to match '" + titlePattern + "', Actual page title: '" + Driver.Title + "'";
            wait.Until(d => d.Title.IsRegExMatch(titlePattern));
        }
        protected void WaitForMatchingTitle(string titlePattern)
        {
            WaitForMatchingTitle(titlePattern, pageTitleWaitSeconds);
        }
        protected IWebElement WaitUntilElementIsVisible(By locator, int timeoutSeconds = DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Element " + locator + " did not become visible before timeout.";
            wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return FindElement(locator);
        }
        protected IWebElement WaitUntilElementIsClickable(By locator, int timeoutSeconds = DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Element " + locator + " did not become clickable before timeout.";
            wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            return FindElement(locator);
        }
        /// <summary>
        /// Function used to check if item in list both exists and is clickable/enabled
        /// </summary>
        /// <param name="locator">The by locator for the elements which form the list, not the list itself</param>
        /// <param name="index">Index in the list you are checking</param>
        /// <param name="timeoutSeconds">The maximum wait time for the element to become clickable</param>
        /// <returns></returns>
        protected IWebElement WaitUntilListElementIsClickable(By locator, int
            index = 0, int timeoutSeconds = DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds
                (timeoutSeconds));
            wait.Message = "Element " + locator + " did not become clickable before timeout.";
            var elements = FindElements(locator);
            if (!(elements.Count > index))
            {
                throw new ElementNotVisibleException("Element not found in list");
            }
            wait.Until(ExpectedConditions.ElementToBeClickable(elements[index]));
            return elements[index];
        }
        protected IWebElement WaitUntilElementHasText(By locator, string text, int timeoutSeconds = DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Element " + locator + " did not become clickable before timeout.";
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(locator, text));
            return FindElement(locator);
        }
        protected IWebElement WaitUntilElementHasMatchingText(By locator, string regExPattern, int timeoutSeconds = DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Element " + locator + " did not have text matching '" + regExPattern + "' before timeout.";
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(d => Driver.FindElement(locator).Text.IsRegExMatch(regExPattern));
            return FindElement(locator);
        }
        protected IWebElement WaitUntilElementHasChangedText(By locator, string oldText, int timeoutSeconds = DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Element " + locator + " did not change text before timeout.";
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(d => !Driver.FindElement(locator).Text.Equals(oldText));
            return FindElement(locator);
        }
        protected void WaitUntilElementIsNotDisplayed(By locator, int timeoutSeconds = DefaultWaitSeconds)
        {
            SetImplicitWaitTimeout(0);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds
                (timeoutSeconds));
            wait.Message = "Element " + locator + " was still visible at timeout.";
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated
                (locator));
            RestoreImplicitWaitTimeout();
        }
        protected IList<IWebElement> WaitUntilElementsExist(By locator, int timeoutSeconds = DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Element " + locator + " did not become visible before timeout.";
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(d => d.FindElements(locator).Count > 0);
            return FindElements(locator);
        }
        protected IAlert WaitUntilAlertIsPresent(int timeoutSeconds = DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Alert did not pop up before timeout.";
            wait.Until(ExpectedConditions.AlertIsPresent());
            return Driver.SwitchTo().Alert();
        }
        protected IWebElement WaitUntilElementHasFinalSize(By locator, int checkIntervalMilliSeconds = 500, int timeoutSeconds = 10)
        {
            var lastSize = FindElement(locator).Size;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Element " + locator + " did not become stable before timeout.";
            wait.Until(d =>
            {
                Thread.Sleep(checkIntervalMilliSeconds);
                var currentSize = d.FindElement(locator).Size;
                if (currentSize.Equals(lastSize))
                    return true;
                lastSize = currentSize;
                return false;
            });
            return FindElement(locator);
        }
        protected IWebElement WaitUntilElementHasFinalLocation(By locator, int checkIntervalMilliSeconds = 500, int timeoutSeconds = 10)
        {
            var lastPosition = FindElement(locator).Location;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Message = "Element " + locator + " did not become stable before timeout.";
            wait.Until(d =>
            {
                Thread.Sleep(checkIntervalMilliSeconds);
                var currentPosition = FindElement(locator).Location;
                if (currentPosition.Equals(lastPosition))
                    return true;
                lastPosition = currentPosition;
                return false;
            });
            return FindElement(locator);
        }
        protected bool IsElementVisible(By locator, int timeoutSeconds = 10)
        {
            SetImplicitWaitTimeout(0);
            try
            {
                WaitUntilElementIsVisible(locator, timeoutSeconds);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                RestoreImplicitWaitTimeout();
            }
        }
        protected bool IsElementClickable(By locator, int timeoutSeconds = 10)
        {
            SetImplicitWaitTimeout(0);
            try
            {
                WaitUntilElementIsClickable(locator, timeoutSeconds);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                RestoreImplicitWaitTimeout();
            }
        }
        protected bool DoesElementExist(By locator)
        {
            return FindElements(locator).Count > 0;
        }
        protected void ScrollToTop()
        {
            new Actions(Driver).SendKeys(Keys.Home).Perform();
            Thread.Sleep(500);
        }
        protected void ScrollToBottom()
        {
            new Actions(Driver).SendKeys(Keys.End).Perform();
            Thread.Sleep(500);
        }
        protected Point ScrollIntoView(By locator)
        {
            var p = ((ILocatable)FindElement(locator)).LocationOnScreenOnceScrolledIntoView;
            if (IsMobile())
            {
                ScrollVertically(-100); // Another element at the top is hiding the element of interest.so we scroll up a little bit, which moves our element into view.
                // Once RMOL-313 is fixed the scrolling can most likely be removed.
                p.Y = p.Y - 100;
            }
            return p;
        }
        protected Point ScrollIntoView(IWebElement element)
        {
            var p = ((ILocatable)element).LocationOnScreenOnceScrolledIntoView;
            if (IsMobile())
            {
                ScrollVertically(-100); // see cooment in ScrollIntoView(Bylocator)
                p.Y = p.Y - 100;
            }
            return p;
        }
        protected void ScrollVertically(int yOffset)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollBy(0," + yOffset + ")");
            Thread.Sleep(500);
        }
        protected void SetImplicitWaitTimeout(int seconds)
        {
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(seconds));
        }
        protected void RestoreImplicitWaitTimeout()
        {
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(AppSettings.Current.ImplicitWaitTimeoutSeconds));
        }
        public void Refresh()
        {
            Driver.Navigate().Refresh();
        }
        public bool IsFileDownloadable(string url, string contentType = "application/pdf")
        {
            bool isDownloadable = false;
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Accept = contentType;
            var getcookies = Driver.Manage().Cookies.AllCookies;
            CookieContainer container = new CookieContainer();
            foreach (var a in getcookies)
            {
                if (a.Value.IsNotNullOrEmpty())
                {
                    var c = new System.Net.Cookie
                    {
                        Name = a.Name,
                        Value = a.Value,
                        Domain = a.Domain,
                        Path = a.Path
                    };
                    container.Add(c);
                }
            }
            req.CookieContainer = container;
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            if (HttpStatusCode.OK == resp.StatusCode)
                isDownloadable = resp.ContentType.Contains(contentType) && resp.ContentLength > 0;
            resp.Dispose();
            return isDownloadable;
        }

        /*public LoginPage NavigateBackwardsFromLogoutPage()
        {
            Driver.Navigate().Back();
            return new LoginPage(Driver);
        }*/

        public T NavigateBack<T>(T previousPage) where T : BasePage
        {
            Driver.Navigate().Back();
            return Activator.CreateInstance(previousPage.GetType(), Driver) as T;
        }
        public bool IsIE()
        {
            return ((RemoteWebDriver)Driver).Capabilities.BrowserName.Equals("internet explorer");
        }
        public bool IsChrome()
        {
            return ((RemoteWebDriver)Driver).Capabilities.BrowserName.Equals("chrome");
        }
        public bool IsFirefox()
        {
            return ((RemoteWebDriver)Driver).Capabilities.BrowserName.Equals("firefox");
        }
        public bool IsMobile()
        {
            return GetViewportWidth() < 700; // just a rough value for now
        }
        public void SendAutoItKeys(string autoitKeys, string windowTitle)
        {
            Thread.Sleep(2000); // just in case some notification should appear
            AutoItX.WinActivate(windowTitle);
            AutoItX.Send(autoitKeys);
            // see https://www.autoitscript.com/autoit3/docs/functions/Send.htm for more information on keys
        }
        public void DownloadFile(IWebElement downloadElement, string pageTitle)
        {
            downloadElement.Click();
            if (IsIE())
                SendAutoItKeys("!s", pageTitle);
            Thread.Sleep(2000);
        }
        public void PrintCookies()
        {
            Utilities.PrintCookies(Driver);
        }
        private int GetViewportWidth()
        {
            return Convert.ToInt32(((IJavaScriptExecutor)Driver).ExecuteScript("return document.documentElement.clientWidth"));
        }
    }
}