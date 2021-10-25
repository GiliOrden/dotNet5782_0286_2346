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
            internal static int cntDroneId = 0;

        }

        internal static Drone[] drones = new Drone[10];

        static Random rand = new Random();//current time
        private static void createDrones(int num)
        {
            //for i-->num
            drones[0] = new Drone()
            {
                Id = rand.Next(100, 200),
                MaxWeight = (WeightCategories)rand.Next(3),
                Status = DroneStatuses.Available

              };
         Config.cntDroneId++;

        }
       // private static Initialize -a method that called to all the create functions 
    }
}
