// ***********************************************************************
// Assembly         : DMM
// Author           : TJ (DieHard)
// Created          : 10-08-2013
//
// Last Modified By : TJ (DieHard)
// Last Modified On : 10-08-2013
// ***********************************************************************
// <copyright file="FileWriter.cs" company="DieHard Development">
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Extensions;

namespace ModManagerUlt {
	/// <summary>
	///     Class FileWriter.
	/// </summary>
	internal class FileWriter {
		/// <summary>
		///     Changes the game settings.
		/// </summary>
		/// <param name="tmp">The temporary.</param>
		public void ChangeGameSettings(IEnumerable<string> tmp) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);

            switch (gam) {
                case "FS15":
                    File.WriteAllLines(reg.Read(Fs15RegKeys.FS15_GAME_SETTINGS_XML), tmp);
                    break;

                case "FS17":
                    File.WriteAllLines(reg.Read(Fs17RegKeys.FS17_GAME_SETTINGS_XML), tmp);
                    break;
            }
        }

		/// <summary>
		///     Loads the TXT file into List <string>tmp</string>.
		/// </summary>
		/// <param name="strFile">The STR file.</param>
		/// <returns>list</returns>
		internal List<string> FileToList(string strFile) {
            var tmp = new List<string>();
            if (!strFile.FileExists()) return tmp;
            using (var sr = new StreamReader(strFile)) {
                while (sr.Peek() > 0) tmp.Add(sr.ReadLine());
                sr.Close();
                sr.Dispose();
            }

            return tmp;
        }

		/// <summary>
		///     Saves a file
		/// </summary>
		/// <param name="content">Content of the file</param>
		/// <param name="fileName">Path of the file</param>
		/// <exception cref="System.Exception">Directory must be specified for the file</exception>
		/// <exception cref="Exception">Directory must be specified for the file</exception>
		internal void SaveFile(string content, string fileName) {
            FileStream writer = null;
            try {
                var contentBytes = Encoding.UTF8.GetBytes(content);
                var index = fileName.LastIndexOf('/');
                if (index <= 0) index = fileName.LastIndexOf(Path.DirectorySeparatorChar);
                if (index <= 0) throw new Exception("Directory must be specified for the file");
                var directory = fileName.Remove(index) + Path.DirectorySeparatorChar;
                if (!directory.FolderExists()) Directory.CreateDirectory(directory);
                var opened = false;
                while (!opened)
                    try {
                        writer = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                        opened = true;
                    }
                    catch (IOException e) {
                        MsgBx.Msg(e.Message, "File Open Error");
                    }

                writer.Write(contentBytes, 0, contentBytes.Length);
                writer.Close();
            }
            catch (Exception a) {
                MsgBx.Msg(a.Message, "File Write Error");
            }
            finally {
                if (writer == null) {
                }
                else {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }
    }
}