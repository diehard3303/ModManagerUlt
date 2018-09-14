using System.Diagnostics;

namespace ModManagerUlt {
    internal static class Browser {
        /// <summary>
        ///     Browses the folders.
        /// </summary>
        /// <param name="fold">The fold.</param>
        public static void BrowseFolders(string fold) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            const string modList = "sortedFileListComplete.xml";
            var pth = string.Empty;

            switch (fold) {
                case "repo":
                    switch (gam) {
                        case "ATS":
                            pth = reg.Read(AtsRegKeys.ATS_REPO);
                            break;

                        case "ETS":
                            pth = reg.Read(EtsRegKeys.ETS_REPO);
                            break;

                        case "FS15":
                            pth = reg.Read(Fs15RegKeys.FS15_REPO);
                            break;

                        case "FS17":
                            pth = reg.Read(Fs17RegKeys.FS17_REPO);
                            break;
                    }

                    break;

                case "profiles":
                    switch (gam) {
                        case "ATS":
                            pth = reg.Read(AtsRegKeys.ATS_PROFILES);
                            break;

                        case "ETS":
                            pth = reg.Read(EtsRegKeys.ETS_PROFILES);
                            break;

                        case "FS15":
                            pth = reg.Read(Fs15RegKeys.FS15_PROFILES);
                            break;

                        case "FS17":
                            pth = reg.Read(Fs17RegKeys.FS17_PROFILES);
                            break;
                    }

                    break;

                case "groups":
                    switch (gam) {
                        case "ATS":
                            pth = reg.Read(AtsRegKeys.ATS_GROUPS);
                            break;

                        case "ETS":
                            pth = reg.Read(AtsRegKeys.ATS_GROUPS);
                            break;

                        case "FS15":
                            pth = reg.Read(Fs15RegKeys.FS15_GROUPS);
                            break;

                        case "FS17":
                            pth = reg.Read(Fs17RegKeys.FS17_GROUPS);
                            break;
                    }

                    break;

                case "work":
                    switch (gam) {
                        case "ATS":
                            pth = reg.Read(AtsRegKeys.ATS_WORK);
                            break;

                        case "ETS":
                            pth = reg.Read(EtsRegKeys.ETS_WORK);
                            break;

                        case "FS15":
                            pth = reg.Read(Fs15RegKeys.FS15_WORK);
                            break;

                        case "FS17":
                            pth = reg.Read(Fs17RegKeys.FS17_WORK);
                            break;
                    }

                    break;

                case "list":
                    switch (gam) {
                        case "ATS":
                            pth = reg.Read(AtsRegKeys.ATS_XML) + modList;
                            break;

                        case "ETS":
                            pth = reg.Read(EtsRegKeys.ETS_XML) + modList;
                            break;

                        case "FS15":
                            pth = reg.Read(Fs15RegKeys.FS15_XML) + modList;
                            break;

                        case "FS17":
                            pth = reg.Read(Fs17RegKeys.FS17_XML) + modList;
                            break;
                    }

                    break;

                case "fav":
                    switch (gam) {
                        case "FS17":
                            pth = reg.Read(Fs17RegKeys.FS17_GROUPS) + "BypassSorting.xml";
                            break;
                    }

                    break;
            }

            Process.Start(pth);
        }
    }
}