using System;
using TechTalk.SpecFlow;
using Framework.Utils;
//using Unisuper.MemberData

namespace Framework.ZTargetProject.StepDefinitions
{
    [Binding]
    public sealed class MemberDataSteps
    {
        [Given(@"I am a registered Defined Benefit Division member who joined (after|before) 1-Jan-2015 with: (NA|clause 34, Benefit Improvement, Suplementary Improvement and Additional Benefit Service|Benefit Improvement)")]
        public void GivenIAmARegisteredDefinedBenefitDivisionMemberWhoJoinedWith(string joined, string condition)
        {
            string username;
            if (joined.Equals("after"))
                username = "abhi4u82";
            else
            {
                switch (condition)
                {
                    case "clause 34, Benefit Improvement, Suplementary Improvement and Additional Benefit Service":
                        username = "";
                        break;
                    case "":
                        username = "";
                        break;
                    default:
                        throw  new NotImplementedException();
                }
            }
            SetMemberAttributes(username);
        }

        private void SetMemberAttributes(string username)
        {
            Console.WriteLine("Username: " + username);
            ScenarioContextData.Current.Username = username;                                                                                                                                            var password="";
            ScenarioContextData.Current.Password = password;
            ScenarioContextData.Current.AccountType = "Defined Benifit Division";
            ScenarioContextData.Current.AccountBaseType = "Super";
            ScenarioContextData.Current.MemberNumber = "14223482";
        }
    }
}
