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
       
        /*
       void deleteStation()
       {
       }
       UpDateStation(){}*/

        public void AddStation(Station s)
        {
            stations.Add(s);
        }

        public  void AddCustomer(Customer c)
        {
            customers.Add(c);
        }
        public void AddDrone(Drone d)
        {
            drones.Add(d);
        }
        public void AddParcel(Parcel p)
        {
            parcels.Add(p);
        }
        public void AssignParcelToDrone(Parcel parcel, Drone drone)
        {
            drone.Status = ;
            parcel.DroneId = drone.Id;
            parcel.Delivered = DateTime.Now;

        }
    }
}
