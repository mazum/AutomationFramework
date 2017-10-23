using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Framework.ZTargetProject.Data;

namespace Framework.ZTargetProject.Pages
{
    public class RegistrationPage : BasePage
    {
        public RegistrationPage(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
            WaitForTitle("Registration", 30);
        }

        private By memberNumberTextbox = By.Id("memberNumber");
        private By lastNameTextbox = By.Id("lastName");
        private By dobTextbox = By.Id("dob");
        private By termsAcceptedCheckbox = By.Id("termsAccepted");
        private By nextButton = By.XPath("//button[contains(@class,'button minimal-cta')]");
        private By invalidDetailsErrorMessage = By.XPath("//p[@ng-if='vm.invalidUser']");
        private By alreadyRegisteredMessage = By.XPath("//div[@class='ctrl-holder fade']");
        private By usernameTextbox = By.Id("username");
        private By submitButton = By.XPath("//button[contains(.,'Submit')]");
        private By tempPasswordTextbox = By.Id("password");
        private By loginButton = By.XPath("//input[@type='submit']");
        private By confirmationMessage = By.XPath("//div[@class='l-content-column main no-top-pad']");
        private By contactEmailLabel = By.XPath("//label[contains(.,'@unisuper.com.au')] | //p[contains(@class,'_email')]");
        private By indexedPensionerMessageText = By.CssSelector("form[name='registerForm'] > div");
        private By waitOverlay = By.CssSelector("div.wait");

        public RegistrationPage EnterRegistrationDetails(string memberNumber, MemberPersonalDetails personalDetails)
        {
            EnterRegistrationDetailsPart1(memberNumber, personalDetails);
            return new RegistrationPage(Driver);
        }

        public LoginPage EnterRegistrationDetailsWhenAlreadyRegistered(string memberNumber, MemberPersonalDetails personalDetails)
        {
            EnterRegistrationDetailsPart1(memberNumber, personalDetails);
            return new LoginPage(Driver);
        }

        public string GetRegistrationDetailsError()
        {
            return WaitUntilElementHasMatchingText(invalidDetailsErrorMessage, @"\w+").Text;
        }

        public string GetAlreadyRegisteredMessage()
        {
            WaitUntilElementIsNotDisplayed(nextButton);
            return FindElement(alreadyRegisteredMessage).Text;
        }

        public RegistrationPage EnterRegistrationDetailsPart2(string username)
        {
            WaitUntilElementIsVisible(usernameTextbox).SendKeys(username);
            WaitUntilElementIsVisible(waitOverlay); // this might take a second before showing up
            WaitUntilElementIsNotDisplayed(waitOverlay, 60);
            FindElement(contactEmailLabel).Click();
            WaitUntilElementIsClickable(submitButton).Click();
            WaitUntilElementIsNotDisplayed(submitButton);
            return new RegistrationPage(Driver);
        }

        public string GetConfirmationMessage()
        {
            return FindElement(confirmationMessage).Text;
        }

        public ChangePasswordPage EnterTempPassword(string tempPassword)
        {
            FindElement(tempPasswordTextbox).SendKeys(tempPassword);
            FindElement(loginButton).Click();
            return new ChangePasswordPage(Driver);
        }

        public string GetIndexedPensionerMessageText()
        {
            return
                WaitUntilElementHasMatchingText(indexedPensionerMessageText, "As a Defined Benefit Indexed Pension *.")
                    .Text;
        }

        private void EnterRegistrationDetailsPart1(string memberNumber, MemberPersonalDetails personalDetails)
        {
            FindElement(memberNumberTextbox).SendKeys(memberNumber);
            FindElement(lastNameTextbox).SendKeys(personalDetails.Surname);
            FindElement(dobTextbox).SendKeys(personalDetails.DateOfBirth);
            FindElement(termsAcceptedCheckbox).Click();
            WaitUntilElementIsClickable(nextButton).Click();
        }
    }
}