using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using IDAL;

namespace BL
{
    public partial class BL //: IBL.IBL
    {
        double ChargingRatePerHour;
        public BL()//ctor
        {
            IDal dl=new DalObject.DalObject();
            ChargingRatePerHour = dl.GetDronePowerConsumption()[];
        }

    }
}
