using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    internal class EtsLoader {
        /// <summary>
        ///     Loads the ets group mods.
        /// </summary>
        /// <param name="grp">The GRP.</param>
        /// <returns></returns>
        public IEnumerable<string> LoadEtsGroupMods(string grp) {
            var ets = new EtsRegWork(true);
            var lst = GetFilesFolders.GetFiles(ets.Read(EtsRegKeys.ETS_GROUPS) + grp + "\\", "*.scs");

            return lst;
        }

        /// <summary>
        ///     Loads the ats groups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> LoadEtsGroups() {
            var ets = new AtsRegWork(true);
            var lst = GetFilesFolders.GetFolders(ets.Read(EtsRegKeys.ETS_GROUPS), "*.*");

            return lst;
        }

        /// <summary>
        ///     Loads the ats mod choice.
        /// </summary>
        /// <param name="chc">The CHC.</param>
        /// <returns></returns>
        public string LoadEtsModChoice(string chc) {
            var ets = new EtsRegWork(true);
            var fnd = string.Empty;
            var dic = Serializer.DeserializeDictionary(ets.Read(EtsRegKeys.ETS_XML) + "sortedFileListComplete.xml");
            if (dic.Any(v => string.Equals(v.Key, chc, StringComparison.OrdinalIgnoreCase)))
                dic.TryGetValue(chc, out fnd);
            return fnd + chc;
        }

        /// <summary>
        ///     Loads the ats profile mods.
        /// </summary>
        /// <param name="prof">The PTH.</param>
        /// <returns></returns>
        public Dictionary<string, string> LoadEtsProfileMods(string prof) {
            var ets = new EtsRegWork(true);
            var pro = ets.Read(EtsRegKeys.ETS_PROFILES) + ets.Read(RegKeys.CURRENT_PROFILE) + "\\" + prof + ".xml";
            if (!pro.FileExists()) return null;
            var dic = Serializer.DeserializeDictionary(pro);

            return dic;
        }

        /// <summary>
        ///     Loads the ats profiles.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> LoadEtsProfiles() {
            var ets = new EtsRegWork(true);
            var lst = GetFilesFolders.GetFolders(ets.Read(EtsRegKeys.ETS_PROFILES), "*.*");

            return lst;
        }

        /// <summary>
        ///     Starts the ats.
        /// </summary>
        public void StartEts() {
            var ets = new EtsRegWork(true);
            var gam = ets.Read(EtsRegKeys.ETS_START_GAME_PATH);

            if (gam.IsNullOrEmpty()) {
                var ofd = new OpenFileDialog {
                    CheckFileExists = true,
                    Title = @"Navigate to the Euro Truck Simulator Exe"
                };

                ofd.ShowDialog();
                gam = ofd.FileName;
                ets.Write(EtsRegKeys.ETS_START_GAME_PATH, ofd.FileName);
            }

            Process.Start(gam);
        }
    }
}