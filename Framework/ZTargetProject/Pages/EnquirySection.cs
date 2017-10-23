using OpenQA.Selenium;

namespace Framework.ZTargetProject.Pages
{
    public class EnquirySection : BaseMemberPage
    {
        public EnquirySection(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
            WaitForMatchingTitle("(Contact us|Financial advice)");
        }

        private By enquiryTypeDropdown = By.Id("EnquiryType");
        private By existingEmailLabel = By.XPath("(//label[contains(.,'Email ')])[1]");
        private By existingPhoneLabel = By.XPath("(//label[contains(.,'Phone ')])[1]");
        private By preferredCallTime1Label = By.XPath("(//input[contains(@name,'PreferredCallTimes')])[1]");
        private By enquiryMessageTextArea = By.Id("EnquiryMessage");
        private By submitButton = By.XPath("//button[contains(@class,'cta primary-cta button fn_validate_submit')]");
        private By specifyPhoneLabel = By.XPath("//label[contains(.,'Specify phone')]");
        private By specifyEmailLabel = By.XPath("//label[contains(.,'Specify email')]");
        private By newEmailTextbox = By.Id("OtherEmail");
        private By newPhoneTextbox = By.Id("OtherPhone");
        private By enquirySubTypeDropdown = By.Id("EnquirySubType");
        private By contactOptionsRadioGroup = By.Id("ContactMethod");

        public EnquirySection SetEnquiryType(string enquiryType)
        {
            FindSelectElement(enquiryTypeDropdown).SelectByText(enquiryType);
            return this;
        }

        public EnquirySection SetEnquirySubType(string enquirySubType)
        {
            FindSelectElement(enquirySubTypeDropdown).SelectByText(enquirySubType);
            return this;
        }

        public EnquirySection SelectWithEmailContact()
        {
            FindElement(existingEmailLabel).Click();
            return this;
        }

        public EnquirySection SelectPhoneContact()
        {
            FindElement(existingPhoneLabel).Click();
            FindElement(preferredCallTime1Label).Click();
            return this;
        }

        public EnquirySection EnterNewPhoneContact(string newPhone)
        {
            FindElement(specifyPhoneLabel).Click();
            FindElement(newPhoneTextbox).SendKeys(newPhone);
            FindElement(preferredCallTime1Label).Click();
            return this;
        }

        public EnquirySection EnterNewEmailContact(string newEmail)
        {
            FindElement(specifyEmailLabel).Click();
            FindElement(newEmailTextbox).SendKeys(newEmail);
            return this;
        }

        public EnquirySection EnterMessage(string enquiryMessage)
        {
            FindElement(enquiryMessageTextArea).SendKeys(enquiryMessage);
            return this;
        }

        public ContacUsPage Submit()
        {
            FindElement(submitButton).Click();
            return new ContacUsPage(Driver);
        }

        public string GetContactOptionsText()
        {
            return FindElement(contactOptionsRadioGroup).Text;
        }
    }
}