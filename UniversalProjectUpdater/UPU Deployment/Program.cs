using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace UPU_Deployment
{
    static class Program
    {

        #region Vars

        private static Process updateProcess;

        private static string ApplicationName;
        private static string ApplicationDirectory;
        private static string DownloadURL;
        private static string TempDownloadLocation;

        #endregion Vars
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationName = args[0];
            ApplicationDirectory = args[1];
            DownloadURL = args[2];

            TempDownloadLocation = Application.StartupPath + @"\" + ApplicationName + ".zip";

            updateProcess = Process.GetProcessesByName(ApplicationName).FirstOrDefault();

            if (updateProcess == null) { MessageBox.Show("Couldn't find update process! Exiting", "UPU Deployment"); return; }

            DownloadUpdate();
            UpdateLoop();
        }

        private static void DownloadUpdate()
        {
            if (File.Exists(TempDownloadLocation)) File.Delete(TempDownloadLocation);

            using (WebClient webClient = new WebClient())
            {
                Uri URL = new Uri(DownloadURL);

                try
                {
                    webClient.DownloadFile(URL, TempDownloadLocation);
                } catch { }
            }
        }

        private static void UpdateLoop()
        {
            while (true)
            {
                if (updateProcess.HasExited)
                {
                    updateProcess = Process.GetProcessesByName(ApplicationName).FirstOrDefault();

                    if (updateProcess == null)
                    {
                        ExtractUpdate();
                    }
                }

                Thread.Sleep(5000);
            }
        }

        private static void ExtractUpdate()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(ApplicationDirectory);

                foreach(FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach(DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            } catch { }

            ZipFile.ExtractToDirectory(TempDownloadLocation, ApplicationDirectory);

            Environment.Exit(1);
        }
    }

}
