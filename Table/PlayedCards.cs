using System.Collections.Generic;

namespace SoftwareDesignEksamen {
    public class PlayedCards {

        public List<Card> CurrentlyPlayedCardsAtTable { get; set; } = new();
        public bool ClearLastPlayedCardsNextAdd { get; set; } = false;
        public Card UnplacedCard { get; set; } // Face-down or chance card that failed validation. Used to announce card to other players next turn

        public void Add(Card card)
        {
            UnplacedCard = null;
            MarkCardAsLastPlayed(card);
            CurrentlyPlayedCardsAtTable.Add(card);

            if (card.CardAbility == CardAbility.Throw) {
                ThrowDeck();
            }
        }

        public bool AreTherePlayedCardsAtTable() {
            return CurrentlyPlayedCardsAtTable.Count > 0;
        }

        public bool CheckForFourEquals() {
	        if (CurrentlyPlayedCardsAtTable.Count < 4) {
		        return false;
	        }

	        bool has4Equals = false;
	        int countOfEquals = 0;

	        for (int i = 1; i < CurrentlyPlayedCardsAtTable.Count; i++) {
		        if (CurrentlyPlayedCardsAtTable[i].CompareTo((CurrentlyPlayedCardsAtTable[i - 1])) == 0) {
			        countOfEquals++;
			        if (countOfEquals == 3) {
                        ThrowDeck();
                        has4Equals = true;
			        }
		        } else {
			        countOfEquals = 0;
		        }
	        }

	        return has4Equals;
        }

        public bool ValidateChosenCard(Card cardToValidate) {
	        if (CurrentlyPlayedCardsAtTable.Count == 0) return true;

	        switch (cardToValidate.CardAbility) {
		        case CardAbility.Reset:
		        case CardAbility.Throw:
			        return true;
		        default:
			        return cardToValidate.Rank >= CurrentlyPlayedCardsAtTable[CurrentlyPlayedCardsAtTable.Count-1].Rank;
	        }
        }

        private void MarkCardAsLastPlayed(Card card)
        {
            if (AreTherePlayedCardsAtTable()) {
                Card lastCard = CurrentlyPlayedCardsAtTable[CurrentlyPlayedCardsAtTable.Count-1];

                if (lastCard.Rank != card.Rank || ClearLastPlayedCardsNextAdd) {

                    // Reset all playedCards marked as played last turn
                    foreach (Card c in CurrentlyPlayedCardsAtTable) {
                        c.WasPlayedLastTurn = false;
                        ClearLastPlayedCardsNextAdd = false;
                    }
                }
            }

            card.WasPlayedLastTurn = true;
        }

        private void ThrowDeck() {
            CurrentlyPlayedCardsAtTable.Clear();
        }

    }

}
