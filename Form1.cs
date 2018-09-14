using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    /// <summary>
    /// </summary>
    /// <seealso cref="Form" />
    public partial class Form1 : Form {
        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:ModManagerUlt.Form1" /> class.
        /// </summary>
        public Form1() {
            InitializeComponent();
        }

        private void americanTruckSImulatorToolStripMenuItem_Click(object sender, EventArgs e) {
            var ldr = new AtsLoader();
            ldr.StartAts();
        }


        private void AtsLoad() {
            var ats = new AtsLoader();
            var profLst = ats.LoadAtsProfiles();
            var groupsLst = ats.LoadAtsGroups();
            lstProfiles.Items.Clear();
            lstSortMods.Items.Clear();

            foreach (var v in profLst) lstProfiles.Items.Add(v.GetLastFolderName());

            foreach (var t in groupsLst) lstSortMods.Items.Add(t.GetLastFolderName());

            LoadProfileMods();
        }

        private void bAddNewProfile_Click(object sender, EventArgs e) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            if (txtAddProfile.Text.IsNullOrEmpty()) return;
            var prof = new ProfileWorker();
            prof.AddNewProfile(txtAddProfile.Text);
            txtAddProfile.Text = string.Empty;
            lblProfiles.Text = @"Active Profiles: " + lstProfiles.Items.Count;
            GameLoader(gam);
        }


        private void checkForBaseFilesUpdateToolStripMenuItem_Click(object sender, EventArgs e) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            switch (gam) {
                case "ATS":
                case "ETS":
                    var bs = new BaseFileExtractor();
                    bs.CheckBaseHash();
                    break;
            }
        }

        private void GameLoader(string gam) {
            switch (gam) {
                case "ETS":
                    EtsLoad();
                    break;
                case "ATS":
                    AtsLoad();
                    break;

                case "FS15":
                    Fs15Load();
                    break;

                case "FS17":
                    Fs17Load();
                    break;

                case "Fallout":
                    break;
            }
        }

        private void checkForDefFilesUpdateToolStripMenuItem_Click(object sender, EventArgs e) {
            var bs = new BaseFileExtractor();
            bs.ExtractDefFiles();
        }

        private void chkAts_CheckedChanged(object sender, EventArgs e) {
            if (!chkAts.Checked) return;
            var gi = new GameInfo();
            var ats = new AtsInit();
            ats.InitializeAts();
            chkEts.Checked = false;
            chkFs2015.Checked = false;
            chkFs2017.Checked = false;
            gi.SetGame("ATS");
            ClearLstBox();
            AtsLoad();
            lblGame.Text = @"Active Game: American Truck Simulator";
            lblActiveProfile.Text = @"Active Profile:";
            sortModsToolStripMenuItem.Text = @"Sort Mods for ATS";
        }

        private void chkEts_CheckedChanged(object sender, EventArgs e) {
            if (!chkEts.Checked) return;
            var gi = new GameInfo();
            var ets = new EtsInit();
            ets.InitializeEts();
            chkAts.Checked = false;
            chkFs2015.Checked = false;
            chkFs2017.Checked = false;
            gi.SetGame("ETS");
            ClearLstBox();
            EtsLoad();
            lblGame.Text = @"Active Game: Euro Truck Simulator 2";
            lblActiveProfile.Text = @"Active Profile:";
            sortModsToolStripMenuItem.Text = @"Sort Mods for ETS";
        }

        private void chkFs2015_CheckedChanged(object sender, EventArgs e) {
            if (!chkFs2015.Checked) return;
            var gi = new GameInfo();
            var fs15 = new Fs15Init();
            chkAts.Checked = false;
            chkFs2017.Checked = false;
            chkEts.Checked = false;
            fs15.InitializeFs15();
            gi.SetGame("FS15");
            ClearLstBox();
            Fs15Load();
            lblGame.Text = @"Active Game: Farming Simulator 2015";
            lblActiveProfile.Text = @"Active Profile:";
            sortModsToolStripMenuItem.Text = @"Sort Mods for FS2015";
        }

        private void chkFs2017_CheckedChanged(object sender, EventArgs e) {
            if (!chkFs2017.Checked) return;
            var gi = new GameInfo();
            var fs17 = new Fs17Init();
            chkAts.Checked = false;
            chkFs2015.Checked = false;
            chkEts.Checked = false;
            fs17.InitializeFs17();
            gi.SetGame("FS17");
            ClearLstBox();
            Fs17Load();
            lblGame.Text = @"Active Game: Farming Simulator 2017";
            lblActiveProfile.Text = @"Active Profile:";
            sortModsToolStripMenuItem.Text = @"Sort Mods for FS2017";
        }

        private void ClearLstBox() {
            lstAvailableMods.Items.Clear();
            lstProfileMods.Items.Clear();
            lstProfiles.Items.Clear();
            lstSortMods.Items.Clear();
        }

        private void copyModsToolStripMenuItem_Click(object sender, EventArgs e) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var prof = reg.Read(RegKeys.CURRENT_PROFILE);

            if (prof.IsNullOrEmpty() || gam.IsNullOrEmpty()) {
                MsgBx.Msg("You need to select a Game / Profile to use this Function", "Missing Data");
                return;
            }

            var cm = new CopyMods();
            var dic = cm.CopyProfileMods();
            if (dic.IsNull()) return;
            lstProfileMods.Items.Clear();
            foreach (var v in dic) lstProfileMods.Items.Add(v.Key);
        }

        private void DeleteRepoMod(string pth) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var dic = Serializer.DeserializeDictionary(pth);
            var lc = new ListCreator();
            var key = lstAvailableMods.SelectedItem.ToString();
            string pt;
            if (!dic.Any(v => string.Equals(v.Key, key, StringComparison.OrdinalIgnoreCase))) return;
            dic.TryGetValue(key, out pt);
            var mPth = pt + key;
            DeleteFiles.DeleteFilesOrFolders(mPth);
            lstAvailableMods.Items.Remove(key);
            lc.CreateSortedLists();
            lc.SortedFileListComplete();
            switch (gam) {
                case "ETS":
                    EtsLoad();
                    break;
                case "ATS":
                    AtsLoad();
                    break;
                case "FS15":
                    Fs15Load();
                    break;
                case "FS17":
                    Fs17Load();
                    CheckModCount();
                    break;
            }
        }

        private void CheckModCount() {
            if (lstAvailableMods.Items.Count > 0)
                lblAvailableMods.Text = @"Available Mods: " + lstAvailableMods.Items.Count;
            else
                lblAvailableMods.Text = @"Available Mods: 0";
        }

        private void editModToolStripMenuItem_Click(object sender, EventArgs e) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            Dictionary<string, string> dic = null;
            const string sortedList = "sortedFileListComplete.xml";
            var key = lstAvailableMods.SelectedItem.ToString();
            var pth = string.Empty;

            switch (gam) {
                case "ATS":
                    dic = Serializer.DeserializeDictionary(reg.Read(AtsRegKeys.ATS_XML) + sortedList);
                    break;

                case "ETS":
                    dic = Serializer.DeserializeDictionary(reg.Read(EtsRegKeys.ETS_XML) + sortedList);
                    break;

                case "FS15":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs15RegKeys.FS15_XML) + sortedList);
                    break;

                case "FS17":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_XML) + sortedList);
                    break;
            }

            if (dic != null && dic.Any(v => string.Equals(v.Key, key, StringComparison.OrdinalIgnoreCase)))
                dic.TryGetValue(key, out pth);

            reg.Write(RegKeys.CURRENT_ORIGINAL_FILE_EDIT_PATH, pth + "\\" + key);
            var mt = new ModEdit();
            reg.Write(RegKeys.CURRENT_FILE_EDIT, mt.EditMod(lstAvailableMods.SelectedItem.ToString()));
            var frm = new EditMod();
            frm.LoadTree();
            frm.ShowDialog();
        }

        private void eleteModToolStripMenuItem_Click(object sender, EventArgs e) {
            var reg = new AtsRegWork(true);
            const string sortedList = "sortedFileListComplete.xml";
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var pth = string.Empty;

            switch (gam) {
                case "ATS":
                    pth = reg.Read(AtsRegKeys.ATS_XML) + sortedList;
                    break;

                case "ETS":
                    pth = reg.Read(EtsRegKeys.ETS_XML) + sortedList;
                    break;

                case "FS15":
                    pth = reg.Read(Fs15RegKeys.FS15_XML) + sortedList;
                    break;

                case "FS17":
                    pth = reg.Read(Fs17RegKeys.FS17_XML) + sortedList;
                    break;
            }

            DeleteRepoMod(pth);
        }

        private void EtsLoad() {
            var ets = new EtsLoader();
            var profLst = ets.LoadEtsProfiles();
            var groupsLst = ets.LoadEtsGroups();
            lstProfiles.Items.Clear();
            lstSortMods.Items.Clear();
            foreach (var v in profLst) lstProfiles.Items.Add(v.GetLastFolderName());

            foreach (var t in groupsLst) lstSortMods.Items.Add(t.GetLastFolderName());

            LoadProfileMods();
        }

        private void euroTruckSimulatorToolStripMenuItem_Click(object sender, EventArgs e) {
            var ldr = new EtsLoader();
            ldr.StartEts();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void farmingSimulator2015ToolStripMenuItem_Click(object sender, EventArgs e) {
            var ldr = new Fs15Loader();
            ldr.StartFs15();
        }

        private void farmingSimulator2017ToolStripMenuItem_Click(object sender, EventArgs e) {
            var ldr = new Fs17Loader();
            ldr.StartFs17();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            var gi = new GameInfo();
            var reg = new AtsRegWork(true);
            reg.Write(RegKeys.CURRENT_PROFILE, "");
            gi.SetGame("");
        }

        private void Fs15Load() {
            var fs15 = new Fs15Loader();
            var profLst = fs15.LoadFs15Profiles();
            var groupsLst = fs15.LoadFs15Groups();
            lstProfiles.Items.Clear();
            lstSortMods.Items.Clear();
            foreach (var v in profLst) lstProfiles.Items.Add(v.GetLastFolderName());

            foreach (var t in groupsLst) lstSortMods.Items.Add(t.GetLastFolderName());

            LoadProfileMods();
        }

        private void Fs17Load() {
            var fs17 = new Fs17Loader();
            var profLst = fs17.LoadFs17Profiles();
            var groupsLst = fs17.LoadFs17Groups();
            lstProfiles.Items.Clear();
            lstSortMods.Items.Clear();
            foreach (var v in profLst) lstProfiles.Items.Add(v.GetLastFolderName());

            lblProfiles.Text = @"Active Profiles: " + lstProfiles.Items.Count;

            foreach (var t in groupsLst) lstSortMods.Items.Add(t.GetLastFolderName());

            LoadProfileMods();
        }

        private void groupsToolStripMenuItem_Click(object sender, EventArgs e) {
            Browser.BrowseFolders("groups");
        }

        private void loadDataBrowserToolStripMenuItem_Click(object sender, EventArgs e) {
            var th = new EditMod();
            th.ShowDialog();
        }

        private void LoadProfileMods() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            if (lstSortMods.SelectedItem.IsNull()) return;
            var chc = lstSortMods.SelectedItem.ToString();
            lstAvailableMods.Items.Clear();
            IEnumerable<string> lst = null;

            switch (gam) {
                case "ATS":
                    var ats = new AtsLoader();
                    lst = ats.LoadAtsGroupMods(chc);
                    break;

                case "ETS":
                    var ets = new EtsLoader();
                    lst = ets.LoadEtsGroupMods(chc);
                    break;

                case "FS15":
                    var fs15 = new Fs15Loader();
                    lst = fs15.LoadFs15GroupMods(chc);
                    break;

                case "FS17":
                    var fs17 = new Fs17Loader();
                    lst = fs17.LoadFs17GroupMods(chc);
                    break;
            }

            if (lst == null) return;
            foreach (var v in lst) lstAvailableMods.Items.Add(v.GetFileName());

            CheckModCount();
        }

        private void lstAvailableMods_SelectedIndexChanged(object sender, EventArgs e) {
        }

        private void lstProfiles_SelectedIndexChanged(object sender, EventArgs e) {
            if (lstProfiles.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            if (gam.IsNullOrEmpty()) return;
            lstProfileMods.Items.Clear();
            Dictionary<string, string> lst = null;
            lblActiveProfile.Text = @"Active Profile: " + lstProfiles.SelectedItem;
            reg.Write(RegKeys.CURRENT_PROFILE, lstProfiles.SelectedItem);

            switch (gam) {
                case "ATS":
                    var prof = new ProfileWorker();
                    var ats = new AtsLoader();
                    lst = ats.LoadAtsProfileMods(lstProfiles.SelectedItem.ToString());
                    prof.SetProfileActive();
                    break;

                case "ETS":
                    prof = new ProfileWorker();
                    var ets = new EtsLoader();
                    lst = ets.LoadEtsProfileMods(lstProfiles.SelectedItem.ToString());
                    prof.SetProfileActive();
                    break;

                case "FS15":
                    prof = new ProfileWorker();
                    var fs15 = new Fs15Loader();
                    lst = fs15.LoadFs15ProfileMods(lstProfiles.SelectedItem.ToString());
                    prof.SetProfileActive();
                    break;

                case "FS17":
                    prof = new ProfileWorker();
                    var fs17 = new Fs17Loader();
                    lst = fs17.LoadFs17ProfileMods(lstProfiles.SelectedItem.ToString());
                    prof.SetProfileActive();
                    lblProfiles.Text = @"Active Profiles: " + lstProfiles.Items.Count;
                    break;
            }

            if (lst == null) return;
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);

            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void lstSortMods_SelectedIndexChanged(object sender, EventArgs e) {
            Vars.FldName = lstSortMods.SelectedItem.ToString();
            LoadProfileMods();
        }

        private void modListToolStripMenuItem_Click(object sender, EventArgs e) {
            Browser.BrowseFolders("list");
        }

        private void profilesToolStripMenuItem_Click(object sender, EventArgs e) {
            Browser.BrowseFolders("profiles");
        }

        private void readLogFileToolStripMenuItem_Click(object sender, EventArgs e) {
            var rf = new ReadLogFile();
            rf.ReadLogFiles();
        }

        private void reMakeModListsToolStripMenuItem_Click(object sender, EventArgs e) {
            var lc = new ListCreator();
            lc.CreateSortedLists();
            lc.SortedFileListComplete();
        }

        private void removeModToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstProfileMods.SelectedItem.IsNull()) return;
            var prof = new ProfileWorker();
            var dic = prof.ProfileRemoveMod(lstProfileMods.SelectedItem.ToString());
            lstProfileMods.Items.Clear();

            foreach (var v in dic) lstProfileMods.Items.Add(v.Key);

            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void removeProfileToolStripMenuItem_Click(object sender, EventArgs e) {
            var prof = new ProfileWorker();
            var lst = prof.ProfileRemove();
            lstProfiles.Items.Clear();
            foreach (var v in lst) lstProfiles.Items.Add(v.GetLastFolderName());

            lblProfiles.Text = @"Active Profiles: " + lstProfiles.Items.Count;
        }

        private void repoToolStripMenuItem_Click(object sender, EventArgs e) {
            Browser.BrowseFolders("repo");
        }

        private void sortModsToolStripMenuItem_Click(object sender, EventArgs e) {
            var sort = new ModSorter();
            var lc = new ListCreator();
            sort.SortMods();
            sort.DoDeepSort();
            lc.CreateSortedLists();
            lc.SortedFileListComplete();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            string tmp;
            string profPath;
            Dictionary<string, string> lst = null;
            lstProfileMods.Items.Clear();

            switch (gam) {
                case "ATS":
                    var ats = new AtsLoader();
                    var atsp = new AtsRegWork(true);
                    var prof = new ProfileWorker();
                    tmp = ats.LoadAtsModChoice(lstAvailableMods.SelectedItem.ToString());
                    profPath = atsp.Read(AtsRegKeys.ATS_GAME_MOD_FOLDER) + tmp.GetFileName();
                    prof.BuildProfileList(lstAvailableMods.SelectedItem.ToString());
                    SymLinks.CreateSymLink(profPath, tmp, 0);
                    lst = ats.LoadAtsProfileMods(reg.Read(RegKeys.CURRENT_PROFILE));
                    break;

                case "ETS":
                    var ets = new EtsLoader();
                    var etsp = new EtsRegWork(true);
                    prof = new ProfileWorker();
                    tmp = ets.LoadEtsModChoice(lstAvailableMods.SelectedItem.ToString());
                    profPath = etsp.Read(EtsRegKeys.ETS_GAME_MOD_FOLDER) + tmp.GetFileName();
                    prof.BuildProfileList(lstAvailableMods.SelectedItem.ToString());
                    SymLinks.CreateSymLink(profPath, tmp, 0);
                    lst = ets.LoadEtsProfileMods(reg.Read(RegKeys.CURRENT_PROFILE));
                    break;

                case "FS15":
                    var fs15 = new Fs15Loader();
                    var fs15P = new Fs15RegWork(true);
                    tmp = fs15.LoadFs15ModChoice(lstAvailableMods.SelectedItem.ToString());
                    profPath = fs15P.Read(Fs15RegKeys.FS15_PROFILES) + reg.Read(RegKeys.CURRENT_PROFILE) + "\\" +
                               tmp.GetFileName();
                    prof = new ProfileWorker();
                    prof.BuildProfileList(lstAvailableMods.SelectedItem.ToString());
                    SymLinks.CreateSymLink(profPath, tmp, 0);
                    lst = fs15.LoadFs15ProfileMods(reg.Read(RegKeys.CURRENT_PROFILE));
                    break;

                case "FS17":
                    var fs17 = new Fs17Loader();
                    var fs17P = new Fs17RegWork(true);
                    tmp = fs17.LoadFs17ModChoice(lstAvailableMods.SelectedItem.ToString());
                    profPath = fs17P.Read(Fs17RegKeys.FS17_PROFILES) + reg.Read(RegKeys.CURRENT_PROFILE) + "\\" +
                               tmp.GetFileName();
                    prof = new ProfileWorker();
                    prof.BuildProfileList(lstAvailableMods.SelectedItem.ToString());
                    SymLinks.CreateSymLink(profPath, tmp, 0);
                    lst = fs17.LoadFs17ProfileMods(reg.Read(RegKeys.CURRENT_PROFILE));
                    break;
            }

            if (lst == null) return;
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);

            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;

            // MsgBx.Msg("Profile Link Created", "Linker");
        }

        private void workToolStripMenuItem_Click(object sender, EventArgs e) {
            Browser.BrowseFolders("work");
        }

        private void loadModManagerToolStripMenuItem_Click(object sender, EventArgs e) {
            var fall = new Fallout();
            fall.ShowDialog();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e) {
        }

        private void doDeepSortToolStripMenuItem_Click(object sender, EventArgs e) {
            var ms = new ModSorter();
            ms.DoDeepSort();
        }

        private void sortMapsToolStripMenuItem_Click(object sender, EventArgs e) {
            var ms = new ModSorter();
            ms.OrganizeMaps();
        }

        private void bypassSortingToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstSortMods.SelectedItem.IsNull()) return;
            var sb = new SortingBypass();
            sb.BypassSort(lstSortMods.SelectedItem.ToString());
        }

        private void moveModToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var mm = new Fs17MoveMod();
            mm.MoveMod(lstAvailableMods.SelectedItem.ToString());
        }

        private void addToFavoritesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            
            LoadOuts.CreateFavorite(lstAvailableMods.SelectedItem.ToString());
        }

        private void loadFavoritesToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("Case");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void favoritesToolStripMenuItem_Click(object sender, EventArgs e) {
            Browser.BrowseFolders("fav");
        }

        private void addToSpecialsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

          LoadOuts.CreateLoadouts("Specials", lstAvailableMods.SelectedItem.ToString());
        }

        private void addToToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            LoadOuts.CreateLoadouts("Krone", lstAvailableMods.SelectedItem.ToString());
        }

        private void addToFendtToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            LoadOuts.CreateLoadouts("Fendt", lstAvailableMods.SelectedItem.ToString());
        }

        private void addToCaseToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            LoadOuts.CreateLoadouts("Case", lstAvailableMods.SelectedItem.ToString());
        }

        private void addToPlowsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            
            LoadOuts.CreateLoadouts("Plows", lstAvailableMods.SelectedItem.ToString());
        }

        private void addToPlaceablesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            LoadOuts.CreateLoadouts("Placeables", lstAvailableMods.SelectedItem.ToString());
        }

        private void addToCultivatorsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            LoadOuts.CreateLoadouts("Cultivators", lstAvailableMods.SelectedItem.ToString());
        }

        private void addToJohnDeereToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            LoadOuts.CreateLoadouts("JohnDeere", lstAvailableMods.SelectedItem.ToString());
        }

        private void addToNewHollandToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

           LoadOuts.CreateLoadouts("NewHolland", lstAvailableMods.SelectedItem.ToString());
        }

        private void addToMasseyFergusonToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

           LoadOuts.CreateLoadouts("MasseyFerguson", lstAvailableMods.SelectedItem.ToString());
        }

        private void loadSpecialsToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("Specials");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadClaasToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("Claas");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadKroneToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("Krone");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadFendtToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("Fendt");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadPlowsToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("Plows");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadPlaceablesToolStripMenuItem_Click(object sender, EventArgs e) {
           var lst = LoadOuts.LoadFavorites("Placeables");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadCultivatorsToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("Cultivators");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadJohnDeereToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("JohnDeere");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadNewHollandToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("NewHolland");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadMasseyFergusonToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("MasseyFerguson");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void addToTractorsOtherToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            LoadOuts.CreateLoadouts("TractorsOther", lstAvailableMods.SelectedItem.ToString());
        }

        private void loadTractorsotherToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("TractorsOther");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadToolsToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("Tools");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void addToToolsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

           LoadOuts.CreateLoadouts("Tools", lstAvailableMods.SelectedItem.ToString());
        }

        private void addToBalersToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

           LoadOuts.CreateLoadouts("Balers", lstAvailableMods.SelectedItem.ToString());
        }

        private void loadBalersToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("Balers");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void loadTrailersToolStripMenuItem_Click(object sender, EventArgs e) {
            var lst = LoadOuts.LoadFavorites("Trailers");
            if (lst == null) return;
            lstProfileMods.Items.Clear();
            foreach (var v in lst) lstProfileMods.Items.Add(v.Key);
            CheckModCount();
            label2.Text = @"Active Mods: " + lstProfileMods.Items.Count;
        }

        private void addToTrailersToolStripMenuItem_Click(object sender, EventArgs e) {
            if (lstAvailableMods.SelectedItem.IsNull()) return;
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return;
            }

            LoadOuts.CreateLoadouts("Trailers", lstAvailableMods.SelectedItem.ToString());
        }
    }
}