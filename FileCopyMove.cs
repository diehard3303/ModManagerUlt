// ***********************************************************************
// Assembly         : SCSModMaker
// Author           : TJ
// Created          : 03-08-2015
//
// Last Modified By : TJ
// Last Modified On : 03-08-2015
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

using System;
using System.Collections.Generic;
using System.IO;
using Extensions;

namespace ModManagerUlt {
    /// <summary>
    ///     Class FileCopyMove.
    /// </summary>
    public static class FileCopyMove {
        /// <summary>
        ///     Copies the directory.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="deleteSrc">if set to <c>true</c> [delete source].</param>
        public static void CopyDirectory(string source, string target, bool deleteSrc = false) {
            //if (!source.FolderExists() || !target.FolderExists()) return;
            var stack = new Stack<Folders>();
            stack.Push(new Folders(source, target));

            while (stack.Count > 0) {
                var folders = stack.Pop();
                Directory.CreateDirectory(folders.Target);
                foreach (var file in Directory.EnumerateFiles(folders.Source, "*.*")) {
                    if (Equals(file, null))
                        continue;
                    var targetFile = Path.Combine(folders.Target, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);
                    File.Copy(file, targetFile);
                }

                foreach (var folder in Directory.EnumerateDirectories(folders.Source))
                    if (folder != null)
                        stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
            }

            if (deleteSrc) Directory.Delete(source, true);
        }

        /// <summary>
        ///     Fast file move with big buffers
        /// </summary>
        /// <param name="source">Source file path</param>
        /// <param name="destination">Destination file path</param>
        /// <param name="deleteSrc">if set to <c>true</c> [delete source].</param>
        public static void FileCopy(string source, string destination, bool deleteSrc = false) {
            if (Equals(source, destination)) return;
            if (!source.FileExists()) return;
            var arrayLength = (int) Math.Pow(2, 19);
            var dataArray = new byte[arrayLength];
            if (!destination.DirectoryName().FolderExists())
                try {
                    Directory.CreateDirectory(destination.DirectoryName());
                }
                catch (Exception e) {
                    MsgBx.Msg(e.Message, "File IO ERROR");
                }

            using (var fsread = new FileStream
                (source, FileMode.Open, FileAccess.Read, FileShare.None, arrayLength)) {
                using (var bwread = new BinaryReader(fsread)) {
                    using (var fswrite = new FileStream
                        (destination, FileMode.Create, FileAccess.Write, FileShare.None, arrayLength)) {
                        using (var bwwrite = new BinaryWriter(fswrite)) {
                            for (;;) {
                                var read = bwread.Read(dataArray, 0, arrayLength);
                                if (0 == read)
                                    break;
                                bwwrite.Write(dataArray, 0, read);
                            }
                        }
                    }
                }
            }

            if (deleteSrc) DeleteFiles.DeleteFilesOrFolders(source, false);
        }

        /// <summary>
        ///     Class Folders.
        /// </summary>
        private class Folders {
            /// <summary>
            ///     Initializes a new instance of the <see cref="Folders" /> class.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="target">The target.</param>
            public Folders(string source, string target) {
                Source = source;
                Target = target;
            }

            /// <summary>
            ///     Gets or sets the source.
            /// </summary>
            /// <value>The source.</value>
            public string Source { get; }

            /// <summary>
            ///     Gets or sets the target.
            /// </summary>
            /// <value>The target.</value>
            public string Target { get; }
        }
    }
}