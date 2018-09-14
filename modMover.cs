using System;
using System.Deployment.Application;
using System.Windows.Forms;

namespace ModManagerUlt {
    /// <summary>
    /// 
    /// </summary>
    public partial class ModMover : Form {
        /// <summary>
        /// 
        /// </summary>
        public ModMover() {
            InitializeComponent();
        }

        private void lstModMove_SelectedIndexChanged(object sender, EventArgs e) {
            Dispose();
        }
    }
}