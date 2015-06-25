using System;
using System.Diagnostics;
using System.Threading;

namespace FifthElement.Cordova.Core
{
    /// <summary>
    /// Implements logic to execute native command and return result back.
    /// All commands are executed asynchronous.
    /// </summary>
    public class NativeExecution
    {
        /// <summary>
        /// Reference to web part where application is hosted
        /// </summary>
        private readonly WebBrowser webBrowser;

        /// <summary>
        /// Creates new instance of a NativeExecution class. 
        /// </summary>
        /// <param name="browser">Reference to web part where application is hosted</param>
        public NativeExecution(WebBrowser browser)
        {
            if (browser == null)
            {
                throw new ArgumentNullException("browser");
            }
            webBrowser = browser;
        }

        /// <summary>
        /// Returns where application is running on emulator
        /// </summary>
        /// <returns>True if running on emulator, otherwise False</returns>
        public static bool IsRunningOnEmulator()
        {
            return false;
        }

        /// <summary>
        /// Executes command and returns result back.
        /// </summary>
        /// <param name="commandCallParams">Command to execute</param>
        public void ProcessCommand(CordovaCommandCall commandCallParams)
        {
            if (commandCallParams == null)
            {
                throw new ArgumentNullException("commandCallParams");
            }
            webBrowser.AddCommandId(commandCallParams.CallbackId);
            try
            {
                BaseCommand bc = CommandFactory.CreateByServiceName(commandCallParams.Service);

                if (bc == null)
                {
                    OnCommandResult(commandCallParams.CallbackId,
                                    new PluginResult(PluginResult.Status.CLASS_NOT_FOUND_EXCEPTION));
                    return;
                }

                EventHandler<PluginResult> onCommandResultHandler =
                    (o, res) => OnCommandResult(commandCallParams.CallbackId, res);

                bc.OnCommandResult += onCommandResultHandler;

                EventHandler<ScriptCallback> onCustomScriptHandler =
                    (o, script) => InvokeScriptCallback(script);


                bc.OnCustomScript += onCustomScriptHandler;

                // TODO: alternative way is using thread pool (ThreadPool.QueueUserWorkItem) instead of 
                // new thread for every command call; but num threads are not sufficient - 2 threads per CPU core


                var thread = new Thread(func =>
                                        {
                                            try
                                            {
                                                bc.InvokeMethodNamed(commandCallParams.Action, commandCallParams.Args);
                                            }
                                            catch (Exception)
                                            {
                                                bc.OnCommandResult -= onCommandResultHandler;
                                                bc.OnCustomScript -= onCustomScriptHandler;

                                                Debug.WriteLine("failed to InvokeMethodNamed :: " +
                                                                commandCallParams.Action + " on Object :: " +
                                                                commandCallParams.Service);

                                                OnCommandResult(commandCallParams.CallbackId,
                                                                new PluginResult(PluginResult.Status.INVALID_ACTION));

                                                return;
                                            }
                                        });

                thread.Start();
            }
            catch (Exception ex)
            {
                // ERROR
                Debug.WriteLine(String.Format("Unable to execute command :: {0}:{1}:{3} ",
                                              commandCallParams.Service, commandCallParams.Action, ex.Message));

                OnCommandResult(commandCallParams.CallbackId, new PluginResult(PluginResult.Status.ERROR));
                return;
            }
        }

        /// <summary>
        /// Handles command execution result.
        /// </summary>
        /// <param name="callbackId">Command callback identifier on client side</param>
        /// <param name="result">Execution result</param>
        private void OnCommandResult(string callbackId, PluginResult result)
        {
            #region  args checking

            if (result == null)
            {
                Debug.WriteLine("OnCommandResult missing result argument");
                return;
            }

            if (String.IsNullOrEmpty(callbackId))
            {
                Debug.WriteLine("OnCommandResult missing callbackId argument");
                return;
            }

            #endregion

            var callbackIdToRemove = callbackId;

            int ignore;
            if(!Int32.TryParse(callbackId, out ignore) && !(callbackId.StartsWith("'") || callbackId.StartsWith("\"")))
            {
                callbackId = "'" + callbackId + "'";
            }

            string status = ((int) result.Result).ToString();
            string jsonResult = result.ToJSONString();

            ScriptCallback scriptCallback = String.IsNullOrEmpty(result.Cast)
                ? new ScriptCallback("CordovaCommandResult", new[] { status, callbackId, jsonResult})
                : new ScriptCallback("CordovaCommandResult", new[] { status, callbackId, jsonResult, result.Cast});

            InvokeScriptCallback(scriptCallback);

            webBrowser.RemoveCommandId(callbackIdToRemove);

        }

        /// <summary>
        /// Executes client java script
        /// </summary>
        /// <param name="script">Script to execute on client side</param>
        private void InvokeScriptCallback(ScriptCallback script)
        {
            if (script == null)
            {
                throw new ArgumentNullException("script");
            }

            if (String.IsNullOrEmpty(script.ScriptName))
            {
                throw new ArgumentNullException("ScriptName");
            }


            try
            {
                //Debug.WriteLine("InvokingScript::" + script.ScriptName + " with args ::" + script.Args[0] + ", " + script.Args[1] + ", " + script.Args[2]);
                webBrowser.InvokeScript(script.ScriptName,
                                        script.Args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    "Exception in InvokeScriptCallback :: " +
                    ex.Message);
            }
        }
    }
}