using Riku_fighter.Race;
using Riku_fighter.State;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using System.Web.Script.Serialization;

namespace Riku_fighter
{
    public class SimulatorFacade
    {
        public DateTime CurrentDate = DateTime.Now;
        private static readonly double ACCIDENT_PROB = 0.99;

        private static readonly double PARTNER_PROB = 0.7;
        private static readonly double BREAKUP_PROB = 0.99;
        private static readonly double PREGNANT_PROB = 0.8;

        private static readonly double RELIGION_PROB = 0.9;

        public List<Person> Humanity = new List<Person>();
        public List<Person> TempHumanity = new List<Person>();
        public List<Person> TempDeadPeople = new List<Person>();
        public List<Person> AliveHumans = new List<Person>();
        public List<Person> DeadHumans = new List<Person>();
        public List<SpriteClass> HumanitySprites = new List<SpriteClass>(); 
        public List<SpriteClass> TempBabySprites = new List<SpriteClass>();
        public List<SpriteClass> TempDeadSprites = new List<SpriteClass>();
        public List<SpriteClass> AliveSprites = new List<SpriteClass>();
        public List<SpriteClass> DeadSprites = new List<SpriteClass>();

        public SimulatorFacade()
        {
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
                Race = new Mongoloid()
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
                Race = new Negroid()
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
                Race = new Caucasoid()
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
                Race = new Australoid()
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
                Race = new Mongoloid()
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
                Race = new Negroid()
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
                Race = new Caucasoid()
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
                Race = new Australoid()
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

            Mongoloid Adam = new Mongoloid("Adam", Matthew, Saskia, Gender.Genders.male, DateTime.Now);
            Negroid Madison = new Negroid("Madison", Matthew, Saskia, Gender.Genders.female, DateTime.Now);
            Caucasoid Jacob = new Caucasoid("Jacob", Marnix, Scarlett, Gender.Genders.male, DateTime.Now);
            Australoid Eve = new Australoid("Eve", Marnix, Scarlett, Gender.Genders.female, DateTime.Now);

            Australoid James = new Australoid("James", Gwyn, Priscilla, Gender.Genders.male, DateTime.Now);
            Mongoloid Laura = new Mongoloid("Laura", John, Mable, Gender.Genders.female, DateTime.Now);
            Negroid Gottard = new Negroid("Gottard", John, Mable, Gender.Genders.male, DateTime.Now);
            Caucasoid Gwynevere = new Caucasoid("Gwynevere", Gwyn, Priscilla, Gender.Genders.female, DateTime.Now);

            SpriteClass SpriteAdam = new SpriteClass(Adam);
            SpriteClass SpriteMadison = new SpriteClass(Madison);
            SpriteClass SpriteJacob = new SpriteClass(Jacob);
            SpriteClass SpriteEve = new SpriteClass(Eve);
            SpriteClass SpriteJames = new SpriteClass(James);
            SpriteClass SpriteLaura = new SpriteClass(Laura);
            SpriteClass SpriteGottard = new SpriteClass(Gottard);
            SpriteClass SpriteGwynevere = new SpriteClass(Gwynevere);

            Adam.State = new Healthy();
            Madison.State = new Healthy();
            Jacob.State = new Healthy();
            Eve.State = new Healthy();
            James.State = new Healthy();
            Laura.State = new Healthy();
            Gottard.State = new Healthy();
            Gwynevere.State = new Healthy();

            Humanity.Add(Adam);
            Humanity.Add(Eve);
            Humanity.Add(Jacob);
            Humanity.Add(Madison);
            Humanity.Add(James);
            Humanity.Add(Laura);
            Humanity.Add(Gottard);
            Humanity.Add(Gwynevere);
            Humanity.Add(Marnix);
            Humanity.Add(Scarlett);
            Humanity.Add(Matthew);
            Humanity.Add(Saskia);
            Humanity.Add(Gwyn);
            Humanity.Add(Priscilla);
            Humanity.Add(Mable);
            Humanity.Add(John);

            AliveHumans.Add(Adam);
            AliveHumans.Add(Eve);
            AliveHumans.Add(Jacob);
            AliveHumans.Add(Madison);
            AliveHumans.Add(James);
            AliveHumans.Add(Laura);
            AliveHumans.Add(Gottard);
            AliveHumans.Add(Gwynevere);
            AliveHumans.Add(Marnix);
            AliveHumans.Add(Scarlett);
            AliveHumans.Add(Matthew);
            AliveHumans.Add(Saskia);
            AliveHumans.Add(Gwyn);
            AliveHumans.Add(Priscilla);
            AliveHumans.Add(Mable);
            AliveHumans.Add(John);

            AliveSprites.Add(SpriteAdam);
            AliveSprites.Add(SpriteMadison);
            AliveSprites.Add(SpriteJacob);
            AliveSprites.Add(SpriteEve);
            AliveSprites.Add(SpriteJames);
            AliveSprites.Add(SpriteLaura);
            AliveSprites.Add(SpriteGottard);
            AliveSprites.Add(SpriteGwynevere);

        }
             
        public List<SpriteClass> GetBabiesThisRound()
        {
            List<SpriteClass> babies = TempBabySprites;
            TempBabySprites.Clear();
            return babies;
        }

        public List<SpriteClass> GetDeadThisRound()
        {
            List<SpriteClass> dead = TempDeadSprites;
            TempDeadSprites.Clear();
            return dead;
        }

        public void RunSimulator()
        {
            testSim();
            CurrentDate = CurrentDate.AddYears(1);
            List<Person> population = new List<Person>();

            foreach (var sprite in AliveSprites)
            {
                int age = sprite.person.GetAge(CurrentDate);
                var dieProb = new Probability().GetRandomDouble();
                var accProb = new Probability().GetRandomDouble();
                //Console.WriteLine("Die Probability: " + dieProb + "-- Accident Probability: " + accProb);
                if (dieProb > sprite.person.GetDieProb() && sprite.person.Mother != null && sprite.person.Father != null || accProb > ACCIDENT_PROB && sprite.person.Mother != null && sprite.person.Father != null)
                {
                    sprite.person.State = new Deceased(CurrentDate);
                }

                if (sprite.person.State.GetType() == typeof(Healthy))
                {
                    sprite.person.State.GetAliveState();
                    if (new Probability().GetRandomDouble() > sprite.person.State.CurrStatus.DISEASE_PROB)
                    {
                        sprite.person.State = new Sick();
                    }
                }
                else if(sprite.person.State.GetType() == typeof(Sick))
                {
                    if (new Probability().GetRandomDouble() > sprite.person.State.CurrStatus.CURE_PROB)
                    {
                        sprite.person.State = new Healthy();
                    }
                }              

                //Gives person a Religion
                

                //Checks if person is alive or not
                if (sprite.person.State.GetType() != typeof(Deceased))
                {
                    //every year a persons die probability will increase with 0.005
                    sprite.person.IncDieProb(0.005);
                    if (sprite.person.Partner != null)
                    {
                        //break up
                        if (new Probability().GetRandomDouble() > BREAKUP_PROB && sprite.person.Father != null && sprite.person.Mother != null)
                        {
                            sprite.person.Partner.Partner = null;
                            sprite.person.Partner = null;
                        }

                        if(sprite.person.Partner != null)
                        {
                            //todo: probability
                            if (new Probability().GetRandomDouble() < PREGNANT_PROB)
                            {
                                if (sprite.person.GetAge(CurrentDate) > 18 && sprite.person.Partner.Gender != sprite.person.Gender && sprite.person.Mother != null && sprite.person.Father != null && sprite.person.Partner.State.GetType() != typeof(Deceased))
                                {
                                    //Needs to add child name and random gender

                                    Person Child = sprite.person.MakeBaby(CurrentDate);
                                    SpriteClass Baby = new SpriteClass(Child);
                                    TempBabySprites.Add(Baby);
                                }
                            }
                        }                        
                    }
                    else
                    {
                        Probability partnerProb = new Probability(10);
                        if (PARTNER_PROB * partnerProb.rInt > 3.5 && sprite.person.Mother != null && sprite.person.Father != null && sprite.person.Age > 16)
                        {
                            foreach (var newpartner in AliveSprites)
                            {
                                //Checks if person if alive or not
                                if (newpartner.person.State.GetType() != typeof(Deceased) && newpartner.person.Age > 16)
                                {
                                    int ageDifference = Math.Abs(sprite.person.GetAge(CurrentDate) - newpartner.person.GetAge(CurrentDate));
                                    if (newpartner.person.Father != sprite.person.Father || sprite.person.Mother != newpartner.person.Mother)
                                    {
                                        if (newpartner.person != sprite.person && ageDifference < 15 && newpartner.person.Partner == null)
                                        {
                                            newpartner.person.Partner = sprite.person;
                                            sprite.person.Partner = newpartner.person;
                                        }
                                    }
                                }                                
                            }
                        }
                    }
                }
                //Adds religion
                if (new Probability().GetRandomDouble() > RELIGION_PROB && sprite.person.GetAge(CurrentDate) > 5)
                {
                    Probability prob = new Probability(5);
                    switch (prob.rInt)
                    {
                        case 0:
                            sprite.person.Religion = "Atheist";
                            break;
                        case 1:
                            sprite.person.Religion = "Buddhism";
                            break;
                        case 2:
                            sprite.person.Religion = "Christianity";
                            break;
                        case 3:
                            sprite.person.Religion = "Hinduism";
                            break;
                        case 4:
                            sprite.person.Religion = "Islam";
                            break;
                    }
                }
                population.Add(sprite.person);
                if(sprite.person.State.GetType() == typeof(Deceased))
                {
                    TempDeadSprites.Add(sprite);
                    DeadSprites.Add(sprite);
                }
            }
            foreach(var deadHuman in TempDeadSprites)
            {
                AliveSprites.Remove(deadHuman);
            }
            foreach (var temphuman in TempBabySprites)
            {
                population.Add(temphuman.person);
                AliveSprites.Add(temphuman);
            }
            HumanitySprites = AliveSprites.Concat(DeadSprites).ToList();
        }

        public void testSim()
        {
            Debug.WriteLine(AliveSprites.Count);
            Debug.WriteLine(DeadSprites.Count);
        }

        public String getCurrentDate()
        {
            return CurrentDate.ToString();
        }

    }
}
