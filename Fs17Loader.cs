using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    internal class Fs17Loader {
        /// <summary>
        ///     Loads the fs17 group mods.
        /// </summary>
        /// <param name="grp">The GRP.</param>
        /// <returns></returns>
        public IEnumerable<string> LoadFs17GroupMods(string grp) {
            var fs17 = new Fs17RegWork(true);
            var lst = GetFilesFolders.GetFiles(fs17.Read(Fs17RegKeys.FS17_GROUPS) + grp + "\\", "*.zip");

            return lst;
        }

        /// <summary>
        ///     Loads the ats groups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> LoadFs17Groups() {
            var fs17 = new Fs17RegWork(true);
            var lst = GetFilesFolders.GetFolders(fs17.Read(Fs17RegKeys.FS17_GROUPS), "*.*");

            return lst;
        }

        /// <summary>
        ///     Loads the Fs17 mod choice.
        /// </summary>
        /// <param name="chc">The CHC.</param>
        /// <returns></returns>
        public string LoadFs17ModChoice(string chc) {
            var ets = new Fs17RegWork(true);
            var fnd = string.Empty;
            var dic = Serializer.DeserializeDictionary(ets.Read(Fs17RegKeys.FS17_XML) + "sortedFileListComplete.xml");
            if (dic.Any(v => string.Equals(v.Key, chc, StringComparison.OrdinalIgnoreCase)))
                dic.TryGetValue(chc, out fnd);
            return fnd + chc;
        }

        /// <summary>
        ///     Loads the ats profile mods.
        /// </summary>
        /// <param name="prof">The PTH.</param>
        /// <returns></returns>
        public Dictionary<string, string> LoadFs17ProfileMods(string prof) {
            var fs17 = new Fs17RegWork(true);
            var pro = fs17.Read(Fs17RegKeys.FS17_PROFILES) + prof + "\\" + prof + ".xml";
            if (!pro.FileExists()) return null;
            var dic = Serializer.DeserializeDictionary(pro);

            return dic;
        }

        /// <summary>
        ///     Loads the ats profiles.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> LoadFs17Profiles() {
            var fs17 = new Fs17RegWork(true);
            var lst = GetFilesFolders.GetFolders(fs17.Read(Fs17RegKeys.FS17_PROFILES), "*.*");

            return lst;
        }

        /// <summary>
        ///     Starts the FS17.
        /// </summary>
        public void StartFs17() {
            var fs17 = new Fs17RegWork(true);
            var gam = fs17.Read(Fs17RegKeys.FS17_START_GAME_PATH);

            if (gam.IsNullOrEmpty()) {
                var ofd = new OpenFileDialog {
                    CheckFileExists = true,
                    Title = @"Navigate to the Farming Simulator 2017 Exe"
                };

                ofd.ShowDialog();
                gam = ofd.FileName;
                fs17.Write(Fs17RegKeys.FS17_START_GAME_PATH, ofd.FileName);
            }

            Process.Start(gam);
        }
    }
}