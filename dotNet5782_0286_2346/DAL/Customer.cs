using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public int Id { get; set; }//property
            public string Name { get; set; }//property
            public string Phone { get; set; }//property
            public double Longitude { get; set; }//property
            public double Latitude { get; set; }//property
            public override string ToString()//Print all the fields (Override the Object's 'ToString()')
            {
                return @$"Customer
                          Id:       {Id}
                          Name:     {Name} 
                          Phone:    {Phone}
                          Longitude:{Longitude}
                          Latitude: {Latitude}";

            }
            public string Distunce(int longitude2, int latitude2)//Print the distunce between the customer and other location
            {
                return $"The distunce is:{Math.Sqrt(Math.Pow(Longitude - longitude2, 2)) + (Math.Pow(Latitude - latitude2, 2))}";

            }
           

        }
    }
}
