using System;
using FifthElement.Cordova.Commands.LogforceLoadingHybrid;
using FifthElement.Cordova.Commands.LogforceLoadingHybrid.Util;
using FifthElement.Cordova.Core;
using FifthElement.LogforceLoadingHybrid.Core.Model;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace FifthElement.Cordova.Commands
// ReSharper restore CheckNamespace
{
    public class NetworkStateCommand : BaseCommand
    {
        private LogforceLoadingCommand _kotkaInternal;

        public NetworkStateCommand()
        {
            _kotkaInternal = new LogforceLoadingCommand();
        }
        public DateTime LastWatcherUpdate { get; private set; }

        public void AddWatch(string argumentString)
        {
            var args = JsonConvert.DeserializeObject<AddWatchCommandArgs>(argumentString);
            var onelineStatus = _kotkaInternal.GetOnlineStatusString(null);

            var added = NetworkStateManager.Instance.AddWatcherCommand(args.ListenerId, this);
            ExecuteCallback(new Callback(added,onelineStatus), added);
        }

        /// <summary>
        /// Removes the specified command as watcher
        /// </summary>
        /// <param name="argumentString"></param>
        public void ClearWatch(string argumentString)
        {
            var args = JsonConvert.DeserializeObject<ClearWatchCommandArgs>(argumentString);
            var onelineStatus = _kotkaInternal.GetOnlineStatusString(null);

            var cleared = NetworkStateManager.Instance.ClearWatcherCommand(args.ListenerId);
            ExecuteCallback(new Callback(cleared,onelineStatus));
        }

        public void UpdateWatcherState(bool isOnline)
        {
            var onelineStatus = _kotkaInternal.GetOnlineStatusString(isOnline);
            var callback = new Callback(true, onelineStatus);
            ExecuteCallback(callback, true);
        }
        /// <summary>
        /// Executes a callback function
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        private void ExecuteCallback(Callback callback, bool keepCallback = false)
        {
            var status = callback.Success
                ? PluginResult.Status.OK
                : PluginResult.Status.ERROR;

            var result = new PluginResult(status, callback.ReturnValue) { KeepCallback = keepCallback };
            base.DispatchCommandResult(result);
        }

    }
}
