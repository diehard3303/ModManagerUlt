using System;
using System.Security.Principal;

namespace ModManagerUlt {
    internal static class FsUtils {
        /// <summary>
        ///     Gets the app path.
        /// </summary>
        /// <value>The application path.</value>
        public static string AppPath => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        ///     Users the name.
        /// </summary>
        /// <returns>username</returns>
        public static string UserName() {
            var tmp = GetUserName();
            return tmp;
        }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <returns></returns>
        private static string GetUserName() {
            var windowsIdentity = WindowsIdentity.GetCurrent();
            var userName = windowsIdentity.Name.Split(Convert.ToChar("\\"))[1];
            return userName;
        }
    }
}