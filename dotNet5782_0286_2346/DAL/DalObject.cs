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

        public void AddStation(Station s)
        {
            stations.Add(s);
        }

        public  void AddCustomer()
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

        public void AddDrone()
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
        public void AddParcel()
        {
            int ans;
            Console.WriteLine("Enter ID, senderId, targetId, weight and priority of your parcel(Press enter after each one of them)");
            Parcel p = new Parcel();
            p.Id = int.Parse(Console.ReadLine());
            p.SenderId = int.Parse(Console.ReadLine());
            p.TargetId = int.Parse(Console.ReadLine());
            WeightCategories.TryParse(Console.ReadLine(),out ans);
            p.Weight = (WeightCategories)ans;
            Priorities.TryParse(Console.ReadLine(), out ans);
            p.Priority = (Priorities)ans;
            p.DroneId = 0;
            p.Requested = DateTime.Now;
            parcels.Add(p);
        }
        public void AssignParcelToDrone()
        {
            Console.WriteLine();
        }
    }
}
