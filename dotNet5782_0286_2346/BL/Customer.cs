using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location Location { get; set; }

            public IEnumerable<ParcelAtCustomer> ListOfParcelsFromMe { get; set; };//parcels he have gotten,

            public IEnumerable<ParcelAtCustomer> ListOfParcelsIntendedToME { get; set; }//i dont like the"me" and i dont sure it should be public and look like that//should it be property?
            public override string ToString()                                           //parcel he broght for others
            {
                return this.ToStringProperty();
            }
        }
    }
}
