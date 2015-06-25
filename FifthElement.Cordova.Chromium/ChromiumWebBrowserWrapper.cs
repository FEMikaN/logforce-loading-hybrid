using System;
using System.Collections.Generic;
using System.Diagnostics;
using CefSharp.WinForms;
using FifthElement.Cordova.Core;


namespace FifthElement.Cordova.Chromium
{
    public class ChromiumWebBrowserWrapper : WebBrowser
    {
        private readonly Action<Action> _guiInvoke;
        private readonly NativeExecution _nativeExecution;
        private readonly ChromiumWebBrowser _webView;

        private readonly List<string> _commandList;

        public ChromiumWebBrowserWrapper(ChromiumWebBrowser webView)
        {
            _webView = webView;
            _nativeExecution = new NativeExecution(this);
            _guiInvoke = invoke => _webView.Invoke(invoke);
            _commandList = new List<string>();
            //Register this object as window.external to mirror the IE8 mobile browser naming
            _webView.RegisterJsObject("external", this);
            //Obsolete in CEF 3: Cef.RegisterJsObject("external", this);
        }

        /// <summary>
        /// Receives calls from javascript and dispatches them to command processors
        /// </summary>
        /// <param name="message">The message string sent from javascript</param>
        public override void Notify(string message)
        {
            //Parse the message
            CordovaCommandCall call = CordovaCommandCall.Parse(message);

            //If parsing did not succeed
            if (call == null)
                Debug.WriteLine("ScriptNotify :: " + message);
            
            //If the message is sent by Cordova CoreEvents, ignore
            else if (call.Service == "CoreEvents")
                Debug.WriteLine("CoreEvents :: " + message);

            //Process the message
            else
                _nativeExecution.ProcessCommand(call);
            
        }

        /// <summary>
        /// Executes a globally accessible javascript function with provided arguments
        /// </summary>
        /// <param name="scriptName">Name of the script to execute</param>
        /// <param name="args">Arguments</param>
        /// <returns></returns>
        public override string InvokeScript(string scriptName, params string[] args)
        {
            //Build the function call -> scriptName(arg1, arg2, ...)
            string script = String.Format("{0}({1})", scriptName, String.Join(",", args));
            _guiInvoke(() => _webView.ExecuteScriptAsync(script));
            return String.Empty;
        }
        public override void AddCommandId(string commandId)
        {
            _commandList.Add(commandId);
        }
        public override void RemoveCommandId(string commandId)
        {
            _commandList.Remove(commandId);
        }
        public string ExecutingCommands()
        {
            return String.Join(",", _commandList);
        }
    }
}