#define INITIALSETTINGSDIALOG
#define WFSPROXYTHREADFIX

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using CefSharp;
using FifthElement.Cordova.Commands;
using FifthElement.Cordova.Commands.LogforceLoadingHybrid;
using FifthElement.Cordova.Commands.LogforceLoadingHybrid.Util;
using FifthElement.LogforceLoadingHybrid.Core;
using FifthElement.LogforceLoadingHybrid.Core.Util;
using FifthElement.LogforceLoadingHybrid.Database;
using FifthElement.LogforceLoadingHybrid.Database.Repository;

namespace FifthElement.LogforceLoadingHybrid
{
    static class Program
    {
        public static MainWindow MainWindow { get; private set; }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main(string[] args)
        {
            //Only allow one running instance of the application
            // -------------------------------------------------------------------
            var processCount = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count();
            if (processCount > 1)
            {
                MessageBox.Show(@"Sovellus on jo käynnissä.
Et voi käynnistää uutta istuntoa.", @"Virhe", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
                return;
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += UncaughtExceptionHandler;

            LogWriter writer = LogWriter.Instance;

            Trace.Listeners.Add(new TextWriterTraceListener(LogWriter.Instance.GetStream()));
            Trace.AutoFlush = true;

            writer.WriteToLog("Starting application.");
            writer.WriteToLog(String.Format("Version: {0}", GetProductVersionText()));
            writer.WriteToLog(String.Format("Application file date: {0}", GetFileDateTime(Assembly.GetExecutingAssembly().Location)));
            writer.WriteToLog(String.Format("Configuration file date: {0}", GetFileDateTime(Path.Combine(AppConfiguration.InstallationPath, "KotkaApp.exe.config"))));
            writer.WriteToLog(String.Format("Installation path: {0}", AppConfiguration.InstallationPath));

            AppConfiguration.SetArguments(args);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            writer.WriteToLog(String.Format("Disabled commands: {0}", AppConfiguration.DisabledCommands));

            writer.WriteToLog(String.Format("Local cache path is set to: {0}", AppConfiguration.CachePath));
            var settings = new CefSettings
            {
                CachePath = AppConfiguration.CachePath,
                LogSeverity = LogSeverity.Verbose
            };

            writer.WriteToLog("Initializing CEF");
            //Initialize CEF, and if fails, just die
            if (!Cef.Initialize(settings))
            {
                writer.WriteToLog("CEF initialisointi epäonnistui. Boottaa kone.");
                Application.Exit();
                return;
            }
            writer.WriteToLog(String.Format("Chromium {0}", Cef.ChromiumVersion));
            writer.WriteToLog(String.Format("CefSharp {0}", Cef.CefSharpVersion));
            writer.WriteToLog(String.Format("Cef {0}", Cef.CefVersion));

            //Sign up for cleanup upon exit
            Application.ApplicationExit += ApplicationExit;

            writer.WriteToLog(String.Format(@"Database path is set to: {0}", AppConfiguration.DatabasePath));

            writer.WriteToLog(@"Initializing local database");
            string databaseFilename = AppConfiguration.DatabasePath;
            var databaseDirectory = Path.GetDirectoryName(databaseFilename);

            // delete existing database file and create a new database
            if (databaseDirectory != null) {
                if (File.Exists(databaseFilename))
                    File.Delete(databaseFilename);
                if (!Directory.Exists(databaseDirectory)) 
                    Directory.CreateDirectory(databaseDirectory);
            }
            if (!Directory.Exists(databaseDirectory)) Directory.CreateDirectory(databaseDirectory);
            DbConnectionManager.Instance.SetDefaultDatabase(databaseFilename, GetProductVersion());




            NetworkStateManager.Instance.Startup(0);
            MainWindow = new MainWindow(String.Format("Kotka v. {0}", GetProductVersionText()));
            Application.Run(MainWindow);
        }

        /// <summary>
        /// Handles cleanup upon application exit
        /// </summary>
        static void ApplicationExit(object sender, EventArgs e)
        {
            //Upon closing we should clean up CEF to release unmanaged resources
            Cef.Shutdown();
        }

        /// <summary>
        /// Handles uncaught exceptions
        /// </summary>
        static void UncaughtExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            LogWriter.Instance.WriteToLog(String.Format(@"Uncaught exception: {0}", e.Message));
            LogWriter.Instance.WriteToLog(String.Format(@"Runtime terminating: {0}", args.IsTerminating));
        }

        private static string GetProductVersion()
        {
            var info = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return String.Format("{0}.{1}.{2}", info.ProductMajorPart, info.ProductMinorPart, info.ProductBuildPart);
        }

        /// <summary>
        /// Gets version information as string
        /// </summary>
        private static string GetProductVersionText()
        {
            return String.Format("{0} - {1}", GetProductVersion(), AppConfiguration.EnvironmentText);
        }

        private static DateTime GetFileDateTime(string fileName)
        {
            return File.GetCreationTime(fileName);
        }


    }
}
