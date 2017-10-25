using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using Framework.ZTargetProject.Data;
using Framework.Extensions;
using Framework.Utils;

namespace Framework.ZTargetProject.Pages
{
    public class ChangePasswordPage : BaseMemberPage
    {
        public ChangePasswordPage(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
            WaitForTitle("Change password");
        }

        private By existingPasswordTextbox = By.Id("ExistingPassword");
        private By newPasswordTextbox = By.Id("NewPassword");
        private By confirmPasswordTextbox = By.Id("ConfirmPassword");
        private By passwordRequirementsContainer = By.XPath("//div[contains(@class,'update-password-validation')]");
        private By passwordRequirementsLabel = By.CssSelector("li.validation-errors__message.ng-scope");
        private By passwordMeetsRequirementsMessage = By.XPath("//li[contains(@class,'message -success')]");
        private By changePasswordButton = By.CssSelector(".primary-cta.button.update-password-actions__submit");
        private By existingPasswordInlineMessage = By.CssSelector("#ExistingPassword_error span");
        private By newPasswordInlineMessage = By.CssSelector("#NewPassword_error span");
        private By confirmPasswordInlineMessage = By.CssSelector("#ConfirmPassword_error span");
        private By errorMessage = By.CssSelector(".error-summary li");
        private By cancelButton = By.CssSelector("a.button.secondary-cta");
        private By passwordChangeSuccessMessage = By.CssSelector("div.success-summary");

        public IList<string> GetPasswordRequirements()
        {
            WaitUntilElementHasFinalSize(passwordRequirementsContainer, 2000, 20);
            WaitUntilElementsExist(passwordRequirementsLabel, 30); // for sometimes slow common password check
            return FindElements(passwordRequirementsLabel).GetTextFromElements();
        }

        public ChangePasswordPage EnterRequiredFields(ChangePasswordDetails pwdDetails)
        {
            FindElement(existingPasswordTextbox).SendKeys(pwdDetails.ExistingPassword);
            FindElement(newPasswordTextbox).SendKeys(pwdDetails.NewPassword);
            FindElement(confirmPasswordTextbox).SendKeys(pwdDetails.ConfirmPassword);
            return this;
        }

        public ChangePasswordPage RemoveJsValidation()
        {
            var js = Driver as IJavaScriptExecutor;
            js.ExecuteScript("$('button.primary-cta.button.update-password-actions__submit').removeClass('button.primarycta.button.update-password-actions__submit disabled')");
            js.ExecuteScript("$('form').removeClass('form fn_validate fn_validate_container')");
            return this;
        }

        public ChangePasswordPage CancelChangePassword()
        {
            FindElement(cancelButton).Click();
            return this;
        }

        public ChangePasswordPage Submit()
        {
            WaitUntilElementIsClickable(changePasswordButton).Click();
            return new ChangePasswordPage(Driver);
        }

        public OverviewPage SubmitWithRedirectForSingleAccount()
        {
            WaitUntilElementIsClickable(changePasswordButton).Click();
            return new OverviewPage(Driver);
        }

        public YourAccountsPage SubmitWithRedirectForMultiAccounts()
        {
            WaitUntilElementIsClickable(changePasswordButton).Click();
            return new YourAccountsPage(Driver);
        }

        public ChangePasswordPage SubmitWithError()
        {
            WaitUntilElementIsClickable(changePasswordButton).Click();
            return this;
        }

        public string GetErrorMessage()
        {
            return WaitUntilElementIsVisible(errorMessage).Text;
        }

        public ChangePasswordPage SubmitAttemptWithDisabledButton()
        {
            FindElement(changePasswordButton).Click();
            return new ChangePasswordPage(Driver);
        }

        public string GetPasswordRequirementsMetConfirmation()
        {
            return WaitUntilElementIsVisible(passwordMeetsRequirementsMessage).Text;
        }

        public string GetPasswordChangeSuccessMessage()
        {
            return FindElement(passwordChangeSuccessMessage).Text;
        }
    }
}