using System.Collections.Generic;
using Extensions;

namespace ModManagerUlt {
    class SortingBypass {
        public void BypassSort(string fldName) {
            var fs = new Fs17RegWork(false);
            var chkName = fldName.GetLastFolderName();
            const string BYPASS = "BypassSorting.xml";
            var pth = fs.Read(Fs17RegKeys.FS17_GROUPS) + "\\" + BYPASS;

            if (pth.FileExists()) {
                var dic = Serializer.DeserializeDictionary(pth);
                if (dic.ContainsKey(chkName)) {
                    MsgBx.Msg("That Mod has already been added to the bypass list.", "Error");
                    return;
                }
                dic.Add(fldName, fs.Read(Fs17RegKeys.FS17_GROUPS) + "\\");
                Serializer.SerializeDictionary(pth, dic);
            }
            else {
                var sb = new Dictionary<string, string> {{fldName, fs.Read(Fs17RegKeys.FS17_GROUPS) + "\\"}};
                Serializer.SerializeDictionary(pth, sb);
            }
        }

        public Dictionary<string, string> GetBypassList() {
            var fs = new Fs17RegWork(false);
            const string BYPASS = "BypassSorting.xml";
            var pth = fs.Read(Fs17RegKeys.FS17_GROUPS) + "\\" + BYPASS;

            return Serializer.DeserializeDictionary(pth);
        }
    }
}