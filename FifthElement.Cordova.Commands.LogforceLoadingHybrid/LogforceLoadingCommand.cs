using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using FifthElement.Cordova.Commands.LogforceLoadingHybrid;
using FifthElement.Cordova.Core;
using FifthElement.LogforceLoadingHybrid.Core.Model;
using FifthElement.LogforceLoadingHybrid.Core.Util;
using Newtonsoft.Json;
using FifthElement.LogforceLoadingHybrid.Database.Repository;
using System.Net.NetworkInformation;


//Notice that the class is in FifthElement.Cordova.Commands, not FifthElement.Cordova.Commands.KotkaCommand.
//That's because Cordova expects to find the custom plugins in this namespace.
// ReSharper disable CheckNamespace
namespace FifthElement.Cordova.Commands
// ReSharper restore CheckNamespace
{
    public class LogforceLoadingCommand : BaseCommand
    {
        /// <summary>
        /// Here we need to do literally nothing. The only reason this method needs to exist
        /// is that because Cordova uses reflection to find the command classes, if we don't 
        /// make some code reference to any class in this class library, the library never
        /// gets loaded into appdomain, and thus will not be found by reflection.

        /// However, if you do have some initialization to be done, you don't need a separate
        /// method just to do this. As long as you call some code in the command library
        /// before using the command from javascript, it'll be fine.
        /// </summary>
        public static void Init(bool verboseLog, string disabledCommands)
        {
            _isNetworkOnline = NetworkInterface.GetIsNetworkAvailable();
        }

        // ReSharper disable NotAccessedField.Local
        private static bool _isNetworkOnline;
        // ReSharper restore NotAccessedField.Local
        readonly object _lockThis = new object();

        public static bool IsNetWorkOnLine
        {
            get
            {
                return _isNetworkOnline;
            }
        }

        public string GetOnlineStatusString(bool? online)
        {
            if (online == null)
                online = IsNetWorkOnLine;
            var result = new IntegrationStatus { IsOnline = (bool)online };
            return JsonConvert.SerializeObject(result);
        }
        
        public void TestMemory(string arguments)
        {
            try
            {
                var args = JsonConvert.DeserializeObject<TestMemoryParameter>(arguments);
                var response = new TestMemoryResponse { LongSize = args.MemorySize, IntSize = (int)args.MemorySize };
                response.Data = new string('*', response.IntSize);
                DispatchCommandResult(new PluginResult(PluginResult.Status.OK) { Message = JsonConvert.SerializeObject(response) });
            }
            catch (Exception ex)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR) { Message = ex.Message });
            }
        }

        public void GetMultipleResponses(string arguments)
        {
            try
            {
                // if multiple messages need to be sent (e.g., progress info), then keep the Cordova callback 
                for (int i = 0; i < 5; i++) {
                    string message = i.ToString();
                    DispatchCommandResult(new PluginResult(PluginResult.Status.OK, message) { KeepCallback = true });
                    Thread.Sleep(1000);
                }
                DispatchCommandResult(new PluginResult(PluginResult.Status.OK, 5.ToString()) { KeepCallback = false });

            }
            catch (Exception ex)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR) { Message = ex.Message });
            }
        }


        public void GenerateError(string arguments)
        {
            try
            {
                var b = 0;
                int a = 1 / b;
                DispatchCommandResult(new PluginResult(PluginResult.Status.OK) { Message = JsonConvert.SerializeObject("") });
            }
            catch (Exception ex)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR,ex.Message));
            }
        }

        #region Database access commands
        public void GetData(string arguments)
        {
            try
            {
                // <KeyValueStorage> defines _both_ a Json object and a Database row object.
                // Therefore the databaserow that we get can be serialized to Json and returned to the JavaScript
                var args = JsonConvert.DeserializeObject<KeyValueStorage>(arguments);
                var result = new KeyValueRepository().GetValue(args.Key);
                DispatchCommandResult(new PluginResult(PluginResult.Status.OK) { Message = JsonConvert.SerializeObject(result) });
            }
            catch (Exception ex)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, ex.Message));
            }
        }
        public void SaveData(string arguments)
        {
            try
            {
                // <KeyValueStorage> defines _both_ a Json object and a Database row object.
                // Therefore the Json that we get from the JavaScript can be save to the database right after deserialization
                var args = JsonConvert.DeserializeObject<KeyValueStorage>(arguments);
                var repo = new KeyValueRepository();

                var updated = (repo.UpdateValue(args) != 0);
                if (!updated)
                    repo.InsertValue(args);

                DispatchCommandResult(new PluginResult(PluginResult.Status.OK, updated ? "Updated" : "Inserted"));
            }
            catch (Exception ex)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, ex.Message));
            }
        }

        public void GetList(string arguments)
        {
            try
            {
                var result = new KeyValueRepository().GetList(new KeyValueStorage()); // GetList with a "dummy" filter
                DispatchCommandResult(new PluginResult(PluginResult.Status.OK) { Message = JsonConvert.SerializeObject(result) });
            }
            catch (Exception ex)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, ex.Message));
            }
        }
        #endregion


    }
}
