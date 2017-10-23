using OpenQA.Selenium;

namespace Framework.ZTargetProject.Pages
{
    public class ForgotPasswordPage : BasePage
    {
        public ForgotPasswordPage(IWebDriver driver)
            : base(driver)
        {
            this.Driver = driver;
            WaitForTitle("Forgot password");
        }

        private By usernameTextBox = By.Id("UserName");
        private By retrievalMethodRadioButtonGroup = By.Name("RetrievalMethod");
        private By retrievePasswordButton = By.XPath("//*[@id=\"main\"]/div[2]/div/div[1]/form/p/a[1]");
        private By errorMessage = By.CssSelector(".error-summary > p:nth-child(1)");
        private By emailTextBox = By.Id("EmailValue");
        private By phoneTextBox = By.Id("PhoneValue");

        public void EnterForgotPasswordDetails(string username, string type, string email, string phone)
        {
            var text = type == "Email" ? email : phone;
            FindRadioButtonGroup(retrievalMethodRadioButtonGroup).SelectRadioButtonClickLabel(type);
            if (FindRadioButtonGroup(retrievalMethodRadioButtonGroup).GetSelectedRadioButtonValue().Equals("Email"))
            {
                WaitUntilElementIsClickable(emailTextBox).SendKeys(text);
            }
            else
            {
                WaitUntilElementIsClickable(phoneTextBox).SendKeys(text);
            }
            FindElement(usernameTextBox).SendKeys(username);
            FindElement(retrievePasswordButton).Click();
        }

        public string GetErrorMessage()
        {
            return WaitUntilElementIsVisible(errorMessage).Text;
        }

    }
}