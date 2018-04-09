using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter
{
    public class Sick : Alive
    {
        public static readonly double CURE_PROB = 0.1;
        public Sick()
        {
            CurrStatus = this;
            INC_DIE_PROB = -0.03;
        }
    }
}
