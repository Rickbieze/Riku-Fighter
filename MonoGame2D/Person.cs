using Riku_fighter.Race;
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
        private List<string> FemaleNames = new List<string>();
        private List<string> MaleNames = new List<string>();
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
            PopulateNames();
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

        public void PopulateNames()
        {
           
            // string maleNames = System.IO.File.ReadAllText(@"C:\Users\" + Environment.UserName + @"\Source\Repos\LifeSimulator\LifeSimulator\LifeSimulator\MaleNames.txt");
            Task<String> femaleTask = new Task<String>(() =>
            {
                string femaleName = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "\\FemaleNames.txt");
                return femaleName;
            });

            Task<String> maleTask = new Task<String>(() =>
            {
                string maleName = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "\\MaleNames.txt");
                return maleName;
            });

            femaleTask.Start();
            maleTask.Start();

            femaleTask.Wait();
            maleTask.Wait();

            string femaleNames = femaleTask.Result;
            string maleNames = maleTask.Result;

            string[] tempFemale = femaleNames.Split(',');
            foreach (var x in tempFemale)
            {
                var y = x.Substring(1);
                var name = y.TrimEnd('"');
                FemaleNames.Add(name);
            }

            string[] tempMale = maleNames.Split(',');
            foreach (var x in tempMale)
            {
                var y = x.Substring(1);
                MaleNames.Add(y.TrimEnd('"'));
            }
        }

        public Person MakeBaby(DateTime born)
        {
            PopulateNames();
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
                    Probability femaleNameProb = new Probability(FemaleNames.Count);
                    if (Father.Race.GetType() == typeof(Mongoloid))
                    {
                        Child = new Mongoloid(FemaleNames[femaleNameProb.rInt], ChildsFather, ChildsMother, Mother.Gender, born);
                    }
                    else if (Father.Race.GetType() == typeof(Australoid))
                    {
                        Child = new Australoid(FemaleNames[femaleNameProb.rInt], ChildsFather, ChildsMother, Mother.Gender, born);
                    }
                    else if (Father.Race.GetType() == typeof(Negroid))
                    {
                        Child = new Negroid(FemaleNames[femaleNameProb.rInt], ChildsFather, ChildsMother, Mother.Gender, born);
                    }
                    else
                    {
                        Child = new Caucasoid(FemaleNames[femaleNameProb.rInt], ChildsFather, ChildsMother, Mother.Gender, born);
                    }

                    Children.Add(Child);
                    Partner.Children.Add(Child);
                }
                else
                {
                    Probability maleNameProb = new Probability(MaleNames.Count);
                    if (Father.Race.GetType() == typeof(Mongoloid))
                    {
                        Child = new Mongoloid(MaleNames[maleNameProb.rInt], ChildsFather, ChildsMother, Father.Gender, born);
                    }
                    else if (Father.Race.GetType() == typeof(Australoid))
                    {
                        Child = new Australoid(MaleNames[maleNameProb.rInt], ChildsFather, ChildsMother, Father.Gender, born);
                    }
                    else if (Father.Race.GetType() == typeof(Negroid))
                    {
                        Child = new Negroid(MaleNames[maleNameProb.rInt], ChildsFather, ChildsMother, Father.Gender, born);
                    }
                    else
                    {
                        Child = new Caucasoid(MaleNames[maleNameProb.rInt], ChildsFather, ChildsMother, Father.Gender, born);
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
    }
}
