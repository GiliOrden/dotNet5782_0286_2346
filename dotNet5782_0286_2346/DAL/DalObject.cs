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

        }

        public  void AddCustomer(List<Customer> customers)
        {
            Console.WriteLine("Enter ID, name, Phone, Longitude and  Latitude" +
                " of the customer (Press enter after each one of them)");
            customers.Add(new Customer()
            {
                Id = int.Parse(Console.ReadLine()),
                Name = Console.ReadLine(),
                Phone = Console.ReadLine(),
                Longitude = double.Parse(Console.ReadLine()),
                Latitude = double.Parse(Console.ReadLine()),
            });

        }

        public void AddDrone(List<Drone> drones)
        {
            Console.WriteLine("Enter ID, model and maximum weight(Press enter after each one of them)");
            Drone d= new();
            d.Id= int.Parse(Console.ReadLine());
            d.Model = Console.ReadLine();
            int ans;
            WeightCategories.TryParse(Console.ReadLine(),out ans);
            d.MaxWeight =(WeightCategories)ans;
            WeightCategories.TryParse(Console.ReadLine(), out ans);
            d.Status =(DroneStatuses)ans;
            drones.Add(d);
        }
    }
}
