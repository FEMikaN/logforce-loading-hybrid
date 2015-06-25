using System.Collections.Generic;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public static class ServiceResults
    {
        private static readonly List<string> ListResults = new List<string> {"GetActivityList",
"GetCampaignList",
"GetOutlookActivityList",
"GetOutlookTaskList",
"GetCustomerList",
"GetCustomersPartners",
"GetBriefcaseCustomers",
"GetLastOpened",
"GetParameterTables",
"GetOfferList",
"GetSilvicultureOfferList",
"GetMunicipalityList",
"GetCrmCustomerList",
"GetWoodPurchaseList",
"GetSilvicultureOrderList",
"GetMeasurementCertificates",
"SaveMapSymbols"};


        public static bool IsListResult(string commandName)
        {
            return ListResults.Contains(commandName);
        }
    }
}
