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

        /// <summary>
        /// Adding customer element to the customers list
        /// </summary>
        /// <param name="c">element ,Customer type, we adding the list<Customer>
        void AddCustomer(DO.Customer c);

        /// <summary>
        ///  this function returns a specific customer from the list
        /// </summary>
        /// <param name="id">the customer id</param>
        /// <returns>Customer element</returns>
        DO.Customer GetCustomer(int id);

        /// <summary>
        /// This is a 'set' function for class Customer
        /// </summary>
        /// <param name="id">the customer id which need to be changed</param>
        /// <param name="c">the customer after the changing</param>
        public void SetCustomer(int id,DO.Customer c);
                    
        /// <summary>
        /// the function returns the list of the customers
        /// </summary>
        /// <returns><"listOfCustomers">
        IEnumerable<DO.Customer> GetListOfCustomers();

        /// <summary>
        /// returns customer element which the name is his 
        /// </summary>
        /// <param name="name">name of customer</param>
        /// <returns></returns>
        DO.Customer DistanceFromCustomer(string name);
        #endregion

        #region Drone
        /// <summary>
        /// the function receives as parameter drone and adds it to the drones list
        /// </summary>
        /// <param name="d"><Drone>
        void AddDrone(DO.Drone d);

        /// <summary>
        /// updating of sending drone to staion for charging
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <param name="id2">the station id</param>
        void SendDroneToCharge(int id, int id2);
        void ReleaseDroneFromCharge(int id);
        DO.Drone GetDrone(int id);
        IEnumerable<DO.Drone> GetListOfDrones();
        double[] getDronePowerConsumption();
       
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
