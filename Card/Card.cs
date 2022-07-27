using System;

namespace SoftwareDesignEksamen {
    public class Card : ICard, IComparable<Card>
    {
        private bool _wasRecentlyDrawn = false;
        public bool WasPlayedLastTurn { get; set; } = false;
        public int DeckSlotId { get; set; }

        public Suit Suit { get; set; }
        public Rank Rank { get; set; }
        public CardAbility CardAbility { get; set; }

        #region Constructor
        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;

            switch (rank) {
                case Rank.Two:
                    CardAbility = CardAbility.Reset;
                    break;
                case Rank.Ten:
                    CardAbility = CardAbility.Throw;
                    break;
                default:
                    CardAbility = CardAbility.None;
                    break;
            }
        }
        #endregion

        #region Compare functions
        // Compares the Card Value + Card Suit (Card Suit also have a value from 0-3)
        public bool CompareTotalValueCard(Card card)
        {
            if ((int)card.Rank == 2) {  // Because card 2 is a special card and 3 is the lowest value in the deck
                return true;
            }

            int _sumCard1 = (int)Rank * 4 + (int)Suit;
            int _sumCard2 = (int)card.Rank * 4 + (int)card.Suit;

            return _sumCard1 > _sumCard2;
        }

        public int CompareTo(Card that) {
            if (this.Rank < that.Rank) return -1;
            if (this.Rank == that.Rank) return 0;
            return 1;
		}
        #endregion

        #region Operator overloading
        public static bool operator <=(Card cardA, Card cardB) {
            return cardA.Rank <= cardB.Rank;
        }

        public static bool operator >=(Card cardA, Card cardB) {
            return cardA.Rank >= cardB.Rank;
        }
        #endregion

        #region ToString function
        public override string ToString() {
            return Rank + " of " + Suit;
        }
        #endregion

        #region State functions
        public bool ToggleWasRecentlyDrawn()
        {
            if (_wasRecentlyDrawn) {
                _wasRecentlyDrawn = false;
                return true;
            }

            return false;
        }

        public void SetAsRecentlyDrawn()
        {
            _wasRecentlyDrawn = true;
        }
        #endregion
    }
}
