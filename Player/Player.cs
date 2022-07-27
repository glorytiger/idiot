using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareDesignEksamen {

    public class Player {

        public HandFacade hf = new();
        public int PlayerId { get; private set; }
        public bool IsFinished { get; private set; } = false;

        public event EventHandler TriggerPlayerEvent;

        private int _cardsPlayed;
        private int _drawPlayedOption;
        private int _takeAChanceOption;

        private List<Card> _playableDeck;

        public Player(int playerId) {
            PlayerId = playerId;
        }

        public void PlayOneRound(IDeck deck, PlayedCards playedCards, List<Player> players, DrawTable drawTable) {
            ChooseCard(deck, playedCards, players, drawTable); 
            if (_playableDeck.Count < 3) {
	            DrawCard(deck);
            }
        }

        #region Card functions
        private void DrawCard(IDeck deck) {
            for(int i = 0; i < _cardsPlayed && !deck.IsDeckEmpty(); i++) {
                Card card = deck.DrawCardFromDeck();
                card.SetAsRecentlyDrawn();
                hf.AddToHandCards(card);
            }
        }

        private void ChooseCard(IDeck deck, PlayedCards playedCards, List<Player> players, DrawTable drawTable) {
            int userAnswer;
            bool endTurn = false;
            bool lastCardValidation = true;
            bool printYourTurnAgainText = false;
            bool printFourEqualsText = false;

            while (!endTurn) {
                UpdatePlayableDeck();
                
                PrintMessages.Instance.PrintPlayerAndHandName(_playableDeck, hf.HandCards, hf.FaceUpCards, hf.FaceDownCards, PlayerId);
                bool isHuman = this.GetType().IsAssignableFrom(typeof(Human));
                if (this.GetType().IsAssignableFrom(typeof(Human)) || Config.Instance.ShowComputerOptions) {
                    int computerPlayerId = isHuman ? -1 : PlayerId;
                    PrintMessages.PrintPlayStatusMessage(printYourTurnAgainText, printFourEqualsText, lastCardValidation, computerPlayerId);
                    PrintMessages.Instance.PrintHandCardOptions(_playableDeck, hf.FaceDownCards);
                } else {
                    PrintMessages.PrintComputerStatus(PlayerId);
                }

                ShowTacticOptions(deck, playedCards);

                int numOfOptions = CheckDeckAndPlayedDeckStatus(_playableDeck, playedCards, deck);
                userAnswer = GetPlayOption(numOfOptions);

                bool chooseTacticOption = ExecuteTacticOption(deck, playedCards, userAnswer);
                endTurn = chooseTacticOption;

                if (!chooseTacticOption) {
                    endTurn = PlayChosenCard(userAnswer, deck, playedCards, ref lastCardValidation, ref printYourTurnAgainText);
                }

                if (playedCards.CheckForFourEquals()) {
                    printFourEqualsText = true;
                    endTurn = false;
                }

                if (!endTurn) {
                    PrintMessages.ClearScreen();
                    drawTable.Draw(players, playedCards.CurrentlyPlayedCardsAtTable);
                    PrintMessages.Instance.PrintPlayedCards(playedCards, null);
                }

                if (CheckIfPlayerHasMoreCardsToPlay()) {
                    endTurn = true;
                    IsFinished = true;
                }
            }
        }

        private bool CheckIfPlayerHasMoreCardsToPlay() {
            return (hf.GetFaceDownCardSize() == 0) && (hf.GetHandCardSize() == 0);
        }

		private static int CheckDeckAndPlayedDeckStatus(List<Card> playableDeck, PlayedCards playedCards, IDeck deck) {
			var numOfOptions = playableDeck.Count;

			if (playedCards.AreTherePlayedCardsAtTable()) numOfOptions++;
			if (!deck.IsDeckEmpty()) numOfOptions++;

            return numOfOptions;
		}

		private bool PlayChosenCard(int userAnswer, IDeck deck, PlayedCards playedCards, ref bool cardValidationState, ref bool printYourTurnAgainText)
        {
            Card chosenCard = _playableDeck[userAnswer - 1];
            cardValidationState = playedCards.ValidateChosenCard(chosenCard);

            if (cardValidationState) {
                _cardsPlayed = 1;

                playedCards.Add(chosenCard);
                _playableDeck.RemoveAt(userAnswer - 1);

                if (_playableDeck != hf.FaceDownCards.Cards) {
                    HandleIfPlayerHasMultipleEqualRank(chosenCard, playedCards);
                }
                
                if (chosenCard.CardAbility == CardAbility.Throw) {
                    printYourTurnAgainText = true;
                        
                    for (int i = 0; i < _cardsPlayed; i++) {
                        if (_playableDeck == hf.HandCards && _playableDeck.Count < 3) {
                            _playableDeck.Add(deck.DrawCardFromDeck());
                        }
                    }
                    return false;
                }

                return true;
            } else {

                // If a Face-down card couldn't be placed, it should be added to the player's Hand cards along with all played cards.
                if (_playableDeck == hf.FaceDownCards.Cards) {     
                    DrawPlayedCards(playedCards);
                    hf.FaceDownCards.Cards.Remove(chosenCard);
                    _playableDeck.Add(chosenCard);
                    chosenCard.SetAsRecentlyDrawn();
                    playedCards.UnplacedCard = chosenCard;
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region Deal functions
        public void DealHandFaceDownCard(IDeck deck) {
            hf.DealFaceDownCards(deck);
        }

        public void DealHandFaceUpCard(IDeck deck) {
            hf.DealFaceUpCards(deck);
        }

        public void DealHandCards(IDeck deck) {
            hf.DealHandCards(deck);
        }
        #endregion

        #region Deck functions
        private void UpdatePlayableDeck() {
            if (hf.GetHandCardSize() > 0) {
                _playableDeck = hf.HandCards.Cards;
            } else if (hf.GetFaceUpCardSize() > 0) {
                _playableDeck = hf.FaceUpCards.Cards;
            } else if (hf.GetFaceDownCardSize() > 0) {
                _playableDeck = hf.FaceDownCards.Cards;
            }
            hf.SortHandCards();
        }
        
        public List<Card> UpdateAndGetPlayableDeck() {
            UpdatePlayableDeck();
            return _playableDeck;
        }

        #endregion

        #region Tactic functions
        
        private void ShowTacticOptions(IDeck deck, PlayedCards playedCards) {
            _drawPlayedOption = 0;
            _takeAChanceOption = 0;

            if (playedCards.CurrentlyPlayedCardsAtTable.Count > 0) {
                _drawPlayedOption = _playableDeck.Count + 1;
                if (this.GetType().IsAssignableFrom(typeof(Human)) || Config.Instance.ShowComputerOptions)
                    PrintMessages.Instance.PrintDrawPlayedOption(_drawPlayedOption, playedCards.CurrentlyPlayedCardsAtTable.Count);
            }

            if (!deck.IsDeckEmpty()) {
                if (playedCards.CurrentlyPlayedCardsAtTable.Count > 0) {
                    _takeAChanceOption = _playableDeck.Count + 2;
                } else {
                    _takeAChanceOption = _playableDeck.Count + 1;
                }
                if (this.GetType().IsAssignableFrom(typeof(Human)) || Config.Instance.ShowComputerOptions)
                    PrintMessages.Instance.PrintChanceOption(_takeAChanceOption);
            }
        }

        private bool ExecuteTacticOption(IDeck deck, PlayedCards playedCards, int userAnswer) {
            if (userAnswer == _drawPlayedOption) {
                DrawPlayedCards(playedCards);
                return true;
            } else if (userAnswer == _takeAChanceOption) {
                TakeAChanceCard(deck, playedCards);
                return true;
            }
             
            return false;
        }

        private void TakeAChanceCard(IDeck deck, PlayedCards playedCards) {
            Card chanceCard = deck.DrawCardFromDeck();

            if (playedCards.ValidateChosenCard(chanceCard)) {
                playedCards.Add(chanceCard);
            } else {
                _playableDeck.Add(chanceCard);
     
                DrawPlayedCards(playedCards);
                playedCards.UnplacedCard = chanceCard;
            }
        }

   

        private void DrawPlayedCards(PlayedCards playedCards) {
            if (_playableDeck != hf.HandCards.Cards) {
                _playableDeck = hf.HandCards.Cards;
            }
            foreach (Card card in playedCards.CurrentlyPlayedCardsAtTable) {
                _playableDeck.Add(card);
                card.SetAsRecentlyDrawn();
            }
            playedCards.CurrentlyPlayedCardsAtTable.Clear();
        }
        #endregion

        #region Swap functions
        public void SwapCardsBetweenHandAndFaceUp(List<Card> handCards, List<Card> faceUpTableCards, int indexHandCard, int indexTableCard) {
	        Card tmpCard = handCards[indexHandCard];
            tmpCard.DeckSlotId = faceUpTableCards[indexTableCard].DeckSlotId;

	        handCards[indexHandCard] = faceUpTableCards[indexTableCard];
            handCards[indexHandCard].DeckSlotId = faceUpTableCards[indexTableCard].DeckSlotId;

            faceUpTableCards[indexTableCard] = tmpCard;
        }

        public virtual int GetSwapHandIndex() {
            return 0;
        }

        public virtual int GetSwapTableIndex() {
            return 0;
        }
        #endregion

        private void HandleIfPlayerHasMultipleEqualRank(Card playerChosenCard, PlayedCards playedCards) {
            List<Card> tmpPlayerDeck = _playableDeck;

            foreach (Card card in tmpPlayerDeck.ToList()) {
                if (playerChosenCard.Rank == card.Rank) {
                    if (AskPlayDuplicateCard()) {
                        playedCards.Add(card);
                        _playableDeck.Remove(card);
                        _cardsPlayed++;
                    }
                }
            }
        }

        public virtual int GetPlayOption(int numOptions) {
            return 0;
        }

        public virtual bool AskPlayDuplicateCard() {
            return false;
        }

        public void OnPlayerOutOfCards() {
            TriggerPlayerEvent?.Invoke(this, EventArgs.Empty);
        }        
    }
}
