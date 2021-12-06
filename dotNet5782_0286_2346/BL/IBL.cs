using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface IBL
    {
        #region BaseStation
        /// <summary>
        /// the function returns the base station with the id is sent as parameter
        /// </summary>
        /// <param name="id"></param>
        BO.Station GetBaseStation(int id);
        /// <summary>
        /// the function adds the station that is sent as parameter
        /// </summary>
        /// <param name="station"></param>
        void addBaseStation(Station station);
        void UpdateBaseStation(int id, string name, int numOfChargeSlots);
        IEnumerable<StationForList> GetListOfBaseStations();
        IEnumerable<StationForList> GetListOfStationsWithAvailableChargeSlots();
        #endregion

        #region Drone
         void SendDroneToCharge(int id);
         void CollectingParcelByDrones(int droneId);
        void SupplyDeliveryToCustomer(int droneId);
        BO.Drone GetDrone(int id);
        void addDrone(DroneForList drone, int idOfStation);
        void UpdateDrone(int id, string newModel);
        void ReleaseDroneFromCharge(int id, int chargingTime);
        IEnumerable<DroneForList> GetListOfDrones();
        #endregion

        #region Location
        #endregion

        #region Customer
         void addCustomer(Customer cust);
         void UpdatingCustomerData(int id, string name, string phone);
         BO.Customer GetCustomer(int id);
        IEnumerable<CustomerForList> GetListOfCustomers();
        #endregion

        #region Parcel
        void addParcel(BO.Parcel p);
         BO.Parcel GetParcel(int id);
        void AssignParcelToDrone(int idOfDrone);
        IEnumerable<ParcelForList> GetListOfParcels();
        IEnumerable<ParcelForList> GetListOfDParcelsThatHaveNotYetBeenAssignedToDrone();
        #endregion




    }
}
