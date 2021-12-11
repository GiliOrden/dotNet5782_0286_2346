using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Receiver { get; set; }
            public EnumsBL.WeightCategories Weight { get; set; }
            public EnumsBL.Priorities Priority { get; set; }
            public  DroneForParcel Drone{ get; set; }    
            public DateTime ParcelCreationTime { get; set; }
            public DateTime AssociationTime { get; set; }
            public DateTime CollectionTime{ get; set; }
            public DateTime DeliveryTime { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }

        }
    }
}
