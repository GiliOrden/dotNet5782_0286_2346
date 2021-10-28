
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
            internal static int cntDrone = 0;
            internal static int cntStation = 0;
            internal static int cntCustomer = 0;
            internal static int cntDroneCharge = 0;
            internal static int cntParcels = 0;
            internal static int codeOfParcel=0;
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
                Config.cntDrone++;
            }
        }

        private static void createStations(int num)
        {
            for (int i = 0; i < num; i++)
            {
                stations[i] = new Station()
                {
                    Id = rand.Next(1, 100),
                    Name = string.Format($"{(Streets)rand.Next(8)} {i}"),
                    ChargeSlots = rand.Next(3, 10),//need checking!!, supposed to be randomal?
                    Longitude = rand.NextDouble() * (33.5 - 29.3) + 29.3,
                    Latitude = rand.NextDouble() * (36.3 - 33.7) + 33.7
                 };
                Config.cntStation++;
            }
        }

        private static void createCustomers(int num)
        {
            for (int i = 0; i < num; i++)
            {
                customers[i] = new Customer()
                {
                    Id = rand.Next(100, 1000),
                    Name=string.Format($"{(Names)rand.Next(12)}"),// 
                    Phone = string.Format($"0{ 0 }",rand.Next(510000000, 529999999)),
                    Longitude = rand.NextDouble() * (33.5 - 29.3) + 29.3,
                    Latitude = rand.NextDouble() * (36.3 - 33.7) + 33.7
                };
                Config.cntCustomer++;
            }
        }

        public static void Initialize ()
        {
            createDrones(rand.Next(5, 8));
        }
       
    }
}
