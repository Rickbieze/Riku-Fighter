using Riku_fighter.Race;
using Riku_fighter.State;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        }
                
        public void RunSimulator()
        {
            CreateHumanity();
            Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!");
            CurrentDate = CurrentDate.AddYears(30);
            List<Person> newHumanity = new List<Person>();

            foreach (var human in Humanity)
            {
                int age = human.GetAge(CurrentDate);
                var dieProb = new Probability().GetRandomDouble();
                var accProb = new Probability().GetRandomDouble();
                Debug.WriteLine(human.FirstName + " " + human.LastName);
                //Console.WriteLine("Die Probability: " + dieProb + "-- Accident Probability: " + accProb);
                if (dieProb > human.GetDieProb() && human.Mother != null && human.Father != null || accProb > ACCIDENT_PROB && human.Mother != null && human.Father != null)
                {
                    human.State = new Deceased(CurrentDate);
                }

                if (human.State.GetType() == typeof(Healthy))
                {
                    human.State.GetAliveState();
                    if (new Probability().GetRandomDouble() > human.State.CurrStatus.DISEASE_PROB)
                    {
                        human.State = new Sick();
                    }
                }
                else if(human.State.GetType() == typeof(Sick))
                {
                    if (new Probability().GetRandomDouble() > human.State.CurrStatus.CURE_PROB)
                    {
                        human.State = new Healthy();
                    }
                }              

                //Gives person a Religion
                

                //Checks if person is alive or not
                if (human.State.GetType() != typeof(Deceased))
                {
                    //every year a persons die probability will increase with 0.005
                    human.IncDieProb(0.005);
                    if (human.Partner != null)
                    {
                        //break up
                        if (new Probability().GetRandomDouble() > BREAKUP_PROB && human.Father != null && human.Mother != null)
                        {
                            human.Partner.Partner = null;
                            human.Partner = null;
                        }

                        if(human.Partner != null)
                        {
                            //todo: probability
                            if (new Probability().GetRandomDouble() < PREGNANT_PROB)
                            {
                                if (human.GetAge(CurrentDate) > 18 && human.Partner.Gender != human.Gender && human.Mother != null && human.Father != null && human.Partner.State.GetType() != typeof(Deceased))
                                {
                                    //Needs to add child name and random gender
                                    Person Child = human.MakeBaby(CurrentDate);
                                    TempHumanity.Add(Child);
                                }
                            }
                        }                        
                    }
                    else
                    {
                        Probability partnerProb = new Probability(10);
                        if (PARTNER_PROB * partnerProb.rInt > 3.5 && human.Mother != null && human.Father != null && human.Age > 16)
                        {
                            foreach (var newpartner in Humanity)
                            {
                                //Checks if person if alive or not
                                if (newpartner.State.GetType() != typeof(Deceased) && newpartner.Age > 16)
                                {
                                    int ageDifference = Math.Abs(human.GetAge(CurrentDate) - newpartner.GetAge(CurrentDate));
                                    if (newpartner.Father != human.Father || human.Mother != newpartner.Mother)
                                    {
                                        if (newpartner != human && ageDifference < 15 && newpartner.Partner == null)
                                        {
                                            newpartner.Partner = human;
                                            human.Partner = newpartner;
                                        }
                                    }
                                }                                
                            }
                        }
                    }
                }
                //Adds religion
                if (new Probability().GetRandomDouble() > RELIGION_PROB && human.GetAge(CurrentDate) > 5)
                {
                    Probability prob = new Probability(5);
                    switch (prob.rInt)
                    {
                        case 0:
                            human.Religion = "Atheist";
                            break;
                        case 1:
                            human.Religion = "Buddhism";
                            break;
                        case 2:
                            human.Religion = "Christianity";
                            break;
                        case 3:
                            human.Religion = "Hinduism";
                            break;
                        case 4:
                            human.Religion = "Islam";
                            break;
                    }
                }
                newHumanity.Add(human);
            }
            foreach (var temphuman in TempHumanity)
            {
                newHumanity.Add(temphuman);
            }
            TempHumanity.Clear();
            Humanity = newHumanity;
        }
    }
}
