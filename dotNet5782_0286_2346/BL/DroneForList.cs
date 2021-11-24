using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class DroneForList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public EnumsBL.WeightCategories MaxWeight { get; set; }//property
            public double BatteryStatus { get; set; }
            public EnumsBL.DroneStatuses DroneStatus { get; set; }
            public int NumberOfTheDeliveredParcel  { get; set; }
            public Location Location { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}
