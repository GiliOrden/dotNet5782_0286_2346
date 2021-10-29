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
           DataSource.Initialize();//actually produce the data base 
        }
       public void AddDrone(int Id)
       {

       }
        /*
       void deleteStation()
       {
       }
       UpDateStation(){}*/

        public void AddStation(Station[] stations)
        {
            Console.WriteLine("Enter ID, name, chargeSlots, Longitude and  Latitude" + 
                " of the station (Press enter after each one of them)");
            stations[stations.Length] = new Station()
            {

                Id = int.Parse(Console.ReadLine()),
                Name = Console.ReadLine(),
                ChargeSlots = int.Parse(Console.ReadLine()),
                Longitude = double.Parse(Console.ReadLine()),
                Latitude = double.Parse(Console.ReadLine())
            };
            Config.CntStation++;

        }

        public  void AddCustomer(Customer[] customers)
        {
            Console.WriteLine("Enter ID, name, Phone, Longitude and  Latitude" +
                " of the customer (Press enter after each one of them)");
            customers[customers.Length] = new Customer()
            {
                Id = int.Parse(Console.ReadLine()),
                Name = Console.ReadLine(),
                Phone = Console.ReadLine(),
                Longitude = double.Parse(Console.ReadLine()),
                Latitude = double.Parse(Console.ReadLine()),
            };
            Config.CntCustomer++;

        }

        public void AddDrone(Drone[]drones)
        {
            Console.WriteLine("Enter ID, model and maximum weight-0 for light,1 for medium,2 for heavy(Press enter after each one of them)");
            drones[drones.Length] = new Drone()
            {
                Id = int.Parse(Console.ReadLine()),
                Model=Console.ReadLine(),

                MaxWeight=(WeightCategories)int.Parse(Console.ReadLine())
            };
        }


    }
}
