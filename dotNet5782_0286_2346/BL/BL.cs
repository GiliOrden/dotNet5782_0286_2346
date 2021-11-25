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
        internal double chargingRatePerHour;
        internal double emptyDronePowerConsumption;
        //should it be public?
        internal double lightWeightCarrierPowerConsumption;
        internal double mediumWeightCarrierPowerConsumption;
        internal double heavyWeightCarrierPowerConsumption;
        double[] dronePowerConsumption;
        IDal dl;
        List<Drone> drones = new List<Drone>();
        IEnumerable<IDAL.DO.Drone> dalDrones;
        public BL()//ctor
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            dl = new DalObject.DalObject();
            dalDrones = dl.GetListOfDrones();
            dronePowerConsumption = dl.getDronePowerConsumption();
            emptyDronePowerConsumption = dronePowerConsumption[0];
            lightWeightCarrierPowerConsumption = dronePowerConsumption[1];
            mediumWeightCarrierPowerConsumption = dronePowerConsumption[2];
            heavyWeightCarrierPowerConsumption = dronePowerConsumption[3];
            chargingRatePerHour = dronePowerConsumption[4];
            foreach(var drone in dalDrones)
            {
                drones.Add(new DroneForList()
                { 
                  Id = drone.Id,
                  Model = drone.Model,
                  MaxWeight=(EnumsBL.WeightCategories)drone.MaxWeight,
                    DroneStatus = (dl.GetListOfParcels().Any(Parcel => Parcel.Id == drone.Id)) ? EnumsBL.DroneStatuses.OnDelivery : rand.Next((int)EnumsBL.DroneStatuses.Maintance),
                  BatteryStatus =
                  
        public int NumberOfTheDeliveredParcel { get; set; }
        public Location Location { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    });
                    
            }
        }
        
    }
}
