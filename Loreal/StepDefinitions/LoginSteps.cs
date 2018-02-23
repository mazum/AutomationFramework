using System;
using TechTalk.SpecFlow;

namespace Loreal.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        [Given(@"Enter username as '(.*)'")]
        public void GivenEnterUsernameAs(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"Enter password as '(.*)'")]
        public void GivenEnterPasswordAs(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I press submit")]
        public void WhenIPressSubmit()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I land up on accounts page")]
        public void ThenILandUpOnAccountsPage()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
