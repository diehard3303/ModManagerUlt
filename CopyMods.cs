// 2016
// 11
// ModManagerUlt
// ModManagerUlt
// T J Frank

using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    /// <summary>
    ///     Copy multiple mods to profile
    /// </summary>
    public class CopyMods {
        /// <summary>
        ///     Copy the mods to profile
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> CopyProfileMods() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var ofd = new FolderBrowserDialog {
                Description = @"Navigate to mods",
                ShowNewFolderButton = false
            };
            ofd.ShowDialog();
            if (!ofd.SelectedPath.FolderExists()) return null;
            IEnumerable<string> lst = null;
            switch (gam) {
                case "ATS":
                    lst = GetFilesFolders.GetFiles(ofd.SelectedPath, "*.scs");
                    break;

                case "ETS":
                    lst = GetFilesFolders.GetFiles(ofd.SelectedPath, "*.scs");
                    break;

                case "FS15":
                    lst = GetFilesFolders.GetFiles(ofd.SelectedPath, "*.zip");
                    break;

                case "FS17":
                    lst = GetFilesFolders.GetFiles(ofd.SelectedPath, "*.zip");
                    break;
            }

            return CreateLinks(lst);
        }

        private static Dictionary<string, string> CreateLinks(IEnumerable<string> lst) {
            var reg = new AtsRegWork(true);
            var pf = new ProfileWorker();
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var profile = reg.Read(RegKeys.CURRENT_PROFILE);
            var pth = string.Empty;
            var dic = new Dictionary<string, string>();
            var linkPath = string.Empty;

            switch (gam) {
                case "ATS":
                    pth = reg.Read(AtsRegKeys.ATS_PROFILES) + profile + "\\" + profile + ".xml";
                    linkPath = reg.Read(AtsRegKeys.ATS_GAME_MOD_FOLDER);
                    break;

                case "ETS":
                    pth = reg.Read(EtsRegKeys.ETS_PROFILES) + profile + "\\" + profile + ".xml";
                    linkPath = reg.Read(EtsRegKeys.ETS_GAME_MOD_FOLDER);
                    break;

                case "FS15":
                    pth = reg.Read(Fs15RegKeys.FS15_PROFILES) + profile + "\\" + profile + ".xml";
                    linkPath = reg.Read(Fs15RegKeys.FS15_PROFILES) + profile + "\\";
                    break;

                case "FS17":
                    pth = reg.Read(Fs17RegKeys.FS17_PROFILES) + profile + "\\" + profile + ".xml";
                    linkPath = reg.Read(Fs17RegKeys.FS17_PROFILES) + profile + "\\";
                    break;
            }

            if (pth.FileExists()) {
                dic = Serializer.DeserializeDictionary(pth);
                foreach (var v in lst) {
                    var file = v.GetFileName();
                    var pt = Path.GetDirectoryName(v) + "\\";
                    SymLinks.CreateSymLink(linkPath + file, v, 0);
                    dic.Add(file, pt);
                }

                Serializer.SerializeDictionary(pth, dic);
            }
            else if (!pth.FileExists()) {
                foreach (var v in lst) {
                    var file = v.GetFileName();
                    var pt = Path.GetDirectoryName(v) + "\\";
                    SymLinks.CreateSymLink(linkPath + file, v, 0);
                    dic.Add(file, pt);
                }

                Serializer.SerializeDictionary(pth, dic);
            }

            pf.SetProfileActive();

            return dic;
        }
    }
}