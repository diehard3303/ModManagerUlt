using System;
using System.Windows.Forms;

namespace ModManagerUlt {
    /// <summary>
    /// </summary>
    public partial class Fallout : Form {
        /// <summary>
        /// </summary>
        public Fallout() {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Dispose();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e) {
            if (radioButton4.Checked) {
                //set registry
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) {
            if (radioButton3.Checked) {
                //set registry
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) {
            if (radioButton2.Checked) {
                //set registry
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            if (radioButton1.Checked) {
                //set registry
            }
        }
    }
}