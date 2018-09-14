// ***********************************************************************
// Assembly         : ModMaker
// Author           : TJ
// Created          : 08-27-2015
//
// Last Modified By : TJ
// Last Modified On : 08-27-2015
// ***********************************************************************
// <copyright file="InternalEditor.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace ModManagerUlt {
    /// <summary>
    ///     Class InternalEditor.
    /// </summary>
    public partial class InternalEditor : Form {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InternalEditor" /> class.
        /// </summary>
        public InternalEditor() {
            InitializeComponent();
        }

        /// <summary>
        ///     save
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e) {
            //save
        }

        /// <summary>
        ///     dispose
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void button3_Click(object sender, EventArgs e) {
            Dispose();
        }

        /// <summary>
        ///     load
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void InternalEditor_Load(object sender, EventArgs e) {
        }
    }
}