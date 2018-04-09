using Riku_fighter.Race;
using Riku_fighter.State;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Riku_fighter
{
    public abstract class Person
    {
        protected double DIE_PROB = 0.99;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Person Father { get; set; }
        public Person Mother { get; set; }
        public Gender.Genders Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public IState State { get; set; }
        public IRace Race { get; set; }
        public Person Partner { get; set; }
        public int Age { get; set; }
        public List<Person> Children { get; set; }
        public string Religion { get; set; }

        public Person()
        {
            // empty
        }

        public Person(string FirstName, Person Father, Person Mother, Gender.Genders Gender, DateTime Birthdate)
        {
            this.FirstName = FirstName;
            LastName = Father.LastName;
            this.Father = Father;
            this.Mother = Mother;
            this.Gender = Gender;
            this.Birthdate = Birthdate;
            this.State = new Healthy();
            RaceFactory raceFactory = new RaceFactory();
            Race = raceFactory.CreateRace(Father, Mother);
            Children = new List<Person>();
        }

        public int GetAge(DateTime CurrentDate)
        {
            // Calculate age
            int age = CurrentDate.Year - Birthdate.Year;
            // Go back to the year the person was born in case of a leap year
            if (Birthdate > CurrentDate.AddYears(-age)) age--;
            if (Birthdate.Day == CurrentDate.Day && Birthdate.Month == CurrentDate.Month && Birthdate.Year == CurrentDate.Year) age = 0;
            Age = age;
            return age;
        }

        public void GetState()
        {
            State.CurrStatus.GetAliveState();
        }

        public double GetDieProb()
        {
            if(State != null)
            {
                return DIE_PROB + State.INC_DIE_PROB;
            }
            else
            {
                State = new Healthy();
                return DIE_PROB + State.INC_DIE_PROB;
            }                   
        }

        public void IncDieProb(double amount)
        {
            DIE_PROB += amount;
        }

        public Person MakeBaby(DateTime born, String Mname, String Fname)
        {
            RaceFactory factory = new RaceFactory();

            var race = factory.CreateRace(Father, Mother);

            Person Child;
            Person ChildsMother;
            Person ChildsFather;

            if (Gender == Riku_fighter.Gender.Genders.female)
            {
                ChildsMother = this;
                ChildsFather = Partner;
            }
            else
            {
                ChildsMother = Partner;
                ChildsFather = this;
            }

            if (Gender != Partner.Gender)
            {
                
                Probability getGender = new Probability(2);
                if (getGender.rInt == 0)
                {
                    if (Father.Race.GetType() == typeof(Mongoloid))
                    {
                        Child = new Mongoloid(Fname, ChildsFather, ChildsMother, Mother.Gender, born);
                    }
                    else if (Father.Race.GetType() == typeof(Australoid))
                    {
                        Child = new Australoid(Fname, ChildsFather, ChildsMother, Mother.Gender, born);
                    }
                    else if (Father.Race.GetType() == typeof(Negroid))
                    {
                        Child = new Negroid(Fname, ChildsFather, ChildsMother, Mother.Gender, born);
                    }
                    else
                    {
                        Child = new Caucasoid(Fname, ChildsFather, ChildsMother, Mother.Gender, born);
                    }

                    Children.Add(Child);
                    Partner.Children.Add(Child);
                }
                else
                {
                    if (Father.Race.GetType() == typeof(Mongoloid))
                    {
                        Child = new Mongoloid(Mname, ChildsFather, ChildsMother, Father.Gender, born);
                    }
                    else if (Father.Race.GetType() == typeof(Australoid))
                    {
                        Child = new Australoid(Mname, ChildsFather, ChildsMother, Father.Gender, born);
                    }
                    else if (Father.Race.GetType() == typeof(Negroid))
                    {
                        Child = new Negroid(Mname, ChildsFather, ChildsMother, Father.Gender, born);
                    }
                    else
                    {
                        Child = new Caucasoid(Mname, ChildsFather, ChildsMother, Father.Gender, born);
                    }
                    Children.Add(Child);
                    Partner.Children.Add(Child);
                }
                return Child;
            }
            else
            {
                Child = null;
                return Child;
            }    
            
        }
        public String getCurrentState()
        {
            String status = "";
            if(State.GetType() == typeof(Healthy))
            {
                status = "Healthy";
            }
            if(State.GetType() == typeof(Deceased))
            {
                status = "Dead";
            }
            if(State.GetType() == typeof(Sick))
            {
                status = "Sick";
            }
            return status;
        }
    }
}
