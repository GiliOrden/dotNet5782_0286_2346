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

                string longitude, latitude;
                double absValOfDegree = Math.Abs(Longitude);
                double minute = (absValOfDegree - (int)absValOfDegree) * 60;//a formula that converts the decimal value of a coordinate to it sexagesimal value
                longitude = string.Format("{0}° {1}\' {2}\" {3}", (int)Longitude, (int)(minute), (int)Math.Round((minute - (int)minute) * 60), Longitude < 0 ? "S" : "N");
                absValOfDegree = Math.Abs(Latitude);
                minute = (absValOfDegree - (int)absValOfDegree) * 60;
                latitude = string.Format("{0}° {1}\' {2}\" {3}", (int)Latitude, (int)(minute), (int)Math.Round((minute - (int)minute) * 60), Latitude < 0 ? "W" : "E");
                return @$"Station
                          Id:         {Id,-15}
                          Name:       {Name,-30} 
                          ChargeSlots:{ChargeSlots,-15}
                          Longitude:  {longitude,-15}
                          Latitude:   {latitude,-15}";

            }
        }
    }
}
