using System;
using System.Collections.Generic;
using Framework.ZTargetProject.Pages;
//using USM.Simple.API.MemberChannel;

namespace Framework.Utils
{
    public interface IScenarioContext
    {
        BasePage CurrentPage { get; set; }
        BaseMemberPage CurrentMemberPage { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string AccountNumber { get; set; }
        string AccountType { get; set; }
        string AccountBaseType { get; set; }
        string AccountSubType { get; set; }
        string MemberNumber { get; set; }
        bool HasDemoVideoDisplayDate { get; set; }
        DateTime DemoVideoDisplayDate { get; set; }
        bool IsSpouseAccount { get; set; }
        string PersonalEmailAddress { get; set; }
        string PersonalMobileNumber { get; set; }
        MailItem LastEmail { get; set; }
        bool HasMultipleAccounts { get; set; }
        bool IsWEHI { get; set; }
        int ActiveSuper { get; set; }
        int ActivePension { get; set; }
        int ActiveIndexedPension { get; set; }
        bool HasExitedAccounts { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime DateOfBirth { get; set; }
        //List<AttributeData> SmvAccountsAttributes { get; set; }
    }
}