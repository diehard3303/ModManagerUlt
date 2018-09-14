namespace ModManagerUlt {
    internal class GameInfo {
        /// <summary>
        ///     Gets the game.
        /// </summary>
        /// <returns></returns>
        public string GetGame() {
            var ats = new AtsRegWork(true);

            return ats.Read(RegKeys.CURRENT_GAME);
        }

        /// <summary>
        ///     Sets the game.
        /// </summary>
        /// <param name="game">The game.</param>
        public void SetGame(string game) {
            var ats = new AtsRegWork(true);

            ats.Write(RegKeys.CURRENT_GAME, game);
        }
    }
}