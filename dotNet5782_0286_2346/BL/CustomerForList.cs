using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerForList
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int SentAndDeliveredParcels { get; set; }
            public int SentButNotDeliveredParcels { get; set; }
            public int ReceivedParcels { get; set; }
            public int OnTheWayToCustomerParcels { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}



