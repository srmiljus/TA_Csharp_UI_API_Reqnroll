using Newtonsoft.Json;

namespace API_Automation.Models.Request
{
    public class PostBookRequest
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("collectionOfIsbns")]
        public List<IsbnItem> CollectionOfIsbns { get; set; }

        public class IsbnItem
        {
            [JsonProperty("isbn")]
            public string Isbn { get; set; }
        }
    }
}
