using Newtonsoft.Json;

namespace FifthElement.LogforceLoadingHybrid.Core.Model
{
    [JsonObject]
    public class AddWatchCommandArgs
    {
        [JsonProperty("listenerId")]
        public string ListenerId { get; set; }
    }
}
