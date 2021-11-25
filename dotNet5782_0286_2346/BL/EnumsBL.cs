using System;

namespace IBL
{
    namespace BO 
    {
        public class EnumsBL
        {
            public enum DroneStatuses {Available,Maintenance,OnDelivery}

            public enum WeightCategories { Light, Medium, Heavy }

            public enum Priorities { Regular, Fast, Emergency }

            public enum ParcelStatuses { Defined, Associated, Collected, Supplied}

        }
    }
}
