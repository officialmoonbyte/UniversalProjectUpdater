using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Moonbyte.Net.UniversalProjectUpdater
{
    public class UniversalProjectUpdater
    {

        #region Vars

        UniversalClient.UniversalClient MoonbyteServerConnection;

        string DeploymentDirectory = @"C:\Moonbyte\UniversalProjectUpdater\";
        string DeploymentFile = @"C:\Moonbyte\UniversalProjectUpdater\UPUDeployment.exe";
        string DownloadURI = "https://moonbyte.net/Download/UPUDeployment.exe";
        string applicationName;


        #endregion Vars

        #region Initialization

        /// <summary>
        /// Initiailize the connections and values of UniversalProjectFramework
        /// </summary>
        public UniversalProjectUpdater(string ApplicationName, string ServerIP = "moonbyte.net", int ServerPort = 7777)
        {
            string externalip = new WebClient().DownloadString("http://icanhazip.com");
            if (externalip.Contains(Dns.GetHostAddresses(new Uri("http://moonbyte.us").Host)[0].ToString())) { ServerIP = "192.168.0.16"; }

            MoonbyteServerConnection = new UniversalClient.UniversalClient(false);
            MoonbyteServerConnection.ConnectToRemoteServer(ServerIP, ServerPort);
            applicationName = ApplicationName;
        }

        #endregion Initialization

        #region CreateProject

        public string CreateProject(string Version, string DownloadURL)
        { return MoonbyteServerConnection.SendCommand("universalprojectupdater", new string[] { "CREATEPROJECT", applicationName, Version, DownloadURL }); }

        #endregion CreateProject

        #region GetVersion

        public string GetVersion()
        { return MoonbyteServerConnection.SendCommand("universalprojectupdater", new string[] { "GETVERSION", applicationName }); }

        #endregion GetVersion

        #region GetDownloadURL

        public string GetDownloadURL()
        { return MoonbyteServerConnection.SendCommand("universalprojectupdater", new string[] { "GETDOWNLOADURL", applicationName }); }

        #endregion GetDownloadURL

        #region Initialize IDownloader

        public void InitializeIDownloader(string DownloadURL)
        {
            if (!Directory.Exists(DeploymentDirectory)) Directory.CreateDirectory(DeploymentDirectory);
            if (!File.Exists(DeploymentFile))
            { using (WebClient webClient = new WebClient()) { webClient.DownloadFile(DownloadURI, DeploymentFile); } }
            const string quote = "\""; if (File.Exists(DeploymentFile)) { Process.Start(DeploymentFile, quote + Application.ProductName + quote + " " + quote + Application.StartupPath + quote + " " + quote + DownloadURL + quote); }
        }

        #endregion Initialize IDownloader
    }
}
