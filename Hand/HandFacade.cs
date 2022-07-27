using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignEksamen {
    public class HandFacade {
        public IHand HandCards { get; private set; } = new HandCards();
        public IHand FaceDownCards { get; private set; } = new HandFaceDownCards();
        public IHand FaceUpCards { get; private set; } = new HandFaceUpCards();

        #region HandCards
        public int GetHandCardSize() {
            return HandCards.Cards.Count;
        }
        public void DealHandCards(IDeck deck) {
            HandCards.AddCard(deck);
        }
        public void AddToHandCards(Card card) {
            HandCards.Cards.Add(card);
        }
        public void SortHandCards() {
            HandCards.Cards.Sort();
        }
        #endregion

        #region HandFaceUpCards
        public int GetFaceUpCardSize() {
            return FaceUpCards.Cards.Count;
        }
        public void DealFaceUpCards(IDeck deck) {
            FaceUpCards.AddCard(deck);
        }
        #endregion

        #region HandFaceDownCards
        public int GetFaceDownCardSize() {
            return FaceDownCards.Cards.Count;
        }
        public void DealFaceDownCards(IDeck deck) {
            FaceDownCards.AddCard(deck);
        }

        #endregion

    }
}
