using System;
using Newtonsoft.Json;

namespace FifthElement.LogforceLoadingHybrid.Core.Model
{
    [JsonObject]
    public class TestMemoryParameter
    {
        [JsonProperty("memorySize")]
        public Int64 MemorySize { get; set; }
    }

    [JsonObject]
    public class TestMemoryResponse
    {
        [JsonProperty("longSize")]
        public Int64 LongSize { get; set; }

        [JsonProperty("intSize")]
        public int IntSize { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }

}
