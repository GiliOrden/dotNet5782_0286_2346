using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DalObject.DataSource;

namespace DalObject
{
   public class DalObject
    {
        public DalObject()//ctor
        {
          //  DataSource.Initialize();//actually produce the data base 
        }
       public void AddDrone(int Id)
       {

       }
        /*
       void deleteStation()
       {
       }
       UpDateStation(){}*/

        public static void AddStation(Station[] stations)
        {
            Console.WriteLine("Enter id, name, chargeSlots, Longitude and  Latitude" +
                " of the station (Do enter after each one of them)");
            stations[stations.Length - 1] = new Station()
            {

                Id = int.Parse(Console.ReadLine()),
                Name = Console.ReadLine(),
                ChargeSlots = int.Parse(Console.ReadLine()),
                Longitude = int.Parse(Console.ReadLine()),
                Latitude = int.Parse(Console.ReadLine())
            };
            Config.cntStation++;

        }

    }
}
