using OpenQA.Selenium;

namespace Framework.ZTargetProject.Pages
{
    public class ForgotUsernamePage : BasePage
    {
        public ForgotUsernamePage(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
            WaitForTitle("Forgotten username");
        }

        private By memberNumberTextBox = By.Id("MemberNumber");
        private By retrievalMethodRadioButtonGroup = By.Name("RetrievalMethod");
        private By retrieveUsernameButton = By.XPath("//a[@class='fn_validate_submit primary-cta button']");
        private By errorMessage = By.CssSelector(".error-summary > p:nth-child(1)");
        private By phoneTextBox = By.Id("PhoneValue");
        private By emailTextBox = By.Id("EmailValue");

        public void EnterForgotUsernameDetails(string memberNumber, string type, string email, string phone)
        {
            var text = type == "Email" ? email : phone;
            FindElement(memberNumberTextBox).SendKeys(memberNumber);
            FindRadioButtonGroup(retrievalMethodRadioButtonGroup).SelectRadioButtonClickLabel(type);
            if (FindRadioButtonGroup(retrievalMethodRadioButtonGroup).GetSelectedRadioButtonValue() == "Email")
            {
                WaitUntilElementIsClickable(emailTextBox).SendKeys(text);
            }
            else
            {
                WaitUntilElementIsClickable(phoneTextBox).SendKeys(text);
            }
            FindElement(retrieveUsernameButton).Click();
        }

        public string GetErrorMessage()
        {
            return WaitUntilElementIsVisible(errorMessage).Text;
        }

    }
}