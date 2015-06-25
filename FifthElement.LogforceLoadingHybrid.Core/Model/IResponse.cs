using Newtonsoft.Json.Linq;

namespace FifthElement.LogforceLoadingHybrid.Core.Model
{
    public interface IRestServiceResponse
    {
        int Status { get; set; }
        JToken Response { get; set; }
        bool KeepCallBack { get; set; }
    }

}
