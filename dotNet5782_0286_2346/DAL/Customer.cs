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
                string longitude, latitude;
                double absValOfDegree = Math.Abs(Longitude);
                double minute = (absValOfDegree - (int)absValOfDegree) * 60;
                longitude = string.Format("{0}°{1}\' {2}\"{3}",(int)Longitude, (int)(minute),Math.Round((minute-(int)minute)*60) ,Longitude < 0 ? "S" : "N");
                absValOfDegree = Math.Abs(Latitude);
                minute= (absValOfDegree - (int)absValOfDegree) * 60;
                latitude= string.Format("{0}°{1}\' {2}\"{3}", (int)Latitude, (int)(minute), Math.Round((minute - (int)minute) * 60), Latitude < 0 ? "W" : "E");
                return @$"Customer
                          Id:       {Id}
                          Name:     {Name} 
                          Phone:    {Phone}
                          Longitude:{longitude}
                          Latitude: {latitude}";

            }
            public void Distunce(int longitude2, int latitude2)
            {
                Console.WriteLine($"The distunce is:{Math.Sqrt(Math.Pow(Longitude - longitude2, 2)) + (Math.Pow(Latitude - latitude2, 2))}");

            }
           

        }
    }
}
