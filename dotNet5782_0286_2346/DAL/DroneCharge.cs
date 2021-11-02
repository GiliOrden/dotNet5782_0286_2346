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
            public int DroneId { get; set; }//property
            public int StationId { get; set; }//property
            public override string ToString()//Print all the fields (Override the Object's 'ToString()')
            {
                return @$"DroneCharge
                          Droneld:  {DroneId}
                          Stationld:{StationId}";

            }
        }
    }
}
