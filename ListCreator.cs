using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Extensions;

namespace ModManagerUlt {
    internal class ListCreator {
        private string FolderPath { set; get; }

        /// <summary>
        ///     Creates the sorted lists.
        /// </summary>
        public void CreateSortedLists() {
            var gi = new GameInfo();
            var tmp = gi.GetGame();
            if (tmp.IsNullOrEmpty()) return;
            var src = string.Empty;

            var pth = string.Empty;
            var cProf = string.Empty;

            switch (tmp) {
                case "ATS":
                    var atsp = new AtsRegWork(true);
                    pth = atsp.Read(AtsRegKeys.ATS_GROUPS);
                    src = "*.scs";
                    break;

                case "ETS":
                    var etsp = new EtsRegWork(true);
                    pth = etsp.Read(EtsRegKeys.ETS_GROUPS);
                    src = "*.scs";
                    break;

                case "FS15":
                    var fs15 = new Fs15RegWork(true);
                    pth = fs15.Read(Fs15RegKeys.FS15_GROUPS);
                    src = "*.zip";
                    break;

                case "FS17":
                    var fs17 = new Fs17RegWork(true);
                    pth = fs17.Read(Fs17RegKeys.FS17_GROUPS);
                    cProf = fs17.Read(Fs17RegKeys.FS17_PROFILES);
                    src = "*.zip";
                    break;
            }

            var foldList = GetFilesFolders.GetFolders(pth, "*.*");
            var profList = GetFilesFolders.GetFolders(cProf, "*.*");

            foreach (var v in foldList) {
                var lst = GetFilesFolders.GetFiles(v, src);
                var dic = new Dictionary<string, string>();
                foreach (var t in lst) {
                    if (CheckPath(t)) continue;
                    var file = t.GetFileName();
                    var ph = Path.GetDirectoryName(t);
                    if (dic.ContainsKey(file)) continue;
                    dic.Add(file, ph + "\\");
                }

                var nam = v.GetLastFolderName();
                Serializer.SerializeDictionary(v + "\\" + nam + ".xml", dic);
            }

            foreach (var v in profList)
            {
                var lst = GetFilesFolders.GetFiles(v, src);
                var dic = new Dictionary<string, string>();
                foreach (var t in lst)
                {
                    if (CheckPath(t)) continue;
                    var file = t.GetFileName();
                    var ph = Path.GetDirectoryName(t);
                    if (dic.ContainsKey(file)) continue;
                    dic.Add(file, ph + "\\");
                }

                var nam = v.GetLastFolderName();
                Serializer.SerializeDictionary(v + "\\" + nam + ".xml", dic);
            }
        }

        /// <summary>
        ///     Makes the sorted list.
        /// </summary>
        public void SortedFileListComplete() {
            var dic = new Dictionary<string, string>();
            var gi = new GameInfo();
            var tmp = gi.GetGame();
            if (tmp.IsNullOrEmpty()) return;
            IEnumerable<string> lst = null;
            var path = string.Empty;

            switch (tmp) {
                case "ATS":
                    var atsP = new AtsRegWork(true);
                    path = atsP.Read(AtsRegKeys.ATS_XML) + "sortedFileListComplete.xml";
                    FolderPath = atsP.Read(AtsRegKeys.ATS_GROUPS);
                    lst = GetFilesFolders.GetFilesRecursive(FolderPath, "*.scs");
                    break;

                case "ETS":
                    var etsP = new EtsRegWork(true);
                    path = etsP.Read(EtsRegKeys.ETS_XML) + "sortedFileListComplete.xml";
                    FolderPath = etsP.Read(EtsRegKeys.ETS_GROUPS);
                    lst = GetFilesFolders.GetFilesRecursive(FolderPath, "*.scs");
                    break;

                case "FS15":
                    var fs15 = new Fs15RegWork(true);
                    path = fs15.Read(Fs15RegKeys.FS15_XML) + "sortedFileListComplete.xml";
                    FolderPath = fs15.Read(Fs15RegKeys.FS15_GROUPS);
                    lst = GetFilesFolders.GetFilesRecursive(FolderPath, "*.zip");
                    break;

                case "FS17":
                    var fs17 = new Fs17RegWork(true);
                    path = fs17.Read(Fs17RegKeys.FS17_XML) + "sortedFileListComplete.xml";
                    FolderPath = fs17.Read(Fs17RegKeys.FS17_GROUPS);
                    lst = GetFilesFolders.GetFilesRecursive(FolderPath, "*.zip");
                    break;
            }

            if (lst.IsNull()) return;
            Debug.Assert(lst != null, nameof(lst) + " != null");
            foreach (var v in lst) {
                var file = v.GetFileName();
                var pth = Path.GetDirectoryName(v);
                if (dic.ContainsKey(file)) continue;
                dic.Add(file, pth + "\\");
            }

            Serializer.SerializeDictionary(path, dic);
            MsgBx.Msg("List Creation Completed","List Creator");
        }

        private static bool CheckPath(string t) {
            var inFo = new DirectoryInfo(t);
            var fnd = SymLinks.GetSymbolicLinkTarget(inFo);
            return fnd.IsNullOrEmpty();
        }
    }
}