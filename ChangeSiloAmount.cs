using System.Linq;
using System.Xml.Linq;
using Extensions;

namespace ModManagerUlt {
    internal class ChangeSiloAmount {
        private const string AMOUNT = "10000000.000000";

        /// <summary>
        ///     Changes the silo total amount.
        /// </summary>
        public void ChangeSiloTotalAmount(string saveGame) {
            if (saveGame.IsNullOrEmpty()) {
                MsgBx.Msg("You did not choose a SaveGame to Modify", "Profile Error");
                return;
            }

            var pth = saveGame + "careerSavegame.xml";
            if (!pth.FileExists()) return;

            var doc = XDocument.Load(pth);
            var q = from node in doc.Descendants()
                where node.Name == "totalAmount"
                where node.Attributes().Any()
                select new {NodeName = node.Name, Attributes = node.Attributes()};

            foreach (var node in q)
            foreach (var attribute in node.Attributes)
                if (attribute.Name == "value")
                    attribute.SetValue(AMOUNT);
            doc.Save(pth);

            MsgBx.Msg("Cheat - Change Silo Capacity - has been applied to " + saveGame.GetLastFolderName(), "Cheat");
        }
    }
}