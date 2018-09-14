using System.Runtime.Remoting.Lifetime;
using Extensions;

namespace ModManagerUlt {
    /// <summary>
    /// </summary>
    public class Fs17MoveMod {
        /// <summary>
        /// </summary>
        public void MoveMod(string mod) {
            var loc = GetLocation();
            if (loc.IsNullOrEmpty()) return;
            if (!mod.FileExists()) return;
            FileCopyMove.FileCopy(mod, loc + mod.GetFileName(), true);

            var ls = new ListCreator();
            ls.CreateSortedLists();
            ls.SortedFileListComplete();

            MsgBx.Msg("Operation completed successfully..", "Mod Mover");
        }

        private static string GetLocation() {
            var gi = new GameInfo();
            var tmp = gi.GetGame();
            if (tmp.IsNullOrEmpty()) return null;


            var pth = string.Empty;

            switch (tmp) {
                case "ATS":
                    var atsp = new AtsRegWork(true);
                    pth = atsp.Read(AtsRegKeys.ATS_GROUPS);
                    break;

                case "ETS":
                    var etsp = new EtsRegWork(true);
                    pth = etsp.Read(EtsRegKeys.ETS_GROUPS);
                    break;

                case "FS15":
                    var fs15 = new Fs15RegWork(true);
                    pth = fs15.Read(Fs15RegKeys.FS15_GROUPS);
                    break;

                case "FS17":
                    var fs17 = new Fs17RegWork(true);
                    pth = fs17.Read(Fs17RegKeys.FS17_GROUPS);
                    break;
            }

            var lst = GetFilesFolders.GetFolders(pth, "*.*");
            var frm = new ModMover();
            foreach (var f in lst) {
                frm.lstModMove.Items.Add(f);
            }

            frm.ShowDialog();

            var fld = frm.lstModMove.SelectedItem.ToString();
            frm.Close();
            return fld;
        }
    }
}