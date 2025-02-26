using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    interface IVirus
    {
        string Code { get; }
        bool Reinfection { get; }
        float Lethality { get; }
        float Infection { get; }
        void Infect(Person person);
        bool Death(Person person);
    }
}
