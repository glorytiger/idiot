using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignEksamen {
    public static class DeckFactory {
        private static readonly List<Card> _deck = new();

        public static IDeck CreateDeck() {
            AddCardsToDeck();
            ShuffleDeck();
            return new Deck(_deck);
        }

        private static void AddCardsToDeck() {
            foreach (Suit suit in Enum.GetValues(typeof(Suit))) {
                foreach (Rank rank in Enum.GetValues(typeof(Rank))) {
                    Card newCard = new(suit, rank);
                    _deck.Add(newCard);

                    if (Config.Instance.DeckSize <= _deck.Count)
                        return;
                }
            }
        }

        private static void ShuffleDeck() {
            Random rng = new();

            for (int i = 0; i < _deck.Count; i++) {

                int randomNumber = rng.Next(_deck.Count - 1);

                Card tmp = _deck[i];
                _deck[i] = _deck[randomNumber];
                _deck[randomNumber] = tmp;

            }
        }
    }
}
