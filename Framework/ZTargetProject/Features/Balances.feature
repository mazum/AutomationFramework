@Balances
Feature: Balances
	As a member
	I want to view my balance

Scenario Outline: View estimated balances for DBD Member
	Given I am a registered Defined Benefit Division member who joined <Join Date> with: <Condition(s)>
	And I am logged into the MOL site
	When I navigate to the Balances page
	Then I can see my estimated balance on the Balances Page
	| Key                                | Value      |
	| Account Balance                    | $14,747.96 |
	| Accumulation Balance               | $14,747.96 |
	| DBD Balance                        | $0.00      |
	| Preserved Balance                  | $14,747.96 |
	| Non-Preserved Restricted Balance   | $0.00      |
	| Non-Preserved Unrestricted Balance | $0.00      |
	| Balance Effective Date             | 23-10-2017 |
	#And I can see the DBD Formula
	#And I can see the relevant DBD Fact sheet
	When I navigate to the Overview page
	Then I can see my balance summary on the Overview Page

	Examples: 
	| Join Date        | Condition(s) |
	| after 1-Jan-2015 | NA           |

@Mobile @Devices
Scenario: View estimated balances for Accumulation 1 Member
	Given I am a registered Accumulation 1 member
	And I am logged into the MOL site
	When I navigate to the Balances page
	Then I can see my estimated balance on the Balances Page
	| Key                                | Value      |
	| Account Balance                    | $14,797.29 |
	| Accumulation Balance               | NA         |
	| DBD Balance                        | NA         |
	| Preserved Balance                  | $14,797.29 |
	| Non-Preserved Restricted Balance   | $0.00      |
	| Non-Preserved Unrestricted Balance | $0.00      |
	| Balance Effective Date             | 26-10-2017 | 
	When I navigate to the Overview page
	Then I can see my balance summary on the Overview Page