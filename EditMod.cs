using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    /// <summary>
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class EditMod : Form {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EditMod" /> class.
        /// </summary>
        public EditMod() {
            InitializeComponent();
        }

        /// <summary>
        ///     Loads the tree.
        /// </summary>
        /// <param name="path">The path.</param>
        public void LoadTree(string path = null) {
            var reg = new AtsRegWork(true);
            tree.Nodes.Clear();
            tree.ImageList = imageList1;
            var dir = reg.Read(RegKeys.CURRENT_EXTRACTED_FOLDER_PATH);

            if (path != null) dir = path;
            if (dir.IsNullOrEmpty() && path.IsNullOrEmpty()) return;

            var stack = new Stack<TreeNode>();
            var rootDirectory = new DirectoryInfo(dir);
            var node = new TreeNode(rootDirectory.FullName) {Tag = rootDirectory, ImageIndex = 4};
            stack.Push(node);

            while (stack.Count > 0) {
                var currentNode = stack.Pop();
                var directoryInfo = (DirectoryInfo) currentNode.Tag;
                foreach (
                    var childDirectoryNode in
                    directoryInfo.EnumerateDirectories()
                        .Select(directory => new TreeNode(directory.Name) {Tag = directory, ImageIndex = 0})) {
                    if (childDirectoryNode.Text == @"mods" || childDirectoryNode.Text == @"music"
                                                           || childDirectoryNode.Text == @"pdlc" ||
                                                           childDirectoryNode.Text == @"pending_downloads"
                                                           || childDirectoryNode.Text == @"savegameBackup" ||
                                                           childDirectoryNode.Text == @"shader_cache"
                                                           || childDirectoryNode.Text == @"screenshots" ||
                                                           childDirectoryNode.Text == @"vehicleSort")
                        continue;
                    currentNode.Nodes.Add(childDirectoryNode);
                    stack.Push(childDirectoryNode);
                }

                foreach (var file in directoryInfo.GetFiles()) {
                    var ext = file.Extension;
                    switch (ext) {
                        case ".scs":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 9, 9));
                            break;

                        case ".xml":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 6, 6));
                            break;

                        case ".tobj":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 3, 3));
                            break;

                        case ".dds":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 7, 7));
                            break;

                        case ".jpg":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 7, 7));
                            break;

                        case ".png":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 7, 7));
                            break;

                        case ".bmp":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 7, 7));
                            break;

                        case ".mat":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 10, 10));
                            break;

                        case ".sii":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 13, 13));
                            break;

                        case ".zip":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 8, 8));
                            break;

                        case ".gdm":
                        case ".grle":
                        case "":
                        case ".dat":
                        case ".gmss":
                        case ".cache":
                            break;

                        case ".pmc":
                        case ".pmd":
                        case ".pmg":
                        case ".pma":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 5, 5));
                            break;

                        case ".ogg":
                            currentNode.Nodes.Add(new TreeNode(file.Name, 12, 12));
                            break;

                        default:
                            currentNode.Nodes.Add(new TreeNode(file.Name, 3, 3));
                            break;
                    }
                }
            }

            tree.Nodes.Add(node);
        }

        private void bLoadExtraction_Click(object sender, EventArgs e) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);

            switch (gam) {
                case "ATS":
                    tree.Nodes.Clear();
                    LoadTree(reg.Read(AtsRegKeys.ATS_EXTRACTION));
                    break;

                case "ETS":
                    tree.Nodes.Clear();
                    LoadTree(reg.Read(EtsRegKeys.ETS_EXTRACTION));
                    break;
            }
        }

        private void bLoadGameFiles_Click(object sender, EventArgs e) {
            var reg = new Fs15RegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            string pth;

            switch (gam) {
                case "FS15":
                    pth = reg.Read(Fs15RegKeys.FS15_DATA_DIR);
                    if (pth.IsNullOrEmpty()) {
                        tree.Nodes.Clear();
                        var ofd = new FolderBrowserDialog {Description = @"Navigate to your Game Data Folder"};
                        ofd.ShowDialog();
                        if (!ofd.SelectedPath.FolderExists()) return;
                        reg.Write(Fs15RegKeys.FS15_DATA_DIR, ofd.SelectedPath + "\\vehicles\\");
                        LoadTree(ofd.SelectedPath);
                        return;
                    }

                    tree.Nodes.Clear();
                    LoadTree(pth);
                    break;

                case "FS17":
                    pth = reg.Read(Fs17RegKeys.FS17_DATA_DIR);
                    if (pth.IsNullOrEmpty()) {
                        tree.Nodes.Clear();
                        var ofd = new FolderBrowserDialog {Description = @"Navigate to your Game Data Folder"};
                        ofd.ShowDialog();
                        if (!ofd.SelectedPath.FolderExists()) return;
                        reg.Write(Fs17RegKeys.FS17_DATA_DIR, ofd.SelectedPath + "\\vehicles\\");
                        LoadTree(reg.Read(Fs17RegKeys.FS17_DATA_DIR));
                        return;
                    }

                    tree.Nodes.Clear();
                    LoadTree(pth);
                    break;
            }
        }

        private void bLoadSaveGames_Click(object sender, EventArgs e) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            string savePath;

            switch (gam) {
                case "FS15":
                    savePath = reg.Read(Fs15RegKeys.FS15_SAVE_GAMES);
                    if (savePath.IsNullOrEmpty()) {
                        var ofd = new FolderBrowserDialog {
                            Description = @"Navigate to the FS 15 SaveGame folder",
                            ShowNewFolderButton = false
                        };
                        ofd.ShowDialog();
                        if (!ofd.SelectedPath.IsNullOrEmpty()) {
                            savePath = ofd.SelectedPath;
                            reg.Write(Fs15RegKeys.FS15_SAVE_GAMES, savePath);
                        }
                        else {
                            return;
                        }
                    }

                    LoadTree(savePath);
                    break;

                case "FS17":
                    savePath = reg.Read(Fs17RegKeys.FS17_SAVE_GAMES);
                    if (savePath.IsNullOrEmpty()) {
                        var ofd = new FolderBrowserDialog {
                            Description = @"Navigate to the FS 17 SaveGame folder",
                            ShowNewFolderButton = false
                        };
                        ofd.ShowDialog();
                        if (!ofd.SelectedPath.IsNullOrEmpty()) {
                            savePath = ofd.SelectedPath;
                            reg.Write(Fs17RegKeys.FS17_SAVE_GAMES, savePath);
                        }
                        else {
                            return;
                        }
                    }

                    LoadTree(savePath);
                    break;
            }
        }

        private void bSaveWork_Click(object sender, EventArgs e) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var outPath = reg.Read(RegKeys.CURRENT_ORIGINAL_FILE_EDIT_PATH);
            FsZip.CreateArchive(outPath, reg.Read(RegKeys.CURRENT_EXTRACTED_FOLDER_PATH));
            DeleteFiles.DeleteFilesOrFolders(reg.Read(RegKeys.CURRENT_EXTRACTED_FOLDER_PATH));

            tree.Nodes.Clear();

            switch (gam) {
                case "ATS":
                    FolderCreator.CreatePublicFolders(reg.Read(AtsRegKeys.ATS_WORK));
                    break;

                case "ETS":
                    FolderCreator.CreatePublicFolders(reg.Read(EtsRegKeys.ETS_WORK));
                    break;

                case "FS15":
                    FolderCreator.CreatePublicFolders(reg.Read(Fs15RegKeys.FS15_WORK));
                    break;

                case "FS17":
                    FolderCreator.CreatePublicFolders(reg.Read(Fs17RegKeys.FS17_WORK));
                    break;
            }
        }

        private void changeSiloAmountToolStripMenuItem_Click(object sender, EventArgs e) {
            var cng = new ChangeSiloAmount();
            cng.ChangeSiloTotalAmount(tree.SelectedNode.FullPath + "\\");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Dispose();
        }

        private void moneyCheat5000000ToolStripMenuItem_Click(object sender, EventArgs e) {
            var fsc = new FsCheatMoney();
            fsc.SetCheatValues(tree.SelectedNode.FullPath);
        }

        private void openEditFileToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!tree.SelectedNode.FullPath.FileExists()) return;
            Process.Start(tree.SelectedNode.FullPath);
        }

        private void tree_MouseDown(object sender, MouseEventArgs e) {
            var fs = new FsCheatMoney();
            var hitInfo = tree.HitTest(e.X, e.Y);
            if (hitInfo.IsNull()) return;
            if (hitInfo.Location != TreeViewHitTestLocations.Label) return;
            tree.SelectedNode = hitInfo.Node;
            label2.Text = @"Save Game Name: " + fs.GetMapName(tree.SelectedNode.FullPath);
        }

        private void tree_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) return;
            var hitInfo = tree.HitTest(e.X, e.Y);
            if (hitInfo.IsNull()) return;
            if (hitInfo.Location != TreeViewHitTestLocations.Label) return;
            tree.SelectedNode = hitInfo.Node;
        }
    }
}