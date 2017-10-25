using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using Framework.ZTargetProject.Data;
using Framework.Extensions;
using Framework.Utils;
using Framework.WebElements;

namespace Framework.ZTargetProject.Pages
{
    public class OverviewPage : BaseMemberPage, IHasCurrentInvestmentAllocationSection
    {
        public OverviewPage(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
            WaitForTitle("Overview");
        }

        private By accountNameLabel = By.XPath("//h2[contains(@class,'account-name')]");
        private By memberNumberLabel = By.XPath("//div[contains(@class,'account-number')]");
        private By beneficiaryTable = By.Id("__beneficiaries");
        private By beneficiaryFormTitleText = By.CssSelector("div[url*='BeneficiarySummary'] div h2");
        private By reversionaryBeneficiaryDetailsList = By.CssSelector("div[url*='BeneficiarySummary'] div h2 + p +fieldset > ul > li > span.value");
        private By seeMoreBeneficiaryButton = By.CssSelector("a.button.primary-cta[href='/beneficiaries']");
        private By transactionsTable = By.XPath("//h2[contains(.,'Recent transactions')]/..//table");
        private By performanceTable = By.XPath("//h2[contains(.,'options have performed')]/..//table");
        private By performanceTableOptionHeading = By.XPath("//th[@colspan='5']");
        private By overviewInsuranceTable = By.XPath("//h2[contains(.,'Insurance')]/..//table");
        private By estimatedBalanceText = By.CssSelector("div.overview-balance");
        private By balanceBreakDownTable = By.CssSelector("table.overview-table");
        private By accountBalanceDate = By.CssSelector("span.date");
        private By balOverTimeTableButton = By.CssSelector("button.switch-button.icon-table");
        private By balOverTimeGraphButton = By.CssSelector("button.switch-button.icon-chart");
        private By balOverTimeTable = By.XPath("//h2[contains(.,'Balance over time')]/..//table");
        private By balOverTimeGraph = By.ClassName("barchart-chart");
        private By balOverTimeGraphSeries = By.CssSelector(".highcharts-series");
        private By balOverTimeGraphBars = By.CssSelector(".highcharts-series > rect");
        private By balGraphXLabels = By.CssSelector("g.highcharts-axis-labels.highcharts-xaxis-labels > text > tspan");
        private By balGraphHighchartsTooltip = By.CssSelector("g.highcharts-tooltip > text");
        private By investmentsContainer = By.XPath("//div[contains(@class,'investments-container')]");
        private By smoothRatedPensionerDisclaimerHeading = By.CssSelector("table.overview-table + h6");
        private By smoothRatedPensionerDisclaimerBody = By.CssSelector("table.overview-table + h6 + p");
        private By passwordChangeSuccessMessage = By.CssSelector("div.success-summary");
        private By contributionCapsSection = By.XPath("//div[@class='content-target'][contains(.,'How your')]");
        private By mobileTransactionsShowHideColumnsButton = By.XPath("//h2[contains(.,'Recent transactions')]/..//a[contains(@class, 'responsive-table-menu-btn')]");
        private By mobileTransactionsColumnShowHideOptionCheckbox = By.XPath("//h2[contains(.,'Recent transactions')]/..//input");
        private By mobileBalanceShowHideColumnsButton = By.XPath("//h2[contains(.,'Balance over time')]/..//a[contains(@class, 'responsive-table-menu-btn')]");
        private By mobileBalanceColumnShowHideOptionCheckbox = By.XPath("//h2[contains(.,'Balance over time')]/..//input");


        public string GetMemberNumber()
        {
            return FindElement(memberNumberLabel).Text.GetRegExMatch(@"\d+$");
        }

        public string GetAccountType()
        {
            return FindElement(accountNameLabel).Text;
        }

        public int GetBeneficiariesCount()
        {
            WaitUntilElementIsVisible(beneficiaryTable);
            ScrollIntoView(beneficiaryTable);
            return FindTable(beneficiaryTable).RowCount;
        }

        public IList<BeneficiaryDetails> GetBeneficiaries()
        {
            var rows = new List<BeneficiaryDetails>();
            foreach (var row in FindTable(beneficiaryTable).GetRows())
            {
                var percent = Convert.ToDecimal(row.GetCellElement(2).Text);
                rows.Add(new BeneficiaryDetails() { FullName = row.GetCellElement(1).Text, Percentage = percent });
            }
            return rows;
        }

        public string GetBeneficiaryFormTitleText()
        {
            return WaitUntilElementIsVisible(beneficiaryFormTitleText).Text;
        }

        public bool IsBeneficiarySectionVisible()
        {
            return IsElementVisible(beneficiaryTable) && IsElementVisible(beneficiaryFormTitleText);
        }

        public string GetReversionaryBeneficiaryDetails()
        {
            return WaitUntilElementIsVisible(reversionaryBeneficiaryDetailsList).Text;
        }

        /*public BeneficiariesPage SeeMoreBeneficiaries()
        {
            FindElement(seeMoreBeneficiaryButton).Click();
            return new BeneficiariesPage(Driver);
        }*/

        public IList<TransactionHistoryEntry> GetRecentTransactions()
        {
            WaitUntilElementIsVisible(transactionsTable);
            if (IsMobile())
            {
                ScrollIntoView(mobileTransactionsShowHideColumnsButton);
                FindElement(mobileTransactionsShowHideColumnsButton).Click();
                foreach (var checkbox in FindElements(mobileTransactionsColumnShowHideOptionCheckbox))
                    new Checkbox(checkbox).Select(true);
            }
            ScrollIntoView(transactionsTable);
            var t = FindTable(transactionsTable);
            var amountColIndex = t.GetColumnIndex("Amount");
            var hasComponentCol = t.ColumnTitles.Contains("Component");
            var entries = new List<TransactionHistoryEntry>();

            foreach (var row in t.Rows)
            {
                var entry = new TransactionHistoryEntry()
                {
                    Date = row.GetCellElement(1).Text,
                    TransactionType = row.GetCellElement(2).Text,
                    Description = row.GetCellElement(3).Text,
                    Amount = row.GetCellElement(amountColIndex).Text.Trim()
                };
                if (hasComponentCol)
                    entry.Component = row.GetCellElement(4).Text;

                entries.Add(entry);
            }
            return entries;
        }

        public IList<string> GetTransactionsColumnTitles()
        {
            return FindTable(transactionsTable).ColumnTitles;
        }

        public IList<string> GetPerformanceOverviewOptionNames()
        {
            ScrollToBottom();
            WaitUntilElementHasFinalLocation(performanceTable);
            ScrollIntoView(performanceTable);
            return FindElement(performanceTable).FindElements(performanceTableOptionHeading).GetTextFromElements();
        }

        public MemberInsuranceDetails GetInsuranceOverview()
        {
            ScrollIntoView(WaitUntilElementIsVisible(overviewInsuranceTable));
            var table = FindTable(overviewInsuranceTable);

            if (ScenarioContextData.Current.AccountType.Equals("Defined Benefit Division"))
                return new MemberInsuranceDetails
                {
                    InbuiltDeathInsuranceAmount = table.GetCellElement(1, 3).Text,
                    InbuiltTIInsuranceAmount = table.GetCellElement(4, 3).Text,
                    OptionalDeathBenefitAmount = table.GetCellElement(2, 3).Text,
                    OptionalTPDBenefitAmount = table.GetCellElement(3, 3).Text
                };
            return new MemberInsuranceDetails
            {
                OptionalDeathBenefitAmount = table.GetCellElement(1, 3).Text,
                OptionalTPDBenefitAmount = table.GetCellElement(2, 3).Text
            };
        }

        public MemberBalancesDetails GetBalanceData(bool doesBalanceTableExists)
        {
            var balanceData = new MemberBalancesDetails()
            {
                AccountBalanceAmount = FindElement(estimatedBalanceText).Text
            };
            if (doesBalanceTableExists)
            {
                var components = FindTable(balanceBreakDownTable);
                balanceData.DefinedBenefitAmount = components.GetCellElement(1, 2).Text;
                balanceData.AccumulationAmount = components.GetCellElement(2, 2).Text;
            }
            return balanceData;
        }

        public string GetAccountBalanceEffectiveDate()
        {
            return FindElement(accountBalanceDate).Text.GetRegExMatch(@"\d.*");
        }

        public void ViewBalanceOverTimeTable()
        {
            WaitUntilElementIsClickable(balOverTimeTableButton).Click();
            WaitUntilElementIsVisible(balOverTimeTable);
        }

        public IList<string> GetBalanceHistory()
        {
            var history = new List<string>();

            if (IsMobile() && DoesElementExist(mobileBalanceShowHideColumnsButton))
            {
                ScrollIntoView(mobileBalanceShowHideColumnsButton);
                FindElement(mobileBalanceShowHideColumnsButton).Click();
                foreach (var checkbox in FindElements(mobileBalanceColumnShowHideOptionCheckbox))
                    new Checkbox(checkbox).Select(true);
            }

            foreach (var row in FindTable(balOverTimeTable).Rows)
            {
                history.Add(row.Text.ReplaceRegExMatch(@"\s{2,}", " "));
            }
            return history;
        }

        public bool IsBalanceOverTimeTableDisplayed()
        {
            return IsElementVisible(balOverTimeTable, 30);
        }

        public void ViewBalanceOverTimeGraph()
        {
            WaitUntilElementIsClickable(balOverTimeGraphButton).Click();
            WaitUntilElementIsVisible(balOverTimeGraph);
            WaitUntilElementsExist(balOverTimeGraphBars);
        }

        public bool IsBalanceOverTimeGraphDisplayed()
        {
            return IsElementVisible(balOverTimeGraph) && FindElements(balOverTimeGraphBars).Count > 0;
        }

        public IList<string> GetBalanceGraphHistory()
        {
            var history = new List<string>();
            var dates = FindElements(balGraphXLabels).GetMatchingTextFromElements(@"\d+/\d+/\d+$");
            var series = FindElements(balOverTimeGraphSeries);

            var numberOfBars = (FindElements(balOverTimeGraphBars).Count) / series.Count;

            for (int i = numberOfBars - 1; i >= 0; i--)
            {
                var entryParts = new List<string>();
                entryParts.Add(dates[i]);
                var bar = FindElements(balOverTimeGraphBars)[i];
                bar.Click();
                var total = FindElement(balGraphHighchartsTooltip).Text.GetRegExCaptureGroup(@"Total:\s+(.*)");
                if (series.Count > 1)
                {
                    var accum = FindElement(balGraphHighchartsTooltip).Text.GetRegExCaptureGroup(@"Accumulation amount:\s+(.*),Total");
                    var dbdBarPart = FindElements(balOverTimeGraphBars)[i + numberOfBars];
                    string dbd = "$0.00";
                    if (dbdBarPart.Size.Height > 0)
                    {
                        dbdBarPart.Click();
                        dbd = FindElement(balGraphHighchartsTooltip).Text.GetRegExCaptureGroup(@"Defined benefit amount:\s+(.*),Total");
                    }
                    else
                    {
                        accum = total; // workaround for issue. Dan is checking how info should be displayed in this case.
                    }
                    entryParts.Add(dbd);
                    entryParts.Add(accum);
                }
                entryParts.Add(total);
                var entry = string.Join(" ", entryParts);
                history.Add(entry);
            }
            return history;
        }

        public string GetAnnualPension()
        {
            return FindTable(balanceBreakDownTable).FindRowWithMatchInColumn(1, "Annual pension income").GetCellElement(2).Text;
        }

        public string GetFuturePensionAmount()
        {
            return FindTable(balanceBreakDownTable).FindRowWithMatch(@"(?:Next|Future) pension.*?\$").GetCellElement(2).Text;
        }

        public string GetNextPensionPaymentDate()
        {
            return FindTable(balanceBreakDownTable).FindRowWithMatchInColumn(1, "Next pension payment date").GetCellElement(2).Text;
        }

        public string GetDbdAmount()
        {
            return FindTable(balanceBreakDownTable).FindRowWithMatchInColumn(1, "Defined benefit option").GetCellElement(2).Text;
        }

        public string GetAccumAmount()
        {
            return FindTable(balanceBreakDownTable).FindRowWithMatchInColumn(1, "Other investment options").GetCellElement(2).Text;
        }

        public string GetBalanceTableText()
        {
            return FindElement(balanceBreakDownTable).Text;
        }

        public bool IsBalanceTableDisplayed()
        {
            return IsElementVisible(balanceBreakDownTable);
        }

        public bool IsHowYourAccumIsInvestedDisplayed()
        {
            return IsElementVisible(investmentsContainer);
        }

        public bool IsHowYourOptionsHavePerformedDisplayed()
        {
            return IsElementVisible(performanceTable);
        }

        public string GetSmoothRatedPensionerDisclaimerText()
        {
            return FindElement(smoothRatedPensionerDisclaimerHeading).Text + " " +
                   FindElement(smoothRatedPensionerDisclaimerBody).Text;
        }

        public CurrentInvestmentAllocationSection GetCurrentInvestmentAllocationSection()
        {
            return new CurrentInvestmentAllocationSection(Driver);
        }

        public ContributionCapsSection GetContributionCapsSection()
        {
            WaitUntilElementIsVisible(contributionCapsSection);
            return new ContributionCapsSection(Driver);
        }

        public string GetPasswordChangeSuccessMessage()
        {
            return WaitUntilElementIsVisible(passwordChangeSuccessMessage).Text;
        }

        /// <summary>
        /// Refreshes the page to retrieve the latest balance if it is older than 2 days.
        /// Delay between retries: 6 seconds
        /// </summary>
        public void RefreshBalanceIfNeeded(int refreshAttempts = 3)
        {
            for (var i = 0; i < refreshAttempts; i++)
            {
                DateTime dateOfSuper = DateTime.ParseExact(GetAccountBalanceEffectiveDate(), "dd MMMM yyyy", CultureInfo.InvariantCulture);
                if (DateTime.Today.AddDays(-2) <= dateOfSuper)
                    break;
                Thread.Sleep(6000);
                Refresh();
            }
        }
    }
}