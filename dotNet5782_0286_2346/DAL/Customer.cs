﻿using System;
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
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return @$"Customer
                          Id:       {Id,15}
                          Name:     {Name,30} 
                          Phone:    {Phone,15}
                          Longitude:{Longitude,15}
                          Latitude: {Latitude,15}";

            }
            public string Distunce(int longitude2, int latitude2)
            {
                return @$"The distunce is:{Math.Sqrt(Math.Pow(Longitude-longitude2,2))+ (Math.Pow(Latitude - latitude2,2))}";

            }

        }
    }
}
