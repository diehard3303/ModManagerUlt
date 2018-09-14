using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    internal class AtsLoader {
        /// <summary>
        ///     Loads the ats group mods.
        /// </summary>
        /// <param name="grp">The GRP.</param>
        /// <returns></returns>
        public IEnumerable<string> LoadAtsGroupMods(string grp) {
            var ats = new AtsRegWork(true);
            var lst = GetFilesFolders.GetFiles(ats.Read(AtsRegKeys.ATS_GROUPS) + grp + "\\", "*.scs");

            return lst;
        }

        /// <summary>
        ///     Loads the ats groups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> LoadAtsGroups() {
            var ats = new AtsRegWork(true);
            var lst = GetFilesFolders.GetFolders(ats.Read(AtsRegKeys.ATS_GROUPS), "*.*");

            return lst;
        }

        /// <summary>
        ///     Loads the ats mod choice.
        /// </summary>
        /// <param name="chc">The CHC.</param>
        /// <returns></returns>
        public string LoadAtsModChoice(string chc) {
            var ats = new AtsRegWork(true);
            var fnd = string.Empty;
            var dic = Serializer.DeserializeDictionary(ats.Read(AtsRegKeys.ATS_XML) + "sortedFileListComplete.xml");
            if (dic.Any(v => string.Equals(v.Key, chc, StringComparison.OrdinalIgnoreCase)))
                dic.TryGetValue(chc, out fnd);
            return fnd + chc;
        }

        /// <summary>
        ///     Loads the ats profile mods.
        /// </summary>
        /// <param name="prof">The PTH.</param>
        /// <returns></returns>
        public Dictionary<string, string> LoadAtsProfileMods(string prof) {
            var ats = new AtsRegWork(true);
            var pro = ats.Read(AtsRegKeys.ATS_PROFILES) + prof + "\\" + prof + ".xml";
            if (!pro.FileExists()) return null;
            var dic = Serializer.DeserializeDictionary(pro);

            return dic;
        }

        /// <summary>
        ///     Loads the ats profiles.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> LoadAtsProfiles() {
            var ats = new AtsRegWork(true);
            var lst = GetFilesFolders.GetFolders(ats.Read(AtsRegKeys.ATS_PROFILES), "*.*");

            return lst;
        }

        /// <summary>
        ///     Starts the ats.
        /// </summary>
        public void StartAts() {
            var ats = new AtsRegWork(true);
            var gam = ats.Read(AtsRegKeys.ATS_START_GAME_PATH);

            if (gam.IsNullOrEmpty()) {
                var ofd = new OpenFileDialog {
                    CheckFileExists = true,
                    Title = @"Navigate to the American Truck Simulator Exe"
                };

                ofd.ShowDialog();
                gam = ofd.FileName;
                ats.Write(AtsRegKeys.ATS_START_GAME_PATH, ofd.FileName);
            }

            Process.Start(gam);
        }
    }
}