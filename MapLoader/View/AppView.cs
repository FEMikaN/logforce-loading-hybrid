using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FifthElement.Kotka.Core;
using FifthElement.Kotka.MapPackageManager.Model;
using FifthElement.Kotka.MapPackageManager.Model.Events;
using FifthElement.Kotka.MapPackageManager.Model.Operations;
using FifthElement.MapLoader.ViewModel;
using Timer = System.Threading.Timer;

namespace FifthElement.MapLoader.View
{
    public partial class AppView : Form
    {
        public MapPackageInstaller Installer { get; private set; }
        public Queue<MapPackageOperation> Operations { get; private set; }
        public IDictionary<Uri, PackageListItem> PackageListItems { get; private set; }


        private const int MaxRetryCount = 5;
        private const int RetryWaitSeconds = 10;

        private bool _running;
        private bool _verified;
        private BackgroundWorker _worker;
        private Timer _retryTimer;

        public AppView(MapPackageInstaller installer, IList<MapPackageOperation> operations)
        {
            
            InitializeComponent();

           //Sources = sources;
            Installer = installer;

            Operations = new Queue<MapPackageOperation>(operations);
            PackageListItems = operations
                .Select(op => new PackageListItem(op))
                .ToDictionary(key => key.Model.SourceUri);

            foreach (var item in PackageListItems)
                PackageListPanel.Controls.Add(item.Value.View);

            Closing += BeforeClose;

            //sign up to package loading events
            Installer.BeginPackage     += OnBeginPackage;
            Installer.BeginPackageDownload += OnBeginPackageDownload;
            Installer.PackageProgress += OnPackageProgress;
            Installer.PackageError     += OnPackageError;
            Installer.EndPackage       += OnEndPackage;
            Installer.SkipPackage      += OnSkipPackage;
            Installer.DownloadPackage  += OnDownloadPackage;
        }


        public void Start()
        {
            _running = true;
            StartButton.Enabled = false;

            //load first package
            LoadPackage();        
        }

        public void Reset()
        {
            BeginInvoke(new Action(() => 
            {
                _running = false;
                StartButton.Enabled = true;
            }));
        }

        public bool AllowClose()
        {
            bool allow = true;

            //if we have any unfinished business
            if (_running)
            {
                var result = MessageBox.Show("Karttojen lataus on kesken. Oletko varma, että haluat keskeyttää latauksen?",
                                "Keskeytä?", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    Program.ApplicationExitCode = ExitCode.Canceled;
                } 
                else
                {
                    allow = false;
                }
                    

            }

            return allow;
        }



        void LoadPackage()
        {
            //aaaand were done!
            if (!Operations.Any())
            {
                AllPackagesCompleted();
            }

            //clear previous
            if(_worker != null)
            {
                if (_worker.WorkerSupportsCancellation)
                    _worker.CancelAsync();
                _worker.Dispose();
            }

            //run loading on a background thread
            _worker = new BackgroundWorker();
            _worker.DoWork += DoWork;
            _worker.RunWorkerAsync();            
        }



        /// <summary>
        /// Loads next source in the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DoWork(object sender, DoWorkEventArgs e)
        {
            if (!_verified)
            {
                foreach (var source in Operations)
                {
                    var listItem = PackageListItems[source.SourceUri];

                    BeginInvoke(new Action(() => listItem.SetStatus(PackageListItem.LoadingStatus.Verifying)));

                    var verified = source.VerifyPackage();

                    BeginInvoke(new Action(() => listItem.SetStatus(verified
                        ? PackageListItem.LoadingStatus.Verified
                        : PackageListItem.LoadingStatus.Failure)));
                    if (source.SourceUri.AbsoluteUri == Constants.EmptyUri)
                    {
                        
                    } else
                    if (!verified)
                    {
                        MessageBox.Show(
                            "Karttapaketteja ei voitu lukea. Varmista, että internet-yhteytesi on kunnossa tai levyasemasi on kiinnitettynä ja yritä uudelleen painamalla Aloita-painiketta.",
                            "Yhteysvirhe", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        this.Reset();
                        return;
                    }
                }

                _verified = true;

            }
            if (Operations.Count == 0) return;
            Installer.ExecuteOperation(Operations.Peek());
        }


        private void AllPackagesCompleted()
        {
            _running = false;
            Program.ApplicationExitCode = ExitCode.Success;
        }


#region Loader Event handlers
        /// <summary>
        /// Handles package loading progress events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPackageProgress(object sender, MapPackageProgressEventArgs e)
        {
            //update the progress indicator
            BeginInvoke(new Action(() => 
            {
                CurrentPackageProgressBar.Value = Math.Min((int)(e.ReadySize / 1024), CurrentPackageProgressBar.Maximum);
                FileStatusText.Text = String.Format("{0:0.0}%", e.ReadySize/(double)e.TotalSize * 100);
            }));
        }
        
        private void OnPackageError(object sender, MapPackageErrorEventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                var item = PackageListItems[e.PackageUri];
                item.SetStatus(PackageListItem.LoadingStatus.Error);
                PackageStatusText.Text = "Latausvirhe! Yritetään uudestaan hetken kuluttua.";
                FileStatusText.Text = "";
            }));

            Delay(LoadPackage, RetryWaitSeconds);
        }

        /// <summary>
        /// Handles package loading begin event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnBeginPackage(object sender, MapPackageLoadingEventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                var item = this.PackageListItems[e.PackageUri];
                item.SetStatus(PackageListItem.LoadingStatus.Loading);

                PackageStatusText.Text = "Asennetaan...";
                FileStatusText.Text = e.PackageUri.ToString();
                
                CurrentPackageProgressBar.Maximum = (int) (e.PackageSize / 1024);
            }));
        }

        /// <summary>
        /// Handles package loading begin download event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnBeginPackageDownload(object sender, MapPackageLoadingEventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                var item = this.PackageListItems[e.PackageUri];
                item.SetStatus(PackageListItem.LoadingStatus.Loading);

                PackageStatusText.Text = "Ladataan karttapaketti palvelusta...";
                FileStatusText.Text = e.PackageUri.ToString();

                CurrentPackageProgressBar.Maximum = (int)(e.PackageSize / 1024);
            }));
        }



        /// <summary>
        /// Handles package loading end event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndPackage(object sender, MapPackageLoadingEventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                var item = this.PackageListItems[e.PackageUri];
                item.SetStatus(PackageListItem.LoadingStatus.Success);

                PackageStatusText.Text = "Valmis!";
                CurrentPackageProgressBar.Maximum = (int)(e.PackageSize / 1024);

                //remove the first item from the source list (ready)
                Operations.Dequeue();

                //process next package
                LoadPackage();
            }));
        }

        /// <summary>
        /// Handles package loading end event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSkipPackage(object sender, MapPackageLoadingEventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                var item = this.PackageListItems[e.PackageUri];
                PackageStatusText.Text = @"Ei löytynyt";

                //remove the first item from the source list (ready)
                Operations.Dequeue();

                //process next package
                LoadPackage();
            }));
        }

        /// <summary>
        /// Handles download progress event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDownloadPackage(object sender, MapPackageLoadingEventArgs e)
        {
            BeginInvoke(new Action(() =>
                {
                    //var item = this.PackageListItems[e.PackageUri];
                    //item.SetDownloadStatusText(String.Format("{0}/{1} MB", e.DownloadProgress / (1024 * 1024),
                    //                                    e.PackageSize/(1024*1024)));
                    CurrentPackageProgressBar.Value = Math.Min((int)(e.DownloadProgress / 1024), CurrentPackageProgressBar.Maximum);
                    FileStatusText.Text = String.Format("{0:0.0}%", e.DownloadProgress / (double)e.PackageSize * 100);
                }));
        }



#endregion Loader Event handlers

#region Delay

        /// <summary>
        /// Runs the specified <paramref name="action"/> in <paramref name="seconds"/> seconds 
        /// </summary>

        private void Delay(Action action, int seconds)
        {
            _retryTimer = new Timer(DelayCallback, action, seconds * 1000, Timeout.Infinite);
        }

        /// <summary>
        /// TimerCallback for the retry timer
        /// </summary>
        /// <param name="o"></param>
        private void DelayCallback(object o)
        {
            var action = (Action)o;
            _retryTimer.Dispose();
            _retryTimer = null;

            action();
        }

#endregion Delay

#region Form Event handlers

        void BeforeClose(object sender, CancelEventArgs e)
        {
            e.Cancel = !AllowClose();
        }

        /// <summary>
        /// Handles the close button click. Closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the start button click. Starts loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartButtonClick(object sender, EventArgs e)
        {
            Start();
        }

#endregion Form event handlers


    }
}
