using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Framework.ZTargetProject.Data;
using Framework.Extensions;
using Framework.ZTargetProject.Pages;
using Framework.Utils;
//using USM.Simple.API.MemberAccountChannel

namespace Framework.ZTargetProject.StepDefinitions
{
    [Binding]
    public sealed class BalancesSteps
    {
        private BalancesPage balancesPage;
        private OverviewPage overviewPage;
        private MemberBalancesDetails apiBalancesDetails;
        private DateTime apiBalanceEffectiveDate;

        [When(@"I navigate to the Balances page")]
        public void WhenINavigateToTheBalancesPage()
        {
            balancesPage = ScenarioContextData.Current.CurrentMemberPage.GoToBalancesPage();
        }

        [Then(@"I can see my estimated balance on the Balances Page")]
        public void ThenICanSeeMyEstimatedBalanceOnTheBalancesPage(Table table)
        {
            var isDbd = ScenarioContextData.Current.AccountType.StartsWith("Defined");
            //Dictionary<string,string> tableDictionary=ta;
            apiBalancesDetails =
                new MemberBalancesDetails
                {
                    AccountBalanceAmount = table.Rows[0][1],
                    DefinedBenefitAmount = table.Rows[2][1],
                    AccumulationAmount = table.Rows[1][1],
                    BenefitPreservedAmount = table.Rows[3][1],
                    BenefitNonPreservedRestrictedAmount = table.Rows[4][1],
                    BenefitNonPreservedUnrestrictedAmount = table.Rows[5][1]
                };
            apiBalanceEffectiveDate = DateTime.ParseExact(table.Rows[6][1],"dd-MM-yyyy",null);
            var balances = balancesPage.GetBalanceData(isDbd);
            if (isDbd)
            {
                Assert.That(balances.DefinedBenefitAmount, Is.EqualTo(apiBalancesDetails.DefinedBenefitAmount),"DB Balances doesn't match");
                Assert.That(balances.AccumulationAmount, Is.EqualTo(apiBalancesDetails.AccumulationAmount), "Accumulation Balances doesn't match");
            }
            Assert.That(balances.BenefitPreservedAmount, Is.EqualTo(apiBalancesDetails.BenefitPreservedAmount), "Preserved Balances doesn't match");
            Assert.That(balances.AccountBalanceAmount, Is.EqualTo(apiBalancesDetails.AccountBalanceAmount), "Account Balances doesn't match");
            Assert.That(balances.BenefitNonPreservedRestrictedAmount, Is.EqualTo(apiBalancesDetails.BenefitNonPreservedRestrictedAmount), "Non-Preserved Restricted Balances doesn't match");
            Assert.That(balances.BenefitNonPreservedUnrestrictedAmount, Is.EqualTo(apiBalancesDetails.BenefitNonPreservedUnrestrictedAmount), "Non-Preserved Unrestricted Balances doesn't match");
            Assert.That(balancesPage.GetAccountBalanceEffectiveDate(), Is.EqualTo(apiBalanceEffectiveDate.ToString("dd/MM/yyyy")), "Balance Effective Date doesn't match");
        }

        [When(@"I navigate to the Overview page")]
        public void WhenINavigateToTheOverviewPage()
        {
            overviewPage = ScenarioContextData.Current.CurrentMemberPage.GoToOverviewPage();
            ScenarioContextData.Current.CurrentMemberPage = overviewPage;
        }

        [Then(@"I can see my balance summary on the Overview Page")]
        public void ThenICanSeeMyBalanceSummaryOnTheOverviewPage()
        {
            overviewPage = (OverviewPage) ScenarioContextData.Current.CurrentMemberPage;
            var isDbdWithAccumBalance = ScenarioContextData.Current.AccountType.StartsWith("Defined") &&
                                        !apiBalancesDetails.AccumulationAmount.Equals("$0.00");
            var balances = overviewPage.GetBalanceData(isDbdWithAccumBalance);
            Assert.That(balances.AccountBalanceAmount,Is.EqualTo(apiBalancesDetails.AccountBalanceAmount),"Account Balances doesn't match");
            if (isDbdWithAccumBalance)
            {
                Assert.That(balances.AccumulationAmount, Is.EqualTo(apiBalancesDetails.AccumulationAmount), "Accumulation Balances doesn't match");
                Assert.That(balances.DefinedBenefitAmount, Is.EqualTo(apiBalancesDetails.DefinedBenefitAmount), "DBD Balances doesn't match");
            }
            Assert.That(overviewPage.GetAccountBalanceEffectiveDate(), Is.EqualTo(apiBalanceEffectiveDate.ToString("dd MMMM yyyy")), "Account Balance Effective Date doesn't match");
        }

    }
}