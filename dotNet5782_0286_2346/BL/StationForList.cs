using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        public class StationForList
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int AvailableChargingPositions { get; set; }
            public int InaccessibleChargingPositions { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }


        }
    }
}
