using API_Automation.Client;
using API_Automation.Constants;
using API_Automation.Models.Request;
using API_Automation.Models.Response;
using API_Automation.Utils;
using Newtonsoft.Json;
using NUnit.Framework;

namespace API_Automation.StepDefinitions
{
    [Binding]
    public class ReplaceABookStepDefinitions
    {
        private readonly RestApiClient _client = new(ConfigReader.GetBaseUrl("bookstore"));
        private string _userId = string.Empty;
        private string _token = string.Empty;
        private BooksGetResponse _booksResponse;

        [Given("A user is created and authorized")]
        public async Task GivenAUserIsCreatedAndAuthorized()
        {
            var username = TestDataGenerator.RandomUsername();
            var password = TestDataGenerator.RandomPassword();

            var userResponse = await _client.CreateUser(username, password);
            Assert.That(userResponse, Is.Not.Null, "Failed to create user");
            Assert.That(userResponse.UserId, Is.Not.Null.And.Not.Empty, "User ID should not be null or empty");

            _userId = userResponse.UserId;

            _token = await _client.GenerateToken(username, password);
            Assert.That(_token, Is.Not.Null, "Token was not generated");
        }

        [When("I get all books")]
        public async Task WhenIGetAllBooks()
        {
            var response = await _client.GetAsync(ApiEndpoints.Books.GetAll);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
                $"Expected 200 but got {response.StatusCode}");

            _booksResponse = JsonConvert.DeserializeObject<BooksGetResponse>(response.Content);
            Assert.That(_booksResponse?.Books, Is.Not.Null.And.Not.Empty, "Books list should not be empty");
        }

        [When("I add the first book to user's list")]
        public async Task WhenIAddTheFirstBookToUserSList()
        {
            var firstIsbn = _booksResponse.Books[0].Isbn;

            var bookRequest = new PostBookRequest
            {
                UserId = _userId,
                CollectionOfIsbns = new System.Collections.Generic.List<PostBookRequest.IsbnItem>
                {
                    new PostBookRequest.IsbnItem { Isbn = firstIsbn }
                }
            };

            var response = await _client.PostAsync(ApiEndpoints.Books.AddBooks, bookRequest, _token);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created),
                $"Expected 201 Created but got {response.StatusCode}. Response: {response.Content}");
        }

        [Then("User has only one book and it matches the added one")]
        public async Task ThenUserHasOnlyOneBookAndItMatchesTheAddedOne()
        {
            var response = await _client.GetAsync(ApiEndpoints.Account.GetUser(_userId), _token);
            var userData = JsonConvert.DeserializeObject<UserResponse>(response.Content);

            Assert.That(userData?.Books?.Count, Is.EqualTo(1), "Unexpected number of books");

            string expectedIsbn = _booksResponse.Books[0].Isbn;
            string actualIsbn = userData.Books[0].Isbn;
            Assert.That(actualIsbn, Is.EqualTo(expectedIsbn), "Book does not match the one added");
        }

        [When("I replace the book with the second one")]
        public async Task WhenIReplaceTheBookWithTheSecondOne()
        {
            string firstIsbn = _booksResponse.Books[0].Isbn;
            string secondIsbn = _booksResponse.Books[1].Isbn;

            // Delete first book
            var deleteResponse = await _client.DeleteBookAsync(_userId, firstIsbn, _token);
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NoContent),
                $"Expected 204 NoContent but got {deleteResponse.StatusCode}. Response: {deleteResponse.Content}");

            // Add second book using proper model
            var bookRequest = new PostBookRequest
            {
                UserId = _userId,
                CollectionOfIsbns = new System.Collections.Generic.List<PostBookRequest.IsbnItem>
                {
                    new PostBookRequest.IsbnItem { Isbn = secondIsbn }
                }
            };

            var response = await _client.PostAsync(ApiEndpoints.Books.AddBooks, bookRequest, _token);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created),
                $"Expected 201 Created but got {response.StatusCode}. Response: {response.Content}");
        }

        [Then("The user's book list contains only the replaced book")]
        public async Task ThenTheUserSBookListContainsOnlyTheReplacedBook()
        {
            var response = await _client.GetAsync(ApiEndpoints.Account.GetUser(_userId), _token);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var userData = JsonConvert.DeserializeObject<UserResponse>(response.Content);
            string actualIsbn = userData.Books[0].Isbn;
            string expectedIsbn = _booksResponse.Books[1].Isbn;

            Assert.That(actualIsbn, Is.EqualTo(expectedIsbn), "Book was not replaced correctly");
        }

        [AfterScenario]
        public async Task Cleanup()
        {
            if (!string.IsNullOrEmpty(_userId))
            {
                var client = new RestApiClient(ConfigReader.GetBaseUrl("bookstore"));
                var response = await client.DeleteAsync(ApiEndpoints.Account.DeleteUser(_userId), _token);

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent ||
                    response.StatusCode == System.Net.HttpStatusCode.OK)
                    TestContext.Out.WriteLine($"User {_userId} deleted successfully ({response.StatusCode}).");
                else
                    TestContext.Out.WriteLine($"Failed to delete user {_userId}. Status: {response.StatusCode}, Response: {response.Content}");
            }
        }
    }
}
