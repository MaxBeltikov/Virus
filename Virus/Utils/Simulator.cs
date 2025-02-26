using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus.Utils
{
    class Simulator
    {
        private const double _mortality = (double)14 / 1000;
        private const double _birthrate = (double)8 / 1000;

        private static Random _random = new Random();
        private List<Person> _alive;
        private List<Person> _dead;
        private int _maxDays;
        private int _day;
        private Vir _virus;
        private int _illed;
        private int _recovered;

        public int Days => _day;
        public Simulator(int countPopulation, int maxDays, Vir virus)
        {
            _day = 1;
            _maxDays = maxDays;
            _alive = new List<Person>();
            _dead = new List<Person>();
            _virus = virus;
            Population(countPopulation);
        }
        public int TotalPopulation => _alive.Count;

        public int DaedPopulation => _dead.Count;
        public int Illed => _illed;
        public int Recovered => _recovered;
        public void RunSimmulation()
        {
            StartInfection();
            for (int i = 0; i < _maxDays; i++)
            {
                _day = i;
                _alive.RemoveAll((p) =>
                {
                    if (p.IsAlive)
                    {
                        _dead.Add(p);
                        return true;
                    }

                    return false;
                });

                if (i % 365 == 0)
                {
                    _alive.RemoveAll((p) =>
                    {
                        p.UpdateAge();
                        if (p.Age >= p.MaxAge)
                        {
                            _dead.Add(p);
                            return true;
                        }
                        return false;
                    });

                }

                StartInfection();
                Mortaliti();
                Birth();
            }
        }
        public int InfectedPopulation() => _alive.FindAll((p) => p.Status).Count;
        private void Mortaliti()
        {
            int range = (int)Math.Round(_alive.Count * _mortality / 365);
            List<Person> a = _alive.GetRange(0, range);
            _alive.RemoveRange(0, (int)Math.Round(_alive.Count * _mortality));
            for (int i = 0; i < a.Count; i++)
            {
                _dead.Add(a[i]);
            }
        }
        private void Birth()
        {
            int range = (int)Math.Round(_alive.Count * _mortality / 365);
            for (int i = 0; i < range; i++)
            {
                Person person = new Person(
                    _random.Next(0, 2) == 0 ? "Male" : "Famale", 0,
                    _random.Next(65, 76) / 100);
                _alive.Add(person);
            }
        }
        private void StartInfection()
        {
            for (int i = 0; i < Math.Round(_alive.Count * 0.02); i++)
            {
                _alive.Find((p) => (p.Age >= _virus.AgeToInfect) && (!p.Status)).Status = true;
            }
            _alive = _alive.OrderBy(_ => _random.Next()).ToList();
        }
        private void Infection()
        {
            var allInfected = _alive.FindAll((p) => p.Status);
            Person[] copy = new Person[allInfected.Count];
            allInfected.CopyTo(copy, 0);
            foreach (Person p in copy)
            {
                if (_virus.Death(p)) continue;
                if (p.UpdateInfection() == _virus.DayToRecover)
                {
                    if (!_virus.Reinfection)
                        p.CreateTotalImmunity();
                    p.Recover();
                    _recovered++;
                    continue;
                }
                if (_random.Next(101) <= 28) continue;

                for (int i = 0; i < p.Friends / 2; i++)
                {
                    Person meeting = _alive[_random.Next(0, _alive.Count)];
                    if (!meeting.Status && meeting.Age >= _virus.AgeToInfect && !meeting.TotalImmunity)
                    {
                        _virus.Infect(meeting);
                        _illed++;
                    }
                }
            }
        }
        private void Population(int countPopulation)
        {
            string[] gender = new string[2] { "Male", "Famale" };
            int maxAge = 29201;
            double maxImmunity = 0.75;

            for (int i = 0; i < countPopulation; i++)
            {
                Person person = new Person(
                    gender[_random.Next(0, 2)],
                    _random.Next(0, 81),
                    _random.Next(65, 76) / 100);
                if (person.Age >= person.MaxAge)
                    _dead.Add(person);
                else
                    _alive.Add(person);
            }

        }
    }
}
