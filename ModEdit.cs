using System;
using System.Collections.Generic;
using System.Linq;

namespace ModManagerUlt {
    internal class ModEdit {
        /// <summary>
        ///     Edits the mod.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <returns></returns>
        public string EditMod(string mod) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            Dictionary<string, string> dic;
            string xmlPath;
            var pth = string.Empty;
            string modPath;

            switch (gam) {
                case "ATS":
                    xmlPath = reg.Read(AtsRegKeys.ATS_XML) + "sortedFileListComplete.xml";
                    dic = Serializer.DeserializeDictionary(xmlPath);
                    pth = GetPath(mod, dic, pth);
                    modPath = pth + mod;
                    FsZip.ExtractModScs(modPath);
                    break;

                case "ETS":
                    xmlPath = reg.Read(EtsRegKeys.ETS_XML) + "sortedFileListComplete.xml";
                    dic = Serializer.DeserializeDictionary(xmlPath);
                    pth = GetPath(mod, dic, pth);
                    modPath = pth + mod;
                    FsZip.ExtractModScs(modPath);
                    break;

                case "FS15":
                case "FS17":
                    FsZip.ExtractArchive(mod);
                    break;
            }

            return pth + mod;
        }

        private static string GetPath(string mod, IReadOnlyDictionary<string, string> dic, string pth) {
            if (dic != null && dic.Any(v => string.Equals(v.Key, mod, StringComparison.OrdinalIgnoreCase)))
                dic.TryGetValue(mod, out pth);
            return pth;
        }
    }
}