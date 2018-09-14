namespace ModManagerUlt {
    partial class EditMod {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditMod));
            this.tree = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openEditFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.changeSiloAmountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moneyCheat5000000ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bLoadExtraction = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bLoadGameFiles = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.bSaveWork = new System.Windows.Forms.Button();
            this.bLoadSaveGames = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.ContextMenuStrip = this.contextMenuStrip1;
            this.tree.Location = new System.Drawing.Point(12, 39);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(546, 566);
            this.tree.TabIndex = 0;
            this.tree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDown);
            this.tree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tree_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.openEditFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.changeSiloAmountToolStripMenuItem,
            this.moneyCheat5000000ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(207, 82);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(203, 6);
            // 
            // openEditFileToolStripMenuItem
            // 
            this.openEditFileToolStripMenuItem.Name = "openEditFileToolStripMenuItem";
            this.openEditFileToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.openEditFileToolStripMenuItem.Text = "Open / Edit File";
            this.openEditFileToolStripMenuItem.Click += new System.EventHandler(this.openEditFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(203, 6);
            // 
            // changeSiloAmountToolStripMenuItem
            // 
            this.changeSiloAmountToolStripMenuItem.Name = "changeSiloAmountToolStripMenuItem";
            this.changeSiloAmountToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.changeSiloAmountToolStripMenuItem.Text = "Change Silo Amount";
            this.changeSiloAmountToolStripMenuItem.Click += new System.EventHandler(this.changeSiloAmountToolStripMenuItem_Click);
            // 
            // moneyCheat5000000ToolStripMenuItem
            // 
            this.moneyCheat5000000ToolStripMenuItem.Name = "moneyCheat5000000ToolStripMenuItem";
            this.moneyCheat5000000ToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.moneyCheat5000000ToolStripMenuItem.Text = "Money Cheat - (5000000)";
            this.moneyCheat5000000ToolStripMenuItem.Click += new System.EventHandler(this.moneyCheat5000000ToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folders.png");
            this.imageList1.Images.SetKeyName(1, "browser.png");
            this.imageList1.Images.SetKeyName(2, "website.png");
            this.imageList1.Images.SetKeyName(3, "tag.png");
            this.imageList1.Images.SetKeyName(4, "server.png");
            this.imageList1.Images.SetKeyName(5, "link.png");
            this.imageList1.Images.SetKeyName(6, "xml.png");
            this.imageList1.Images.SetKeyName(7, "png.png");
            this.imageList1.Images.SetKeyName(8, "zip.png");
            this.imageList1.Images.SetKeyName(9, "diskette.png");
            this.imageList1.Images.SetKeyName(10, "poster.png");
            this.imageList1.Images.SetKeyName(11, "pixelated.png");
            this.imageList1.Images.SetKeyName(12, "mp4.png");
            this.imageList1.Images.SetKeyName(13, "txt.png");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(710, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // bLoadExtraction
            // 
            this.bLoadExtraction.BackColor = System.Drawing.Color.White;
            this.bLoadExtraction.Location = new System.Drawing.Point(579, 232);
            this.bLoadExtraction.Name = "bLoadExtraction";
            this.bLoadExtraction.Size = new System.Drawing.Size(119, 40);
            this.bLoadExtraction.TabIndex = 4;
            this.bLoadExtraction.Text = "Load Extraction Files";
            this.bLoadExtraction.UseVisualStyleBackColor = false;
            this.bLoadExtraction.Click += new System.EventHandler(this.bLoadExtraction_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(596, 216);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "ATS / ETS Only";
            // 
            // bLoadGameFiles
            // 
            this.bLoadGameFiles.BackColor = System.Drawing.Color.White;
            this.bLoadGameFiles.Location = new System.Drawing.Point(579, 371);
            this.bLoadGameFiles.Name = "bLoadGameFiles";
            this.bLoadGameFiles.Size = new System.Drawing.Size(119, 40);
            this.bLoadGameFiles.TabIndex = 7;
            this.bLoadGameFiles.Text = "Load Game Data";
            this.bLoadGameFiles.UseVisualStyleBackColor = false;
            this.bLoadGameFiles.Click += new System.EventHandler(this.bLoadGameFiles_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(596, 355);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "FS15 - FS17 Only";
            // 
            // bSaveWork
            // 
            this.bSaveWork.BackColor = System.Drawing.Color.White;
            this.bSaveWork.Location = new System.Drawing.Point(579, 278);
            this.bSaveWork.Name = "bSaveWork";
            this.bSaveWork.Size = new System.Drawing.Size(119, 40);
            this.bSaveWork.TabIndex = 9;
            this.bSaveWork.Text = "Save Mod Edit";
            this.bSaveWork.UseVisualStyleBackColor = false;
            this.bSaveWork.Click += new System.EventHandler(this.bSaveWork_Click);
            // 
            // bLoadSaveGames
            // 
            this.bLoadSaveGames.BackColor = System.Drawing.Color.White;
            this.bLoadSaveGames.Location = new System.Drawing.Point(579, 417);
            this.bLoadSaveGames.Name = "bLoadSaveGames";
            this.bLoadSaveGames.Size = new System.Drawing.Size(119, 40);
            this.bLoadSaveGames.TabIndex = 10;
            this.bLoadSaveGames.Text = "Load Save Games";
            this.bLoadSaveGames.UseVisualStyleBackColor = false;
            this.bLoadSaveGames.Click += new System.EventHandler(this.bLoadSaveGames_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(564, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 57);
            this.label2.TabIndex = 11;
            this.label2.Text = "Save Game Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EditMod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 617);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bLoadSaveGames);
            this.Controls.Add(this.bSaveWork);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bLoadGameFiles);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bLoadExtraction);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditMod";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Mod";
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openEditFileToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button bLoadExtraction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bLoadGameFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button bSaveWork;
        private System.Windows.Forms.Button bLoadSaveGames;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem changeSiloAmountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moneyCheat5000000ToolStripMenuItem;
        private System.Windows.Forms.Label label2;
    }
}
