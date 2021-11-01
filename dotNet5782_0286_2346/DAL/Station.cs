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
                          Id:         {Id,15}
                          Name:       {Name,30} 
                          ChargeSlots:{ChargeSlots,15}
                          Longitude:  {Longitude,15}
                          Latitude:   {Latitude,15}";

            }

           
        }
    }
}
