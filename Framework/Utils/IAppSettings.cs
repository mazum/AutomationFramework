namespace Framework.Utils
{
    public interface IAppSettings
    {
        string Browser { get; set; }
        string BrowserUrl { get; set; }
        int ImplicitWaitTimeoutSeconds { get; set; }
        int PageLoadTimeoutSeconds { get; set; }
        int PageTitleWaitSeconds { get; set; }
        string GridUrl { get; set; }
        bool EnableCookieInfoLogging { get; set; }
        string ScreenshotFolder { get; set; }
        bool IgnoreProductionIssues { get; set; }
    }
}