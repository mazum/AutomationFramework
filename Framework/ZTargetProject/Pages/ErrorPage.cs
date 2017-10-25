using OpenQA.Selenium;

namespace Framework.ZTargetProject.Pages
{
    public class ErrorPage : BasePage
    {
        public ErrorPage(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
            WaitForMatchingTitle("(Access Denied|Page not found|www.unisuper.com.au)");
        }
        public string GetPageTitle()
        {
            return Driver.Title;
        }
    }
}