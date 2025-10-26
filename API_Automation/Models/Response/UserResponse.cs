using Newtonsoft.Json;

namespace API_Automation.Models.Response
{
    public class UserResponse
    {
        [JsonProperty("userID")]
        public string UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("books")]
        public List<Book> Books { get; set; }

        public class Book
        {
            [JsonProperty("isbn")]
            public string Isbn { get; set; }
        }
    }
}
