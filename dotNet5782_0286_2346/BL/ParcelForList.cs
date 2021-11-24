using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.EnumsBL;//to check if this using is allowed

namespace IBL
{
    namespace BO
    {
        class ParcelForList
        {
            public int Id { get; set; }
            public string SenderName { get; set; }
            public string ReceiverName { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority{ get; set; }
            public ParcelStatuses ParcelStatus { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }

        }
    }
}
