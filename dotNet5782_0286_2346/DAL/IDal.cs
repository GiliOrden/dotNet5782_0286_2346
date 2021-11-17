using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IDAL
{
    public interface IDal//i'm not sure it should be  public maybe internal
    {
        #region Station
        void AddStation(DO.Station s);
        DO.Station DisplayBaseStation(int id);
        IEnumerable<DO.Station> ListOfBaseStations();
        IEnumerable<DO.Station> ListOfAvailableChargingStations();
        DO.Station DistanceFromStation(int id);
        #endregion

        #region Customer
        void AddCustomer(DO.Customer c);
        DO.Customer DisplayCustomer(int id);
        IEnumerable<DO.Customer> ListOfCustomers();
        DO.Customer DistanceFromCustomer(string name);
        #endregion

        #region Drone
        void AddDrone(DO.Drone d);
        void SendDroneToCharge(int id, int id2);
        void ReleaseDroneFromCharge(int id);
        DO.Drone DisplayDrone(int id);
        IEnumerable<DO.Drone> ListOfDrones();
        #endregion

        #region Parcel
        void AddParcel(DO.Parcel p);
        void AssignParcelToDrone(int parcelId, int droneId);//here or in drone?
        void CollectParcelByDrone(int id);
        void SupplyDeliveryToCustomer(int id);
        DO.Parcel DisplayParcel(int id);
        IEnumerable<DO.Parcel> ListOfParcels();
        IEnumerable<DO.Parcel> ListOfNotAssociatedParsels();
        #endregion
    }
}
