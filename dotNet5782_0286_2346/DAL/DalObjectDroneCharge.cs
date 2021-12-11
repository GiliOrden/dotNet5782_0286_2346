using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DalObject.DataSource;
using IDAL;
namespace DalObject
{
    public partial class DalObject : IDal
    {
        public void AddDroneCharge(int idOfDrone,int idOfStation)
        {
            DroneCharge droneCharger = new();
            droneCharger.DroneId = idOfDrone;
            droneCharger.StationId = idOfStation;
            droneCharges.Add(droneCharger);
        }
        public IEnumerable<DroneCharge> GetListOfBusyChargeSlots()
        {
            return from DroneCharge in droneCharges
                   select DroneCharge;
        }
    }
}