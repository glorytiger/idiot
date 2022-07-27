using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDesignEksamen {
    public interface IDeck { 
        int Size();
        Card DrawCardFromDeck();
        bool IsDeckEmpty();
    }
}
