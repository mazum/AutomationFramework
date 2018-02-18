Feature: Login
	As a user,
	I want to login to Loreal site

Scenario: Login
	Given Enter username as 'loreal@surepayd.com'
	And Enter password as 'Password1?'
	When I press submit
	Then I land up on accounts page
