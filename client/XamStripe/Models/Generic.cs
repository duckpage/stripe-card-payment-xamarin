using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XamStripe.Models
{
    public class Item
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
    }

    public class ItemsRequest
    {
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }
    }

    
}
