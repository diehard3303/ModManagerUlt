using System;
using System.IO;
using System.Runtime.InteropServices;
using Extensions;

// ***********************************************************************
// Assembly         : ModMakerV2
// Author           : TJ
// Created          : 03-28-2015
//
// Last Modified By : TJ
// Last Modified On : 03-28-2015
// ***********************************************************************
//
//  Copyright (c) DieHard Development 2003-2015. All rights reserved.
//
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

namespace ModManagerUlt {
    /// <summary>
    ///     Class DeleteFiles.
    /// </summary>
    internal static class DeleteFiles {
        /// <summary>
        ///     Deletes the folders / files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="pop"></param>
        public static void DeleteFilesOrFolders(string path, bool pop = true) {
            var del = false;
            var attr = new FileAttributes();
            if (!path.IsNullOrEmpty())
                if (IsDirectory(path) || IsFile(path))
                    attr = File.GetAttributes(path);
            //detect whether its a directory or file
            if (attr.IsEmpty()) return;
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory) {
                var directory = new DirectoryInfo(path);
                try {
                    foreach (var file in directory.GetFiles()) DeleteFile(file.ToString());
                    foreach (var subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
                    directory.EmptyThisDirectory();
                    directory.Delete(true);
                }
                catch (Exception g) {
                    MsgBx.Msg(g.ToString(), "Deletion Error");
                }
            }
            else {
                try {
                    if (!path.FileExists()) return;
                    del = DeleteFile(path);
                }
                catch (Exception d) {
                    MsgBx.Msg(d.ToString(), "Deletion Error");
                }
            }

            if (!pop) return;
            if (del) MsgBx.Msg("File(s) have been deleted", "File Deletion");
        }

        /// <summary>
        ///     Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DeleteFile(string path);

        private static bool IsDirectory(string dta) {
            var isDir = false;
            if (!dta.FolderExists()) return false;
            var attr = File.GetAttributes(dta);

            if (attr == FileAttributes.Directory) isDir = true;
            return isDir;
        }

        private static bool IsFile(string dta) {
            var isFile = false;
            if (!dta.FileExists()) return false;

            var attr = File.GetAttributes(dta);
            if (attr.IsEmpty()) return false;
            if (attr.Equals(FileAttributes.Archive) || attr.Equals(FileAttributes.Normal)) isFile = true;
            return isFile;
        }
    }
}