using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignEksamen
{
	public interface ICard {
		public Suit Suit { get; set; }
		public Rank Rank { get; set; }
		public CardAbility CardAbility { get; set; }
		public bool CompareTotalValueCard(Card card);
        public int CompareTo(Card that);
        public bool ToggleWasRecentlyDrawn();
		public void SetAsRecentlyDrawn();
	}
}
