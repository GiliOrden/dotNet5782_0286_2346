using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IBL
    {
        #region BaseStation
        /// <summary>
        /// the function returns the base station with the id is sent as parameter
        /// </summary>
        /// <param name="id">returns the id's base station</param>
        BO.Station GetBaseStation(int id);
        
        /// <summary>
        /// the function adds the station that is sent as parameter
        /// </summary>
        /// <param name="station">a new station to add </param>
        void AddBaseStation(Station station);

        /// <summary>
        /// this function update name and  number of Charging slots in a station
        /// </summary>
        /// <param name="id">id of the station</param>
        /// <param name="name">a new name</param>
        /// <param name="numOfChargeSlots">a new number of Charging slots</param>
        void UpdateBaseStation(int id, string name, int numOfChargeSlots);

        /// <summary>
        /// this function retuns the all list of stations
        /// </summary>
        /// <returns>list of stations</returns>
        IEnumerable<StationForList> GetListOfBaseStations();

        /// <summary>
        /// this function returns list of stations with available slots
        /// </summary>
        /// <returns>list of stations with available slots</returns>
        IEnumerable<StationForList> GetListOfStationsWithAvailableChargeSlots();
        #endregion

        #region Drone
        /// <summary>
        /// the available drone is sent tto the closest station for charging
        /// </summary>
        /// <param name="id">id of the drone</param> 
        void SendDroneToCharge(int id);
        
        /// <summary>
        /// a parcel,which hasn't been collected yet, collects by a drone
        /// </summary>
        /// <param name="droneId">id of the drone</param>
        void CollectParcelByDrone(int droneId);

        /// <summary>
        /// a parcel,which has been collected but hasn't supplied yet, comes to the target and supplies to the customer
        /// </summary>
        /// <param name="droneId">id of the drone</param>
        void SupplyDeliveryToCustomer(int droneId);

        /// <summary>
        /// this function assign parcel to drone accroding the priority, wight, location of theparcel 
        /// </summary>
        /// <param name="idOfDrone">drone id</param>
        void AssignParcelToDrone(int idOfDrone);

        /// <summary>
        /// This function return a drone from the customers collection by its id
        /// </summary>
        /// <param name="id">id of the drone</param>
        /// <returns>return a drone from the customers collection</returns>
        BO.Drone GetDrone(int id);

        /// <summary>
        /// This function adds a new drone to the customers collection 
        /// </summary>
        /// <param name="drone"> a new drone for adding</param>
        /// <param name="idOfStation">the id station (the drone will be in it at the begginning)</param>
        void AddDrone(DroneForList drone, int idOfStation);

        /// <summary>
        /// This function update the new model of a drone
        /// </summary>
        /// <param name="id">id of the drone</param>
        /// <param name="newModel">a new model for the drone</param>
        void UpdateDrone(int id, string newModel);

        /// <summary>
        /// This function releasing drone from charging
        /// </summary>
        /// <param name="id">id of the drone</param>
        /// <param name="chargingTime">the time drone wasin charging</param>
        void ReleaseDroneFromCharge(int id, DateTime chargingTime);

        /// <summary>
        /// This function retuns all the list of drones
        /// </summary>
        /// <returns>retuns drones list</returns>
        IEnumerable<DroneForList> GetListOfDrones();

        IEnumerable<BO.DroneForList> GetDronesByPredicate(Predicate<BO.DroneForList> predicate);
        
        /// <summary>
        /// The function search and returns DroneForList object
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <returns>DroneForList drone</returns>
        DroneForList GetDroneForList(int id);

        /// <summary>
        /// func for the automatic process!
        /// </summary>
        /// <param name="id">id of a drone</param>
        Drone Simulator(int id, BackgroundWorker automatic);
        #endregion

        #region Location
        #endregion

        #region Customer

        /// <summary>
        /// this functio adds a new customer to customers list
        /// </summary>
        /// <param name="cust">a new customer</param>
        void AddCustomer(Customer cust);
         
        /// <summary>
        /// this function update a new name or/and phone of a customer
        /// </summary>
        /// <param name="id">the customer id</param>
        /// <param name="name">a new name</param>
        /// <param name="phone">a new phone</param>
        void UpdateCustomer(int id, string name, string phone);

        /// <summary>
        /// this function returns a customer from the list of customers
        /// </summary>
        /// <param name="id">the customer id</param>
        /// <returns>returns a customer from the list </returns>
        BO.Customer GetCustomer(int id);

        /// <summary>
        /// this function returns all the list of customers
        /// </summary>
        /// <returns>returns all the list of customers</returns>
        IEnumerable<CustomerForList> GetListOfCustomers();
        #endregion

        #region Parcel
        
        /// <summary>
        /// this function add a new parcel to parcels list
        /// </summary>
        /// <param name="p">a new parcel</param>
        void AddParcel(BO.Parcel p);

        /// <summary>
        /// this function returns a parcel with the id is sent as parameter
        /// </summary>
        /// <param name="id">id of a parcel</param>
        /// <returns>id's parcel value</returns>
        BO.Parcel GetParcel(int id);

        /// <summary>
        /// this function retuns the all list of parcels
        /// </summary>
        /// <returns>list of parcels</returns>
        IEnumerable<ParcelForList> GetListOfParcels();

        /// <summary>
        /// this function retuns list of parcels that hasn't been assigned to a drone
        /// </summary>
        /// <returns>list of parcels that hasn't been assigned to a drone</returns>
        IEnumerable<ParcelForList> GetListOfNotAssociatedParcels();
        /// <summary>
        /// returns list of parcels which theres' wight/status equal to the parameters
        /// </summary>
        /// <param name="weight">tipe of wight, it can be also null</param>
        /// <param name="status">tipe of statusParcel, it can be also null</param>
        /// <returns>list of parcels which theres' wight/status equal to the parameters</returns>
        IEnumerable<BO.ParcelForList> GetParcelsByPredicate(EnumsBL.WeightCategories? weight, EnumsBL.ParcelStatuses? status);

        /// <summary>
        /// an help function for 'GetParcelsByPredicate' func to select which pridicate it needs, return true if the case is correct
        /// </summary>
        /// <param name="weight">type of weight, it can be also null.  </param>
        /// <param name="status">type of statusParcel, it can be also null</param>
        /// <param name="parc">check if it's wight/status equal to the parameters</param>
        /// <returns></returns>
        bool predicatFanc(EnumsBL.WeightCategories? weight, EnumsBL.ParcelStatuses? status, DO.Parcel parc);
        void DeleteParcel(int id);
        #region User
        void AddUser(User u);
        IEnumerable<User> GetListOfUsers();
        User GetUser(string name, string password);
        #endregion
    }
    #endregion

}
