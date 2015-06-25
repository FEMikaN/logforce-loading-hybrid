using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Timers;
using FifthElement.LogforceLoadingHybrid.Core.Util;
using Timer = System.Timers.Timer;

namespace FifthElement.Cordova.Commands.LogforceLoadingHybrid.Util
{
    public sealed class NetworkStateManager : IDisposable
    {
        static Timer _timer;

        // ReSharper disable NotAccessedField.Local
        private static bool _isNetworkOnline;
        // ReSharper restore NotAccessedField.Local

        private readonly Dictionary<string, NetworkStateCommand> _watchers = new Dictionary<string, NetworkStateCommand>();

        public event WebViewStateHandler CheckWebviewOnlineState;
        public delegate bool? WebViewStateHandler(NetworkStateManager m, LogEvent e);

        /// <summary>
        /// Starts up Network device listener. This method should be called once at application startup
        /// </summary>
        /// <param name="updateFrequency">Refresh rate of this manager</param>
        public void Startup(int updateFrequency)
        {
            if (updateFrequency > 0)
            {
            _timer = new Timer(updateFrequency);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true; // Enable it
            }
            _isNetworkOnline = NetworkInterface.GetIsNetworkAvailable();
            
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
        }

        void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            _isNetworkOnline = e.IsAvailable;

            if ((CheckWebviewOnlineState != null) && (_isNetworkOnline))
            {
                for (var milliseconds = 0; milliseconds < 10000; milliseconds += 100)
                {
                    var webviewOnLineState = CheckWebviewOnlineState(null, null);
                    if (!NetworkInterface.GetIsNetworkAvailable())
                    {
                        LogWriter.Instance.WriteToLog(String.Format("Network state change: Network adapter changed back offline @ {0} ms.", milliseconds));
                        return;
                    }
                    if ((webviewOnLineState != null) && ((bool)webviewOnLineState))
                    {
                        LogWriter.Instance.WriteToLog(String.Format("Network state change: Adapter: {0}, Webview: {1}, Online state recovered in {2} ms.", _isNetworkOnline, webviewOnLineState, milliseconds));
                        break;
                    }
                    Thread.Sleep(100);
                }
            }

            UpdateWatchers();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateWatchers();
        }
       
        /// <summary>
        /// Shuts down Network device listener. This method should be called once at application shutdown
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// Adds a command as a watcher
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool AddWatcherCommand(string id, NetworkStateCommand command)
        {
            if (_watchers.ContainsKey(id))
                return false;

            _watchers.Add(id, command);
            return true;
        }

        /// <summary>
        /// Removes a watcher by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ClearWatcherCommand(string id)
        {
            try
            {
                NetworkStateCommand watcher;
                if (_watchers.TryGetValue(id, out watcher))
                {
                    watcher.Dispose();
                    _watchers.Remove(id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

  
        /// <summary>
        /// Updates all registered watchers with the most recent received location
        /// </summary>
        private void UpdateWatchers()
        {
            if (_watchers.Count <= 0) return;
            foreach (var watcher in _watchers.Values.ToList())
            {
                watcher.UpdateWatcherState(_isNetworkOnline);
            }
        }

        #region Singleton

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static readonly NetworkStateManager Instance = new NetworkStateManager();

        

        /// <summary>
        /// Parameterless private instance constructor
        /// </summary>
        NetworkStateManager() { }

        /// <summary>
        ///  Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        /// </summary>
        static NetworkStateManager() { }


        #endregion Singleton

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Shutdown();
        }

        #endregion

    }
}
