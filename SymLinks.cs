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
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Extensions;
using Microsoft.Win32.SafeHandles;

namespace ModManagerUlt {
    internal static class SymLinks {
        /// <summary>
        /// </summary>
        /// <param name="totalFileSize"></param>
        /// <param name="totalBytesTransferred"></param>
        /// <param name="streamSize"></param>
        /// <param name="streamBytesTransferred"></param>
        /// <param name="dwStreamNumber"></param>
        /// <param name="dwCallbackReason"></param>
        /// <param name="hSourceFile"></param>
        /// <param name="hDestinationFile"></param>
        /// <param name="lpData"></param>
        public delegate CopyProgressResult CopyProgressRoutine(
            long totalFileSize,
            long totalBytesTransferred,
            long streamSize,
            long streamBytesTransferred,
            uint dwStreamNumber,
            CopyProgressCallbackReason dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData);

        /// <summary>
        /// </summary>
        [Flags]
        public enum CopyFileFlags : uint {
            /// <summary>
            /// </summary>
            CopyFileFailIfExists = 0x00000001,

            /// <summary>
            /// </summary>
            CopyFileRestartable = 0x00000002,

            /// <summary>
            /// </summary>
            CopyFileOpenSourceForWrite = 0x00000004,

            /// <summary>
            /// </summary>
            CopyFileAllowDecryptedDestination = 0x00000008,

            /// <summary>
            /// </summary>
            CopyFileCopySymlink = 0x00000800 //NT 6.0+
        }

        /// <summary>
        /// </summary>
        public enum CopyProgressCallbackReason : uint {
            /// <summary>
            /// </summary>
            CallbackChunkFinished = 0x00000000,

            /// <summary>
            /// </summary>
            CallbackStreamSwitch = 0x00000001
        }

        /// <summary>
        /// </summary>
        public enum CopyProgressResult : uint {
            /// <summary>
            /// </summary>
            ProgressContinue = 0,

            /// <summary>
            /// </summary>
            ProgressCancel = 1,

            /// <summary>
            /// </summary>
            ProgressStop = 2,

            /// <summary>
            /// </summary>
            ProgressQuiet = 3
        }

        private const int CREATION_DISPOSITION_OPEN_EXISTING = 3;
        private const int FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        private const int FILE_SHARE_READ = 1;
        private const int FILE_SHARE_WRITE = 2;
        private const uint SYMBOLIC_LINK_FLAG_DIRECTORY = 1;
        private const uint SYMBOLIC_LINK_FLAG_FILE = 0;

        /// <summary>
        ///     Copies the file ex.
        /// </summary>
        /// <param name="lpExistingFileName">Name of the lp existing file.</param>
        /// <param name="lpNewFileName">Name of the lp new file.</param>
        /// <param name="lpProgressRoutine">The lp progress routine.</param>
        /// <param name="lpData">The lp data.</param>
        /// <param name="pbCancel">if set to <c>true</c> [pb cancel].</param>
        /// <param name="dwCopyFlags">The dw copy flags.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CopyFileEx(string lpExistingFileName,
            string lpNewFileName, CopyProgressRoutine lpProgressRoutine,
            IntPtr lpData, ref bool pbCancel, CopyFileFlags dwCopyFlags);

        /// <summary>
        ///     Creates the sym link.
        /// </summary>
        /// <param name="symLinkFileName">Name of the file.</param>
        /// <param name="targetName">Name of the target.</param>
        /// <param name="num">The number.</param>
        /// <exception cref="Win32Exception"></exception>
        public static void CreateSymLink(string symLinkFileName, string targetName, uint num) {
            if (symLinkFileName.FileExists()) return;
            var success = CreateSymbolicLink(symLinkFileName, targetName, num);
            if (success) return;
           // var error = Marshal.GetLastWin32Error();
            MsgBx.Msg("There was a problem with the path creating the link, link creation failure", "Link Creator");
        }

        /// <summary>
        ///     Gets the symbolic link target.
        /// </summary>
        /// <param name="symlink">The symlink.</param>
        /// <returns></returns>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static string GetSymbolicLinkTarget(FileSystemInfo symlink) {
            var directoryHandle = CreateFile(symlink.FullName, 0, 2, IntPtr.Zero, CREATION_DISPOSITION_OPEN_EXISTING,
                FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);
            if (directoryHandle.IsInvalid)
                //throw new Win32Exception(Marshal.GetLastWin32Error());
                return null;
            
            var path = new StringBuilder(512);
            var size = GetFinalPathNameByHandle(directoryHandle.DangerousGetHandle(), path, path.Capacity, 0);
            if (size < 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            if (path[0] == '\\' && path[1] == '\\' && path[2] == '?' && path[3] == '\\')
                return path.ToString().Substring(4);
            return path.ToString();
        }

        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode,
            IntPtr securityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CreateSymbolicLink(string symlinkFileName, string targetFileName, uint flags);

        [DllImport("kernel32.dll", EntryPoint = "GetFinalPathNameByHandleW", CharSet = CharSet.Unicode,
            SetLastError = true)]
        private static extern int GetFinalPathNameByHandle(IntPtr handle, [In] [Out] StringBuilder path, int bufLen,
            int flags);

        
    }
}