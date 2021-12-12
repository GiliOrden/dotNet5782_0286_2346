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

        public IEnumerable<DroneCharge> GetListOfBusyChargeSlots()
        {
            return from DroneCharge in droneCharges
                   select DroneCharge;
        }
    }
}