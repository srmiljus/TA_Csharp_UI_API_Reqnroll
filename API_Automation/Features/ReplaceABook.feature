@api @smoke
Feature: Replace a book via Bookstore API

  As a registered user
  I want to search for available books and manage my book list
  So that I can replace a book in my collection with another one using the Bookstore API

  Scenario: Verify that a user can replace a book
    Given A user is created and authorized
    When I get all books
    And I add the first book to user's list
    Then User has only one book and it matches the added one
    When I replace the book with the second one
    Then The user's book list contains only the replaced book
