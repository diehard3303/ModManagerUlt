//
//  Author:
//    DieHard diehard@eclcmail.com
//
//  Copyright (c) 2016, 2007
//
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
//       the documentation and/or other materials provided with the distribution.
//     * Neither the name of the [ORGANIZATION] nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Extensions;

namespace ModManagerUlt {
    internal class ProfileWorker {
        private const string PROFILE_PATH_END = @" />";
        private const string XML_EXT = ".xml";

        private static string ProfilePathPreTrue { get; } = @"<modsDirectoryOverride active="
                                                            + Vars.QuoteMark
                                                            + "True" + Vars.QuoteMark
                                                            + " directory=";

        /// <summary>
        ///     Adds the new profile.
        /// </summary>
        /// <param name="nam">The nam.</param>
        public void AddNewProfile(string nam) {
            var gi = new GameInfo();
            var gam = gi.GetGame();
            if (gam.IsNullOrEmpty()) return;
            var pth = string.Empty;

            switch (gam) {
                case "ATS":
                    var atsp = new AtsRegWork(true);
                    pth = atsp.Read(AtsRegKeys.ATS_PROFILES) + nam;
                    break;

                case "ETS":
                    var etsp = new EtsRegWork(true);
                    pth = etsp.Read(EtsRegKeys.ETS_PROFILES) + nam;
                    break;

                case "FS15":
                    var fs15 = new Fs15RegWork(true);
                    pth = fs15.Read(Fs15RegKeys.FS15_PROFILES) + nam;
                    break;

                case "FS17":
                    var fs17 = new Fs17RegWork(true);
                    pth = fs17.Read(Fs17RegKeys.FS17_PROFILES) + nam;
                    break;
            }

            FolderCreator.CreatePublicFolders(pth);
        }

        /// <summary>
        ///     Builds the mod list.
        /// </summary>
        /// <param name="key">The key.</param>
        public void BuildProfileList(string key) {
            var reg = new AtsRegWork(true);
            var prof = reg.Read(RegKeys.CURRENT_PROFILE);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            string pth;
            string loc;
            var origPath = string.Empty;
            Dictionary<string, string> dic;

            const string xml = @"sortedFileListComplete.xml";

            switch (gam) {
                case "ATS":
                    pth = reg.Read(AtsRegKeys.ATS_PROFILES) + prof + "\\" + prof + XML_EXT;
                    loc = reg.Read(AtsRegKeys.ATS_XML) + xml;
                    dic = Serializer.DeserializeDictionary(loc);
                    origPath = GetOrigPath(key, dic, origPath);

                    BuildModList(pth, key, origPath);
                    BuildGameModsList(pth);
                    break;

                case "ETS":
                    pth = reg.Read(EtsRegKeys.ETS_PROFILES) + prof + "\\" + prof + XML_EXT;
                    loc = reg.Read(AtsRegKeys.ATS_XML) + xml;
                    dic = Serializer.DeserializeDictionary(loc);
                    origPath = GetOrigPath(key, dic, origPath);

                    BuildModList(pth, key, origPath);
                    BuildGameModsList(pth);
                    break;

                case "FS15":
                    pth = reg.Read(Fs15RegKeys.FS15_PROFILES) + prof + "\\" + prof + XML_EXT;
                    loc = reg.Read(Fs15RegKeys.FS15_PROFILES) + prof + "\\";
                    BuildModList(pth, key, loc);
                    break;

                case "FS17":
                    pth = reg.Read(Fs17RegKeys.FS17_PROFILES) + prof + "\\" + prof + XML_EXT;
                    loc = reg.Read(Fs17RegKeys.FS17_PROFILES) + prof + "\\";
                    BuildModList(pth, key, loc);
                    break;
            }
        }

        /// <summary>
        ///     Removes a Profile
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ProfileRemove() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var profile = reg.Read(RegKeys.CURRENT_PROFILE);
            string proPath;
            IEnumerable<string> proList = null;

            switch (gam) {
                case "ATS":
                    var ldr = new AtsLoader();
                    proPath = reg.Read(AtsRegKeys.ATS_PROFILES) + profile;
                    DeleteFiles.DeleteFilesOrFolders(proPath);
                    proList = ldr.LoadAtsProfiles();
                    break;

                case "ETS":
                    var edr = new EtsLoader();
                    proPath = reg.Read(EtsRegKeys.ETS_PROFILES) + profile;
                    DeleteFiles.DeleteFilesOrFolders(proPath);
                    proList = edr.LoadEtsProfiles();
                    break;

                case "FS15":
                    proPath = reg.Read(Fs15RegKeys.FS15_PROFILES) + profile;
                    var fs15 = new Fs15Loader();
                    DeleteFiles.DeleteFilesOrFolders(proPath);
                    proList = fs15.LoadFs15Profiles();
                    break;

                case "FS17":
                    proPath = reg.Read(Fs17RegKeys.FS17_PROFILES) + profile;
                    var fs17 = new Fs15Loader();
                    DeleteFiles.DeleteFilesOrFolders(proPath);
                    proList = fs17.LoadFs15Profiles();
                    break;
            }

            return proList;
        }

        /// <summary>
        ///     Removes the mod.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public Dictionary<string, string> ProfileRemoveMod(string key) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var profile = reg.Read(RegKeys.CURRENT_PROFILE);
            var modPath = string.Empty;

            switch (gam) {
                case "ATS":
                    modPath = reg.Read(AtsRegKeys.ATS_PROFILES) + profile + "\\" + profile + XML_EXT;
                    break;

                case "ETS":
                    modPath = reg.Read(EtsRegKeys.ETS_PROFILES) + profile + "\\" + profile + XML_EXT;
                    break;

                case "FS15":
                    modPath = reg.Read(Fs15RegKeys.FS15_PROFILES) + profile + "\\" + profile + XML_EXT;
                    break;

                case "FS17":
                    modPath = reg.Read(Fs17RegKeys.FS17_PROFILES) + profile + "\\" + profile + XML_EXT;
                    break;
            }

            var dic = Serializer.DeserializeDictionary(modPath);
            if (dic.Any(v => string.Equals(v.Key, key, StringComparison.OrdinalIgnoreCase))) {
                var mod = reg.Read(Fs17RegKeys.FS17_PROFILES) + profile + "\\" + key;
                dic.Remove(key);
                DeleteFiles.DeleteFilesOrFolders(mod);
                
                var ls = new ListCreator();
                ls.CreateSortedLists();
                ls.SortedFileListComplete();
                MsgBx.Msg("Operation completed successfully", "Mod Remover");
            }

            Serializer.SerializeDictionary(modPath, dic);

            return dic;
        }

        /// <summary>
        ///     Sets the profile active.
        /// </summary>
        public void SetProfileActive() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var profile = reg.Read(RegKeys.CURRENT_PROFILE);
            string path;
            string xmlPath;
            Dictionary<string, string> dic;
            Dictionary<string, string> dicRealPath;
            string origModPath;

            switch (gam) {
                case "ATS":
                    path = reg.Read(AtsRegKeys.ATS_GAME_MOD_FOLDER);
                    xmlPath = reg.Read(AtsRegKeys.ATS_PROFILES) + profile + "\\" + profile + XML_EXT;
                    dicRealPath =
                        Serializer.DeserializeDictionary(reg.Read(AtsRegKeys.ATS_XML) + "sortedFileListComplete.xml");
                    DeleteMods();
                    string tmp;
                    if (!xmlPath.FileExists()) return;
                    dic = Serializer.DeserializeDictionary(xmlPath);
                    foreach (var k in dic) {
                        dicRealPath.TryGetValue(k.Key, out tmp);
                        origModPath = tmp + k.Key;
                        if (!origModPath.FileExists()) continue;
                        SymLinks.CreateSymLink(path + k.Key, origModPath, 0);
                    }

                    Serializer.SerializeDictionary(path + profile + XML_EXT, dic);
                    break;

                case "ETS":
                    path = reg.Read(EtsRegKeys.ETS_GAME_MOD_FOLDER);
                    xmlPath = reg.Read(EtsRegKeys.ETS_PROFILES) + profile + "\\" + profile + XML_EXT;
                    dicRealPath =
                        Serializer.DeserializeDictionary(reg.Read(EtsRegKeys.ETS_XML) + "sortedFileListComplete.xml");
                    DeleteMods();
                    if (!xmlPath.FileExists()) return;
                    dic = Serializer.DeserializeDictionary(xmlPath);
                    foreach (var k in dic) {
                        dicRealPath.TryGetValue(k.Key, out tmp);
                        origModPath = tmp + k.Key;
                        if (!origModPath.FileExists()) continue;
                        SymLinks.CreateSymLink(path + k.Key, origModPath, 0);
                    }

                    Serializer.SerializeDictionary(path + profile + XML_EXT, dic);
                    break;

                case "FS15":
                    ModifyGameSettings();
                    break;

                case "FS17":
                    ModifyGameSettings();
                    break;
            }
        }

        private static void BuildGameModsList(string pth) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            Dictionary<string, string> dic;

            switch (gam) {
                case "ATS":
                    if (!pth.FileExists()) return;
                    dic = Serializer.DeserializeDictionary(pth);
                    Serializer.SerializeDictionary(reg.Read(AtsRegKeys.ATS_GAME_MOD_FOLDER)
                                                   + reg.Read(RegKeys.CURRENT_PROFILE) + XML_EXT, dic);
                    break;

                case "ETS":
                    if (!pth.FileExists()) return;
                    dic = Serializer.DeserializeDictionary(pth);
                    Serializer.SerializeDictionary(reg.Read(EtsRegKeys.ETS_GAME_MOD_FOLDER)
                                                   + reg.Read(RegKeys.CURRENT_PROFILE) + XML_EXT, dic);
                    break;
            }
        }

        private static void BuildModList(string pth, string key, string loc) {
            if (!pth.FileExists()) {
                var dic = new Dictionary<string, string> {{key, loc}};
                Serializer.SerializeDictionary(pth, dic);
                return;
            }

            var existing = Serializer.DeserializeDictionary(pth);
            if (!existing.ContainsKey(key)) existing.Add(key, loc);
            Serializer.SerializeDictionary(pth, existing);
        }

        /// <summary>
        ///     Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DeleteFile(string path);

        private static void DeleteMods() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            string pth;
            IEnumerable<string> lst;

            switch (gam) {
                case "ATS":
                    pth = reg.Read(AtsRegKeys.ATS_GAME_MOD_FOLDER);
                    if (!pth.FolderExists()) return;
                    lst = GetFilesFolders.GetFiles(pth, "*.*");
                    foreach (var t in lst)
                        try {
                            DeleteFile(t);
                        }
                        catch (Exception g) {
                            MsgBx.Msg(g.ToString(), "Error");
                        }

                    FolderCreator.CreatePublicFolders(reg.Read(AtsRegKeys.ATS_GAME_MOD_FOLDER));
                    break;

                case "ETS":
                    pth = reg.Read(EtsRegKeys.ETS_GAME_MOD_FOLDER);
                    if (!pth.FolderExists()) return;
                    lst = GetFilesFolders.GetFiles(pth, "*.*");
                    foreach (var s in lst)
                        try {
                            DeleteFile(s);
                        }
                        catch (Exception g) {
                            MsgBx.Msg(g.ToString(), "Error");
                        }

                    FolderCreator.CreatePublicFolders(reg.Read(EtsRegKeys.ETS_GAME_MOD_FOLDER));
                    break;
            }
        }

        private static string GetOrigPath(string key, IReadOnlyDictionary<string, string> dic, string origPath) {
            if (dic.Any(v => string.Equals(v.Key, key, StringComparison.OrdinalIgnoreCase)))
                dic.TryGetValue(key, out origPath);
            return origPath;
        }

        private static void ModifyGameSettings() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var fw = new FileWriter();
            var tmp = new List<string>();

            switch (gam) {
                case "FS15":
                    tmp = fw.FileToList(reg.Read(Fs15RegKeys.FS15_GAME_SETTINGS_XML));
                    break;

                case "FS17":
                    tmp = fw.FileToList(reg.Read(Fs17RegKeys.FS17_GAME_SETTINGS_XML));
                    break;
            }

            foreach (var s in tmp) {
                if (!s.Contains(@"<modsDirectoryOverride")) continue;
                tmp.Remove(s);
                tmp.Insert(6, SetGameSettingString());
                break;
            }

            fw.ChangeGameSettings(tmp);
        }

        private static string SetGameSettingString() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var profile = reg.Read(RegKeys.CURRENT_PROFILE) + "\\";
            string profilePath;
            var pth = string.Empty;

            switch (gam) {
                case "FS15":
                    profilePath = reg.Read(Fs15RegKeys.FS15_PROFILES);
                    pth = ProfilePathPreTrue + Vars.QuoteMark + profilePath + profile
                          + Vars.QuoteMark + " " + PROFILE_PATH_END;
                    break;

                case "FS17":
                    profilePath = reg.Read(Fs17RegKeys.FS17_PROFILES);
                    pth = ProfilePathPreTrue + Vars.QuoteMark + profilePath + profile
                          + Vars.QuoteMark + " " + PROFILE_PATH_END;
                    break;
            }

            return pth;
        }
    }
}