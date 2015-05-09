using Newtonsoft.Json;

namespace HardwareInventory.Datamodel
{
    public class HardwareItem
    {
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "imageurl")]
        public string ImageUrl { get; set; }
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
    }
}
