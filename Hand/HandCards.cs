using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignEksamen {
    class HandCards : IHand {
        public List<Card> Cards { get; set; } = new();

        public void AddCard(IDeck deck) {
            if (!deck.IsDeckEmpty()) {
                Cards.Add(deck.DrawCardFromDeck());
            }
        }

        public void PlayCard(Card card) {
            Cards.Remove(card);
        }
    }
}
