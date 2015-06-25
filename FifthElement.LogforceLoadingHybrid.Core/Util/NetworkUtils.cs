using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public static class NetworkUtils
    {

        public static bool IsNetWorkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        private static string DictionaryValue(string[] keyValuePair)
        {
            if (keyValuePair.Length < 2)
                return null;
            return keyValuePair[1];
        }

        public static Dictionary<string, string> UrlParameters(string fullUrl)
        {
            fullUrl = Uri.UnescapeDataString(fullUrl);
            var baseUrl = fullUrl.Split('?');
            if (baseUrl.Length < 2)
                return new Dictionary<string, string>();

            var result = baseUrl[1].Split('&').Select(kvp => kvp.Split('=')).ToDictionary(kvp => kvp[0].ToUpper(), DictionaryValue);
            result.Add("baseUrl",baseUrl[0]);
            return result;
        }
    }
}
