using Newtonsoft.Json;

namespace FifthElement.LogforceLoadingHybrid.Core.Model
{
    [JsonObject]
    public class ClearWatchCommandArgs
    {
        [JsonProperty("listenerId")]
        public string ListenerId { get; set; }
    }
}
