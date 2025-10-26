using Newtonsoft.Json;
using RestSharp;
using API_Automation.Constants;
using API_Automation.Helpers;
using API_Automation.Models.Response;
using API_Automation.Utils;

namespace API_Automation.Client
{
    public class RestApiClient : IApiClient
    {
        private readonly RestClient _client;

        public RestApiClient(string baseUrl)
        {
        
            var timeoutSeconds = ConfigReader.GetTimeout();

            var options = new RestClientOptions(baseUrl)
            {
                ThrowOnAnyError = false,
                MaxTimeout = timeoutSeconds * 1000 
            };
            _client = new RestClient(options);
        }

        // GET
        public async Task<RestResponse> GetAsync(string endpoint, string token = null)
        {
            var request = new RestRequest(endpoint, Method.Get);

            if (!string.IsNullOrEmpty(token))
                request.AddHeader("Authorization", $"Bearer {token}");

            var response = await _client.ExecuteAsync(request);
            return response;
        }

        // POST
        public async Task<RestResponse> PostAsync<T>(string endpoint, T body, string token = null)
        {
            var request = new RestRequest(endpoint, Method.Post);

            if (!string.IsNullOrEmpty(token))
                request.AddHeader("Authorization", $"Bearer {token}");

            request.AddHeader("Content-Type", "application/json");
            request.AddBody(body, ContentType.Json);

            var response = await _client.ExecuteAsync(request);
            return response;
        }

        // PUT
        public async Task<RestResponse> PutAsync<T>(string endpoint, T body, string token = null)
        {
            var request = new RestRequest(endpoint, Method.Put);

            if (!string.IsNullOrEmpty(token))
                request.AddHeader("Authorization", $"Bearer {token}");

            request.AddHeader("Content-Type", "application/json");
            request.AddBody(body, ContentType.Json);

            var response = await _client.ExecuteAsync(request);
            return response;
        }

        // DELETE user or resource by endpoint
        public async Task<RestResponse> DeleteAsync(string endpoint, string token = null)
        {
            var request = new RestRequest(endpoint, Method.Delete);

            if (!string.IsNullOrEmpty(token))
                request.AddHeader("Authorization", $"Bearer {token}");

            var response = await _client.ExecuteAsync(request);
            Logger.Log($"DELETE {endpoint} → {response.StatusCode}");

            return response;
        }


        // DELETE book for a specific user
        public async Task<RestResponse> DeleteBookAsync(string userId, string isbn, string token)
        {
            var request = new RestRequest(ApiEndpoints.Books.DeleteBook, Method.Delete);

            if (!string.IsNullOrEmpty(token))
                request.AddHeader("Authorization", $"Bearer {token}");

            request.AddHeader("Content-Type", "application/json");

            var body = new
            {
                userId = userId,
                isbn = isbn
            };

            request.AddBody(body, ContentType.Json);

            var response = await _client.ExecuteAsync(request);
            return response;
        }

        // Create user - returns typed response
        public async Task<CreateUserResponse> CreateUser(string username, string password)
        {
            var body = new
            {
                userName = username,
                password = password
            };

            var request = new RestRequest(ApiEndpoints.Account.CreateUser, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(body, ContentType.Json);

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<CreateUserResponse>(response.Content);
            }

            Logger.LogError($"Failed to create user: {response.StatusCode} - {response.Content}");
            return null;
        }

        // Generate token - returns string token
        public async Task<string> GenerateToken(string username, string password)
        {
            var body = new
            {
                userName = username,
                password = password
            };

            var request = new RestRequest(ApiEndpoints.Account.GenerateToken, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(body, ContentType.Json);

            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                return tokenResponse?.Token;
            }

            Logger.LogError($"Failed to generate token: {response.StatusCode} - {response.Content}");
            return null;
        }
    }
}
