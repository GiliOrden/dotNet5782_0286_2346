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
                          Id:         {Id,-15}
                          Name:       {Name,-30} 
                          ChargeSlots:{ChargeSlots,-15}
                          Longitude:  {Longitude,-15}
                          Latitude:   {Latitude,-15}";

            }
            public void Distunce(int longitude2, int latitude2)
            {
                Console.WriteLine($"The distunce is:{Math.Sqrt(Math.Pow(Longitude - longitude2, 2)) + (Math.Pow(Latitude - latitude2, 2))}");
            }

            public void Print2()
            {
                Console.WriteLine($"          {Id,-14}{Name,-27}{ChargeSlots,-15}{Longitude,-25}{Latitude,-25}");


            }
            
        }
    }
}
