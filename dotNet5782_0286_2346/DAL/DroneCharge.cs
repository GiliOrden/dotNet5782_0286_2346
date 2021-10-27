using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        class DroneCharge
        {
            public int Proneld { get; set; }
            public int Stationld { get; set; }
            public override string ToString()
            {
                return @$"DroneCharge
                          Proneld:{Proneld}
                          Stationld:{Stationld}";

            }
        }
    }
}
