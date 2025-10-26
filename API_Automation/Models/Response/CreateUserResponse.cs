using Newtonsoft.Json;

namespace API_Automation.Models.Response
{
    public class CreateUserResponse
    {
        [JsonProperty("userID")]
        public string UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
