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

            [JsonProperty("subTitle")]
            public string SubTitle { get; set; }

            [JsonProperty("author")]
            public string Author { get; set; }

            [JsonProperty("publish_date")]
            public string PublishDate { get; set; }

            [JsonProperty("publisher")]
            public string Publisher { get; set; }

            [JsonProperty("pages")]
            public int Pages { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("website")]
            public string Website { get; set; }
        }
    }
}
