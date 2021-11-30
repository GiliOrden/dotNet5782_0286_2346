using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.EnumsBL;

namespace IBL
{
    namespace BO
    {
        public class ParcelAtCustomer
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatuses Status{ get; set; }
            public BO.CustomerInParcel OtherSide { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }

        }
    }
}




\
