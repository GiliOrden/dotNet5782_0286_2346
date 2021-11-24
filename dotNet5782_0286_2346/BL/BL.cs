using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using IBL.BO;
using IDAL;
using DalObject.DO;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        double ChargingRatePerHour;
        double[] dronesPowerConsumption=new double[4];
        IDal dl;
        List<Drone> drones = new List<Drone>();
        IEnumerable<IDAL.DO.Drone> dalDrones;//i dont know how it should be
        public BL()//ctor
        {
           dl = new DalObject.DalObject();
           dalDrones = dl.GetListOfDrones();
         
           
             dronesPowerConsumption=dl.GetDronePowerConsumption();
           
           ChargingRatePerHour = dl.GetDronePowerConsumption()[5];
          

        }

    }
}
