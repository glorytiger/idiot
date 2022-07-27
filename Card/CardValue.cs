using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignEksamen {
    public enum Rank {
        Two = 2, //CardAbility Reset
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten, //CardAbility Throw
        Jack,
        Queen,
        King,
        Ace
    }

    public enum Suit {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum CardAbility { 
        None,
        Reset,
        Throw
    }
}
