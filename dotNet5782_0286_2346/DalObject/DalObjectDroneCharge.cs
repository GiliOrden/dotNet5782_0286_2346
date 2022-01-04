using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dal.DataSource;
using DalApi;
namespace Dal
{
    sealed partial class DalObject : IDal
    {

        public IEnumerable<DroneCharge> GetListOfBusyChargeSlots()
        {
            return from DroneCharge in droneCharges
                   select DroneCharge;
        }

    }
}