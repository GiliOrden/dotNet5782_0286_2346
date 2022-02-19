using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static BO.EnumsBL;

namespace BO
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
        public override string ToString() => base.ToString() + $"The {EntityName} id:{ID} is already existed,\nPlease check this data field";
    }

    [Serializable]
    public class ExistIdExceptionException : Exception//When trying to look for an id of station/drone/parcel and it does not exist 
    {
        public int ID;
        public string EntityName;
       
        public ExistIdExceptionException(int id, string entity) : base() { ID = id; EntityName = entity; }
        public ExistIdExceptionException(int id, string entity, string message) : base(message) { ID = id; EntityName = entity; }
        public ExistIdExceptionException(int id, string entity, string message, Exception innerException) : base(message, innerException)
        { ID = id; EntityName = entity; }
        protected ExistIdExceptionException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
        public override string ToString() => base.ToString() + $",The {EntityName} Id isn't found";
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
    public class DroneStatusException : Exception
    {
        public int DroneID;
        public string Status;
        public DroneStatusException(int droneId,string status) : base() { DroneID = droneId; Status = status; }
        public DroneStatusException(int droneId, string status, string message) : base(message) { DroneID = droneId; Status = status; }
        public DroneStatusException(int droneId, string status, string message, Exception inner) : base(message, inner) { DroneID = droneId; Status = status; }

        public override string ToString() => base.ToString() + $"Drone:{DroneID} is not {Status} . ";//i changed the sentense a little bit, want it to be good for another func
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
    [Serializable]
    public class NoAvailableChargeSlots : Exception
    {
        public int StationId;
        public NoAvailableChargeSlots(int stationId) : base() { StationId = stationId; }
        public NoAvailableChargeSlots(int stationId, string message) : base(message) { StationId = stationId; }
        public NoAvailableChargeSlots(int stationId, string message, Exception inner) : base(message, inner) { StationId = stationId; }

        public override string ToString() => base.ToString() + $"Station:{StationId} has no availlable charge slots,please try another station. ";
    }
}
[Serializable]
public class UserNotFoundException : Exception//When trying to look for a user and he does not exist 
{
    public string Password;
    public string UserName;
    public UserNotFoundException(string password, string name) : base() { Password = password; UserName = name; }
    public UserNotFoundException(string password, string name, string message) : base(message) { Password = password; UserName = name; }
    public UserNotFoundException(string password, string name, string message, Exception innerException) : base(message, innerException)
    { Password = password; UserName = name; }
    protected UserNotFoundException(SerializationInfo info, StreamingContext context)
    : base(info, context) { }
    public override string ToString() => base.ToString() + $"The user {UserName} Pass:{Password} isn't exist.please check these fields";
}
[Serializable]
public class ExistUserException : Exception//when trying to add a user and he allready exists  
{
    public string Password;
    public string UserName;
    public ExistUserException(string password, string name) : base() { Password = password; UserName = name; }
    public ExistUserException(string password, string name, string message) : base(message) { Password = password; UserName = name; }
    public ExistUserException(string password, string name, string message, Exception innerException) : base(message, innerException)
    { Password = password; UserName = name; }
    protected ExistUserException(SerializationInfo info, StreamingContext context)
    : base(info, context) { }
    public override string ToString() => base.ToString() + $"The user name {UserName} or the password {Password}are already exists in the system.";
}


