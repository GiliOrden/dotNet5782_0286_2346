﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region Station
        void AddStation(DO.Station s);
        DO.Station GetBaseStation(int id);
        IEnumerable<DO.Station> GetListOfBaseStations();
        IEnumerable<DO.Station> GetStationsByPredicate(Predicate<DO.Station> predicate);
        void DeleteStation(int id);
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
        /// the function returns the list of the customers
        /// </summary>
        /// <returns><"listOfCustomers">
        IEnumerable<DO.Customer> GetListOfCustomers();

        
        void DeleteCustomer(int id);
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
        double[] GetDronePowerConsumption();
        void DeleteDrone(int id);

        #endregion
        #region DroneCharge
        /// <summary>
        /// the function calculates and returns a list of busy charge slotes of a station
        /// </summary>
        /// <returns>collection of busy charge slotes of a station</returns>
        IEnumerable<DO.DroneCharge> GetListOfBusyChargeSlots();
        #endregion
        #region Parcel
        int AddParcel(DO.Parcel p);
        void AssignParcelToDrone(int parcelId, int droneId);
        void CollectParcelByDrone(int id);
        void SupplyDeliveryToCustomer(int id);
        DO.Parcel GetParcel(int id);
        IEnumerable<DO.Parcel> GetListOfParcels();
            
         IEnumerable<DO.Parcel> GetParcelsByPredicate(Predicate<DO.Parcel> predicate);
        #endregion

    }
}