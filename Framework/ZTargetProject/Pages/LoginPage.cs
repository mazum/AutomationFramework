using OpenQA.Selenium;

namespace Framework.ZTargetProject.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
            Driver = driver;
            WaitForTitle("Welcome");
        }

        private By usernameTextbox = By.Id("username");
        private By passwordTextbox = By.Id("password");
        private By loginButton = By.CssSelector("input.button.fn_validate_submit[type='submit']");
        private By registerLink = By.XPath("//a[contains(.,'Register')]");
        private By errorMessage = By.XPath("//div[@class='error-summary']");
        private By forgotUsernameLink = By.PartialLinkText("Forgot your username");
        private By forgotPasswordLink = By.PartialLinkText("Forgot your password");
        private By successMessage = By.XPath("//div[@class='success-summary']");


        public OverviewPage Login(string username, string password)
        {
            PerformLoginSteps(username, password);
            return new OverviewPage(Driver);
        }

        public YourAccountsPage LoginAsMultiAccountMember(string username, string password)
        {
            PerformLoginSteps(username, password);
            return new YourAccountsPage(Driver);
        }

        public LoginPage LoginWithInvalidCredentials(string username, string password)
        {
            PerformLoginSteps(username, password);
            return new LoginPage(Driver);
        }

        public ErrorPage LoginCausingGeneralError(string username, string password)
        {
            PerformLoginSteps(username, password);
            return new ErrorPage(Driver);
        }

        public RegistrationPage GoToRegistrationPage()
        {
            WaitUntilElementIsClickable(registerLink).Click();
            return new RegistrationPage(Driver);
        }

        public string GetErrorMessage()
        {
            return FindElement(errorMessage).Text;
        }

        public string GetSuccessMessage()
        {
            return FindElement(successMessage).Text;
        }

        public ForgotUsernamePage GoToForgotUsernamePage()
        {
            WaitUntilElementIsVisible(forgotUsernameLink).Click();
            return new ForgotUsernamePage(Driver);
        }

        public ForgotPasswordPage GoToForgotPasswordPage()
        {
            FindElement(forgotPasswordLink).Click();
            return new ForgotPasswordPage(Driver);
        }

        private void PerformLoginSteps(string username, string password)
        {
            FindElement(usernameTextbox).SendKeys(username);
            FindElement(passwordTextbox).SendKeys(password);
            FindElement(loginButton).Click();
        }

        public ChangePasswordPage LoginWithTempPassword(string username, string password)
        {
            PerformLoginSteps(username, password);
            return new ChangePasswordPage(Driver);
        }
    }
}