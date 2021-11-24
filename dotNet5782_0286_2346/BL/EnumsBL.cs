using System;

namespace IBL
{
    namespace BO 
    {
        public class EnumsBL
        {
            public enum DroneStatuses {Available,Maintance,OnDelivery}

            public enum WeightCategories { Light, Medium, Heavy }//i dont know if they need to be here

            public enum Priorities { Regular, Fast, Emergency }
            public enum ParcelStatuses { Defined, Associated, Collected, Supplied}
        }
    }
}
