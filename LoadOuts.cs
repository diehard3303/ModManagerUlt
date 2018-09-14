using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Extensions;

namespace ModManagerUlt {
    /// <summary>
    /// </summary>
    public static class LoadOuts {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CreateSymbolicLink(
            string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool CreateHardLink(
            string lpFileName,
            string lpExistingFileName,
            IntPtr lpSecurityAttributes
        );

        /// <summary>
        ///     Creates the favorite.
        /// </summary>
        /// <param name="mod">The mod.</param>
        public static void CreateFavorite(string mod) {
            var dic = new Dictionary<string, string>();
            var reg = new Fs17RegWork(true);

            var grpPath = reg.Read(Fs17RegKeys.FS17_GROUPS);
            var xFile = grpPath + "\\claas.xml";

            if (xFile.FileExists()) {
                dic = Serializer.DeserializeDictionary(xFile);
                dic.Add(mod, grpPath + "\\" + Vars.FldName + "\\");
            }
            else {
                dic.Add(mod, grpPath + "\\" + Vars.FldName + "\\");
            }

            Serializer.SerializeDictionary(xFile, dic);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mke"></param>
        /// <param name="mod"></param>
        public static void CreateLoadouts(string mke, string mod) {
            var dic = new Dictionary<string, string>();
            var reg = new Fs17RegWork(true);
            var pth = reg.Read(Fs17RegKeys.FS17_GROUPS);
            var pt = string.Empty;

            switch (mke) {
                case "Claas":
                    pt = pth + @"\claas.xml";
                    break;
                case "Krone":
                    pt = pth + @"\krone.xml";
                    break;
                case "NewHolland":
                    pt = pth + @"\newholland.xml";
                    break;
                case "JohnDeere":
                    pt = pth + @"\johndeere.xml";
                    break;
                case "MasseyFerguson":
                    pt = pth + @"\masseyferguson.xml";
                    break;
                case "Fendt":
                    pt = pth + @"\fendt.xml";
                    break;
                case "Case":
                    pt = pth + @"\case.xml";
                    break;
                case "Plows":
                    pt = pth + @"\plows.xml";
                    break;
                case "Cultivators":
                    pt = pth + @"\cultivators.xml";
                    break;
                case "Placeables":
                    pt = pth + @"\placeables.xml";
                    break;
                case "Specials":
                    pt = pth + @"\specials.xml";
                    break;
                case "TractorsOther":
                    pt = pth + @"\tractorsother.xml";
                    break;
                case "Tools":
                    pt = pth + @"\tools.xml";
                    break;
                case "Balers":
                    pt = pth + @"\balers.xml";
                    break;
                case "Trailers":
                    pt = pth + @"\trailers.xml";
                    break;
            }

            if (pt.FileExists()) {
                dic = Serializer.DeserializeDictionary(pt);
            }

            if (dic.ContainsKey(mod)) return;
            dic.Add(mod, pth + @"\" + Vars.FldName + @"\");
            Serializer.SerializeDictionary(pt, dic);
        }

        /// <summary>
        ///     Loads the favorites.
        /// </summary>
        public static Dictionary<string, string> LoadFavorites(string mke) {
            var fs17 = new Fs17Loader();
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var cProf = reg.Read(RegKeys.CURRENT_PROFILE);
            if (gam.IsNullOrEmpty() || cProf.IsNullOrEmpty()) {
                MsgBx.Msg("You need to have a Profile / Game active", "Process Error");
                return null;
            }

            var dic = new Dictionary<string, string>()
                ;
            switch (mke) {
                case "Claas":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\claas.xml");
                    break;
                case "Krone":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\krone.xml");
                    break;
                case "NewHolland":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\newholland.xml");
                    break;
                case "JohnDeere":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\johndeere.xml");
                    break;
                case "MasseyFerguson":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\masseyferguson.xml");
                    break;
                case "Fendt":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\fendt.xml");
                    break;
                case "Case":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\case.xml");
                    break;
                case "Plows":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\plows.xml");
                    break;
                case "Cultivators":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\cultivators.xml");
                    break;
                case "Placeables":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\placeables.xml");
                    break;
                case "Specials":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\specials.xml");
                    break;
                case "TractorsOther":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\tractorsOther.xml");
                    break;
                case "Balers":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\balers.xml");
                    break;
                case "Trailers":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\trailers.xml");
                    break;
                case "Tools":
                    dic = Serializer.DeserializeDictionary(reg.Read(Fs17RegKeys.FS17_GROUPS) + @"\tools.xml");
                    break;
            }


            var lnkPth = reg.Read(Fs17RegKeys.FS17_PROFILES) + cProf + "\\";

            foreach (var v in dic) {
                var tmp = lnkPth + v.Key;
                if (tmp.FileExists()) continue;
                var org = v.Value + v.Key;
                if (!org.FileExists()) continue;
                //CreateSymbolicLink(tmp, org, SymbolicLink.File);
                //SymLinks.CreateSymLink(tmp, org, 0);
                // FileCopyMove.FileCopy(org, tmp);

                CreateHardLink(tmp, org, IntPtr.Zero);
            }

            var lc = new ListCreator();
            lc.CreateSortedLists();

            var lst = fs17.LoadFs17ProfileMods(reg.Read(RegKeys.CURRENT_PROFILE));
            return lst;
        }

        private enum SymbolicLink {
            File = 0,
            Directory = 1
        }
    }
}