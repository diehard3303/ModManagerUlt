// ***********************************************************************
// Assembly         : ModMakerV094
// Author           : TJ
// Created          : 05-18-2015
//
// Last Modified By : TJ
// Last Modified On : 05-18-2015
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

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ModManagerUlt {
    /// <summary>
    ///     Class Hasher.
    /// </summary>
    internal static class Hasher {
        /// <summary>
        ///     Creates fileStream to file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>System.String.</returns>
        public static string HashFile(string filePath) {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                return HashFile(fs);
            }
        }

        /// <summary>
        ///     Computes the file content hash.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>System.String.</returns>
        private static string HashFile(Stream stream) {
            var sb = new StringBuilder();

            if (stream == null) return sb.ToString();
            stream.Seek(0, SeekOrigin.Begin);

            var sha = SHA1.Create();
            var hash = sha.ComputeHash(stream);
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            stream.Seek(0, SeekOrigin.Begin);

            return sb.ToString();
        }
    }
}