using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignEksamen {
    public class Deck : IDeck {
        private readonly List<Card> _deck;

        public Deck(List<Card> deck) {
            _deck = deck;
        }

        int IDeck.Size() {
            return _deck.Count;
        }

        public bool IsDeckEmpty() {
            return _deck.Count == 0;
        }

        Card IDeck.DrawCardFromDeck() {
            Card _newCard = _deck[_deck.Count - 1];
            _deck.RemoveAt(_deck.Count - 1);

            return _newCard;
        }

    }
}
