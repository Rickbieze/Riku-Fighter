using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter.Race
{
    public class Australoid : IRace
    {
        public Australoid()
        {
            Description = "Australoid";
            SkinColor = "Light Brown";
        }

        public Australoid(string name, Person father, Person mother, Gender.Genders gender, DateTime birthDate) : base(name, father, mother, gender, birthDate)
        {
            Description = "Australoid";
            SkinColor = "Light Brown";
        }
    }
}
