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
        /// <summary>
        /// constructor, produce the data base
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// Adding station element to the stations list
        /// </summary>
        /// <param name="s">  element ,Station tipe, we adding the list</param>
        public static void AddStation(Station s)
        {
            stations.Add(s);
        }
        /// <summary>
        /// Adding customer element to the customers list
        /// </summary>
        /// <param name="c">element ,Customer tipe, we adding the list</param>
        public static void AddCustomer(Customer c)
        {
            customers.Add(c);
        }
        /// <summary>
        /// Adding drone element to the drones list
        /// </summary>
        /// <param name="d">element ,Drone tipe, we adding the list</param>
        public static void AddDrone(Drone d)
        {
            drones.Add(d);
        }

        /// <summary>
        /// Adding customer element to the customers list
        /// </summary>
        /// <param name="p">element ,Parcel tipe, we adding the list</param>
        public static void AddParcel(Parcel p)
        {
            parcels.Add(p);
        }
        /// <summary>
        /// updating of assign parcel to the drone which will deliver it
        /// </summary>
        /// <param name="parcelId">the id of parcel</param>
        /// <param name="droneId">the id of drone</param>
        public static void AssignParcelToDrone(int parcelId, int droneId)
        { 
            foreach (Drone drone in drones)
            {
                if (drone.Id == droneId)
                {
                    Drone d = drone;
                    d.Status = DroneStatuses.Delivery;
                    drones.Add(d);
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
                    parcels.Add(p);
                    parcels.Remove(parcel);
                    break;
                }
            }
        }
        /// <summary>
        /// updating of collection parcel  by drone
        /// </summary>
        /// <param name="id">the id of parcel</param>
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
            
        }
        /// <summary>
        /// updating of supplying delivery to customer
        /// </summary>
        /// <param name="id">the id of parcel</param>
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
                    parcels.Add(p);
                    parcels.Remove(parcel);
                    break;
                }
            }
        }
        /// <summary>
        /// updating of sending drone to staion for charging
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <param name="id2">the station id</param>
        public static void SendDroneToCharge(int id ,int id2)
        {
            Drone d;
            Station s;
            DroneCharge dc = new DroneCharge();
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
                droneCharges.Add(dc);
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
        /// <summary>
        /// updating of releasing drone from Charging station
        /// </summary>
        /// <param name="id">the drone id</param>
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
            foreach(DroneCharge charger in droneCharges)
            {
                Station s;
                if(charger.DroneId==id)
                {
                    foreach(Station station in stations)
                    {
                        if(charger.StationId==station.Id)
                        {
                            s = station;
                            s.ChargeSlots++;
                            stations.Add(s);
                            stations.Remove(station);
                            break;
                        }
                    }
                }
                droneCharges.Remove(charger);
            }
        }
        /// <summary>
        /// this function returns a specific station from the list
        /// </summary>
        /// <param name="id">the station id</param>
        /// <returns>Station element</returns>
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
        /// <summary>
        ///  this function returns a specific drone from the list
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <returns>Drone element</returns>
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
        /// <summary>
        ///  this function returns a specific customer from the list
        /// </summary>
        /// <param name="id">the customer id</param>
        /// <returns>Customer element</returns>
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
        /// <summary>
        ///  this function returns a specific parcel from the list 
        /// </summary>
        /// <param name="id">the parcel id</param>
        /// <returns>Parcel element</returns>
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
        /// <summary>
        ///  this function returns list of stations 
        /// </summary>
        /// <returns>list of stations </returns>
        public static List<Station> ListOfBaseStations()
        {
            List<Station> s = stations;
           return stations;
        }
        /// <summary>
        /// this function returns list of drone
        /// </summary>
        /// <returns></returns>
        public static List<Drone> ListOfDrones()
        {
            List<Drone> d = drones;
            return d;
        }
        /// <summary>
        ///  this function returns list of customers
        /// </summary>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers()
        {
            List<Customer> c = customers;
            return c;
        }
        /// <summary>
        /// this function returns list of parcels
        /// </summary>
        /// <returns></returns>
        public static List<Parcel> ListOfParcels()
        {
            List<Parcel> p = parcels;
            return p;
        }
        /// <summary>
        /// this function returns list of all the parsels which aren't associated to drones
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// this function returns list of all the available charging stations
        /// </summary>
        /// <returns></returns>
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
       /// <summary>
       /// returns customer element which the name is his 
       /// </summary>
       /// <param name="name">name of customer</param>
       /// <returns></returns>
        public static Customer DistanceFromCustomer(string name)
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
        /// <summary>
        ///  returns station element whose the id is its 
        /// </summary>
        /// <param name="id">id of station</param>
        /// <returns></returns>
        public static Station DistanceFromStation(int id)
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
