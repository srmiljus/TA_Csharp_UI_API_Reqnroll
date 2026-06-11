using Newtonsoft.Json;

namespace API_Automation.Models.Response
{
    public class BooksGetResponse
    {
        [JsonProperty("books")]
        public List<Book> Books { get; set; }

        public class Book
        {
            [JsonProperty("isbn")]
            public string Isbn { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("author")]
            public string Author { get; set; }
        }
    }
}
