// ***********************************************************************
// Assembly         : ModMaker1.0
// Author           : TJ
// Created          : 05-06-2015
//
// Last Modified By : TJ
// Last Modified On : 05-06-2015
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
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Extensions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.VisualBasic;
using SevenZip;

namespace ModManagerUlt {
    /// <summary>
    ///     Class FSZip.
    /// </summary>
    internal static class FsZip {
        /// <summary>
        ///     The ziP_7_ z_ executable
        /// </summary>
        private const string Zip7ZExe = @"\7-Zip\7z.exe";

        /// <summary>
        ///     The ziP_7_ z_ wp
        /// </summary>
        private const string Zip7ZWp = @"\7-Zip\";

        private static readonly string LibraryFilePath = ConfigurationManager.AppSettings["7zLocation"] ??
                                                         Path.Combine(
                                                             // ReSharper disable once AssignNullToNotNullAttribute
                                                             Path.GetDirectoryName(
                                                                 Assembly.GetExecutingAssembly().Location),
                                                             IntPtr.Size == 8 ? "7z64.dll" : "7z.dll");

        /// <summary>
        ///     Creates the zip.
        /// </summary>
        /// <param name="outPathname">The out pathname.</param>
        /// <param name="folderName">Name of the folder.</param>
        public static void CreateArchive(string outPathname, string folderName) {
            var fsOut = File.Create(outPathname);
            var zipStream = new ZipOutputStream(fsOut);
            zipStream.SetLevel(7); //0-9, 9 being the highest level of compression

            //zipStream.Password = password;  // optional. Null is the same as not setting. Required if using AES.

            // This setting will strip the leading part of the folder path in the entries, to
            // make the entries relative to the starting folder.
            // To include the full path for each entry up to the drive root, assign folderOffset = 0.
            var folderOffset = folderName.Length +
                               (folderName.EndsWith("\\", StringComparison.OrdinalIgnoreCase) ? 0 : 1);

            CompressFolder(folderName, zipStream, folderOffset);

            zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
            zipStream.Close();
            MsgBx.Msg("Archive Created: " + outPathname.GetFileName(), "Archive Creator");
        }

        /// <summary>
        ///     Extracts the archive.
        /// </summary>
        /// <param name="arcName">Name of the arc.</param>
        public static void ExtractArchive(string arcName) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var extPath = string.Empty;
            var archive = string.Empty;
            Dictionary<string, string> dic = null;

            switch (gam) {
                case "FS15":
                    extPath = reg.Read(Fs15RegKeys.FS15_WORK);
                    dic = Serializer.DeserializeDictionary(
                        reg.Read(Fs15RegKeys.FS15_XML) + "sortedFileListComplete.xml");
                    break;

                case "FS17":
                    extPath = reg.Read(Fs17RegKeys.FS17_WORK);
                    dic = Serializer.DeserializeDictionary(
                        reg.Read(Fs17RegKeys.FS17_XML) + "sortedFileListComplete.xml");
                    break;
            }

            if (dic != null && dic.Any(v => string.Equals(v.Key, arcName, StringComparison.OrdinalIgnoreCase)))
                dic.TryGetValue(arcName, out archive);
            var folderName = arcName.GetNameNoExtension();
            reg.Write(RegKeys.CURRENT_EXTRACTED_FOLDER_PATH, extPath + folderName + "\\");
            archive = archive + arcName;
            if (!archive.FileExists()) {
                MsgBx.Msg("There was a problem with the path to the Archive", "Process Error");
                return;
            }

            var line = @" x " + Vars.QuoteMark + archive + Vars.QuoteMark + " -o" +
                       Vars.QuoteMark + extPath + "\\" + folderName + Vars.QuoteMark + @" -r";

            ProcessArchive(line);

            MsgBx.Msg("Files have been extracted", "Archive Extractor");
        }

        /// <summary>
        ///     Extracts the mod SCS.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        public static string ExtractModScs(string path) {
            SevenZipBase.SetLibraryPath(LibraryFilePath);
            var reg = new AtsRegWork(true);
            var acGam = reg.Read(RegKeys.CURRENT_GAME);
            var extractionPath = string.Empty;

            switch (acGam) {
                case "ATS":
                    extractionPath = reg.Read(AtsRegKeys.ATS_WORK);
                    break;

                case "ETS":
                    extractionPath = reg.Read(EtsRegKeys.ETS_WORK);
                    break;
            }

            var folderName = path.GetNameNoExtension();

            reg.Write(RegKeys.CURRENT_EXTRACTED_FOLDER_PATH, extractionPath + folderName + "\\");

            var sbePath = new StringBuilder();
            sbePath.Append(extractionPath);
            sbePath.Append(Path.DirectorySeparatorChar);
            sbePath.Append(folderName);
            sbePath.Append(Path.DirectorySeparatorChar);

            if (folderName.IsNullOrEmpty() || extractionPath.IsNullOrEmpty())
                return "There was a problem with the archive / " +
                       "extraction path, please try again";

            SevenZipExtractor svChecker = null;
            bool chk;

            try {
                svChecker = new SevenZipExtractor(path);
                chk = svChecker.Check();
            }
            catch (Exception) {
                svChecker?.Dispose();
                return "This archive cannot be extracted, possible encryption or a damaged archive";
            }

            if (chk) {
                using (var extract = new SevenZipExtractor(path)) {
                    extract.ExtractArchive(sbePath.ToString());
                    extract.Dispose();
                }
            }
            else {
                MsgBx.Msg(@"This archive appears to be password protected, please enter password now",
                    @"Password Error");

                var input = Interaction.InputBox("Enter Password", "Enter Password");
                using (var pswdExtract = new SevenZipExtractor(path, input)) {
                    try {
                        pswdExtract.ExtractArchive(sbePath.ToString());
                        pswdExtract.Dispose();
                    }
                    catch (Exception) {
                        pswdExtract.Dispose();
                        MsgBx.Msg(@"That password seems to be invalid, I cannot continue", "Invalid Password");
                        return "Invalid password attempt";
                    }
                }
            }

            return "All files have been extracted from " + path.GetFileName();
        }

        // Recurses down the folder structure
        //
        private static void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset) {
            var files = Directory.GetFiles(path);

            foreach (var filename in files) {
                var fi = new FileInfo(filename);
                var entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                var newEntry = new ZipEntry(entryName) {
                    DateTime = fi.LastWriteTime,
                    Size = fi.Length
                };
                zipStream.PutNextEntry(newEntry);
                var buffer = new byte[4096];
                using (var streamReader = File.OpenRead(filename)) {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }

                zipStream.CloseEntry();
            }

            var folders = Directory.GetDirectories(path);
            foreach (var folder in folders) CompressFolder(folder, zipStream, folderOffset);
        }

        private static void ProcessArchive(string line) {
            var proc = new Process {
                StartInfo = {
                    Arguments = line,
                    FileName = Vars.QuoteMark + FsUtils.AppPath + Zip7ZExe + Vars.QuoteMark,
                    WorkingDirectory = Vars.QuoteMark + FsUtils.AppPath + Zip7ZWp + Vars.QuoteMark,
                    UseShellExecute = true
                }
            };
            proc.Start();
            proc.WaitForExit();
        }
    }
}