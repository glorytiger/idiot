using System;

namespace SoftwareDesignEksamen
{
    public sealed class ReadInput {

        public int AskForNumber(String msg = "", int min = 1, int max = int.MaxValue) {
            String input;
            int number;

            do {
                if (msg != "") Console.Write(msg);
                input = Console.ReadLine();
                msg = "Must be a number between " + min + " and " + max + ". Please try again.\n";
            } while (!Int32.TryParse(input, out number) || number < min || number > max);

            return number;
        }

        public int AskForNumber(int min = int.MinValue, int max = int.MaxValue, String msg = "") {
            return AskForNumber(msg, min, max);
        }

        public char AskForChar(string msg = "", string legalChars = "abcdefghijklmnopqrstuvwxyz") {
            char input;

            do {
                if (msg != "") PrintMessages.PrintLineToScreen(msg);
                input = Console.ReadLine().ToLower()[0];
                msg = "Invalid input. Please try again.";
            } while (!legalChars.Contains(input));

            return input;
        }

        public int AskNumberOfPlayers() {
            int numOfPlayers = AskForNumber("How many players wants to play? (2-4)\n", 2, 4);
            PrintMessages.ClearScreen();
            return numOfPlayers;
        }

        public int AskNumberOfComputerPlayers(int amountOfPlayers) {
            int numOfPlayers = AskForNumber($"How many players should be controlled by the Computer? (0-{amountOfPlayers})\n", 0, amountOfPlayers);
            PrintMessages.ClearScreen();
            return numOfPlayers;
        }

        public bool AskIfGameShouldEnd(int numActivePlayers, int lastPlayerIndex) {  
            char answer = 'n';
            //Console.WriteLine($"Num of players: {numActivePlayers}");

            if (numActivePlayers > 1) {
                answer = AskForChar("Press c to continue or q to quit", "cq");
            } else {
                PrintMessages.GameOver(lastPlayerIndex);
                Console.ReadKey();
            }

            return answer != 'c';
        }
    }
}
