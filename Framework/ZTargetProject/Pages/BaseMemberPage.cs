using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;


namespace Framework.ZTargetProject.Pages
{
    public class BaseMemberPage : BasePage
    {
        public BaseMemberPage(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
        }

        public BaseMemberPage(IWebDriver driver, string expectedPageTitle)
            : base(driver)
        {
            Driver = driver;
            WaitForTitle(expectedPageTitle);
        }

        private By navButton = By.CssSelector("a.nav-toggle");
        private By overviewLink = By.XPath("//div[@class='nav-inner']/ul/li/a[contains(text(),'Overview')]/..");
        private By balancesLink = By.XPath("//a[@href='/balances']");

        private By pensionPaymentsLink =
            By.XPath("//div[@class='nav-inner']/ul/li/a[contains(text(),'Pension payments')]/..");

        private By transactionsLink = By.XPath("//a[@href='/transactions']");
        private By investmentsLink = By.CssSelector("li.primary.has-children a.nav-lvl1.fn-nav-lvl1");
        private By contactUsLink = By.CssSelector("a[href*=contact-us]");
        private By feedbackButton = By.XPath("//span[@class='icon feedback']");
        private By feedbackForm = By.Id("fby-form");
        private By feedbackSendButton = By.CssSelector("div.fbtn-m");
        private By feedbackMobileForm = By.ClassName("fby-form");
        private By feedbackMobileSendButton = By.ClassName("fby-button");
        private By beneficiariesLink = By.XPath("//a[@href='/beneficiaries']");
        private By manageAccountLink = By.XPath("//a[@href='/manage-account']");
        private By statementLink = By.XPath("//a[@href='/benefit-statements']");
        private By insuranceLink = By.XPath("//a[@href='/insurance']");
        private By financialAdviceLink = By.XPath("//a[@href='/financial-advice']");
        private By profile = By.CssSelector("span.icon.nav-icon-profile");
        private By changePasswordLink = By.XPath("//a[@href='/profile/change-password']");
        private By personalDetailsLink = By.XPath("//a[@href='/profile/personal-details']");
        private By resources = By.CssSelector("span.icon.nav-icon-resourcesa");
        private By publicationsLink = By.XPath("//a[@href='/resources/publications']");
        private By calculatorAndToolsLink = By.XPath("//a[@href='/resources/calculators-and-tools']");
        private By seminarsAndWebinarsLink = By.XPath("//a[@href='/resources/seminars-and-webinars']");
        private By investmentsActiveDropdown = By.XPath("//a[@class='nav-lvl1 fn-nav-lvl1 active-dropdown']");
        private By investmentPerformanceLink = By.CssSelector("div.dd-level2-inner a[href*='investment-performance']");
        private By yourInvestmentsLink = By.CssSelector("div.dd-level2-inner a[href*='your-investments']");
        private By investmentHistoryLink = By.XPath("//a[@href='/investment-details/switch-history']");
        private By navList = By.ClassName("fn-nav-list");
        private By introVideoCloseButton = By.CssSelector("a.fancybox-item[title='Close']");
        private By logoutButton = By.XPath("(//a[@href='/logout'])[1]");
        private By sliderTile = By.CssSelector("li.next-enagement-step-item.slide.fn-clickable-container:not(.clone)");
        private By mobileOnlyNextTileButton = By.CssSelector("a.flex-next");
        private By searchTextbox = By.Id("nav_search_input");
        private By switchAccountButton = By.CssSelector("button.account-switch_current");
        private By switchAccountButtonListItem = By.CssSelector("span.switch-account_details_funds");

        private By identityVerificationLink = By.CssSelector("a[href~='/profile/identity-verification']");

        private By identityVerificationManageAccountLink =
            By.CssSelector("a.fn-clickable-link[href~='/profile/identityverification']");


        /*public EVPage GoToEVLeftMenu() //bool alertWillShow = false
        {

            if (!FindElement(identityVerificationLink).Displayed)
                FindElement(profile).Click();
            WaitUntilElementIsClickable(identityVerificationLink).Click();
            //if(alertWillShow)
            // Driver.SwitchTo().Alert().Accept();

            return new EVPage(Driver);
        }

        public EVPage GoToEVManageAccounts()
        {
            GoToManageAccountPage();
            WaitUntilElementIsVisible(identityVerificationManageAccountLink).Click();
            return new EVPage(Driver);
        }*/





        public ChangePasswordPage GoToChangePasswordPage()
        {
            OpenNavMenuForMobile();
            FindElement(profile).Click();
            WaitUntilElementIsVisible(changePasswordLink).Click();
            return new ChangePasswordPage(Driver);
        }

        public void CloseIntroVidBox()
        {
            WaitUntilElementIsClickable(introVideoCloseButton).Click();
            WaitUntilElementIsNotDisplayed(introVideoCloseButton);
        }

        public void OpenFeedbackform()
        {
            WaitUntilElementIsClickable(feedbackButton).Click();
        }

        public bool IsFeedbackFormUsable()
        {
            WaitUntilElementIsVisible(IsMobile() ? feedbackMobileForm : feedbackForm);
            return IsElementClickable(IsMobile() ? feedbackMobileSendButton : feedbackSendButton);
        }

        public OverviewPage GoToOverviewPage()
        {
            OpenNavMenuForMobile();
            FindElement(overviewLink).Click();
            return new OverviewPage(Driver);
        }

        public BalancesPage GoToBalancesPage()
        {
            OpenNavMenuForMobile();
            FindElement(balancesLink).Click();
            return new BalancesPage(Driver);
        }

        /*public PensionPaymentsPage GoToPensionPaymentsPage()
        {
            OpenNavMenuForMobile();
            FindElement(pensionPaymentsLink).Click();
            return new PensionPaymentsPage(Driver);
        }

        public TransactionsPage GoToTransactionsPage()
        {
            OpenNavMenuForMobile();
            FindElement(transactionsLink).Click();
            return new TransactionsPage(Driver);
        }

        public BeneficiariesPage GoToBeneficiariesPage()
        {
            OpenNavMenuForMobile();
            FindElement(beneficiariesLink).Click();
            return new BeneficiariesPage(Driver);
        }

        public ManageAccountPage GoToManageAccountPage()
        {
            OpenNavMenuForMobile();
            FindElement(manageAccountLink).Click();
            return new ManageAccountPage(Driver);
        }

        public StatementsPage GoToStatementsPage()
        {
            OpenNavMenuForMobile();
            FindElement(statementLink).Click();
            return new StatementsPage(Driver);
        }

        public InsurancePage GoToInsurancePage()
        {
            OpenNavMenuForMobile();
            FindElement(insuranceLink).Click();
            return new InsurancePage(Driver);
        }

        public FinancialAdvicePage GoToFinancialAdvicePage()
        {
            OpenNavMenuForMobile();
            FindElement(financialAdviceLink).Click();
            return new FinancialAdvicePage(Driver);
        }

        public PersonalDetailsPage GoToPersonalDetailsPage()
        {
            OpenNavMenuForMobile();
            FindElement(profile).Click();
            FindElement(personalDetailsLink).Click();
            return new PersonalDetailsPage(Driver);
        }

        public PublicationsPage GoToPublicationsPage()
        {
            OpenNavMenuForMobile();
            WaitUntilElementIsClickable(resources).Click();
            WaitUntilElementIsClickable(publicationsLink).Click();
            return new PublicationsPage(Driver);
        }

        public CalculatorsAndToolsPage GoToCalculatorsAndToolsPage()
        {
            OpenNavMenuForMobile();
            FindElement(resources).Click();
            FindElement(calculatorAndToolsLink).Click();
            return new CalculatorsAndToolsPage(Driver);
        }

        public SeminarsAndWebinarsPage GoToSeminarsAndWebinarsPage()
        {
            OpenNavMenuForMobile();
            FindElement(resources).Click();
            WaitUntilElementIsClickable(seminarsAndWebinarsLink).Click();
            return new SeminarsAndWebinarsPage(Driver);
        }

        public InvestmentPerformancePage GoToInvestmentPerformancePage()
        {
            OpenNavMenuForMobile();
            FindElement(investmentsLink).Click();
            WaitUntilElementIsVisible(investmentPerformanceLink).Click();
            return new InvestmentPerformancePage(Driver);
        }

        public YourInvestmentsPage GoToYourInvestmentsPage()
        {
            OpenNavMenuForMobile();
            FindElement(investmentsLink).Click();
            WaitUntilElementIsVisible(yourInvestmentsLink).Click();
            return new YourInvestmentsPage(Driver);
        }

        public InvestmentHistoryPage GoToInvestmentHistoryPage()
        {
            OpenNavMenuForMobile();
            if (!IsElementVisible(investmentsActiveDropdown, 1))
                FindElement(investmentsLink).Click();
            WaitUntilElementIsVisible(investmentHistoryLink).Click();
            return new InvestmentHistoryPage(Driver);
        }*/

        public LogoutPage Logout()
        {
            FindElement(logoutButton).Click();
            return new LogoutPage(Driver);
        }

        public ContacUsPage GoToContactUsPage()
        {
            WaitUntilElementHasFinalLocation(contactUsLink);
            ScrollToBottom();
            FindElement(contactUsLink).Click();
            return new ContacUsPage(Driver);
        }

        public bool IsVideoDisplayed()
        {
            return WaitUntilElementIsVisible(introVideoCloseButton, 60).Displayed;
        }

        private void OpenNavMenuForMobile() // only for mobile
        {
            if (IsMobile())
            {
                FindElement(navButton).Click();
                WaitUntilElementHasFinalSize(navList);
            }
        }

        public bool IsInvestmentMenuItemDisplayed()
        {
            return IsElementVisible(investmentsLink);
        }

        public IList<string[]> GetShortcutsInfo()
        {
            var info = new List<string[]>();
            ScrollIntoView(sliderTile);
            var tiles = FindElements(sliderTile);
            foreach (var tile in tiles)
            {
                var title = tile.FindElement(By.CssSelector("h4")).Text;
                var url = tile.FindElement(By.CssSelector("a")).GetAttribute("href");
                info.Add(new[] {title, url});
                if (IsMobile())
                {
                    FindElement(mobileOnlyNextTileButton).Click();
                    Thread.Sleep(1000);
                }
            }
            return info;
        }

        /*public SearchResultsPage Search(string searchText)
        {
            OpenNavMenuForMobile();
            FindElement(searchTextbox).SendKeys(searchText + Keys.Return);
            return new SearchResultsPage(Driver);
        }

        public OverviewPage SwitchToAccount(string accountBalance)
        {
            FindElement(switchAccountButton).Click();
            WaitUntilElementsExist(switchAccountButtonListItem, 5).First(e => e.Text.Contains(accountBalance)).Click();
            return new OverviewPage(Driver);
        }*/
    }
}