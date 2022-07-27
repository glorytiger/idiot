using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignEksamen {
    class HandFaceUpCards : IHand {
        public List<Card> Cards { get; set; } = new();
        private int _tableSlotIdCounter = 0;

        public void AddCard(IDeck deck) {
            if (!deck.IsDeckEmpty()) {
                Card card = deck.DrawCardFromDeck();
                card.DeckSlotId = _tableSlotIdCounter++;
                Cards.Add(card);
            }
        }

        public void PlayCard(Card card) {
            Cards.Remove(card);
        }
    }
}
