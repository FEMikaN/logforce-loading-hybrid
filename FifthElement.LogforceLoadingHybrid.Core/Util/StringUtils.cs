using System;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public static class StringUtils
    {
        public static string CopyLeft(string text, int length)
        {
            if (String.IsNullOrEmpty(text))
                return text;
            return text.Length < length ? text : text.Substring(0, length);
        }

    }
}
