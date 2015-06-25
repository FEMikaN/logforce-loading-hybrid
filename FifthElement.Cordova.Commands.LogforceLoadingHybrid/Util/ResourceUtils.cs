using System;
using System.IO;
using System.Reflection;

namespace FifthElement.Cordova.Commands.LogforceLoadingHybrid.Util
{
    public static class ResourceUtils
    {

        public static string GetStringFromResource(string resourceId)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceId))
            {
                if (stream == null) return String.Empty;
                using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
