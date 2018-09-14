// *********************************************************************** Assembly : Mod Maker 2.0
// Author : TJ Created : 08-17-2015
//
// Last Modified By : TJ Last Modified On : 10-18-2016 ***********************************************************************
// <copyright file="GetFilesFolders.cs" company="">
//     Copyright © 2015 DieHard Development
// </copyright>
// <summary>
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ModManagerUlt {
    internal static class GetFilesFolders {
        /// <summary>
        ///     Gets the files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns>
        ///     List&lt;System.String&gt;.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     path
        ///     or
        ///     searchPattern
        /// </exception>
        public static IEnumerable<string> GetFiles(string path, string searchPattern) {
            if (string.Equals(path, null, StringComparison.Ordinal)) throw new ArgumentNullException(nameof(path));
            if (string.Equals(searchPattern, null, StringComparison.Ordinal))
                throw new ArgumentNullException(nameof(searchPattern));
            var lst = Directory.EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly).ToList();

            return lst;
        }

        /// <summary>
        ///     Gets the files recursive.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns>
        ///     List&lt;System.String&gt;.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     path
        ///     or
        ///     searchPattern
        /// </exception>
        public static IEnumerable<string> GetFilesRecursive(string path, string searchPattern) {
            if (string.Equals(path, null, StringComparison.Ordinal)) throw new ArgumentNullException(nameof(path));
            if (string.Equals(searchPattern, null, StringComparison.Ordinal))
                throw new ArgumentNullException(nameof(searchPattern));
            var lst = Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories).ToList();

            return lst;
        }

        /// <summary>
        ///     Gets the folders.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns>
        ///     List&lt;System.String&gt;.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     path
        ///     or
        ///     searchPattern
        /// </exception>
        public static IEnumerable<string> GetFolders(string path, string searchPattern) {
            if (string.Equals(path, null, StringComparison.Ordinal)) throw new ArgumentNullException(nameof(path));
            if (string.Equals(searchPattern, null, StringComparison.Ordinal))
                throw new ArgumentNullException(nameof(searchPattern));
            var lst = Directory.EnumerateDirectories(path, searchPattern, SearchOption.TopDirectoryOnly).ToList();

            return lst;
        }

        /// <summary>
        ///     Gets the folders recursive.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///     path
        ///     or
        ///     searchPattern
        /// </exception>
        public static IEnumerable<string> GetFoldersRecursive(string path, string searchPattern) {
            if (string.Equals(path, null, StringComparison.Ordinal)) throw new ArgumentNullException(nameof(path));
            if (string.Equals(searchPattern, null, StringComparison.Ordinal))
                throw new ArgumentNullException(nameof(searchPattern));
            var lst = Directory.EnumerateDirectories(path, searchPattern, SearchOption.AllDirectories).ToList();

            return lst;
        }
    }
}