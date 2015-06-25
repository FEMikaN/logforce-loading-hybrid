using FifthElement.LogforceLoadingHybrid.Core.Model;
using FifthElement.LogforceLoadingHybrid.Core.Util;

namespace FifthElement.LogforceLoadingHybrid.Core
{
    public static class Constants
    {

        public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
        // JavaScript date time format --> actually ISO-8601 sortable format /TNU

        public const string Language = "U";

        public const string TablenameContract = "CONTRACT";
        public const string TablenameCustomer = "CUSTOMER";
        public const string TablenameOffer = "OFFER";
        public const string TablenameSilvicultureOrder = "SILVICULTURALORDER";

        public const string EmptyJsonObjectString = "{}";
        public const string EmptyJsonArrayString = "[]";

        public enum JsonReplaceFlags
        {
            CreateOrReplace,
            Replace
        };

        public enum DatabaseStatusField
        {
            Default = 0,
            Added,
            Modified,
            Removed,
            InMemory
        };

        // ReSharper disable InconsistentNaming
        public enum FlowUpdateMode
        {
            insert = 1,
            refresh,
            delete
        };
        // ReSharper restore InconsistentNaming


        public enum ServiceCallAddress
        {
            ServiceUrlGet = 0,
            ServiceUrlSave,
            ServiceUrlGis,
            ServiceUrlGisMapSymbols,
            ServiceUrlMg0450 // KTJ Haut
        };
        
        // Keys referring to database's keyvaluestorage table
        public const string KeyAvailableSapDocumentIds = @"AvailableSapDocumentIds";
        public const string KeyLastTableRequestUserCodes = @"KeyLastTableRequestUserCodes";
        public const string KeyLastTableRequestUserPricingLists = @"KeyLastTableRequestUserPricing";

        // Map package constants
        public const string PackageFileExtension = ".lfk";
        public const string ConfigFileExtension = ".lfc";
        public const string ConfigMetaFileExtension = ".meta";
        public const string ConfigFileRelativeDirectory = "_index";

        public const string EmptyUri = "file:///";
        public const string PricingTableFilename = @"PricingTable.json";
        public const string CodeTableFilename = @"CodeTable.json";

        public enum ExitCode
        {
            Error = -1,
            NotRun = 0,
            Success = 1,
            Canceled = 2,
        }

        public enum OperativeMapSymbols
        {
            // ReSharper disable InconsistentNaming
            MAPSYMBOLPOINT = 0,
            MAPSYMBOLLINE,
            MAPSYMBOLPOLYGON,
            STORAGEPOINT
            // ReSharper restore InconsistentNaming
        };

        public static string[] OperativeMapSymbolFeatureIdentifiers = new[]
                                                    {
                                                        "MAPSYMBOLPOINT",
                                                        "MAPSYMBOLLINE",
                                                        "MAPSYMBOLPOLYGON",
                                                        "STORAGEPOINT"
                                                    };

        public static string DocumentTypeSilvicultureOrder = "MH";
        public static string DocumentTypePurchaseContract = "OT";

        public static string InProgressResult = "IN_PROGRESS";


    }
}
