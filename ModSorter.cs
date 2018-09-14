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
using Extensions;

namespace ModManagerUlt {
    internal class ModSorter {
        private List<string> _srchLstFs;
        private List<string> _srchLstTs;

        /// <summary>
        ///     Sorts the mods.
        /// </summary>
        public void SortMods() {
            SetSearchLists();

            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);

            if (gam.IsNullOrEmpty()) {
                MsgBx.Msg("Mod Sorting is Game specific, you need to choose a game", "Game Error");
                return;
            }

            string tmp;
            IEnumerable<string> lst;

            switch (gam) {
                case "ATS":
                    tmp = reg.Read(AtsRegKeys.ATS_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.scs");
                    if (lst.IsNull()) return;
                    foreach (var v in lst)
                    foreach (var t in _srchLstTs) {
                        if (!v.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                        FolderCreator.CreatePublicFolders(reg.Read(AtsRegKeys.ATS_GROUPS) + t);
                        FileCopyMove.FileCopy(v, reg.Read(AtsRegKeys.ATS_GROUPS) + t + "\\" + v.GetFileName(),
                            true);
                    }

                    break;

                case "ETS":
                    var ets = new EtsRegWork(true);
                    tmp = ets.Read(EtsRegKeys.ETS_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.scs");
                    if (lst.IsNull()) return;
                    foreach (var v in lst)
                    foreach (var t in _srchLstTs) {
                        if (!v.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                        FolderCreator.CreatePublicFolders(ets.Read(EtsRegKeys.ETS_GROUPS) + t);
                        FileCopyMove.FileCopy(v, ets.Read(EtsRegKeys.ETS_GROUPS) + t + "\\" + v.GetFileName(),
                            true);
                    }

                    break;

                case "FS15":
                    var fs15 = new Fs15RegWork(true);
                    tmp = fs15.Read(Fs15RegKeys.FS15_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.zip");
                    if (lst.IsNull()) return;
                    foreach (var v in lst)
                    foreach (var t in _srchLstFs) {
                        if (!v.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                        FolderCreator.CreatePublicFolders(fs15.Read(Fs15RegKeys.FS15_GROUPS) + t);
                        FileCopyMove.FileCopy(v, fs15.Read(Fs15RegKeys.FS15_GROUPS) + t + "\\" + v.GetFileName(),
                            true);
                    }

                    break;

                case "FS17":
                    var fs17 = new Fs17RegWork(true);
                    tmp = fs17.Read(Fs17RegKeys.FS17_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.zip");
                    if (lst.IsNull()) return;
                    foreach (var v in lst)
                    foreach (var t in _srchLstFs) {
                        if (!v.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                        FolderCreator.CreatePublicFolders(fs17.Read(Fs17RegKeys.FS17_GROUPS) + t);
                        FileCopyMove.FileCopy(v, fs17.Read(Fs17RegKeys.FS17_GROUPS) + t + "\\" + v.GetFileName(),
                            true);
                    }

                    break;
            }

            DoCleanUp();
        }

        private void SetSearchLists() {
            _srchLstFs = new List<string> {
                "NewHolland",
                "CargoBull",
                "Tron",
                "KRAZ",
                "Howe",
                "GMC",
                "Farmall",
                "Bueherer",
                "Forterra",
                "Steyr",
                "Pete",
                "Renault",
                "Arcusin",
                "Coolamon",
                "AutoDrive",
                "ForFarmers",
                "Ihc",
                "Meyer",
                "Texture",
                "Kuhn",
                "FBM17",
                "Fiat",
                "Valmet",
                "Versatile",
                "Veenhuis",
                "Bale",
                "Trailer",
                "MF",
                "AgroMasz",
                "Vogelnoot",
                "Kockerling",
                "ViTesse",
                "Volvo",
                "Service",
                "Bob",
                "Silospace",
                "Fiat",
                "Alex",
                "Laumetris",
                "TSL",
                "Contest",
                "Damman",
                "DCK",
                "FarmingTablet",
                "GEO",
                "GPS",
                "Holmer",
                "Valtra",
                "KST",
                "new_holland",
                "NH",
                "Case",
                "MasseyF",
                "Brantner",
                "Seasons",
                "PUB",
                "Maestro",
                "MengeleSilo",
                "Schmitz",
                "Allrounder",
                "Krone",
                "kotte",
                "Lemkin",
                "Vaderstad",
                "lemken",
                "Deutz",
                "Lexion",
                "Krampe",
                "JCB",
                "Horsch",
                "JohnDeere",
                "john_deere",
                "JD",
                "Fliegl",
                "Fendt",
                "Grimme",
                "Claas",
                "class",
                "Cat",
                "Challenger",
                "Amazone",
                "Bergmann",
                "ZZZ",
                "Same",
                "Huerlimann",
                "Man",
                "Map",
                "Fortuna",
                "Dodge",
                "Fruit",
                "Peterbilt",
                "capello",
                "poettinger",
                "ponsse",
                "placeable",
                "plow",
                "trailer",
                "upk",
                "pack",
                "kw",
                "kenworth",
                "pda",
                "guelle",
                "kinze",
                "kroeger",
                "toyota",
                "keenan",
                "sgt",
                "stevie",
                "valtra",
                "kubota",
                "ursus",
                "joskin",
                "compost",
                "FS15",
                "LS15",
                "Eagle355th",
                "Tatra",
                "Seeders",
                "Trucks",
                "SugarBeets",
                "Combines",
                "Cultivator",
                "Cutters",
                "Buehrer",
                "Kaweco",
                "Lamborghini",
                "Liquid",
                "McCormick",
                "Mack",
                "Shovel",
                "Sprayer",
                "Upk",
                "WoodChips",
                "ford",
                "ballenwagen",
                "tiger"
            };
            _srchLstFs.Sort();

            _srchLstTs = new List<string> {
                "graphics",
                "freightliner",
                "peterbilt",
                "kenworth",
                "volvo",
                "truck",
                "trailer",
                "pack",
                "store",
                "sound",
                "fixes",
                "wabash",
                "diehard",
                "scania",
                "iveco",
                "renault",
                "rjl",
                "gt mike",
                "gtmike",
                "jazzycat",
                "skin",
                "accessories",
                "cat",
                "mack",
                "transmission",
                "traffic",
                "company",
                "companies",
                "kaz",
                "579",
                "589",
                "379",
                "359",
                "w900",
                "mha",
                "trucksim",
                "coastal",
                "frosty"
            };
            _srchLstTs.Sort();
        }


        private static void DoCleanUp() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            string tmp;
            IEnumerable<string> lst;

            switch (gam) {
                case "ATS":
                    tmp = reg.Read(AtsRegKeys.ATS_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.scs");
                    FolderCreator.CreatePublicFolders(reg.Read(AtsRegKeys.ATS_GROUPS) + "misc");
                    foreach (var v in lst)
                        FileCopyMove.FileCopy(v, reg.Read(AtsRegKeys.ATS_GROUPS) + "misc" + "\\" + v.GetFileName(),
                            true);

                    break;

                case "ETS":
                    var ets = new EtsRegWork(true);
                    tmp = ets.Read(EtsRegKeys.ETS_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.scs");
                    FolderCreator.CreatePublicFolders(ets.Read(EtsRegKeys.ETS_GROUPS) + "misc");
                    foreach (var t in lst)
                        FileCopyMove.FileCopy(t, ets.Read(EtsRegKeys.ETS_GROUPS) + "misc" + "\\" + t.GetFileName(),
                            true);

                    break;

                case "FS15":
                    var fs15 = new Fs15RegWork(true);
                    tmp = fs15.Read(Fs15RegKeys.FS15_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.zip");
                    FolderCreator.CreatePublicFolders(fs15.Read(Fs15RegKeys.FS15_GROUPS) + "misc");
                    foreach (var v in lst)
                        FileCopyMove.FileCopy(v, fs15.Read(Fs15RegKeys.FS15_GROUPS) + "misc" + "\\" + v.GetFileName(),
                            true);

                    break;

                case "FS17":
                    var fs17 = new Fs17RegWork(true);
                    tmp = fs17.Read(Fs17RegKeys.FS17_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.zip");
                    FolderCreator.CreatePublicFolders(fs17.Read(Fs17RegKeys.FS17_GROUPS) + "misc");
                    foreach (var v in lst)
                        FileCopyMove.FileCopy(v, fs17.Read(Fs17RegKeys.FS17_GROUPS) + "misc" + "\\" + v.GetFileName(),
                            true);

                    break;
            }

            var lstCreate = new ListCreator();
            lstCreate.CreateSortedLists();
            lstCreate.SortedFileListComplete();
            MsgBx.Msg("Mod Sorting Complete", "Mod Sorter");
        }


        internal void DoDeepSort() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);

            if (gam.IsNullOrEmpty()) {
                MsgBx.Msg("Mod Sorting is Game specific, you need to choose a game", "Game Error");
                return;
            }

            SetSearchLists();

            string tmp;
            IEnumerable<string> lst;

            switch (gam) {
                case "ATS":
                    tmp = reg.Read(AtsRegKeys.ATS_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.scs");
                    if (lst.IsNull()) return;
                    foreach (var v in lst)
                    foreach (var t in _srchLstTs) {
                        if (!v.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                        FolderCreator.CreatePublicFolders(reg.Read(AtsRegKeys.ATS_GROUPS) + t);
                        FileCopyMove.FileCopy(v, reg.Read(AtsRegKeys.ATS_GROUPS) + t + "\\" + v.GetFileName(),
                            true);
                    }

                    break;

                case "ETS":
                    var ets = new EtsRegWork(true);
                    tmp = ets.Read(EtsRegKeys.ETS_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.scs");
                    if (lst.IsNull()) return;
                    foreach (var v in lst)
                    foreach (var t in _srchLstTs) {
                        if (!v.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                        FolderCreator.CreatePublicFolders(ets.Read(EtsRegKeys.ETS_GROUPS) + t);
                        FileCopyMove.FileCopy(v, ets.Read(EtsRegKeys.ETS_GROUPS) + t + "\\" + v.GetFileName(),
                            true);
                    }

                    break;

                case "FS15":
                    var fs15 = new Fs15RegWork(true);
                    tmp = fs15.Read(Fs15RegKeys.FS15_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.zip");
                    if (lst.IsNull()) return;
                    foreach (var v in lst)
                    foreach (var t in _srchLstFs) {
                        if (!v.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                        FolderCreator.CreatePublicFolders(fs15.Read(Fs15RegKeys.FS15_GROUPS) + t);
                        FileCopyMove.FileCopy(v, fs15.Read(Fs15RegKeys.FS15_GROUPS) + t + "\\" + v.GetFileName(),
                            true);
                    }

                    break;

                case "FS17":
                    var fs17 = new Fs17RegWork(true);
                    var byPass = new SortingBypass();
                    var byPassDat = byPass.GetBypassList();
                    
                    tmp = fs17.Read(Fs17RegKeys.FS17_GROUPS);
                    var fldLst = GetFilesFolders.GetFolders(tmp, "*.*");
                    var chkList = new List<string>();
                    var enu = fldLst.ToList();
                    foreach (var v in enu) {
                        var chk = v.GetLastFolderName();
                        if (byPassDat.ContainsKey(chk)) continue;
                        chkList.Add(v);
                    }

                    if (chkList.IsNull()) return;
                    foreach (var v in chkList) {
                        lst = GetFilesFolders.GetFiles(v + "\\", "*.zip");
                        foreach (var f in lst)
                        foreach (var t in _srchLstFs) {
                            if (!f.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                            FolderCreator.CreatePublicFolders(tmp + t);
                            var src = tmp + "\\" + t + "\\" + f.GetFileName();
                            if (src.FileExists()) continue;
                            FileCopyMove.FileCopy(f, src, true);
                        }
                    }

                    break;
            }
            var lstCreate = new ListCreator();
            lstCreate.CreateSortedLists();
            lstCreate.SortedFileListComplete();
            MsgBx.Msg("Mod Sorting Complete", "Mod Sorter");
        }

        public void OrganizeMaps() {
            var mapLst = new List<string> {
                "map",
                "Map"
            };

            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);

            if (gam.IsNullOrEmpty()) {
                MsgBx.Msg("Mod Sorting is Game specific, you need to choose a game", "Game Error");
                return;
            }

            SetSearchLists();

            string tmp;
            IEnumerable<string> lst;

            switch (gam) {
                case "ATS":
                    tmp = reg.Read(AtsRegKeys.ATS_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.scs");
                    if (lst.IsNull()) return;
                    foreach (var v in lst)
                    foreach (var t in _srchLstTs) {
                        if (!v.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                        FolderCreator.CreatePublicFolders(reg.Read(AtsRegKeys.ATS_GROUPS) + t);
                        FileCopyMove.FileCopy(v, reg.Read(AtsRegKeys.ATS_GROUPS) + t + "\\" + v.GetFileName(),
                            true);
                    }

                    break;

                case "ETS":
                    var ets = new EtsRegWork(true);
                    tmp = ets.Read(EtsRegKeys.ETS_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.scs");
                    if (lst.IsNull()) return;
                    foreach (var v in lst)
                    foreach (var t in _srchLstTs) {
                        if (!v.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                        FolderCreator.CreatePublicFolders(ets.Read(EtsRegKeys.ETS_GROUPS) + t);
                        FileCopyMove.FileCopy(v, ets.Read(EtsRegKeys.ETS_GROUPS) + t + "\\" + v.GetFileName(),
                            true);
                    }

                    break;

                case "FS15":
                    var fs15 = new Fs15RegWork(true);
                    tmp = fs15.Read(Fs15RegKeys.FS15_REPO);
                    lst = GetFilesFolders.GetFiles(tmp, "*.zip");
                    if (lst.IsNull()) return;
                    foreach (var v in lst)
                    foreach (var t in _srchLstFs) {
                        if (!v.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                        FolderCreator.CreatePublicFolders(fs15.Read(Fs15RegKeys.FS15_GROUPS) + t);
                        FileCopyMove.FileCopy(v, fs15.Read(Fs15RegKeys.FS15_GROUPS) + t + "\\" + v.GetFileName(),
                            true);
                    }

                    break;

                case "FS17":
                    var fs17 = new Fs17RegWork(true);
                    var bypass = new SortingBypass();
                    var byPassDat = bypass.GetBypassList();
                    tmp = fs17.Read(Fs17RegKeys.FS17_GROUPS);
                    var fldLst = GetFilesFolders.GetFolders(tmp, "*.*");
                    var chkList = new List<string>();
                    var enu = fldLst.ToList();
                    foreach (var v in enu) {
                        var chk = v.GetLastFolderName();
                        if (byPassDat.ContainsKey(chk)) continue;
                        chkList.Add(v);
                    }

                    if (chkList.IsNull()) return;
                    foreach (var v in chkList) {
                        lst = GetFilesFolders.GetFiles(v + "\\", "*.zip");
                        foreach (var f in lst)
                        foreach (var t in mapLst) {
                            if (!f.Contains(t, StringComparison.OrdinalIgnoreCase)) continue;
                            var src = tmp + "\\" + "Map" + "\\" + f.GetFileName();
                            if (src.FileExists()) continue;
                            FileCopyMove.FileCopy(f, src, true);
                        }
                    }

                    break;
            }

            var lstCreate = new ListCreator();
            lstCreate.CreateSortedLists();
            lstCreate.SortedFileListComplete();
            MsgBx.Msg("Map Sorting Complete", "Map Sorter");
        }
    }
}