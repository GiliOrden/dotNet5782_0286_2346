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

            public IEnumerable<ParcelAtCustomer> ListOfParcelsFromMe { get; set; }//parcel he broght for others

            public IEnumerable<ParcelAtCustomer> ListOfParcelsIntendedToME { get; set; }//should it be property?
            public override string ToString()                                       //parcels he have gotten,   
            {
                return this.ToStringProperty();
            }
        }
    }
}
