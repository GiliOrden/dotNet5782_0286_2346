using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{

    class ExceptionsBL : Exception
    {
        
            [Serializable]
            public class NoBatteryException: Exception
            {
                public int ID;
                
                public NoBatteryException(int id) : base() { ID = id; }
                public NoBatteryException(int id,  string message) : base(message) { ID = id; }
                public NoBatteryException(int id,  string message, Exception inner) : base(message, inner) { ID = id;  }

                public override string ToString() => base.ToString() + $"The drone Id: {ID} does not have enough battery to be sent to a base station for charging";
            }

        
        [Serializable]
        public class DroneCanNotCollectParcelException : Exception
        {
            public int DroneID;
            public int ParcelId;
            public DroneCanNotCollectParcelException(int droneId,int parcelId) : base() { DroneID = droneId; ParcelId = parcelId; }
            public DroneCanNotCollectParcelException(int droneId, int parcelId, string message) : base(message) { DroneID = droneId; ParcelId = parcelId; }
            public DroneCanNotCollectParcelException(int droneId, int parcelId, string message, Exception inner) : base(message, inner) { DroneID = droneId; ParcelId = parcelId; }

            public override string ToString() => base.ToString() + $"The {DroneID} drone cann't send , the {ParcelId} parcel   have allready sent ";
        }
        
        [Serializable]
        public class DroneCanNotSupplyDeliveryToCustomerException : Exception
        {
            public int DroneID;
            public int ParcelId;
            public DroneCanNotSupplyDeliveryToCustomerException(int droneId, int parcelId) : base() { DroneID = droneId; ParcelId = parcelId; }
            public DroneCanNotSupplyDeliveryToCustomerException(int droneId, int parcelId, string message) : base(message) { DroneID = droneId; ParcelId = parcelId; }
            public DroneCanNotSupplyDeliveryToCustomerException(int droneId, int parcelId, string message, Exception inner) : base(message, inner) { DroneID = droneId; ParcelId = parcelId; }

            public override string ToString() => base.ToString() + $"The {DroneID} drone cann't supply , the {ParcelId} parcel  ";
        }

    }
}
