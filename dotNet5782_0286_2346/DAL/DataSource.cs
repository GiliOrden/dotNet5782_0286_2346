
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
            //need to add a field for parcels
        }

        internal static Drone[] drones = new Drone[10];
        internal static Station[] stations = new Station[5];
        internal static Customer[] customers = new Customer[100];

        internal static Parcel[] parcels = new Parcel[10];

        static Random rand = new Random(DateTime.Now.Millisecond);
        //current time
        private static void createDrones(int num)
        {
            for (int i = 0; i < num; i++)
            {
                drones[0] = new Drone()
                {
                    Id = rand.Next(100, 200),
                    MaxWeight = (WeightCategories)rand.Next(3),
                    Status = (DroneStatuses)rand.Next(3)
                };
                Config.cntDrone++;
            }
        }

       public static void Initialize ()
        {
            createDrones(rand.Next(5, 8));
        }
    }
}
