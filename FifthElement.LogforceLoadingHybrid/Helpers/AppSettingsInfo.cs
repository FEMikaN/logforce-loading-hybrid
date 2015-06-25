using Newtonsoft.Json;

namespace FifthElement.KotkaApp.Helpers
{
    [JsonObject]
    class AppSettingsInfo
    {
        [JsonProperty("databaseVersion")]
        public string DataBaseVersion { get; set; }
    }
}
