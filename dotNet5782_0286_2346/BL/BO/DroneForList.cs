using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneForList
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public EnumsBL.WeightCategories? MaxWeight { get; set; }
        public double Battery { get; set; }
        public EnumsBL.DroneStatuses? DroneStatus { get; set; }
        public int IdOfTheDeliveredParcel { get; set; }//the id of the parcel the drone need to send
        public Location Location { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}