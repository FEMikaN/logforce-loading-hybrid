#define WFSPROXYTHREADFIX

using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using FifthElement.Cordova.Chromium;
using FifthElement.Cordova.Commands;
using FifthElement.Cordova.Commands.LogforceLoadingHybrid;
using FifthElement.Cordova.Commands.LogforceLoadingHybrid.Util;
using FifthElement.LogforceLoadingHybrid.Core;
using FifthElement.LogforceLoadingHybrid.Core.Util;


namespace FifthElement.LogforceLoadingHybrid
{
    public partial class MainWindow : Form, IWebBrowserService, IRequestHandler
    {
        private readonly string _url;
        private ChromiumWebBrowser _webView;
        // ReSharper disable NotAccessedField.Local
        private ChromiumWebBrowserWrapper _wrapper;
        // ReSharper restore NotAccessedField.Local

        public SynchronizationContext SynchronizationContext { get; private set; }

        public MainWindow(string title)
        {
            LogWriter writer = LogWriter.Instance;
            writer.WriteToLog("Calling InitializeComponent", true);
            InitializeComponent();

            //Path to the document to load into the browser
            _url = AppConfiguration.AppUrl;

            writer.WriteToLog("Calling InitializeCommands", true);
            InitializeCommands();

            writer.WriteToLog("Calling InitializeWebView", true);
            InitializeWebView();

            writer.WriteToLog("Calling InitializeUi", true);
            InitializeUi(title);

            SynchronizationContext = SynchronizationContext.Current;
        }


        /// <summary>
        /// Initializes the WebView component and adds it to the form.
        /// </summary>
        private void InitializeWebView()
        {
            LogWriter writer = LogWriter.Instance;

            //There's plenty of settings to choose from. These seem to work.
            writer.WriteToLog(String.Format("Index page for content: {0}", _url));

            //Create the browser
            writer.WriteToLog("Creating browser", true);
            _webView = new ChromiumWebBrowser(_url) { Dock = DockStyle.Fill };
            _webView.BrowserSettings.WebSecurityDisabled = true;
            _webView.BrowserSettings.FileAccessFromFileUrlsAllowed = true;
            _webView.BrowserSettings.UniversalAccessFromFileUrlsAllowed = true;
            _webView.BrowserSettings.ApplicationCacheDisabled = true;
            _webView.BrowserSettings.JavaDisabled = true;
            _webView.BrowserSettings.TextAreaResizeDisabled = true;

            _webView.RequestHandler = this;

            //Register as the Cordova communication channel
            writer.WriteToLog("Registering as the Cordova communication channel", true);
            _wrapper = new ChromiumWebBrowserWrapper(_webView);

            ClearCookies();

            Controls.Add(_webView);

            NetworkStateManager.Instance.CheckWebviewOnlineState += CheckWebviewOnlineState;

        }

        private void InitializeCommands()
        {
#if DEBUG
// ReSharper disable ConvertToConstant.Local
            var verboseLog = true;
// ReSharper restore ConvertToConstant.Local
#else
            var verboseLog = AppConfiguration.ForceVerboseLog;
#endif
// ReSharper disable ConditionIsAlwaysTrueOrFalse
            LogforceLoadingCommand.Init(verboseLog, AppConfiguration.DisabledCommands);
// ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        /// <summary>
        /// Initializes the UI to have desired look and feel.
        /// </summary>
        private void InitializeUi(string title)
        {
#if DEBUG
            ReloadPage.Visible = true;
            ShowDevTools.Visible = true;
            CopyHref.Visible = true;
            title += @" DEBUG";
#else
            ReloadPage.Visible = true;
            ShowDevTools.Visible = true;
            CopyHref.Visible = AppConfiguration.ShowCopyAddressButton;
#endif
            title += String.Format(" - Chromium {0}", Cef.ChromiumVersion);
            title += String.Format(" - CefSharp {0}", Cef.CefSharpVersion);
            Text = title;
        }

        private void OnShowDevToolsClicked(object sender, EventArgs e)
        {
            _webView.ShowDevTools();
        }

        private bool? CheckWebviewOnlineState(object m, LogEvent e)
        {
            if (_webView == null)
                return null;
            if (_webView.IsLoading)
                return null;

            var task = _webView.EvaluateScriptAsync(@"navigator.onLine");
            task.Wait();
            return Convert.ToBoolean(task.Result.Result);
        }
        private void OnReloadPageClicked(object sender, EventArgs e)
        {
            _webView.Reload(ignoreCache: true);
        }

        /// <summary>
        /// Clears application cookies
        /// </summary>
        public void ClearCookies()
        {
            const string nom = "";
            Cef.DeleteCookies(nom, nom);
        }

        /// <summary>
        /// Reloads the initial url (not current page)
        /// </summary>
        public void Reload()
        {
            _webView.Load(_url);
        }

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect, bool isMainFrame)
        {
            return false;
        }

        public bool OnCertificateError(IWebBrowser browser, CefErrorCode errorCode, string requestUrl)
        {
            throw new NotImplementedException();
        }

        public bool OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, WebPluginInfo info)
        {
            throw new NotImplementedException();
        }
        public void OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
            throw new NotImplementedException();
        }

        //private void ProcessFileResponse(IRequestResponse requestResponse, LocalhostFileHelper localhostFileHelper)
        //{
        //    var fileName = localhostFileHelper.GetLocalFilename;
        //    if (!File.Exists(fileName))
        //        return;
            
        //    LogWriter.Instance.WriteToLog(String.Format("Opening response stream: {0}", fileName));
        //    var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        //    LogWriter.Instance.WriteToLog("Responding with response stream");
        //    requestResponse.RespondWith(fileStream, "application/json");
        //}

        private void ThreadPoolCallback(Object threadContext)
        {
            throw new NotImplementedException();
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequest request, bool isMainFrame)
        {
            return false;
        }

        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme,
                                       ref string username, ref string password)
        {
            return false;
        }

        public void OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status)
        {
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _webView.CloseDevTools();
            LogWriter.Instance.WriteToLog(String.Format(@"MainWindow_FormClosing. Commands in queue: {0}", _wrapper.ExecutingCommands()));
        }

        private void CopyHref_Click(object sender, EventArgs e)
        {
            var pageUrl = _webView.Address;
            var uri = new Uri(pageUrl);
            var uripart = String.IsNullOrEmpty(uri.Fragment) ? uri.PathAndQuery : uri.Fragment;
            Clipboard.SetText(String.Format(@"window.location.href='{0}'", uripart));
        }
    }
}
