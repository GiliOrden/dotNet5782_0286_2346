using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        double chargingRatePerHour;
        double emptyDronePowerConsumption;
        //should it be public?
        double lightWeightCarrierPowerConsumption;
        double mediumWeightCarrierPowerConsumption;
        double heavyWeightCarrierPowerConsumption;
        
        IDal dl;
        List<DroneForList> drones = new List<DroneForList>();
        IEnumerable<IDAL.DO.Drone> dalDrones;
        public BL()//ctor
        {
           dl = new DalObject.DalObject();
           dalDrones = dl.GetListOfDrones();
         
           
             dronesPowerConsumption=dl.GetDronePowerConsumption();
           
           ChargingRatePerHour = dl.GetDronePowerConsumption()[5];
          

        }

    }
}
