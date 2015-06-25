using Newtonsoft.Json;

namespace FifthElement.LogforceLoadingHybrid.Core.Model
{
    public interface IBaseObject
    {
        //[JsonIgnore]
        //string InsertSql { get; }
        //[JsonIgnore]
        //DynamicParameters InsertParameters { get; }
        //[JsonIgnore]
        //string DeleteSql { get; }
        //[JsonIgnore]
        //DynamicParameters DeleteParameters { get; }
        [JsonIgnore]
        string Json { get; set; }
    }
}
