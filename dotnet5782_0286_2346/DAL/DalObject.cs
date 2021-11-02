﻿using IDAL.DO;
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

        public static void AddStation(Station s)
        {
            stations.Add(s);
        }

        public static void AddCustomer(Customer c)
        {
            customers.Add(c);
        }
        public static void AddDrone(Drone d)
        {
            drones.Add(d);
        }
        public static void AddParcel(Parcel p)
        {
            parcels.Add(p);
        }
        public static void AssignParcelToDrone(int parcelId, int droneId)
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
        public static void CollectParcelByDrone(int id)
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
        public static void SupplyDeliveryToCustomer(int id)
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
        public static void SendDroneToCharge(int id ,int id2)
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
        public static void ReleaseDroneFromCharge(int id)
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
        public static Station DisplayBaseStation(int id)
        {
            Station s = new Station();
            foreach (Station baseStaion in stations)
            {
                if (baseStaion.Id == id)
                {
                    return baseStaion;
                }
            }
            return s;
        }
        public static Drone DisplayDrone(int id)
        {
            Drone d = new Drone();
            foreach(Drone drone in drones)
            {
                if(drone.Id==id)
                {
                    return drone;
                }
            }
            return d;
        }
        public static Customer DisplayCustomer(int id)
        {
            Customer c = new Customer();
            foreach(Customer customer in  customers)
            {
                if(customer.Id==id)
                {
                    return customer;
                }
            }
            return c;
        }
        public static Parcel DisplayParcel(int id)
        {
            Parcel p = new Parcel();
            foreach (Parcel parcel in parcels)
            {
                if(parcel.Id==id)
                {
                    return parcel;
                }
            }
            return p;
        }

        public static List<Station> ListOfBaseStations()
        {
           return stations;
        }

        public static List<Drone> ListOfDrones()
        {
            return drones;
        }

        public static List<Customer> ListOfCustomers()
        {
            return customers;
        }

        public static List<Parcel> ListOfParcels()
        {
            return parcels;
        }

        public static List<Parcel> ListOfNotAssociatedParsels()
        {
            List<Parcel> p = new List<Parcel>();
            foreach (Parcel parcel in parcels)
            {
                if (parcel.DroneId == 0)
                    p.Add(parcel);
            }
            return p;
        }

        public static List<Station> ListOfAvailableChargingStations()
        {
            List<Station> s = new List<Station>();
            foreach (Station baseStaion in stations)
            {
                if (baseStaion.ChargeSlots != 0)
                    s.Add(baseStaion);       
            }
            return s;
        }

        public static Customer DistanceFromCustomer(int longitude, int latitude, string name)
        {
            Customer c = new Customer();
            foreach (Customer customer in customers)
            {
                if (customer.Name==name)
                {
                    return customer;
                    
                }

            }
            return c;
        }

        public static Station DistanceFromStation(int longitude, int latitude, int id)
        {
            Station s = new Station();
            foreach (Station station in stations)
            {
            
                if (station.Id==id)
                {
                    return station;
                    
                }
            }
            return s;
        }

        

    }
}
/*DmsLocation Convert(double decimalLongitue ,double decimalLotitude)
{

    return new DmsLocation
        {
            Latitude = new DmsPoint
                {
                    Degrees = ExtractDegrees(decimalLocation.Latitude),
                    Minutes = ExtractMinutes(decimalLocation.Latitude),
                    Seconds = ExtractSeconds(decimalLocation.Latitude),
                    Type = PointType.Lat
                },
            Longitude = new DmsPoint
                {
                    Degrees = ExtractDegrees(decimalLocation.Longitude),
                    Minutes = ExtractMinutes(decimalLocation.Longitude),
                    Seconds = ExtractSeconds(decimalLocation.Longitude),
                    Type = PointType.Lon
                }
        };
}*/