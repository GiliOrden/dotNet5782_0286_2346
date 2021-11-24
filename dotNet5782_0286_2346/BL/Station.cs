using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        public class Station
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public Location LocationOfStation { get; set; }
            public int AvailableChargingPositions { get; set; }
            public List<DroneInCharging> DroneInChargingList { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }


        }
    }
}