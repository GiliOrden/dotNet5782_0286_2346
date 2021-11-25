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

            public IEnumerable<ParcelAtCustomer> ListOfParcelsFromMe;

            public IEnumerable<ParcelAtCustomer> ListOfParcelsIntendedToME;//i dont like the"me" and i dont sure it should be public and look like that
            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}
