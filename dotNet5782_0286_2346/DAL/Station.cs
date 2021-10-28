using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int ChargeSlots { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return @$"Station
                          Id:{Id}
                          Name:{Name} 
                          ChargeSlots:{ChargeSlots}
                          Longitude:{Longitude}
                          Latitude:{Latitude}";

            }


            public static void AddStation(Station[] stations)
            {
                Console.WriteLine("Enter id, name, chargeSlots, Longitude and  Latitude" +
                    " of the station (Do enter after each one of them)");
                stations[stations.Length-1] = new Station()
                {
                    
                    Id = int.Parse(Console.ReadLine()),
                    Name = Console.ReadLine(),
                    ChargeSlots = int.Parse(Console.ReadLine()),
                    Longitude = int.Parse(Console.ReadLine()),
                    Latitude = int.Parse(Console.ReadLine())
                };
                DalObject.DataSource.Config.cntStation++;

            }

        }
    }
}
