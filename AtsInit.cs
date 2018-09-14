using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    internal class AtsInit {
	    /// <summary>
	    ///     Initializes the ats.
	    /// </summary>
	    public void InitializeAts() {
            var atsw = new AtsRegWork(true);
            var tmp = atsw.Read(AtsRegKeys.ATS_REPO);
            if (!tmp.IsNullOrEmpty()) return;

            var diag = MessageBox.Show(@"Do you want me to setup American Truck Simulator?", @"ATS Init",
                MessageBoxButtons.YesNo);
            if (diag != DialogResult.Yes) return;

            CreateFolders();
        }

        private static void CreateFolders() {
            var atsw = new AtsRegWork(true);

            var ofd = new FolderBrowserDialog {
                Description = @"Navigate to where you want the top folder for ATS",
                ShowNewFolderButton = false
            };
            ofd.ShowDialog();
            if (!ofd.SelectedPath.FolderExists()) return;
            var pth = ofd.SelectedPath;
            FolderCreator.CreatePublicFolders(pth + "\\ATSRepo");
            atsw.Write(AtsRegKeys.ATS_REPO, pth + "\\ATSRepo\\");
            var tmp = pth + "\\ATSRepo\\";
            FolderCreator.CreatePublicFolders(tmp + "ATSExtraction");
            atsw.Write(AtsRegKeys.ATS_EXTRACTION, tmp + "ATSExtraction");
            FolderCreator.CreatePublicFolders(tmp + "ATSProfiles");
            atsw.Write(AtsRegKeys.ATS_PROFILES, tmp + "ATSProfiles\\");
            FolderCreator.CreatePublicFolders(tmp + "ATSGroups");
            atsw.Write(AtsRegKeys.ATS_GROUPS, tmp + "ATSGroups\\");
            FolderCreator.CreatePublicFolders(tmp + "ATSXml");
            atsw.Write(AtsRegKeys.ATS_XML, tmp + "ATSXml\\");
            FolderCreator.CreatePublicFolders(tmp + "ATSWork");
            atsw.Write(AtsRegKeys.ATS_WORK, tmp + "ATSWork\\");

            ofd = new FolderBrowserDialog {
                Description = @"Navigate to your ATS Game Mod Folder",
                ShowNewFolderButton = false
            };
            ofd.ShowDialog();
            if (ofd.SelectedPath.FolderExists()) atsw.Write(AtsRegKeys.ATS_GAME_MOD_FOLDER, ofd.SelectedPath + "\\");

            MsgBx.Msg("All folders have been created for ATS", "Game Intializer");
        }
    }
}