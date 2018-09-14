using System;
using System.IO;

namespace ModManagerUlt {
    internal static class FileCopy {
        /// <summary>
        ///     Sets the destination.
        /// </summary>
        /// <value>
        ///     The destination.
        /// </value>
        public static string Destination { set; private get; }

        /// <summary>
        ///     Sets the source.
        /// </summary>
        /// <value>
        ///     The source.
        /// </value>
        public static string Source { set; private get; }

        /// <summary>
        ///     Starts the copy.
        /// </summary>
        public static void DoCopy() {
            CopyMod();
        }

        private static void CopyMod() {
            var arrayLength = (int) Math.Pow(2, 19);
            var dataArray = new byte[arrayLength];
            using (var fsread = new FileStream
                (Source, FileMode.Open, FileAccess.Read, FileShare.None, arrayLength)) {
                using (var bwread = new BinaryReader(fsread)) {
                    using (var fswrite = new FileStream
                        (Destination, FileMode.Create, FileAccess.Write, FileShare.None, arrayLength)) {
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
        }
    }
}