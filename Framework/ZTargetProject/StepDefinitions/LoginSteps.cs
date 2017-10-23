using System;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Framework.Extensions;
using Framework.ZTargetProject.Pages;
using Framework.Utils;

namespace Framework.ZTargetProject.StepDefinitions
{
    [Binding]
    public sealed class LoginSteps
    {
        private LoginPage loginPage;
        private LogoutPage logoutPage;
        private OverviewPage overviewPage;
        private ForgotUsernamePage forgotUsernamePage;
        private ForgotPasswordPage forgotPasswordPage;

        [Given(@"I am logged into the MOL site")]
        public void GivenIAmLoggedIntoTheMOLSite()
        {
            overviewPage=new LoginPage(DriverFactory.Driver).Login(ScenarioContextData.Current.Username, ScenarioContextData.Current.Password);
            ScenarioContextData.Current.CurrentMemberPage = overviewPage;
            overviewPage.PrintCookies();
            overviewPage.RefreshBalanceIfNeeded(5);
        }

    }
}