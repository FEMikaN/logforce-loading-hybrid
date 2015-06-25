using System.IO;
using System.Net;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public static class ExceptionUtils
    {
        static public string WebExceptionAsString(WebException webException)
        {
            string result = "";
            if (webException == null) return result;
            if (webException.Response == null) return result;
            var responseStream = webException.Response.GetResponseStream();
            if (responseStream == null) return result;
            result = new StreamReader(responseStream).ReadToEnd();
            return result;
        }

    }
}
