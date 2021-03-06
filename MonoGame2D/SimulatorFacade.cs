﻿using Riku_fighter.Race;
using Riku_fighter.State;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Script.Serialization;

namespace Riku_fighter
{
    public class SimulatorFacade
    {
        private List<string> FemaleNames = new List<string>();
        private List<string> MaleNames = new List<string>();

        public DateTime CurrentDate = DateTime.Now;
        private static readonly double ACCIDENT_PROB = 0.99;

        private static readonly double PARTNER_PROB = 0.7;
        private static readonly double BREAKUP_PROB = 0.99;
        private static readonly double PREGNANT_PROB = 0.8;

        private static readonly double RELIGION_PROB = 0.9;

        public List<Person> Humanity = new List<Person>();
        public List<Person> TempDeadPeople = new List<Person>();
        public List<Person> AliveHumans = new List<Person>();
        public List<Person> DeadHumans = new List<Person>();
        public List<Person> TempBaby = new List<Person>();

        public SimulatorFacade()
        {
            PopulateNames();
            CreateHumanity();
        }

        public void CreateHumanity()
        {
            Mongoloid Matthew = new Mongoloid()
            {
                FirstName = "Matthew",
                LastName = "Elderhorst",
                Father = null,
                Mother = null,
                Gender = Gender.Genders.male,
                Birthdate = new DateTime(1994, 6, 3),
                State = new Healthy(),
                Race = new Mongoloid(),
                Children = new List<Person>()
            };

            Negroid Saskia = new Negroid()
            {
                FirstName = "Saskia",
                LastName = "Riemeijer",
                Father = null,
                Mother = null,
                Gender = Gender.Genders.female,
                Birthdate = new DateTime(1994, 1, 17),
                State = new Healthy(),
                Race = new Negroid(),
                Children = new List<Person>()
            };

            Caucasoid Marnix = new Caucasoid()
            {
                FirstName = "Marnix",
                LastName = "Manuel",
                Father = null,
                Mother = null,
                Gender = Gender.Genders.male,
                Birthdate = new DateTime(1996, 4, 23),
                State = new Healthy(),
                Race = new Caucasoid(),
                Children = new List<Person>()
            };

            Australoid Scarlett = new Australoid()
            {
                FirstName = "Scarlett",
                LastName = "Johansson",
                Father = null,
                Mother = null,
                Gender = Gender.Genders.female,
                Birthdate = new DateTime(1984, 11, 22),
                State = new Healthy(),
                Race = new Australoid(),
                Children = new List<Person>()
            };

            Mongoloid Gwyn = new Mongoloid()
            {
                FirstName = "Gwyn",
                LastName = "Lord",
                Father = null,
                Mother = null,
                Gender = Gender.Genders.male,
                Birthdate = new DateTime(1994, 6, 3),
                State = new Healthy(),
                Race = new Mongoloid(),
                Children = new List<Person>()
            };

            Negroid Priscilla = new Negroid()
            {
                FirstName = "Priscilla",
                LastName = "Light",
                Father = null,
                Mother = null,
                Gender = Gender.Genders.female,
                Birthdate = new DateTime(1994, 10, 29),
                State = new Healthy(),
                Race = new Negroid(),
                Children = new List<Person>()
            };

            Caucasoid John = new Caucasoid()
            {
                FirstName = "John",
                LastName = "Handleiding",
                Father = null,
                Mother = null,
                Gender = Gender.Genders.male,
                Birthdate = new DateTime(1995, 1, 12),
                State = new Healthy(),
                Race = new Caucasoid(),
                Children = new List<Person>()
            };

            Australoid Mable = new Australoid()
            {
                FirstName = "Mable",
                LastName = "Focus",
                Father = null,
                Mother = null,
                Gender = Gender.Genders.female,
                Birthdate = new DateTime(1994, 6, 23),
                State = new Healthy(),
                Race = new Australoid(),
                Children = new List<Person>()
            };

            Marnix.Partner = Scarlett;
            Scarlett.Partner = Marnix;
            Matthew.Partner = Saskia;
            Saskia.Partner = Matthew;
            Scarlett.Partner = Marnix;
            Gwyn.Partner = Priscilla;
            Priscilla.Partner = Gwyn;
            John.Partner = Mable;
            Mable.Partner = John;



            Person Adam = new Mongoloid("Adam", Matthew, Saskia, Gender.Genders.male, DateTime.Now);
            Person Madison = new Negroid("Madison", Matthew, Saskia, Gender.Genders.female, DateTime.Now);
            Person Jacob = new Caucasoid("Jacob", Marnix, Scarlett, Gender.Genders.male, DateTime.Now);
            Person Eve = new Australoid("Eve", Marnix, Scarlett, Gender.Genders.female, DateTime.Now);

            Person James = new Australoid("James", Gwyn, Priscilla, Gender.Genders.male, DateTime.Now);
            Person Laura = new Caucasoid("Laura", John, Mable, Gender.Genders.female, DateTime.Now);
            Person Gottard = new Negroid("Gottard", John, Mable, Gender.Genders.male, DateTime.Now);
            Person Gwynevere = new Mongoloid("Gwynevere", Gwyn, Priscilla, Gender.Genders.female, DateTime.Now);

            Adam.State = new Healthy();
            Madison.State = new Healthy();
            Jacob.State = new Healthy();
            Eve.State = new Healthy();
            James.State = new Healthy();
            Laura.State = new Healthy();
            Gottard.State = new Healthy();
            Gwynevere.State = new Healthy();

            Marnix.Children.Add(Jacob);
            Marnix.Children.Add(Eve);
            Scarlett.Children.Add(Jacob);
            Scarlett.Children.Add(Eve);
            Matthew.Children.Add(Adam);
            Matthew.Children.Add(Madison);
            Saskia.Children.Add(Adam);
            Saskia.Children.Add(Madison);
            Gwyn.Children.Add(James);
            Gwyn.Children.Add(Gwynevere);
            Priscilla.Children.Add(James);
            Priscilla.Children.Add(Gwynevere);
            John.Children.Add(Laura);
            John.Children.Add(Gottard);
            Mable.Children.Add(Laura);
            Mable.Children.Add(Gottard);

            Humanity.Add(Marnix);
            Humanity.Add(Scarlett);
            Humanity.Add(Matthew);
            Humanity.Add(Saskia);
            Humanity.Add(Gwyn);
            Humanity.Add(Priscilla);
            Humanity.Add(Mable);
            Humanity.Add(John);
            Humanity.Add(Adam);
            Humanity.Add(Eve);
            Humanity.Add(Jacob);
            Humanity.Add(Madison);
            Humanity.Add(James);
            Humanity.Add(Laura);
            Humanity.Add(Gottard);
            Humanity.Add(Gwynevere);

            AliveHumans.Add(Marnix);
            AliveHumans.Add(Scarlett);
            AliveHumans.Add(Matthew);
            AliveHumans.Add(Saskia);
            AliveHumans.Add(Gwyn);
            AliveHumans.Add(Priscilla);
            AliveHumans.Add(Mable);
            AliveHumans.Add(John);
            AliveHumans.Add(Adam);
            AliveHumans.Add(Eve);
            AliveHumans.Add(Jacob);
            AliveHumans.Add(Madison);
            AliveHumans.Add(James);
            AliveHumans.Add(Laura);
            AliveHumans.Add(Gottard);
            AliveHumans.Add(Gwynevere);
        }
             
        public List<Person> GetBabiesThisRound()
        {
            List<Person> babies = TempBaby;
            return babies;
        }

        public List<Person> GetHumanity()
        {
            return Humanity;
        }

        public void DeleteBabyList()
        {
            TempBaby.Clear();
        }
        public List<Person> GetDeadThisRound()
        {
            List<Person> dead = TempDeadPeople;
            return dead;
        }
        public void DeleteDeadList()
        {
            TempDeadPeople.Clear();
        }
        public void RunSimulator()
        {
            testSim();
            CurrentDate = CurrentDate.AddYears(1);
            List<Person> population = new List<Person>();

            foreach (var sprite in AliveHumans.ToList())
            {
                int age = sprite.GetAge(CurrentDate);
                var dieProb = new Probability().GetRandomDouble();
                var accProb = new Probability().GetRandomDouble();
                if (dieProb > sprite.GetDieProb() && sprite.Mother != null && sprite.Father != null || accProb > ACCIDENT_PROB && sprite.Mother != null && sprite.Father != null)
                {
                    sprite.State = new Deceased(CurrentDate);
                }

                if (sprite.State.GetType() == typeof(Healthy))
                {
                    sprite.State.GetAliveState();
                    if (new Probability().GetRandomDouble() > sprite.State.CurrStatus.DISEASE_PROB)
                    {
                        sprite.State = new Sick();
                    }
                }
                else if(sprite.State.GetType() == typeof(Sick))
                {
                    if (new Probability().GetRandomDouble() > sprite.State.CurrStatus.CURE_PROB)
                    {
                        sprite.State = new Healthy();
                    }
                }              

                //Gives person a Religion
                

                //Checks if person is alive or not
                if (sprite.State.GetType() != typeof(Deceased))
                {
                    //every year a persons die probability will increase with 0.005
                    sprite.IncDieProb(0.005);
                    if (sprite.Partner != null)
                    {
                        //break up
                        if (new Probability().GetRandomDouble() > BREAKUP_PROB && sprite.Father != null && sprite.Mother != null)
                        {
                            sprite.Partner.Partner = null;
                            sprite.Partner = null;
                        }

                        if (sprite.Partner != null)
                        {
                            int amountOfChildren;
                            if (sprite.Partner.Children != null)
                            {
                                amountOfChildren = sprite.Children.Count;
                                //attempts to get children by the couples
                                //Debug.WriteLine(sprite.FirstName + " :: " + sprite.Partner.FirstName + " " +  amountOfChildren);
                            }
                            else
                            {
                                amountOfChildren = 0;
                            }

                            if ((new Probability().babyRate() > (135 + AliveHumans.Count())) && AliveHumans.Count() < 50)
                            {
                                Debug.WriteLine("BABY: " + sprite.Gender + " :: " + sprite.Partner.Gender + " " + amountOfChildren);
                                if (sprite.GetAge(CurrentDate) > 18 && sprite.GetAge(CurrentDate) < 65 && amountOfChildren < 5 && sprite.Partner.Gender != sprite.Gender && sprite.Mother != null && sprite.Father != null && sprite.Partner.State.GetType() != typeof(Deceased))
                                {
                                    Debug.WriteLine("SUCCESS");
                                    //Needs to add child name and random gender
                                    Probability femaleNameProb = new Probability(FemaleNames.Count);
                                    Probability maleNameProb = new Probability(MaleNames.Count);
                                    String Fname = FemaleNames[femaleNameProb.rInt];
                                    String Mname = MaleNames[maleNameProb.rInt];
                                    Person Child = sprite.MakeBaby(CurrentDate, Mname, Fname);
                                    Person Baby = Child;
                                    if(Baby != null)
                                    {
                                        TempBaby.Add(Baby);
                                        Humanity.Add(Baby);
                                    }
                                }
                            }
                        }                        
                    }
                    else
                    {
                        Probability partnerProb = new Probability(10);
                        if (PARTNER_PROB * partnerProb.rInt > 3.5 && sprite.Mother != null && sprite.Father != null && sprite.Age > 16)
                        {
                            foreach (var newpartner in AliveHumans)
                            {
                                //Checks if person if alive or not
                                if (newpartner.State.GetType() != typeof(Deceased) && newpartner.Age > 16)
                                {
                                    int ageDifference = Math.Abs(sprite.GetAge(CurrentDate) - newpartner.GetAge(CurrentDate));
                                    if (newpartner.Father != sprite.Father || sprite.Mother != newpartner.Mother)
                                    {
                                        if (newpartner != sprite && ageDifference < 15 && newpartner.Partner == null)
                                        {
                                            Random random = new Random();
                                            int rnd = random.Next(0, 100);

                                            if(newpartner.Gender == sprite.Gender)
                                            {
                                                if(rnd >= 80)
                                                {
                                                    newpartner.Partner = sprite;
                                                    sprite.Partner = newpartner;
                                                }
                                            }
                                            else
                                            {
                                                newpartner.Partner = sprite;
                                                sprite.Partner = newpartner;
                                            }

                                        }
                                    }
                                }                                
                            }
                        }
                    }
                }
                //Adds religion
                if (new Probability().GetRandomDouble() > RELIGION_PROB && sprite.GetAge(CurrentDate) > 5)
                {
                    Probability prob = new Probability(5);
                    switch (prob.rInt)
                    {
                        case 0:
                            sprite.Religion = "Atheist";
                            break;
                        case 1:
                            sprite.Religion = "Buddhism";
                            break;
                        case 2:
                            sprite.Religion = "Christianity";
                            break;
                        case 3:
                            sprite.Religion = "Hinduism";
                            break;
                        case 4:
                            sprite.Religion = "Islam";
                            break;
                    }
                }
                population.Add(sprite);
            }
            //LINQ
            var deadHumanQuery = from humans in AliveHumans where humans.State.CurrStatus == null select humans;

            var aliveHumanQuery = from humans in AliveHumans where humans.State.CurrStatus != null select humans;
            //Humanity.Add((Person)from humans in AliveHumans where humans.State == typeof(Alive) select humans);
            //Humanity.Add((Person)from humans in AliveHumans where humans.State == typeof(Deceased) select humans);
            foreach (var deadHuman in deadHumanQuery.ToList())
            {
                AliveHumans.Remove(deadHuman);
                DeadHumans.Add(deadHuman);
                TempDeadPeople.Add(deadHuman);
            }
            foreach (var temphuman in TempBaby.ToList())
            {
                population.Add(temphuman);
                AliveHumans.Add(temphuman);
            }
        }

        public SimulatorStatistics getSimulatorStatistics()
        {
            SimulatorStatistics stats = new SimulatorStatistics(DeadHumans.Count, AliveHumans.Count);
            return stats;
        }

        public void testSim()
        {
            Debug.WriteLine(AliveHumans.Count + " alive");
            Debug.WriteLine(DeadHumans.Count + " dead");
        }

        public String getCurrentDate()
        {
            return CurrentDate.Year.ToString();
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
    }
}
