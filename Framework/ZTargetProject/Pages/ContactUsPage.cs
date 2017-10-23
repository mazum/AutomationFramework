using OpenQA.Selenium;

namespace Framework.ZTargetProject.Pages
{
    public class ContacUsPage : BaseMemberPage, IHasEnquirySection
    {
        public ContacUsPage(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
            WaitForTitle("Contact us");
        }

        private By enquiryConfirmationMessage = By.CssSelector("div.l-content > div > h2");

        public string GetEnquiryConfirmation()
        {
            return WaitUntilElementIsVisible(enquiryConfirmationMessage).Text;
        }
        public EnquirySection GetEnquirySection()
        {
            return new EnquirySection(Driver);
        }
    }
}