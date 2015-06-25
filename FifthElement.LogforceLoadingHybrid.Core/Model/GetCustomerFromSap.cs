using Newtonsoft.Json;

namespace FifthElement.LogforceLoadingHybrid.Core.Model
{
    [JsonObject]
    public class GetCustomerFromSap 
    {
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
    }
}
