using System;

namespace IBL
{
    namespace BO 
    {
        public class EnumsBL
        {
            public enum DroneStatuses {Available,Maintance,OnDelivery}

            public enum WeightCategories { Light, Medium, Heavy }

            public enum Priorities { Regular, Fast, Emergency }

            public enum ParcelStatuses { Defined, Associated, Collected, Supplied}

            public static explicit operator EnumsBL(IDAL.DO.WeightCategories v)
            {
                throw new NotImplementedException();
            }
        }
    }
}
