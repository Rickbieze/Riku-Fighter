using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter
{
    public interface IState
    {
        double INC_DIE_PROB { get; set; }
        Alive CurrStatus { get; set; }
        DateTime DeathDate { get; set; }
        void GetAliveState();
    }
}
