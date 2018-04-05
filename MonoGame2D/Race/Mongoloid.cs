using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter.Race
{
    public class Mongoloid : IRace
    {
        public Mongoloid()
        {
            Description = "Mongoloid";
            SkinColor = "Yellow";
        }

        public Mongoloid(string name, Person father, Person mother, Gender.Genders gender, DateTime birthDate) : base(name, father, mother, gender, birthDate)
        {
            Description = "Mongoloid";
            SkinColor = "Yellow";
        }
    }
}
