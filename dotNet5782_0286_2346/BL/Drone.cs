using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Drone//should it be public?maybe for the list
        {
            public int Id{ get; set; }
            public string Model { get; set; }
            public EnumsBL.WeightCategories MaxWeight { get; set; }//property
            public int BatteryStatus { get; set; }
            public EnumsBL.DroneStatuses DroneStatus { get; set; }
            public int MyProperty { get; set; }
            // public int MyProperty { get; set; }
            //כאן צריכה להיות חבילה בהעברה
            public Location Location { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }


        }
    }
}
