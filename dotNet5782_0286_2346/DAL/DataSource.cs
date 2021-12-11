
using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    internal class DataSource
    {
      
        internal class Config
        {
           internal static int CodeOfParcel =1;
           internal static double EmptyDronePowerConsumption=0.05;
           internal static double LightWeightCarrierPowerConsumption=0.06;
           internal static double MediumWeightCarrierPowerConsumption=0.07;
           internal static double HeavyWeightCarrierPowerConsumption=0.08;
           internal static double ChargingRatePerHour=25;
        }

        internal static List<Drone>drones = new List<Drone>();//creat list of drones
        internal static List<Station>stations = new List<Station>();//creat list of stations
        internal static List<Customer>customers = new List<Customer>();//creat list of customers
        internal static List<Parcel> parcels = new List<Parcel>();//creat list of parcels
        internal static List<DroneCharge> droneCharges = new List<DroneCharge>();
        /// <summary>
        /// current time
        /// </summary>
        static Random rand = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// creates randomal drones
        /// </summary>
        /// <param name="num">number of drones to add</param>
        private static void createDrones(int num)
        {
            
            for (int i = 1; i <= 5; i++)
            {
                drones.Add(new Drone()
                {
                    Id =i,
                    MaxWeight =WeightCategories.Heavy,
                    Model = string.Format("Drony{0}", i)
                });               
            }
            for (int i =6; i <=num; i++)
            {
                drones.Add(new Drone()
                {
                    Id = i,
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Model = string.Format("Drony{0}", i)
                });
            }
        }

        /// <summary>
        /// creates randomal stations
        /// </summary>
        /// <param name="num">number of stations to add</param>
        private static void CreateStations(int num)
        {

            string[] addresses = new string[] { "DegelReuven7Haifa", "Hatikva6Jerusalem", "Jabotinsky174PetachTikwa", "Yaalom18BeerSheva" };
            for (int i = 0; i < num; i++)
            {
                stations.Add ( new Station()
                {
                    Id = rand.Next(1, 100),
                    Name =addresses[i],
                    ChargeSlots = rand.Next(2,5),
                    Longitude = rand.NextDouble() * (33.5 - 29.3) + 29.3,
                    Latitude = rand.NextDouble() * (36.3 - 33.7) + 33.7
                 });               
            }
        }
        /// <summary>
        ///  creates randomal customers
        /// </summary>
        /// <param name="num">>number of customers to add</param>
        private static void createCustomers(int num)
        {           
            string[] names = new string[] { "Greg", "CafeNeheman", "BurgersBar", "Brurya", "Ron", "Shmulik", "Tzuki", "Mahmud","Dorit", "ShuferSal", "Avrum", "Shoshana", "Gili"," Rivki" };
            int[] ID = new int[] { 325152347, 234678954, 123987456, 246987246, 309390876 };
            for (int i =0; i<5; i++)
            {
                customers.Add ( new Customer()
                {
                    Id =ID[i],
                    Name=names[i],
                    Phone = string.Format("0{0}",rand.Next(510000000, 589999999)),
                    Longitude = rand.NextDouble() * (33.5 - 29.3) + 29.3,
                    Latitude = rand.NextDouble() * (36.3 - 33.7) + 33.7
                });               
            }
            for (int i=5; i<num; i++)
            {
                customers.Add(new Customer()
                {
                    Id =rand.Next(100000000,999999999),
                    Name = names[i],
                    Phone = string.Format("0{0}", rand.Next(510000000, 589999999)),
                    Longitude = rand.NextDouble() * (33.5 - 29.3) + 29.3,
                    Latitude = rand.NextDouble() * (36.3 - 33.7) + 33.7
                });
            }
        }
        /// <summary>
        /// creates randomal parcels
        /// </summary>
        /// <param name="num">number of parcels to add</param>
        private static void createParcels(int num)
        {
            int[] ID = new int[] { 325152347, 234678954, 123987456, 246987246, 309390876 };
            for (int i=1;i<=3;i++)
            {
                parcels.Add(new Parcel()
                {
                    Id = Config.CodeOfParcel++,
                    DroneId = i,
                    SenderId = ID[i],
                    TargetId = ID[i+1],
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested =DateTime.Today,
                    Scheduled =DateTime.Now,
                    PickedUp = new DateTime(),
                    Delivered = new DateTime(),
                });
            }

            parcels.Add(new Parcel()
            {
                Id = Config.CodeOfParcel++,
                SenderId = ID[2],
                TargetId = ID[4],
                Weight = (WeightCategories)rand.Next(3),
                Priority = (Priorities)rand.Next(3),
                Requested = DateTime.Today,
                Scheduled = DateTime.Today,
                PickedUp = DateTime.Today,
                Delivered = default(DateTime),
                DroneId = 4
            });

            parcels.Add(new Parcel()
            {
                Id = Config.CodeOfParcel++,
                SenderId = ID[2],
                TargetId = ID[3],
                Weight = (WeightCategories)rand.Next(3),
                Priority = (Priorities)rand.Next(3),
                Requested = DateTime.Today,
                Scheduled = DateTime.Today,
                PickedUp = DateTime.Today,
                Delivered = DateTime.Now,
                DroneId = 5
            });

            for (int i =0; i < (num - 5); i++)
            {
                parcels.Add(new Parcel()
                {
                    Id = Config.CodeOfParcel++,
                    SenderId = customers[rand.Next(customers.Count)].Id,
                    TargetId = customers[rand.Next(customers.Count -5)].Id,
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested = DateTime.Now,
                    Scheduled = new DateTime(),
                    PickedUp = new DateTime(),
                    Delivered = new DateTime(),
                    DroneId = 0
                });
            }
        }

        /// <summary>
        /// Initializing initial elements in lists
        /// </summary>
        public static void Initialize()
        {
            
            createDrones(rand.Next(5, 8));
            CreateStations(rand.Next(2,4));
            createCustomers(rand.Next(10, 14));
            createParcels(rand.Next(10, 100));
            
        }       
    }
}

