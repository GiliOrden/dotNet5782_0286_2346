using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IDAL
{
    public interface IDal//i'm not sure it should be  public maybe internal and i dont know if the properties also should be here
    {
        #region Station
        void AddStation(DO.Station s);
        DO.Station GetBaseStation(int id);
        IEnumerable<DO.Station> GetListOfBaseStations();
        IEnumerable<DO.Station> GetListOfAvailableChargingStations();
        DO.Station DistanceFromStation(int id);
        #endregion

        #region Customer
        void AddCustomer(DO.Customer c);
        DO.Customer GetCustomer(int id);
        IEnumerable<DO.Customer> GetListOfCustomers();
        DO.Customer DistanceFromCustomer(string name);
        #endregion

        #region Drone
        void AddDrone(DO.Drone d);
        void SendDroneToCharge(int id, int id2);
        void ReleaseDroneFromCharge(int id);
        DO.Drone GetDrone(int id);
        IEnumerable<DO.Drone> GetListOfDrones();
        IEnumerable<double> DronePowerConsumption();
        #endregion

        #region Parcel
        int AddParcel(DO.Parcel p);
        void AssignParcelToDrone(int parcelId, int droneId);//here or in drone?
        void CollectParcelByDrone(int id);
        void SupplyDeliveryToCustomer(int id);
        DO.Parcel GetParcel(int id);
        IEnumerable<DO.Parcel> GetListOfParcels();
        IEnumerable<DO.Parcel> GetListOfNotAssociatedParsels();
        #endregion
    }
}
