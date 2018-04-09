using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter.Race
{
    public class Caucasoid : IRace
    {
        public Caucasoid()
        {
            Description = "Caucasoid";
            SkinColor = "White boi";
        }

        public Caucasoid(string name, Person father, Person mother, Gender.Genders gender, DateTime birthDate) : base(name, father, mother, gender, birthDate)
        {
            Description = "Caucasoid";
            SkinColor = "White boi";
        }
    }
}
