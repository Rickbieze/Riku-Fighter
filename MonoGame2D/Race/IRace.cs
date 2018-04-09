using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riku_fighter.Race
{
    public abstract class IRace : Person
    {
        public IRace (string name, Person father, Person mother, Gender.Genders gender, DateTime birthDate) : base(name, father, mother, gender, birthDate)
        {

        }

        public IRace()
        {

        }
        public string Description { get; set; }
        public string SkinColor { get; set; }
    }
}
