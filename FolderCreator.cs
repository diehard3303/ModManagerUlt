using System.IO;

namespace ModManagerUlt {
    internal static class FolderCreator {
        /// <summary>
        ///     Creates the public folders.
        /// </summary>
        /// <param name="pth">The PTH.</param>
        public static void CreatePublicFolders(string pth) {
            Directory.CreateDirectory(pth);
        }
    }
}