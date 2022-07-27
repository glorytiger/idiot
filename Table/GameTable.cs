using System;
using System.Collections.Generic;

namespace SoftwareDesignEksamen {
    class GameTable {

        private readonly PlayedCards _playedCards = new();
        private List<Player> _players = new();
        private readonly DrawTable _drawTable = new();
        private readonly ReadInput _readInput = new();

        private IDeck _deck = null;
        private bool _gameRunning = true;
        private int _numActivePlayers;
        Player _previousPlayer = null;

        public GameTable(int amountOfPlayers, int amountOfComputerPlayers) {
            InitGame(amountOfPlayers, amountOfComputerPlayers);
        }

        private void InitGame(int amountOfPlayers, int amountOfComputerPlayers) {
            _deck = DeckFactory.CreateDeck();
            AddPlayers(amountOfPlayers, amountOfComputerPlayers);
            DealCardsToPlayersDecks();
        }

        public void RunGame() {
            PrintMessages.SwapMsg();
            FirstRoundCardSwap();

            PrintMessages.PrintPlayerStartInformation();
            CreatePlayingOrder(GetStartingPlayerIndex());

            while (_gameRunning) {
                LoopThroughPlayerTurns();
            }
        }

        #region Functions to set up the table
        private void AddPlayers(int amountOfPlayers, int amountOfComputerPlayers) {
            for (int i = 0; i < amountOfPlayers; i++) {
                if (i >= amountOfComputerPlayers)
                    _players.Add(new Human(i));
                else
                    _players.Add(new Computer(i));
            }
            _numActivePlayers = amountOfPlayers;
        }

        // Deals one card at the time to each player for each hand 
        private void DealCardsToPlayersDecks() {
            for (int i = 0; i < 3; i++)  // i = Amount of decks
            {
                for (int j = 0; j < 3; j++)  // j = Amount of cards to be dealt per deck
                {
                    foreach (Player player in _players) {
                        if (i == 0) {
                            player.DealHandFaceDownCard(_deck);
                        } else if (i == 1) {
                            player.DealHandFaceUpCard(_deck);
                        } else {
                            player.DealHandCards(_deck);
                        }
                    }
                }
            }
        }

        private int GetStartingPlayerIndex() {
            int _lowestCardPlayerIndex = 0; // 0 is the first player
            Card _lowestCardPlayer = new(Suit.Spades, Rank.Ace);  // Ace of Spades is highest possible value

            foreach (Player player in _players) {
                List<Card> _playerDeck = player.UpdateAndGetPlayableDeck();

                for (int i = 0; i < _playerDeck.Count; i++) {
                    if (!_playerDeck[i].CompareTotalValueCard(_lowestCardPlayer)) {
                        _lowestCardPlayer = _playerDeck[i];
                        _lowestCardPlayerIndex = player.PlayerId;

                    }
                }
            }

            return _lowestCardPlayerIndex;
        }

        // Creating a new player order if the player with lowest card does not have playerId = 0 
        private void CreatePlayingOrder(int startingPlayerIndex) {
            List<Player> tmpPlayerList = new();

            if (startingPlayerIndex != 0) {
                for (int i = 0; i < _players.Count; i++) {
                    tmpPlayerList.Add(_players[startingPlayerIndex]);
                    startingPlayerIndex++;

                    if (startingPlayerIndex == _players.Count) {
                        startingPlayerIndex = 0;
                    }
                }
                _players = tmpPlayerList;
            }
        }
        #endregion

        #region Swap functions
        private void FirstRoundCardSwap() {
            bool continueSwapping;
            foreach (Player player in _players) {
                do {
                    _drawTable.Draw(_players, _playedCards.CurrentlyPlayedCardsAtTable);
                    if (player.GetType().IsAssignableFrom(typeof(Human))) {
                        PrintMessages.PrintSwapStateForEachPlayer(player);
                    }
                    continueSwapping = ExecuteSwap(player);

                    PrintMessages.ClearScreen();

                } while (continueSwapping);
            }
        }

        private static bool ExecuteSwap(Player player) {
            int userInputHandIndex = player.GetSwapHandIndex();

            if (userInputHandIndex == 3) {
                PrintMessages.ClearScreen();
                return false;
            }

            if (player.GetType().IsAssignableFrom(typeof(Human))) {
                PrintMessages.PrintSwapQuestion();
            } else {
                PrintMessages.PrintComputerStatus(player.PlayerId);
            }

            int userInputTableIndex = player.GetSwapTableIndex();

            if (userInputTableIndex == 3) {
                PrintMessages.ClearScreen();
                return false;
            }

            player.SwapCardsBetweenHandAndFaceUp(
                player.hf.HandCards.Cards,
                player.hf.FaceUpCards.Cards,
                userInputHandIndex,
                userInputTableIndex);

            return true;
        }
        #endregion

        private void LoopThroughPlayerTurns() {
            
            foreach (Player player in _players) {
                if (!player.IsFinished) {
                    PrintMessages.ClearScreen();
                    
                    _drawTable.Draw(_players, _playedCards.CurrentlyPlayedCardsAtTable);
                    PrintMessages.Instance.PrintPlayedCards(_playedCards, _previousPlayer);

                    player.PlayOneRound(_deck, _playedCards, _players, _drawTable);

                    _playedCards.ClearLastPlayedCardsNextAdd = true;
                   
                    if (player.IsFinished) {
                        HandleSubscribers(player);

                        PrintMessages.PrintPlayerFinished(player.PlayerId + 1);
                        if (_readInput.AskIfGameShouldEnd(_numActivePlayers, GetLosingPlayerIndex()+1)) {
                            _gameRunning = false;
                            break;
                        }
                    }
                    PrintMessages.ClearScreen();
                }
                _previousPlayer = player;
            }
        }

        private int GetLosingPlayerIndex() {
            foreach (Player p in _players) {
                if (!p.IsFinished) { 
                    return p.PlayerId;
                }
            }
            return -1;
        }

        private void HandleSubscribers(Player player) {
            player.TriggerPlayerEvent += p_FinishedGame;
            player.OnPlayerOutOfCards();
        }
        private void p_FinishedGame(object sender, EventArgs eventArgs) {
            _numActivePlayers--;
        }
    }
}
