using System;
using System.Collections.Generic;

namespace SoftwareDesignEksamen {

    public sealed class PrintMessages {

        private static volatile PrintMessages _instance;
        private static readonly object _syncRoot = new();

        private PrintMessages() {}

        public static PrintMessages Instance {
            get {
                if (_instance == null) {
                    lock (_syncRoot) {
                        if (_instance == null) {
                            _instance = new PrintMessages();
                        }
                    }
                }

                return _instance;
            }
        }

        public static void ClearScreen() {
            Console.Clear();
        }

        public static void PrintLineToScreen(String msg) {
            Console.WriteLine(msg);
        }

        public static void PrintToScreen(String msg) {
            Console.Write(msg);
        }

        public static void SwapMsg() {
            PrintLineToScreen("The first round you are allowed to change card from hand to table. Choose card(s) to replace.\n");
        }

        public static void GameOver(int playerId) {
            PrintLineToScreen($"The game is over! Player {playerId} is the idiot.");
        }

        public static void PrintSwapStateForEachPlayer(Player player) {
            int indexCard = 1;

            PrintLineToScreen($"Player {player.PlayerId + 1}'s turn to swap.\n");
            PrintLineToScreen("Currently Hands Cards:");

            foreach (Card card in player.hf.HandCards.Cards) {
                PrintLineToScreen($"{indexCard++}. {card}");
            }

            PrintLineToScreen("\nCurrently Table Face up Cards:");
            indexCard = 1;
            foreach (Card card in player.hf.FaceUpCards.Cards) {
                PrintLineToScreen($"{indexCard++}. {card}");
            }

            PrintLineToScreen("\n4. Skip or finish swap");
        }

        public static void PrintSwapQuestion() {
            PrintLineToScreen("Which card from table do you want to swap? ");
        }

        public static void PrintPlayerStartInformation() {
            PrintLineToScreen("Player with the lowest value starts.");
        }

        public static void PrintNoPlayedCards() {
            PrintLineToScreen("There are no currently played cards, you can play whatever you want.");
        }

        public static void PrintComputerStatus(int playerId) {
            PrintLineToScreen($"Player {playerId + 1} is playing, wait for your turn...");
        }

        public static void PrintPlayerFinished(int playerId) {
            PrintLineToScreen($"Player {playerId} has finished the game!");
        }

        public void PrintChanceOption(int id) {
            PrintLineToScreen($"{id}. Take a chance");
        }

        public void PrintDrawPlayedOption(int id, int numCards) {
            PrintLineToScreen($"{id}. Draw all played cards at table ({numCards})");
        }

        public void PrintHandCardOptions(List<Card> hand, IHand faceDownCards) {
            bool showCardInfo = hand != faceDownCards.Cards;

            PrintLineToScreen("");
            for (int i = 0; i < hand.Count; i++) {
                string newStr = (hand[i].ToggleWasRecentlyDrawn()) ? " (new)" : "";
                string cardStr = (showCardInfo) ? hand[i] + newStr : "?";

                PrintLineToScreen($"{i + 1}. {cardStr}");
            }
        }

        public static void PrintThrownDeck() {
            PrintLineToScreen("You have thrown the deck, it's your turn again!");
        }

        public static void PrintFourEquals() {
            PrintLineToScreen("Four equal cards were played, it's your turn again");
        }

        public static void PrintSelectCard() {
            PrintLineToScreen("What card do you want to play? Type the number of the card");
        }

        public static void PrintInvalidCard() {
            PrintLineToScreen("You chose an invalid card, please try another option");
        }

        public void PrintPlayedCards(PlayedCards playedCards, Player previousPlayer) {
            string pluralChar;
            List<Card> cards = new();

            if (previousPlayer != null && playedCards.UnplacedCard != null) {
                PrintToScreen($"Player {previousPlayer.PlayerId+1} drew ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                PrintToScreen($"{playedCards.UnplacedCard}");
                Console.ResetColor();
                PrintLineToScreen($" and had to take the deck of played cards.");
                return;
            } else if (!playedCards.AreTherePlayedCardsAtTable()) {
                PrintNoPlayedCards();
                return;
            }

            foreach (Card card in playedCards.CurrentlyPlayedCardsAtTable) {
                if (card.WasPlayedLastTurn) {
                    cards.Add(card);
                }
            }

            pluralChar = (cards.Count > 1) ? "s" : "";

            PrintToScreen($"The last played card{pluralChar} was ");

            for (int i = 0; i < cards.Count; i++) {
                if (i > 0) PrintToScreen(", ");

                Console.ForegroundColor = ConsoleColor.Yellow;
                PrintToScreen(cards[i].ToString());
                Console.ResetColor();
            }
            PrintLineToScreen("");
        }

        public void PrintPlayerAndHandName(List<Card> playableDeck, IHand handCards, IHand faceUpCards, IHand faceDownCards, int playerId) {
            string deckName = "none";

            PrintToScreen($"Player {playerId + 1}'s turn, drawing from ");

            if (playableDeck == handCards.Cards) {
                deckName = "Hand cards";
                Console.ForegroundColor = Config.Instance.HandCardsForegroundColor;
                Console.BackgroundColor = Config.Instance.HandCardsBackgroundColor;
            } else if (playableDeck == faceUpCards.Cards) {
                deckName = "Face-up cards";
                Console.ForegroundColor = Config.Instance.FaceupCardsForegroundColor;
                Console.BackgroundColor = Config.Instance.FaceupCardsBackgroundColor;
            } else if (playableDeck == faceDownCards.Cards) {
                deckName = "Face-down cards";
                Console.ForegroundColor = Config.Instance.FacedownCardsForegroundColor;
                Console.BackgroundColor = Config.Instance.FacedownCardsBackgroundColor;
            }

            PrintToScreen(deckName);
            Console.ResetColor();
            PrintLineToScreen("\n");
        }

        public void PrintPlayDuplicateCardQuestion() {
            PrintLineToScreen("You have a extra card with same value, do you want to play it? y/n");
        }

        public static void PrintPlayStatusMessage(bool printYourTurnAgainText, bool printFourEqualsText, bool lastCardValidation, int computerPlayerId) {
	        if (printYourTurnAgainText) {
		        PrintThrownDeck();
	        } else if (printFourEqualsText) {
		        PrintFourEquals();
	        } else if (lastCardValidation) {
                if (computerPlayerId > -1)
                    PrintComputerStatus(computerPlayerId);
                else
                    PrintSelectCard();
	        } else {
		        PrintInvalidCard();
	        }
        }
    }
}
