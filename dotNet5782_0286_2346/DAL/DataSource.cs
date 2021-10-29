
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DataSource
    {
         internal class Config
        {
            internal static int CntDrone = 0;
            internal static int CntStation = 0;
            internal static int CntCustomer = 0;
            internal static int CntDroneCharge = 0;
            internal static int CntParcels = 0;
            internal static int CodeOfParcel=0;
        }

        internal static Drone[] drones = new Drone[10];
        internal static Station[] stations = new Station[5];
        internal static Customer[] customers = new Customer[100];
        internal static Parcel[] parcels = new Parcel[1000];

        static Random rand = new Random(DateTime.Now.Millisecond);
        //current time
        private static void createDrones(int num)
        {
            for (int i = 0; i < num; i++)
            {
                drones[i] = new Drone()
                {
                    Id = rand.Next(100, 200),
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Status = (DroneStatuses)rand.Next(3)
                };
                Config.CntDrone++;
            }
        }

        private static void createStations(int num)
        {
            for (int i = 0; i < num; i++)
            {
                stations[i] = new Station()
                {
                    Id = rand.Next(1, 100),
                    Name = string.Format($"{(Addresses)rand.Next(8)} {i}"),
                    ChargeSlots = rand.Next(3, 10),//need checking!!, supposed to be randomal?
                    Longitude = rand.NextDouble() * (33.5 - 29.3) + 29.3,
                    Latitude = rand.NextDouble() * (36.3 - 33.7) + 33.7
                 };
                Config.CntStation++;
            }
        }

        private static void createCustomers(int num)
        {
            for (int i = 0; i < num; i++)
            {
                customers[i] = new Customer()
                {
                    Id = rand.Next(100000000, 999999999),
                    Name=string.Format($"{(NamesOfCustomers)rand.Next(13)}"),// 
                    Phone = string.Format($"0{ 0 }",rand.Next(510000000, 589999999)),
                    Longitude = rand.NextDouble() * (33.5 - 29.3) + 29.3,
                    Latitude = rand.NextDouble() * (36.3 - 33.7) + 33.7
                };
                Config.CntCustomer++;
            }
        }
        private static void createParcels(int num)
        {
            for(int i=0;i<num;i++)
            {
                parcels[i] = new Parcel()
                {
                    Id = Config.CodeOfParcel++,
                    SenderId = customers[rand.Next(customers.Length)].Id,//i'm not sure it sould look like that
                    TargetId = customers[rand.Next(customers.Length)].Id,
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested = DateTime.Now,
                    DroneId = 0,//i understood from whatapp that we should do it like that 
                    /*Scheduled=//maybe we have to assume how long it will take for the package according to
                     * the distance between the sender and the destination but in the instructions it not look like that
                    PickedUp=
                    Delivered*/
                };
                Config.CntParcels++;
            }
        }
        public static void Initialize()
        {
            createDrones(rand.Next(5, 8));
            createStations(rand.Next(2, 4));
            createCustomers(rand.Next(10, 13));
            createParcels(rand.Next(10, 100));
        }
       
    }
}
