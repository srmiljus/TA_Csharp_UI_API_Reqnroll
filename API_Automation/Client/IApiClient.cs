using API_Automation.Models.Response;
using RestSharp;

namespace API_Automation.Client
{
    public interface IApiClient
    {
        Task<RestResponse> GetAsync(string endpoint, string token = null);
        Task<RestResponse> PostAsync<T>(string endpoint, T body, string token = null);
        Task<RestResponse> PutAsync<T>(string endpoint, T body, string token = null);
        Task<RestResponse> DeleteAsync(string endpoint, string token = null);
        Task<RestResponse> DeleteBookAsync(string userId, string isbn, string token);

        // Helpers
        Task<CreateUserResponse> CreateUser(string username, string password);
        Task<string> GenerateToken(string username, string password);
    }
}

