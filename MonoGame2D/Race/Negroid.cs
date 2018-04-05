using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter.Race
{
    public class Negroid : IRace
    {
        public Negroid()
        {
            Description = "Negroid";
            SkinColor = "Black";
        }

        public Negroid(string name, Person father, Person mother, Gender.Genders gender, DateTime birthDate) : base(name, father, mother, gender, birthDate)
        {
            Description = "Negroid";
            SkinColor = "Black";
        }
    }
}
