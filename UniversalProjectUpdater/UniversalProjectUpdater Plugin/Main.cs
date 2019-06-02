using Moonbyte.UniversalServer.PluginFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonbyte.Net.Updater
{
    public class Main : UniversalPluginFramework
    {

        #region Vars

        List<string> Projects = new List<string>();

        #region Directories

        string ProjectDirectories;

        #endregion Directories

        #endregion Vars

        #region Properties

        public string Name
        {
            get { return "universalprojectupdater"; }
        }

        #endregion Properties

        #region OnLoad

        public void OnLoad(string PluginSettingsDirectory)
        {
            ProjectDirectories = PluginSettingsDirectory + @"\Projects";

            if (!Directory.Exists(ProjectDirectories)) { Directory.CreateDirectory(ProjectDirectories); }
            DirectoryInfo dI = new DirectoryInfo(ProjectDirectories);
            
            foreach (DirectoryInfo dIb in dI.GetDirectories())
            { Projects.Add(dIb.Name); }

            Console.WriteLine("[UniversalProjectUpdater] UPU has fully initialized with " + Projects.Count + " projects loaded!");
        }

        #endregion OnLoad

        #region Invoke 

        public void Invoke(ClientContext Client, string[] RawCommand)
        {
            try
            {
                if (RawCommand[1].ToUpper() == "CREATEPROJECT")
                {
                    CreateProject(RawCommand[2], RawCommand[3], RawCommand[4]);
                    Client.SendMessage("UPU_Sucessful");
                }
                else if (RawCommand[1].ToUpper() == "GETVERSION")
                {
                    string buffer = GetVersion(RawCommand[2]);
                    if (buffer == false.ToString()) { Client.SendMessage("UPU_FALSE"); }
                    else { Client.SendMessage(buffer); }
                }
                else if (RawCommand[1].ToUpper() == "GETDOWNLOADURL")
                {
                    string buffer = GetDownloadURL(RawCommand[2]);
                    if (buffer == false.ToString()) { Client.SendMessage("UPU_FALSE"); }
                    else { Client.SendMessage(buffer); }
                }
                else
                {
                    Client.SendMessage("UPU_FALSE_NORESPONSE");
                }
            }
            catch
            {
                Client.SendMessage("UPU_FALSE_CATCH");
            }
        }

        #endregion Invoke

        #region Create Project

        private void CreateProject(string ProjectName, string VersionNumber, string DownloadURL)
        {
            string projectDirectory = ProjectDirectories + @"\" + ProjectName;

            if (!Directory.Exists(projectDirectory)) { Directory.CreateDirectory(projectDirectory); }

            string versionNumberFile = projectDirectory + @"\version.dat";
            string downloadURLFile = projectDirectory + @"\downloadurl.dat";

            if (!File.Exists(versionNumberFile)) File.Create(versionNumberFile).Close();
            if (!File.Exists(downloadURLFile)) File.Create(downloadURLFile).Close();

            File.WriteAllText(versionNumberFile, VersionNumber);
            File.WriteAllText(downloadURLFile, DownloadURL);
        }

        #endregion Create Project

        #region GetVersion

        private string GetVersion(string ProjectName)
        {
            string projectDirectory = ProjectDirectories + @"\" + ProjectName;

            if (Directory.Exists(projectDirectory))
            {
                string versionNumberFile = projectDirectory + @"\version.dat";

                if (!File.Exists(versionNumberFile)) { return false.ToString(); }
                return File.ReadAllText(versionNumberFile);
            }
            else
            {
                return false.ToString();
            }
        }

        #endregion GetVersion

        #region GetDownloadURL

        private string GetDownloadURL(string ProjectName)
        {
            string projectDirectory = ProjectDirectories + @"\" + ProjectName;

            if (Directory.Exists(projectDirectory))
            {
                string downloadURLFile = projectDirectory + @"\downloadurl.dat";

                if (!File.Exists(downloadURLFile)) { return false.ToString(); }
                return File.ReadAllText(downloadURLFile);
            }
            else
            {
                return false.ToString();
            }
        }

        #endregion GetDownloadURL

    }
}
