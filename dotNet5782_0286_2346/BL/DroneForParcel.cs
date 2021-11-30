using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneForParcel
        {
            public int Id { get; set; }
            public double BatteryStatus { get; set; }
            public Location Location{ get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}
