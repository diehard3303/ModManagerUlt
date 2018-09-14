//
//  Author:
//    DieHard diehard@eclcmail.com
//
//  Copyright (c) 2016, 2007
//
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
//       the documentation and/or other materials provided with the distribution.
//     * Neither the name of the [ORGANIZATION] nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    /// <summary>
    ///     Base file extractor.
    /// </summary>
    public class BaseFileExtractor {
        private const string EXTRACTOR = "\\scs_extractor.exe";
        private static readonly string ScsExtractor = Utils.AppPath + @"extractor\scs_extractor.exe";

        /// <summary>
        ///     Checks the base hash.
        /// </summary>
        public void CheckBaseHash() {
            var reg = new AtsRegWork(true);
            var acGam = reg.Read(RegKeys.CURRENT_GAME);
            string tmp;
            string hsh;
            string newHash;

            switch (acGam) {
                case "ATS":
                    tmp = reg.Read(AtsRegKeys.ATS_BASE_SCS_LOCATION);
                    if (tmp.IsNullOrEmpty()) {
                        LocateBase();
                        tmp = reg.Read(AtsRegKeys.ATS_BASE_SCS_LOCATION);
                    }

                    hsh = reg.Read(AtsRegKeys.ATS_BASE_HASH);
                    newHash = Hasher.HashFile(tmp);
                    if (!string.Equals(newHash, hsh, StringComparison.OrdinalIgnoreCase)) {
                        reg.Write(AtsRegKeys.ATS_BASE_HASH, newHash);
                        ExtractBaseFiles();
                        ExtractDefFiles();
                    }
                    else {
                        MsgBx.Msg("No Update Necessary", "Up to Date Base Files");
                    }

                    break;

                case "ETS":
                    tmp = reg.Read(EtsRegKeys.ETS_BASE_SCS_LOCATION);
                    if (tmp.IsNullOrEmpty()) {
                        LocateBase();
                        tmp = reg.Read(EtsRegKeys.ETS_BASE_SCS_LOCATION);
                    }

                    hsh = reg.Read(EtsRegKeys.ETS_BASE_HASH);
                    newHash = Hasher.HashFile(tmp);
                    if (!string.Equals(newHash, hsh, StringComparison.OrdinalIgnoreCase)) {
                        reg.Write(EtsRegKeys.ETS_BASE_HASH, newHash);
                        ExtractBaseFiles();
                        ExtractDefFiles();
                    }
                    else {
                        MsgBx.Msg("No Update Necessary", "Up to Date Base Files");
                    }

                    break;
            }
        }

        /// <summary>
        ///     Extracts the definition files.
        /// </summary>
        public void ExtractDefFiles() {
            var reg = new AtsRegWork(true);
            var acGam = reg.Read(RegKeys.CURRENT_GAME);
            string tmp;
            string hsh;
            string newHash;

            switch (acGam) {
                case "ATS":
                    tmp = reg.Read(AtsRegKeys.ATS_DEF_SCS_LOCATION);
                    if (tmp.IsNullOrEmpty()) {
                        LocateDef();
                        tmp = reg.Read(AtsRegKeys.ATS_DEF_SCS_LOCATION);
                    }

                    hsh = reg.Read(AtsRegKeys.ATS_DEF_HASH);
                    newHash = Hasher.HashFile(tmp);
                    if (!string.Equals(newHash, hsh, StringComparison.OrdinalIgnoreCase)) {
                        reg.Write(AtsRegKeys.ATS_DEF_HASH, newHash);
                        FileCopy.Source = ScsExtractor;
                        FileCopy.Destination = reg.Read(AtsRegKeys.ATS_EXTRACTION) + ScsExtractor.GetFileName();
                        FileCopy.DoCopy();

                        FileCopy.Source = tmp;
                        FileCopy.Destination = reg.Read(AtsRegKeys.ATS_EXTRACTION) + "def.scs";
                        FileCopy.DoCopy();
                        ExtractScs(reg.Read(AtsRegKeys.ATS_EXTRACTION), tmp);
                    }

                    break;

                case "ETS":
                    tmp = reg.Read(EtsRegKeys.ETS_DEF_SCS_LOCATION);
                    if (tmp.IsNullOrEmpty()) {
                        LocateDef();
                        tmp = reg.Read(EtsRegKeys.ETS_DEF_SCS_LOCATION);
                    }

                    hsh = reg.Read(EtsRegKeys.ETS_DEF_HASH);
                    newHash = Hasher.HashFile(tmp);
                    var fld = reg.Read(EtsRegKeys.ETS_EXTRACTION);
                    if (!string.Equals(newHash, hsh, StringComparison.OrdinalIgnoreCase)) {
                        reg.Write(EtsRegKeys.ETS_DEF_HASH, newHash);
                        FileCopy.Source = ScsExtractor;
                        FileCopy.Destination = fld + ScsExtractor;
                        FileCopy.DoCopy();

                        FileCopy.Source = tmp;
                        FileCopy.Destination = fld + "def.scs";
                        FileCopy.DoCopy();
                        ExtractScs(fld, tmp);
                    }

                    break;
            }

            MsgBx.Msg("All base files have been updated", "Base File Extraction");
        }

        private static void ExtractBaseFiles() {
            var reg = new AtsRegWork(true);
            var acGam = reg.Read(RegKeys.CURRENT_GAME);
            string tmp;

            switch (acGam) {
                case "ATS":
                    var fld = reg.Read(AtsRegKeys.ATS_EXTRACTION);
                    tmp = reg.Read(AtsRegKeys.ATS_BASE_SCS_LOCATION);
                    FileCopy.Source = ScsExtractor;
                    FileCopy.Destination = fld + ScsExtractor.GetFileName();
                    FileCopy.DoCopy();

                    FileCopy.Source = tmp;
                    FileCopy.Destination = fld + "base.scs";
                    FileCopy.DoCopy();

                    ExtractScs(fld, tmp);

                    break;

                case "ETS":
                    tmp = reg.Read(EtsRegKeys.ETS_BASE_SCS_LOCATION);
                    fld = reg.Read(EtsRegKeys.ETS_EXTRACTION);

                    FileCopy.Source = ScsExtractor;
                    FileCopy.Destination = fld + ScsExtractor.GetFileName();
                    FileCopy.DoCopy();

                    FileCopy.Source = tmp;
                    FileCopy.Destination = fld + "base.scs";
                    FileCopy.DoCopy();

                    ExtractScs(fld, tmp);

                    break;
            }
        }

        private static void ExtractScs(string fPath, string arg) {
            var file = arg.GetFileName();
            if (fPath.IsNullOrEmpty()) return;
            var ps = new Process {StartInfo = {FileName = fPath + EXTRACTOR, UseShellExecute = true}};
            if (fPath != null) ps.StartInfo.WorkingDirectory = fPath;
            ps.StartInfo.Arguments = file;

            ps.Start();
            ps.WaitForExit();

            MessageBox.Show(@"All files from " + file + @" have been extracted", @"SCS File Extractor");
        }

        private static void LocateBase() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);

            switch (gam) {
                case "ATS":
                    var ofd = new OpenFileDialog {Title = @"Locate the Base SCS File"};
                    ofd.ShowDialog();

                    if (!ofd.FileName.FileExists()) return;
                    reg.Write(AtsRegKeys.ATS_BASE_SCS_LOCATION, ofd.FileName);
                    break;

                case "ETS":
                    ofd = new OpenFileDialog {Title = @"Locate the Base SCS File"};
                    ofd.ShowDialog();

                    if (!ofd.FileName.FileExists()) return;
                    reg.Write(EtsRegKeys.ETS_BASE_SCS_LOCATION, ofd.FileName);
                    break;
            }
        }

        private static void LocateDef() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);

            switch (gam) {
                case "ATS":
                    var ofd = new OpenFileDialog {Title = @"Locate the Def SCS File"};
                    ofd.ShowDialog();

                    if (!ofd.FileName.FileExists()) return;
                    reg.Write(AtsRegKeys.ATS_DEF_SCS_LOCATION, ofd.FileName);
                    break;

                case "ETS":
                    ofd = new OpenFileDialog {Title = @"Locate the Def SCS File"};
                    ofd.ShowDialog();

                    if (!ofd.FileName.FileExists()) return;
                    reg.Write(EtsRegKeys.ETS_DEF_SCS_LOCATION, ofd.FileName);
                    break;
            }
        }
    }
}