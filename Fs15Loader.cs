using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    internal class Fs15Loader {
        /// <summary>
        ///     Loads the fs15 group mods.
        /// </summary>
        /// <param name="grp">The GRP.</param>
        /// <returns></returns>
        public IEnumerable<string> LoadFs15GroupMods(string grp) {
            var fs15 = new Fs15RegWork(true);
            var lst = GetFilesFolders.GetFiles(fs15.Read(Fs15RegKeys.FS15_GROUPS) + grp + "\\", "*.zip");

            return lst;
        }

        /// <summary>
        ///     Loads the Fs15 groups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> LoadFs15Groups() {
            var fs15 = new Fs15RegWork(true);
            var lst = GetFilesFolders.GetFolders(fs15.Read(Fs15RegKeys.FS15_GROUPS), "*.*");

            return lst;
        }

        /// <summary>
        ///     Loads the Fs15 mod choice.
        /// </summary>
        /// <param name="chc">The CHC.</param>
        /// <returns></returns>
        public string LoadFs15ModChoice(string chc) {
            var ets = new Fs15RegWork(true);
            var fnd = string.Empty;
            var dic = Serializer.DeserializeDictionary(ets.Read(Fs15RegKeys.FS15_XML) + "sortedFileListComplete.xml");
            if (dic.Any(v => string.Equals(v.Key, chc, StringComparison.OrdinalIgnoreCase)))
                dic.TryGetValue(chc, out fnd);
            return fnd + chc;
        }

        /// <summary>
        ///     Loads the Fs15 profile mods.
        /// </summary>
        /// <param name="prof">The PTH.</param>
        /// <returns></returns>
        public Dictionary<string, string> LoadFs15ProfileMods(string prof) {
            var fs15 = new Fs15RegWork(true);
            var pro = fs15.Read(Fs15RegKeys.FS15_PROFILES) + prof + "\\" + prof + ".xml";
            if (!pro.FileExists()) return null;
            var dic = Serializer.DeserializeDictionary(pro);

            return dic;
        }

        /// <summary>
        ///     Loads the Fs15 profiles.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> LoadFs15Profiles() {
            var fs15 = new Fs15RegWork(true);
            var lst = GetFilesFolders.GetFolders(fs15.Read(Fs15RegKeys.FS15_PROFILES), "*.*");

            return lst;
        }

        /// <summary>
        ///     Starts the FS15.
        /// </summary>
        public void StartFs15() {
            var fs15 = new Fs15RegWork(true);
            var gam = fs15.Read(Fs15RegKeys.FS15_START_GAME_PATH);

            if (gam.IsNullOrEmpty()) {
                var ofd = new OpenFileDialog {
                    CheckFileExists = true,
                    Title = @"Navigate to the Farming Simulator 2015 Exe"
                };

                ofd.ShowDialog();
                gam = ofd.FileName;
                fs15.Write(Fs15RegKeys.FS15_START_GAME_PATH, ofd.FileName);
            }

            Process.Start(gam);
        }
    }
}