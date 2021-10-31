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

        /*
       void deleteStation()
       {
       }
       UpDateStation(){}*/

        public void AddStation(Station s)
        {
            stations.Add(s);
        }

        public void AddCustomer(Customer c)
        {
            customers.Add(c);
        }
        public static void AddDrone(Drone d)
        {
            drones.Add(d);
        }
        public void AddParcel(Parcel p)
        {
            parcels.Add(p);
        }
        public static void AssignParcelToDrone(int parcelId, int droneId)
        {

            // drones.[drones.Count - 1].Status = (DroneStatuses)1;
            //parcels.[parcels.Count - 1].DroneId = drones.[drones.Count - 1].Id;
            //parcels.[parcels.Count - 1].Delivered = DateTime.Now;
            foreach (Drone drone in drones)
            {
                if (drone.Id == droneId)
                {
                    drones.Remove(drone);
                    break;
                }
            }
            drones.Add(new Drone() {
            Id:
            });


            Drone d = new();
            d.Id = int.Parse(Console.ReadLine());
            d.Model = Console.ReadLine();
            WeightCategories.TryParse(Console.ReadLine(), out ans);
            d.MaxWeight = (WeightCategories)ans;
            WeightCategories.TryParse(Console.ReadLine(), out ans);
            d.Status = (DroneStatuses)ans;
            d.Battery = 100;
            d1.AddDrone(d);


            foreach (Parcel package in parcels)
            {
                if (package.Id == parcelId)
                {
                    
                }
            }

        }  
        public static void CollectingParcelByDrone(Parcel parcel, Drone drone)
        {
            foreach (Parcel package in parcels)
            {

            }
            drone.Status = ;
            parcel.DroneId = drone.Id;
            parcel.Delivered = DateTime.Now;

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
    }
}
