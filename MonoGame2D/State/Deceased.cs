using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter.State
{
    public class Deceased : IState
    {
        public double INC_DIE_PROB { get; set; }
        public DateTime DeathDate { get; set; }
        public Alive CurrStatus { get; set; }

        public Deceased(DateTime DeathDate)
        {
            this.DeathDate = DeathDate;
        }

        public void GetAliveState()
        {
            CurrStatus = null;
            //A persons state can't change when dead
        }
    }
}
