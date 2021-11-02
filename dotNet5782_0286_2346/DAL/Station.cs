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
            public int Id { get; set; }//property
            public string Name { get; set; }//property
            public int ChargeSlots { get; set; }//property
            public double Longitude { get; set; }//property
            public double Latitude { get; set; }//property
            public override string ToString()//Print all the fields (Override the Object's 'ToString()')
            {
                return @$"Station
                          Id:         {Id,-15}
                          Name:       {Name,-30} 
                          ChargeSlots:{ChargeSlots,-15}
                          Longitude:  {Longitude,-15}
                          Latitude:   {Latitude,-15}";

            }
            public string Distunce(int longitude2, int latitude2)//Print the distunce between the customer and other location
            {
                return $"The distunce is:{Math.Sqrt(Math.Pow(Longitude - longitude2, 2)) + (Math.Pow(Latitude - latitude2, 2))}";
            }

           
            
        }
    }
}
