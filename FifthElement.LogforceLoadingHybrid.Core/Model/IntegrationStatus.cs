using Newtonsoft.Json;

namespace FifthElement.LogforceLoadingHybrid.Core.Model
{
    [JsonObject]
    public class IntegrationStatus
    {
        [JsonProperty("isOnline")]
        public bool IsOnline { get; set; } 
    }
}
