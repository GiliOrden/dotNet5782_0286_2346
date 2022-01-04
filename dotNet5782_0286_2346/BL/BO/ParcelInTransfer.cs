using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.EnumsBL;


namespace BO
{
    public class ParcelInTransfer
    {
        public int Id { get; set; }
        public bool Status { get; set; }//no- Waiting for collection /yes- on the way to the destination
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Receiver { get; set; }
        public BO.Location Source { get; set; }
        public BO.Location Destination { get; set; }
        public double TransportDistance { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}