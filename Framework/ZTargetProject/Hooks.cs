using System;
using TechTalk.SpecFlow;
using Framework.Utils;

namespace Framework.ZTargetProject
{
    [Binding]
    public class Hooks
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            
        }

        [BeforeScenario]
        public static void BeforeScenario()
        {
            DriverFactory.StartDriver(AppSettings.Current.Browser, AppSettings.Current.GridUrl);
            DriverFactory.Driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(AppSettings.Current.ImplicitWaitTimeoutSeconds);
            DriverFactory.Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(AppSettings.Current.PageLoadTimeoutSeconds);
            DriverFactory.Driver.Url = AppSettings.Current.BrowserUrl;
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            if (DriverFactory.Driver != null)
                DriverFactory.QuitDriver();
        }
    }
}