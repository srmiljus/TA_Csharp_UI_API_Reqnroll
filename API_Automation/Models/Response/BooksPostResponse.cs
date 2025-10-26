using Newtonsoft.Json;

namespace API_Automation.Models.Response
{
    public class BooksPostResponse
    {
        [JsonProperty("books")]
        public List<Book> Books { get; set; }

        public class Book
        {
            [JsonProperty("isbn")]
            public string Isbn { get; set; }
        }
    }
}
