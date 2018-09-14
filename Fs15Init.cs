using System;
using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    internal class Fs15Init {
        /// <summary>
        ///     Initializes the Fs15.
        /// </summary>
        public void InitializeFs15() {
            var fs = new Fs15RegWork(true);
            var tmp = fs.Read(Fs15RegKeys.FS15_REPO);
            if (!tmp.IsNullOrEmpty()) return;

            var diag = MessageBox.Show(@"Do you want me to setup Farming Simulator 2015?", @"FS 15 Init",
                MessageBoxButtons.YesNo);
            if (diag != DialogResult.Yes) return;

            CreateFolders();
        }

        private static void CreateFolders() {
            var atsw = new Fs15RegWork(true);

            var ofd = new FolderBrowserDialog {
                Description = @"Navigate to where you want the top folder for FS15",
                ShowNewFolderButton = false
            };
            ofd.ShowDialog();
            if (!ofd.SelectedPath.FolderExists()) return;
            var pth = ofd.SelectedPath;
            FolderCreator.CreatePublicFolders(pth + "\\FS15Repo");
            atsw.Write(Fs15RegKeys.FS15_REPO, pth + "\\FS15Repo\\");
            var tmp = pth + "\\FS15Repo\\";
            FolderCreator.CreatePublicFolders(tmp + "FS15Extraction");
            atsw.Write(Fs15RegKeys.FS15_EXTRACTION, tmp + "FS15Extraction");
            FolderCreator.CreatePublicFolders(tmp + "FS15Profiles");
            atsw.Write(Fs15RegKeys.FS15_PROFILES, tmp + "Fs15Profiles\\");
            FolderCreator.CreatePublicFolders(tmp + "FS15Groups");
            atsw.Write(Fs15RegKeys.FS15_GROUPS, tmp + "FS15Groups\\");
            FolderCreator.CreatePublicFolders(tmp + "FS15Xml");
            atsw.Write(Fs15RegKeys.FS15_XML, tmp + "FS15Xml\\");
            FolderCreator.CreatePublicFolders(tmp + "FS15Work");
            atsw.Write(Fs15RegKeys.FS15_WORK, tmp + "FS15Work\\");

            ofd = new FolderBrowserDialog {
                Description = @"Navigate to Farming Simulator 2015 Mod Folder",
                ShowNewFolderButton = false
            };
            ofd.ShowDialog();
            if (ofd.SelectedPath.FolderExists()) {
                atsw.Write(Fs15RegKeys.FS15_GAME_MOD_FOLDER, ofd.SelectedPath + "\\");
                var t = ofd.SelectedPath.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
                var fix = ofd.SelectedPath.Substring(0, t) + "\\";
                atsw.Write(Fs15RegKeys.FS15_GAME_SETTINGS_XML, fix + "gameSettings.xml");
            }

            MsgBx.Msg("All folders have been created for FS15", "Game Intializer");
        }
    }
}