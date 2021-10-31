
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
            internal static int CodeOfParcel = 0;
        }

        internal static List<Drone>drones = new List<Drone>();
        internal static List<Station>stations = new List<Station>();
        internal static List<Customer>customers = new List<Customer>();
        internal static List<Parcel> parcels = new List<Parcel>();

        static Random rand = new Random(DateTime.Now.Millisecond);
        //current time
        private static void createDrones(int num)
        {
            for (int i = 0; i < num; i++)
            {
                drones.Add( new Drone()
                {
                    Id = rand.Next(100, 200),
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Status = (DroneStatuses)rand.Next(3),
                    Battery=rand.NextDouble()*100
                });
            }
        }

        private static void createStations(int num)
        {

            string[] addresses = new string[] { "DegelReuven7Haifa", "Hatikva6Jerusalem", "Jabotinsky174PetachTikwa", "Yaalom18BeerSheva" };
            for (int i = 0; i < num; i++)
            {
                stations.Add ( new Station()
                {
                    Id = rand.Next(1, 100),
                    Name =addresses[i],
                    ChargeSlots = rand.Next(3, 10),//need checking!!, supposed to be randomal?
                    Longitude = rand.NextDouble() * (33.5 - 29.3) + 29.3,
                    Latitude = rand.NextDouble() * (36.3 - 33.7) + 33.7
                 });
            }
        }

        private static void createCustomers(int num)
        {
            string[] names = new string[] { "Brurya", "Ron", "Shmulik", "Tzuki", "Mahmud","Dorit", "Greg", "CafeNeheman", "BurgersBar", "Avrum", "Shoshana", "Gili"," Rivki" };
            for (int i = 0; i < num; i++)
            {
                customers[i] = new Customer()
                {
                    Id = rand.Next(100000000, 999999999),
                    Name=names[i],
                    Phone = string.Format($"0{ 0 }",rand.Next(510000000, 589999999)),
                    Longitude = rand.NextDouble() * (33.5 - 29.3) + 29.3,
                    Latitude = rand.NextDouble() * (36.3 - 33.7) + 33.7
                };
            }
        }
        private static void createParcels(int num)
        {
            for(int i=0;i<num;i++)
            {
                parcels[i] = new Parcel()
                {
                    Id = Config.CodeOfParcel++,
                    SenderId = rand.Next(100000000, 999999999),//someone tells me it sould look like that
                    TargetId = rand.Next(100000000, 999999999),
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested = DateTime.Now,
                    DroneId = 0
                    /*maybe there are missing fields*/
                };
            }
        }
        public static void Initialize()
        {
            createDrones(rand.Next(5, 8));
            createStations(rand.Next(2,4));
            createCustomers(rand.Next(10, 14));
            createParcels(rand.Next(10, 100));
        }
       
    }
}
