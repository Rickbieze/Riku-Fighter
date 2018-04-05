using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter
{
    public class Alive : IState
    {
        public double INC_DIE_PROB { get; set; }
        public double DISEASE_PROB = 0.1;
        public double CURE_PROB = 0.1;
        public Alive CurrStatus { get; set; }
        public DateTime DeathDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void GetAliveState()
        {
            if(CurrStatus == null)
            {
                CurrStatus = new Healthy();
            }
            if (CurrStatus.GetType() == typeof(Healthy))
            {
                if (new Probability().GetRandomDouble() < DISEASE_PROB)
                {
                    CurrStatus = new Sick();
                }
            }
            else if(CurrStatus.GetType() == typeof(Sick))
            {
                if (new Probability().GetRandomDouble() < CURE_PROB)
                {
                    CurrStatus = new Healthy();
                }
            }
        }
    }
}
