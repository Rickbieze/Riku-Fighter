using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter
{
    public class Probability
    {
        public int rInt = 0;
        public Probability()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
        }
        public Probability(int chance)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            rInt = random.Next(0, chance);
        }

        public double GetRandomDouble()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            return random.NextDouble() * (1.0 - 0.0) + 0.0;
        }
    }
}
