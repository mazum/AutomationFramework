using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using TechTalk.SpecFlow;
using Framework.Extensions;

namespace Framework.Utils
{
    public static class DriverFactory
    {
        public static IWebDriver Driver { get; private set; }
        private static bool isMobile;

        public static void StartDriver(string browser, string gridUrl)
        {
            if (Driver == null)
            {
                CreateNewDriverInstance(browser, gridUrl);
            }
        }

        private static void CreateNewDriverInstance(string browser, string gridUrl)
        {
            switch (browser.ToLower())
            {
                case "ie":
                    //configure node to run IE
                    var ieOptions = new InternetExplorerOptions
                    {
                        PageLoadStrategy = InternetExplorerPageLoadStrategy.Eager,
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        UnexpectedAlertBehavior = InternetExplorerUnexpectedAlertBehavior.Accept
                    };
                    if (gridUrl.IsNullOrEmpty())
                        Driver = new InternetExplorerDriver(ieOptions);
                    else
                        Driver = new RemoteWebDriver(new Uri(gridUrl), ieOptions.ToCapabilities(), TimeSpan.FromSeconds(240));
                    break;
                case "firefox":
                    //configure node to run FF
                    if (gridUrl.IsNullOrEmpty())
                        Driver = new FirefoxDriver();
                    else
                        Driver = new RemoteWebDriver(new Uri(gridUrl), DesiredCapabilities.Firefox(), TimeSpan.FromSeconds(240));
                    break;
                case "chrome":
                    var options = new ChromeOptions();
                    options.AddArgument("--use-gl=desktop"); // needed to get rid of 'ANGLE Display' error
                    // options.AddArgument("--disable-extensions");
                    options.AddArguments("disable-infobars");
                    options.AddUserProfilePreference("credentials_enable_service", false);
                    options.AddUserProfilePreference("password_manager_enabled", false);
                    options.AddUserProfilePreference("download.prompt_for_download", false);
                    var capabilities = (DesiredCapabilities)options.ToCapabilities();
                    capabilities.SetCapability(CapabilityType.Platform, "WINDOWS");
                    capabilities.SetCapability(CapabilityType.IsJavaScriptEnabled, true);
                    if (gridUrl.IsNullOrEmpty())
                        Driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromSeconds(240));
                    //Driver = new ChromeDriver(options);
                    else
                        Driver = new RemoteWebDriver(new Uri(gridUrl), capabilities, TimeSpan.FromSeconds(240));
                    break;
                case "mobileemulation":
                    var mobileOptions = new ChromeOptions();
                    mobileOptions.EnableMobileEmulation("iPhone 6");
                    mobileOptions.AddArguments("disable-infobars");
                    mobileOptions.AddUserProfilePreference("credentials_enable_service", false);
                    mobileOptions.AddUserProfilePreference("password_manager_enabled", false);
                    mobileOptions.AddUserProfilePreference("download.prompt_for_download", false);
                    var mobileCapabilities = (DesiredCapabilities)mobileOptions.ToCapabilities();
                    if (gridUrl.IsNullOrEmpty())
                        Driver = new ChromeDriver(mobileOptions);
                    else
                        Driver = new RemoteWebDriver(new Uri(gridUrl), mobileCapabilities, TimeSpan.FromSeconds(240));
                    isMobile = true;
                    break;
                case "phantom":
                    var phantomOptions = new PhantomJSOptions();
                    PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService();
                    service.IgnoreSslErrors = true;
                    // phantomOptions.AddAdditionalCapability("phantomjs.page.settings.resourceTimeout", "1200");
                    Driver = new PhantomJSDriver(service);
                    //Driver.Manage().Window.Size = new Size(337, 667);
                    //isMobile = true;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (!isMobile)
                Driver.Manage().Window.Maximize();
            Driver.Manage().Cookies.DeleteAllCookies();

            Console.WriteLine("Browser: {0}, version {1}; Driver: {2}", browser, ((RemoteWebDriver)Driver).Capabilities.Version, Driver);
            Console.WriteLine("Session Id: " + ((RemoteWebDriver)Driver).SessionId);
        }

        public static void QuitDriver()
        {
            if (ScenarioContext.Current.TestError != null)
            {
                Utilities.PrintCookies(Driver);
                TakeScreenshot(Driver);
            }
            Driver.Close();
            Driver.Quit();
            Driver = null;
        }

        private static void TakeScreenshot(IWebDriver driver)
        {
            var id = (FeatureContext.Current.FeatureInfo.Title + "-" + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ScenarioContext.Current.ScenarioInfo.Title)).ReplaceRegExMatch(@"[\s-_/]+", "");
            var fileNameBase = string.Format("error_{0}_{1:yyyyMMdd_HHmmssfff}", id, DateTime.Now);
            //var artifactDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
            var artifactBaseDirectory = AppSettings.Current.ScreenshotFolder;
            if (!Directory.Exists(artifactBaseDirectory))
                Directory.CreateDirectory(artifactBaseDirectory);

            var artifactDirectory = Path.Combine(artifactBaseDirectory, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(artifactDirectory))
                Directory.CreateDirectory(artifactDirectory);

            //string pageSource = driver.PageSource;
            //string sourceFilePath = Path.Combine(artifactDirectory,fileNameBase + "_source.html");
            //File.WriteAllText(sourceFilePath, pageSource, Encoding.UTF8);
            //Console.WriteLine("Page source: {0}", new Uri(sourceFilePath));

            var takesScreenshot = driver as ITakesScreenshot;
            if (takesScreenshot != null)
            {
                var screenshot = takesScreenshot.GetScreenshot();
                var screenshotFilePath = Path.Combine(artifactDirectory, fileNameBase + "_screenshot.png");
                try
                {
                    screenshot.SaveAsFile(screenshotFilePath, ImageFormat.Png);
                }
                catch (Exception e)
                {
                    Console.WriteLine("DBG: Saving the screenshot failed. " + e.Message);
                }
                Console.WriteLine("Screenshot:\n{0}", new Uri(screenshotFilePath));
                Console.WriteLine(screenshotFilePath);
            }
        }

    }
}