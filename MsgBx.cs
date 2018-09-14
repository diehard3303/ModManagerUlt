// ***********************************************************************
// Assembly         : DieHardsModManagerV3
// Author           : DieHard
// Created          : 10-18-2013
//
// Last Modified By : DieHard
// Last Modified On : 10-18-2013
// ***********************************************************************
// <copyright file="MsgBx.cs" company="DieHard Development">
//     Copyright (c) DieHard Development. All rights reserved.
// </copyright>
// Released under the FreeBSD  license
//Redistribution and use in source and binary forms, with or without
//modification, are permitted provided that the following conditions are met:
//
//1. Redistributions of source code must retain the above copyright notice, this
// list of conditions and the following disclaimer.
//2. Redistributions in binary form must reproduce the above copyright notice,
// this list of conditions and the following disclaimer in the documentation
// and/or other materials provided with the distribution.
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
//ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//The views and conclusions contained in the software and documentation are those
//of the authors and should not be interpreted as representing official policies,
//either expressed or implied, of the FreeBSD Project.
// ***********************************************************************

using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ModManagerUlt {
    /// <summary>
    ///     Class MsgBx.
    /// </summary>
    internal static class MsgBx {
        /// <summary>
        ///     internal MSG box.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="title">The title.</param>
        internal static void Msg(string statement, string title) {
            MessageBoxA(0, statement, title, 0);
        }

        /// <summary>
        ///     MSG ok.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="title">The title.</param>
        internal static void MsgOk(string msg, string title) {
            MessageBox.Show(msg, title, MessageBoxButtons.OK);
        }

        /// <summary>
        ///     MSG box yes no.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="title">The title.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal static bool MsgYesNo(string msg, string title) {
            var dlg = MessageBox.Show(msg, title, MessageBoxButtons.YesNo);
            return dlg.Equals(DialogResult.Yes);
        }

        /// <summary>
        ///     MsgBx box A.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="type">The type.</param>
        /// <returns>msgbox</returns>
        [DllImport("user32.dll")]
        private static extern int MessageBoxA(int hWnd,
            string msg,
            string caption,
            int type);
    }
}