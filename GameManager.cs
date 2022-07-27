namespace SoftwareDesignEksamen {
    class GameManager {
        public static void LaunchGame() {

            Config.Instance.Init();

            ReadInput readInput = new();
            
            int amountOfPlayers = readInput.AskNumberOfPlayers();
            int amountOfComputerPlayers = readInput.AskNumberOfComputerPlayers(amountOfPlayers);

            GameTable game = new(amountOfPlayers, amountOfComputerPlayers);
            game.RunGame();
        }
    }
}
