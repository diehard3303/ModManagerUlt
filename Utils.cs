using System;

namespace ModManagerUlt {
    internal static class Utils {
        /// <summary>
        ///     Gets the app path.
        /// </summary>
        /// <value>The application path.</value>
        public static string AppPath => AppDomain.CurrentDomain.BaseDirectory;
    }
}