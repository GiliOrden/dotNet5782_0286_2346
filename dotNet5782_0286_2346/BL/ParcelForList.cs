using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class ParcelForList
        {
            public int Id { get; set; }
            public string SenderName { get; set; }
            public string ReceiverName { get; set; }
            public EnumsBL.WeightCategories Weight { get; set; }
            public EnumsBL.Priorities Priority{ get; set; }
            public EnumsBL.ParcelStatuses ParcelStatus { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }

        }
    }
}
