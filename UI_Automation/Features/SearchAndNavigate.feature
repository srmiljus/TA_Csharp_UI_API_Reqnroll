@regression
@allure.epic:SteamWebInterface
@allure.feature:Search
@allure.owner:SrdjanMiljus
@allure.severity:critical
Feature: Search and navigate

  As a user
  I want to search for game and navigate to the official Steam About page
  So that I can play games on the Steam platform

@ui @regression
Scenario: Search for Steam game and navigate to the About page
	Given I open Store page
	When I search for "FIFA" game
	Then I should see the first search result "EA SPORTS FC™ 25"
	And I should see the second search result "FIFA 22"
	When I search for "THE FINALS" game
	And I click on the first search result in the search results
	Then I should be redirected to the "THE_FINALS" page
	And I should see the game name "THE FINALS" from the 1st search result
	When I click on Play Game button
	And I click on No, I need Steam button
	Then I should be redirected to the "about" page
	And I should see the Install Steam button is clickable
	And I should see that Playing Now gamers status are less than Online gamers status

@ui @e2e
Scenario: Navigate to the About page
	Given I open Store page
	When I search for "THE FINALS" game
	And I click on the first search result in the search results
	Then I should be redirected to the "THE_FINALS" page
	And I should see the game name "THE FINALS" from the 1st search result
	When I click on Play Game button
	And I click on No, I need Steam button
	Then I should be redirected to the "adout" page
	And I should see the Install Steam button is clickable
	And I should see that Playing Now gamers status are less than Online gamers status

@ui @smoke
Scenario: About page
	Given I open Store page
	When I search for "THE FINALS" game
	And I click on the first search result in the search results
	Then I should be redirected to the "THE_FINALS" page
	And I should see the game name "THE FINALS" from the 1st search result
	When I click on Play Game button
	