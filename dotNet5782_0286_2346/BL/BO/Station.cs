using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Station
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int ChargeSlots { get; set; }
        public IEnumerable<DroneInCharging> DroneInChargingList { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }


    }
}