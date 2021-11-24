using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Parcel
        {
            public int Id { get; set; }
            //פה צריך להיות לקוח בחבילה -השולח
            //לקוח בחבילה-המקבל
            public EnumsBL.WeightCategories Weight { get; set; }
            public EnumsBL.Priorities Priority { get; set; }
            //public int MyProperty { get; set; }    פה צריך להיות רחפן בחבילה -ישות שאני צריכה לכתוב
            public DateTime ParcelCreationTime { get; set; }
            public DateTime AssociationTime { get; set; }
            public DateTime CollectionTime{ get; set; }
            public DateTime DeliveryTime { get; set; }

        }
    }
}
