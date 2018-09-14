using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Extensions;

namespace ModManagerUlt {
    internal class FsCheatMoney {
        private const string MONEY = "5000000";
        private const string WITHER = "false";

        public string GetMapName(string saveGame) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);
            var tmp = string.Empty;

            switch (gam) {
                case "FS15":

                    break;

                case "FS17":
                    var xdoc = new XmlDocument();
                    if (!saveGame.FolderExists()) return string.Empty;
                    xdoc.Load(saveGame + "\\careerSavegame.xml");
                    var nodes = xdoc.SelectNodes("careerSavegame/settings/savegameName");
                    if (nodes == null) return string.Empty;
                    foreach (XmlNode node in nodes) {
                        if (node.Name != "savegameName") continue;
                        tmp = node.InnerText;
                        break;
                    }

                    break;
            }

            return tmp;
        }

        /// <summary>
        ///     Sets the cheat values.
        /// </summary>
        public void SetCheatValues(string saveGame) {
            var reg = new AtsRegWork(true);
            var gam = reg.Read(RegKeys.CURRENT_GAME);

            switch (gam) {
                case "FS15":
                    var pth = saveGame;
                    if (!pth.FileExists()) return;
                    var xmlFile = XDocument.Load(pth + "\\careerSavegame.xml");
                    var query = from c in xmlFile.Elements("careerSavegame")
                        select c;

                    foreach (var element in query) {
                        var xAttribute = element.Attribute("money");
                        if (xAttribute != null) xAttribute.Value = MONEY;
                        var attribute = element.Attribute("isPlantWitheringEnabled");
                        if (attribute != null)
                            attribute.Value = WITHER;
                    }

                    xmlFile.Save(pth + "\\careerSavegame.xml");

                    MsgBx.Msg("Money / Wither Cheat Applied to " + saveGame.GetLastFolderName(), "Cheat");
                    break;

                case "FS17":
                    var xdoc = new XmlDocument();
                    xdoc.Load(saveGame + "\\careerSavegame.xml");

                    var nodes = xdoc.SelectNodes("careerSavegame/statistics/money");
                    if (nodes == null) return;
                    foreach (XmlNode node in nodes) {
                        if (node.Name != "money") continue;
                        var amt = node.InnerText;
                        var cash = Convert.ToInt32(amt);
                        var done = cash + Convert.ToInt32(MONEY);
                        node.InnerText = done.ToString();
                        break;
                    }

                    xdoc.Save(saveGame + "\\careerSavegame.xml");
                    MsgBx.Msg("Money Cheat Applied to " + saveGame.GetLastFolderName(), "Money Cheat");
                    break;
            }
        }
    }
}