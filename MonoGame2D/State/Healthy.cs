using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter
{
    public class Healthy : Alive
    {
        public static readonly double DISEASE_PROB = 0.8;
        public Healthy()
        {
            CurrStatus = this;
            INC_DIE_PROB = 0.01;
        }
    }
}
