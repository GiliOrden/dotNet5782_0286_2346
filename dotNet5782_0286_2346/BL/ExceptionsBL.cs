using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{

    [Serializable]
    public class ExistIdException : Exception//when trying to add an id of station/drone/parcel and it allready exists 
    {
        public int ID;
        public string EntityName;
        public ExistIdException(int id, string entity) : base() { ID = id; EntityName = entity; }
        public ExistIdException(int id, string entity, string message) : base(message) { ID = id; EntityName = entity; }
        public ExistIdException(int id, string entity, string message, Exception inner) : base(message, inner) { ID = id; EntityName = entity; }
        protected ExistIdException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
        public override string ToString() => base.ToString() + $"The {EntityName} Id: {ID} ,is already exists.";
    }

    [Serializable]
    public class IdNotFoundException : Exception//When trying to look for an id of station/drone/parcel and it does not exist 
    {
        public int ID;
        public string EntityName;
        public IdNotFoundException(int id, string entity) : base() { ID = id; EntityName = entity; }
        public IdNotFoundException(int id, string entity, string message) : base(message) { ID = id; EntityName = entity; }
        public IdNotFoundException(int id, string entity, string message, Exception innerException) : base(message, innerException)
        { ID = id; EntityName = entity; }
        protected IdNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
        public override string ToString() => base.ToString() + $",The {EntityName} Id:{ID} isn't found";
    }

    [Serializable]
    public class NoBatteryException : Exception
    {
        public int ID;

        public NoBatteryException(int id) : base() { ID = id; }
        public NoBatteryException(int id, string message) : base(message) { ID = id; }
        public NoBatteryException(int id, string message, Exception inner) : base(message, inner) { ID = id; }

        public override string ToString() => base.ToString() + $"The drone Id: {ID} does not have enough battery to perform the task.";
    }

    [Serializable]
    public class DroneCanNotCollectParcelException : Exception
    {
        public int DroneID;
        public int ParcelId;
        public DroneCanNotCollectParcelException(int droneId, int parcelId) : base() { DroneID = droneId; ParcelId = parcelId; }
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
    [Serializable]
    public class DroneIsNotAvailableException : Exception
    {
        public int DroneID;

        public DroneIsNotAvailableException(int droneId) : base() { DroneID = droneId; }
        public DroneIsNotAvailableException(int droneId, string message) : base(message) { DroneID = droneId; }
        public DroneIsNotAvailableException(int droneId, int parcelId, string message, Exception inner) : base(message, inner) { DroneID = droneId; }

        public override string ToString() => base.ToString() + $"Drone:{DroneID} is not Available, please try another drone. ";
    }

    [Serializable]
    public class DroneMaxWeightIsLowException : Exception
    {
        public int DroneID;

        public DroneMaxWeightIsLowException(int droneId) : base() { DroneID = droneId; }
        public DroneMaxWeightIsLowException(int droneId, string message) : base(message) { DroneID = droneId; }
        public DroneMaxWeightIsLowException(int droneId, int parcelId, string message, Exception inner) : base(message, inner) { DroneID = droneId; }

        public override string ToString() => base.ToString() + $"Drone:{DroneID} can't carry any parcel becuse its max weight is too low, please try another drone. ";
    }

}   
