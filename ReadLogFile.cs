using System.Diagnostics;
using System.Windows.Forms;
using Extensions;

namespace ModManagerUlt {
    internal class ReadLogFile {
        /// <summary>
        ///     Reads the log files.
        /// </summary>
        public void ReadLogFiles() {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var ofd = new OpenFileDialog();
            var pth = string.Empty;

            switch (gam) {
                case "ATS":
                    pth = reg.Read(AtsRegKeys.ATS_LOG_FILE);
                    if (pth.IsNullOrEmpty()) {
                        ofd.Title =
                            @"Navigate to the ATS Log File, usually in my documents under American Truck Simulator";
                        ofd.ShowDialog();
                        if (!ofd.FileName.FileExists()) return;
                        reg.Write(AtsRegKeys.ATS_LOG_FILE, ofd.FileName);
                        pth = ofd.FileName;
                    }

                    break;

                case "ETS":
                    pth = reg.Read(EtsRegKeys.ETS_LOG_FILE);
                    if (pth.IsNullOrEmpty()) {
                        ofd.Title =
                            @"Navigate to the ETS Log File, usually in my documents under Euro Truck Simulator 2";
                        ofd.ShowDialog();
                        if (!ofd.FileName.FileExists()) return;
                        reg.Write(EtsRegKeys.ETS_LOG_FILE, ofd.FileName);
                        pth = ofd.FileName;
                    }

                    break;

                case "FS15":
                    pth = reg.Read(Fs15RegKeys.FS15_LOG_FILE);
                    if (pth.IsNullOrEmpty()) {
                        ofd.Title =
                            @"Navigate to the FS15 Log File, usually in my documents/My Games under Farming Simulator 2015";
                        ofd.ShowDialog();
                        if (!ofd.FileName.FileExists()) return;
                        reg.Write(Fs15RegKeys.FS15_LOG_FILE, ofd.FileName);
                        pth = ofd.FileName;
                    }

                    break;

                case "FS17":
                    pth = reg.Read(Fs17RegKeys.FS17_LOG_FILE);
                    if (pth.IsNullOrEmpty()) {
                        ofd.Title =
                            @"Navigate to the FS17 Log File, usually in my documents/My Games under Farming Simulator 2017";
                        ofd.ShowDialog();
                        if (!ofd.FileName.FileExists()) return;
                        reg.Write(Fs17RegKeys.FS17_LOG_FILE, ofd.FileName);
                        pth = ofd.FileName;
                    }

                    break;
            }

            if (!pth.FileExists()) return;
            Process.Start(pth);
        }
    }
}