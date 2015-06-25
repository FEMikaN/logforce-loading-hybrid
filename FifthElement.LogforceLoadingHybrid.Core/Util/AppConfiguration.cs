using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using FifthElement.LogforceLoadingHybrid.Core.Model;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public static class AppConfiguration
    {
        private static Dictionary<string, string> _commandlineArguments;
        public static void SetArguments(string[] args)
        {
            _commandlineArguments = new Dictionary<string, string>();
            foreach (var arg in args)
            {
                const char keyValueDelimiter = '=';
                if (arg.IndexOf(keyValueDelimiter) == -1) continue;
                var key = arg.Split(keyValueDelimiter)[0];
                var value = arg.Split(keyValueDelimiter)[1];
                _commandlineArguments.Add(key,value);
            }
        }

        private static string GetInstallationPath()
        {
            string codeBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            Uri uri = codeBase != null ? new Uri(codeBase) : new Uri("");
            return uri.LocalPath;
        }

        public static string InstallationPath
        {
            get { return GetInstallationPath(); }
        }

        public static bool DisplayDeveloperTools
        {
            get
            {
                return GetBool("displayDevTools");
            }
        }

        public static int GpsUpdateFrequency
        {
            get
            {
                int freq;
                return Int32.TryParse(Get("gpsUpdateFrequency"), out freq) ? freq : 3000;
            }
        }

        public static string AppUrl
        {
            get
            {
                return Path.Combine(GetInstallationPath(), GetFromConfigOrParameter("appUrl"));
            }
        }

        public static string NoContentUrl
        {
            get
            {
                return Path.Combine(GetInstallationPath(), GetFromConfigOrParameter("noContentUrl"));
            }
        }

        public static string DisabledCommands
        {
            get
            {
                return GetFromConfigOrParameter("disabledCommands");
            }
        }

        public static string ForceOtherUser
        {
            get
            {
                return GetFromConfigOrParameter("forceOtherUser");
            }
        }


        public static bool ForceOffline
        {
            get
            {
                return GetBool("forceOffline");
            }
        }

        public static bool CheckTablesAtStartup
        {
            get
            {
                return GetBool("checkTablesAtStartup");
            }
        }

        public static bool ForceVerboseLog
        {
            get
            {
                return GetBool("forceVerboseLog");
            }
        }


        public static string CachePath
        {
            get
            {
                return Path.Combine(GetInstallationPath(), Get("cachePath"));
            }
        }


        public static string DatabasePath
        {
            get
            {
                return Path.Combine(GetInstallationPath(), GetFromConfigOrParameter("databasePath"));
            }
        }

        public static string MapInstallPath
        {
            get
            {
                var path = Environment.ExpandEnvironmentVariables(Get("mapInstallPath"));
                return Path.Combine(GetInstallationPath(), path);
            }
        }

        public static string ConfigurationsPath
        {
            get
            {
                var path = Environment.ExpandEnvironmentVariables(Get("configurationsPath"));
                return Path.Combine(GetInstallationPath(), path);
            }
        }

        public static string OfflineRasterPackageName
        {
            get
            {
                return Path.Combine(GetInstallationPath(), Get("offlineRasterPackageName"));
            }
        }


        public static string MapLoaderPath
        {
                
            get
            {
                var path = Environment.ExpandEnvironmentVariables(Get("mapLoaderPath"));
                return Path.Combine(GetInstallationPath(), path);
            }
        }

        public static string SapAttachmentsCachePath
        {
            get
            {
                var path = Environment.ExpandEnvironmentVariables(Get("sapAttachmentsCachePath"));
                return Path.Combine(GetInstallationPath(), path);
            }
        }

        public static bool ShowCopyAddressButton
        {
            get
            {
                return GetBool("showCopyAddrressButton");
            }
        }

        public static string EnvironmentText
        {
            get
            {
                return Get("environmentText");
            }
        }


        public static string ServiceRoot
        {
            get { return Get("serviceRoot"); }
        }

        private static string ApplicationDatabaseVersion
        {
            get { return Get("databaseVersion"); }
        }
        public static bool ForceDatabaseUpdate
        {
            get {
                return GetBool("forceDatabaseUpdate");
            }
        }

        private static string Get(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        private static bool GetBool(string key)
        {
            bool returnValue;
            return Boolean.TryParse(Get(key), out returnValue) && returnValue;
        }

        private static string GetFromConfigOrParameter(string key)
        {
           return _commandlineArguments.ContainsKey(key) ? _commandlineArguments[key] : Get(key);
        }

        private static bool GetBoolFromConfigOrParameter(string key)
        {
            bool returnValue;
            return Boolean.TryParse(GetFromConfigOrParameter(key), out returnValue) && returnValue;
        }

    }
}
