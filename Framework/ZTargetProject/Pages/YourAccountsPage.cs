using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using Framework.ZTargetProject.Data;
using Framework.Extensions;

namespace Framework.ZTargetProject.Pages
{
    public class YourAccountsPage : BasePage
    {
        public YourAccountsPage(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
            WaitForTitle("Your Accounts");
        }

        private By balanceDateText = By.ClassName("multi-account__date");
        private By totalBalanceText = By.ClassName("total-amount");
        private By descriptionText = By.ClassName("explore-prompt");
        private By accountLink = By.CssSelector("a.multi-account__account-link");
        private By accountTypeText = By.CssSelector("span.multi-account__type");
        private By accountNumberText = By.CssSelector("span.multi-account__number");
        private By accountBalanceText = By.CssSelector("span.multi-account__balance");

        public OverviewPage SelectAccount(string memberNumber)
        {
            FindElement(By.XPath("//span[contains(@class,'multi-account__number') and contains(.,'" + memberNumber +
                                 "')]")).Click();
            return new OverviewPage(Driver);
        }

        public string GetDescriptionText()
        {
            return WaitUntilElementIsVisible(descriptionText).Text;
        }

        public string GetTotalBalance()
        {
            return WaitUntilElementIsVisible(totalBalanceText).Text;
        }

        /*public List<SmvAccountAttributes> GetAccountDetails()
        {
            return FindElements(accountLink)
                .Select(m => new SmvAccountAttributes
                {
                    AccountType = GetAccountType(m),
                    AccountNumber = GetAccountNumber(m),
                    Balance = GetAccountBalance(m)
                }).ToList();
        }*/

        /// <summary>
        /// Refreshes the page to retrieve the latest balance if it is older than 2 days.
        /// Delay between retries: 6 seconds
        /// </summary>
        public void RefreshBalanceIfNeeded(int refreshAttempts = 3)
        {
            for (var i = 0; i < refreshAttempts; i++)
            {
                DateTime dateOfSuper = GetBalanceDate(FindElements(accountLink).Last()); // last is super if any
                if (DateTime.Today.AddDays(-2) <= dateOfSuper)
                    break;
                Thread.Sleep(6000);
                Refresh();
            }
        }

        private string GetAccountNumber(IWebElement element)
        {
            return element.FindElement(accountNumberText).Text;
        }

        private DateTime GetBalanceDate(IWebElement element)
        {
            return DateTime.ParseExact(element.FindElement(balanceDateText).Text.GetRegExMatch(@"\d.*\d"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        private string GetAccountBalance(IWebElement element)
        {
            return string.Join("", element.FindElements(accountBalanceText).GetTextFromElements());
        }

        private string GetAccountType(IWebElement element)
        {
            return element.FindElement(accountTypeText).Text;
        }

    }

}