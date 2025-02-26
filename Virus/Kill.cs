using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    internal class Kill: Vir
    {
    private static Random rand = new Random();
    public Kill(string code, bool reinfection, float infection,
        float letality) : base(code, reinfection, infection, letality)
    {
        _letality = letality + (float)rand.Next(-10, 10) / 100;
    }


    public override int AgeToInfect => 12;
    public override int DayToRecover => 7;

    public override bool Death(Person person)
    {
        if (rand.NextDouble() <= Lethality)
        {
            return true;
        }
        return false;
    }

    public override void Infect(Person person)
    {
        if (person.Immunity <= Infection)
        {
            person.Status = true;
        }
    }
}
    }

