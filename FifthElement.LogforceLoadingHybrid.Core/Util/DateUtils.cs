using System;
using System.Globalization;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public static class DateUtils
    {
        private const string Date10Characters = "yyyy-MM-dd";
        private const string DateUtc = "yyyy-MM-ddTHH:mm:ssZ";
        private const string DateUtcMs = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public static DateTime? DeserializeIso8601Date(string jsonDateString)
        {
            if (String.IsNullOrEmpty(jsonDateString)) return null;
            return SoapDateTime.Parse(jsonDateString);
        }

        public static string SerializeIso8601Date(DateTime? dateTime)
        {
            return (dateTime.HasValue) ? SoapDateTime.ToString(dateTime.Value): null; 
        }

        // Handle yyyy-MM-dd

        public static DateTime? Deserialize10CharacterIso8601Date(string jsonDateString)
        {
            if (String.IsNullOrEmpty(jsonDateString)) return null;
            if (jsonDateString == "0000-00-00") return null;
            if (jsonDateString.Length != Date10Characters.Length) return DeserializeIso8601Date(jsonDateString);
            return DateTime.ParseExact(jsonDateString, Date10Characters, CultureInfo.InvariantCulture);
        }

        public static string Serialize10CharacterIso8601Date(DateTime? dateTime)
        {
            return (dateTime.HasValue) ? ((DateTime)dateTime).ToString(Date10Characters) : null;
        }

        // Handle UTC date
        public static string SerializeUtcDateTime(DateTime? dateTime)
        {
            return (dateTime.HasValue) ? ((DateTime)dateTime).ToString(DateUtc) : null;
        }

        public static string SerializeUtcDateTimeMilliseconds(DateTime? dateTime)
        {
            return (dateTime.HasValue) ? ((DateTime)dateTime).ToString(DateUtcMs) : null;
        }

    }
}
