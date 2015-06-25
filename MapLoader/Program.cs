using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FifthElement.Kotka.Core.Model;
using FifthElement.Kotka.MapPackageManager.Model;
using FifthElement.Kotka.MapPackageManager.Model.Operations;
using FifthElement.Kotka.MapPackageManager.Util;
using FifthElement.MapLoader.View;

namespace FifthElement.MapLoader
{
    public enum ExitCode
    {
        Error = -1,
        NotRun = 0,
        Success = 1,
        Canceled = 2,
    }

    static class Program
    {

        public static ExitCode ApplicationExitCode { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            //args = new[] { @"C:\ProgramData\Fifth Element Oy\Metsä Group Kotka Maps\_temp\test.lfp" };

            MapLoaderArgument arguments;

            //parse arguments
            try
            {
                string controlFile = args[0];

                //if relative path was passed, assume relative to current working directory
                if (!Path.IsPathRooted(controlFile))
                    controlFile = Path.Combine(Environment.CurrentDirectory, controlFile);

                //check that control file exists
                if (!File.Exists(controlFile))
                    throw new ArgumentException("Input file " + controlFile + " does not exist", "input");

                string json = File.ReadAllText(controlFile, Encoding.UTF8).Trim();
                arguments = JsonHelper.Deserialize<MapLoaderArgument>(json);

                if (!VerifyDiskSpaceRequirements(arguments))
                    return (int)ExitCode.Canceled;

                var operations = new List<MapPackageOperation>();

                //removals first (to enable installing large packages while removing others)
                operations.AddRange(from package in arguments.PackageInfos
                                    where package.ActionEnumValue == MapLoaderAction.Remove
                                    select new RemovalOperation(
                                        new Uri(package.Url), 
                                        package.DisplayName, 
                                        package.EstimatedSize
                                    ));
                //additions after
                operations.AddRange(from package in arguments.PackageInfos
                                    where package.ActionEnumValue == MapLoaderAction.Install
                                    select InstallationOperation.Create(
                                        package.Url, 
                                        package.DisplayName, 
                                        package.EstimatedSize
                                    ));

                var loader = new MapPackageInstaller(arguments.TargetDirectory);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);


                Application.Run(new AppView(loader, operations));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR in Program#Main. Caught, returning error code. " + ex.Message);
                return (int)ExitCode.Error; 
            }

            return (int) ApplicationExitCode;
        }


        public static bool VerifyDiskSpaceRequirements(MapLoaderArgument arguments)
        {
            const int megabyte = 1024*1024;
            const int gigabyte = megabyte * 1024;
            const int minimumAvailable = 200*megabyte;


                            //calculate required disk space
            long requiredDiskSpace = arguments.PackageInfos
                .Where(p => p.ActionEnumValue == MapLoaderAction.Install)
                .Select(p => p.EstimatedSize)
                .Sum();

            requiredDiskSpace -= arguments.PackageInfos
                .Where(p => p.ActionEnumValue == MapLoaderAction.Remove)
                .Select(p => p.EstimatedSize)
                .Sum();

            
            var drive = new DriveInfo(arguments.TargetDirectory.First().ToString());

            long availableDiskSpace = drive.AvailableFreeSpace;
            long remainsAfterInstallation = availableDiskSpace - requiredDiskSpace;
            var recommendedAvailable = Math.Min(gigabyte, drive.TotalSize / 10);

            //does not fit
            if (remainsAfterInstallation <= minimumAvailable)
            {
                string msg = "Vapaa levytila ei riitä valittujen karttojen asentamiseen. ";
                msg += "Järjestelmän toimivuuden kannalta levylle on jäätävä vähintään " +
                       FileSizeUtil.Humanize(minimumAvailable) + " vapaata tilaa.\r\n\r\n";

                msg += "Vapaa tila: " + FileSizeUtil.Humanize(availableDiskSpace) + "\r\n";
                msg += "Tarvittava tila: " + FileSizeUtil.Humanize(requiredDiskSpace) + "\r\n";
                msg += "\r\n";

                msg += "Vapauta levytilaa poistamalla tarpeettomia tiedostoja tai valitse pienempi alue ladattavaksi.";
                MessageBox.Show(msg, "Levytila ei riitä", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            //leaves under 10% free
            if (remainsAfterInstallation <= recommendedAvailable)
            {
                string msg = "Järjestelmän vapaa levytila on vähissä. ";
                msg += "Järjestelmän toimivuuden kannalta levylle suositellaan jätettäväksi vähintään " +
                    FileSizeUtil.Humanize(recommendedAvailable) + " vapaata tilaa.\r\n\r\n";

                msg += "Vapaa tila: " + FileSizeUtil.Humanize(availableDiskSpace) + "\r\n";
                msg += "Tarvittava tila: " + FileSizeUtil.Humanize(requiredDiskSpace) + "\r\n";
                msg += "Vapaaksi jää: " + FileSizeUtil.Humanize(remainsAfterInstallation) + "\r\n";
                msg += "\r\n";
                msg += "Halutessasi voit peruuttaa toiminnon ja valita pienemmän alueen ladattavaksi. Oletko varma, että haluat asentaa valitut kartat?";

                var proceed = MessageBox.Show(msg, "Levytila vähissä", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return proceed == DialogResult.Yes;
            }

            return true;
        }


    }
}
