using System;
using SQLite;
using Newtonsoft.Json;


namespace FifthElement.LogforceLoadingHybrid.Core.Model
{
    [Table("KEYVALUESTORAGE")]
    [JsonObject]
    public class KeyValueStorage 
    {
        [PrimaryKey, Column("KEYVALUESTORAGEID")]
        [JsonProperty("key")]
        public string Key { get; set; }

        [Column("INFO_TEXT")]
        [JsonProperty("infoText")]
        public string Infotext { get; set; }

        [Column("INFO_CODE")]
        [JsonProperty("infoCode")]
        public int? InfoCode { get; set; }

        [Column("UPDATED_ON")]
        [JsonIgnore]
        public DateTime? UpdatedOn { get; set; }

        [Column("SYNCDATE")]
        [JsonIgnore]
        public DateTime? Syncdate { get; set; }
    }

}
