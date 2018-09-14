using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    internal class EtsInit {
        /// <summary>
        ///     Initializes the ats.
        /// </summary>
        public void InitializeEts() {
            var atsw = new EtsRegWork(true);
            var tmp = atsw.Read(EtsRegKeys.ETS_REPO);
            if (!tmp.IsNullOrEmpty()) return;

            var diag = MessageBox.Show(@"Do you want me to setup Euro Truck Simulator 2?", @"ETS Init",
                MessageBoxButtons.YesNo);
            if (diag != DialogResult.Yes) return;

            CreateFolders();
        }

        private static void CreateFolders() {
            var atsw = new EtsRegWork(true);

            var ofd = new FolderBrowserDialog {
                Description = @"Navigate to where you want the top folder for ETS",
                ShowNewFolderButton = false
            };
            ofd.ShowDialog();
            if (!ofd.SelectedPath.FolderExists()) return;
            var pth = ofd.SelectedPath;
            FolderCreator.CreatePublicFolders(pth + "\\ETSRepo");
            atsw.Write(EtsRegKeys.ETS_REPO, pth + "\\ETSRepo\\");
            var tmp = pth + "\\ETSRepo\\";
            FolderCreator.CreatePublicFolders(tmp + "ETSExtraction");
            atsw.Write(EtsRegKeys.ETS_EXTRACTION, tmp + "ETSExtraction");
            FolderCreator.CreatePublicFolders(tmp + "ETSProfiles");
            atsw.Write(EtsRegKeys.ETS_PROFILES, tmp + "ETSProfiles\\");
            FolderCreator.CreatePublicFolders(tmp + "ETSGroups");
            atsw.Write(EtsRegKeys.ETS_GROUPS, tmp + "ETSGroups\\");
            FolderCreator.CreatePublicFolders(tmp + "ETSXml");
            atsw.Write(EtsRegKeys.ETS_XML, tmp + "ETSXml\\");
            FolderCreator.CreatePublicFolders(tmp + "ETSWork");
            atsw.Write(EtsRegKeys.ETS_WORK, tmp + "ETSWork\\");

            ofd = new FolderBrowserDialog {
                Description = @"Navigate to your ETS Game Mod Folder",
                ShowNewFolderButton = false
            };
            ofd.ShowDialog();
            if (ofd.SelectedPath.FolderExists()) atsw.Write(EtsRegKeys.ETS_GAME_MOD_FOLDER, ofd.SelectedPath + "\\");

            MsgBx.Msg("All folders have been created for ETS", "Game Intializer");
        }
    }
}