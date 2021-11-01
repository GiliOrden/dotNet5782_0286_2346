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

        public void AddStation(Station s)
        {
            stations.Add(s);
        }

        public void AddCustomer(Customer c)
        {
            customers.Add(c);
        }
        public  void AddDrone(Drone d)
        {
            drones.Add(d);
        }
        public void AddParcel(Parcel p)
        {
            parcels.Add(p);
        }
        public  void AssignParcelToDrone(int parcelId, int droneId)
        { 
            foreach (Drone drone in drones)
            {
                if (drone.Id == droneId)
                {
                    Drone d = drone;
                    d.Status = DroneStatuses.Delivery;
                    drones.Remove(drone);
                    break;
                }
            }

            foreach (Parcel parcel in parcels)
            {
                if (parcel.Id == parcelId)
                {
                    Parcel p = parcel;
                    p.DroneId = drones[drones.Count - 1].Id;
                    p.Scheduled = DateTime.Now;
                    parcels.Remove(parcel);
                    break;
                }
            }
        }  
        public  void CollectParcelByDrone(int id)
        {
            Parcel p;
            foreach (Parcel parcel in parcels)
            {
              if(parcel.Id==id)
                {
                    p = parcel;
                    p.PickedUp = DateTime.Now;
                    parcels.Add(p);
                    parcels.Remove(parcel);
                    break;
                }
            }
            //i think we should do something to the baterry but i dont no where
        }
        public void SupplyDeliveryToCustomer(int id)
        {
            Parcel p;
            foreach(Parcel parcel in parcels)
            {
                if(parcel.Id==id)
                {
                    p = parcel;
                    p.Delivered = DateTime.Now;
                    foreach (Drone drone in drones)
                    {
                        if (drone.Id == p.DroneId)
                        {
                            Drone d = drone;
                            d.Battery = 20;
                            drones.Add(d);
                            drones.Remove(drone);
                            break;
                        }
                    }
                    parcels.Remove(parcel);
                    break;
                }
            }
        }
        public void SendDroneToCharge(int id ,int id2)
        {
            Drone d;
            Station s;
            DroneCharge dc = new DroneCharge();//question:is it should look like that?
            foreach(Drone drone in drones)
            {
                if(drone.Id==id)
                {
                    d = drone;
                    d.Status = DroneStatuses.Maintenance;
                    dc.DroneId = d.Id;
                    dc.StationId = id2;
                    drones.Add(d);
                    drones.Remove(drone);
                    break;
                }
            }
            foreach(Station station in stations)
            {
                if(station.Id==id2)
                {
                    s = station;
                    s.ChargeSlots = s.ChargeSlots - 1;
                    stations.Add(s);
                    stations.Remove(station);
                    break;
                }
            }
        }
        public void ReleaseDroneFromCharge(int id)
        {
            Drone d;
            foreach(Drone drone in drones)
            {
                if(drone.Id==id)
                {
                    d = drone;
                    d.Battery = 100;
                    d.Status = DroneStatuses.Available;
                    drones.Add(d);
                    drones.Remove(drone);
                    break;
                }
            }
        }
        public void DisplayBaseStation(int id)
        {
            foreach (Station baseStaion in stations)
            {
                if (baseStaion.Id == id)
                {
                    baseStaion.ToString();
                    break;
                }
            }
        }
        public void DisplayDrone(int id)
        {
            foreach(Drone drone in drones)
            {
                if(drone.Id==id)
                {
                    drone.ToString();
                    break;
                }
            }
        }
        public void DisplayCustomer(int id)
        {
            foreach(Customer customer in  customers)
            {
                if(customer.Id==id)
                {
                    customer.ToString();
                    break;
                }
            }
        }
        public void DisplayParcel(int id)
        {
            foreach(Parcel parcel in parcels)
            {
                if(parcel.Id==id)
                {
                    parcel.ToString();
                    break;
                }
            }
        }

        public void ListOfBaseStations()
        {
            foreach (Station allStaions in stations)
                allStaions.ToString();
        }

        public void ListOfDrones()
        {
            foreach (Drone allDrones in drones)
                allDrones.ToString();
        }

        public void ListOfCustomers()
        {
            foreach (Customer customer in customers)
                customer.ToString();
        }

        public void ListOfParcels()
        {
            foreach (Parcel allParcel in parcels)
                allParcel.ToString();
        }

        public void ListOfNotAssociatedParsels()
        {
            foreach (Parcel parcel in parcels)
            {
                if (parcel.DroneId == 0)
                    parcel.ToString();
            }
        }

        public void ListOfAvailableChargingStations()
        {
            foreach (Station baseStaion in stations)
            {
                foreach(DroneCharge droneCharge in  )
                {
                    if (baseStaion.Id == droneCharge.StationId)
                    {
                        baseStaion.ToString();

                    }
                }
            }
        }
    }
}
