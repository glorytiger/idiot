using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignEksamen {
    public interface IHand {
        List<Card> Cards {get; set;}
        public void AddCard(IDeck deck);
        public void PlayCard(Card card);
    }
}
