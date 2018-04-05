using Riku_fighter.Race;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter
{
    public class RaceFactory : Person
    {        
        public IRace CreateRace(Person Father, Person Mother)
        {
            IRace race;

            Probability probability = new Probability(20);
            if (probability.rInt < 11)
            {
                race = Father.Race;
            }
            else
            {
                race = Mother.Race;
            }

            return race;
        }
    }
}
