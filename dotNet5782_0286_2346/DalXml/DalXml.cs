
using System;
using System.Collections.Generic;
using DalApi;
using DO;

namespace Dal
{
    sealed class DalXml : IDal
    {
        #region singelton
        static readonly IDal instance = new DalXml();
        static DalXml() { }// static ctor to ensure instance init is done just before first usage
        DalXml() { }
        public static IDal Instance { get => instance; }
        #endregion

        #region DS XML Files
        string customersPath = @"CustomersXml.xml"; //XElement
        string dronesPath = @"DronesXml.xml"; //XMLSerializer
        string droneChargesPath = @"DroneChargesXml.xml"; //XMLSerializer
        string parcelsPath = @"ParcelsXml.xml"; //XMLSerializer
        string stationsPath = @"StationsXml.xml"; //XMLSerializer

        public void AddStation(Station s)
        {
            throw new NotImplementedException();
        }

        public Station GetBaseStation(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> GetListOfBaseStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> GetStationsByPredicate(Predicate<Station> predicate)
        {
            throw new NotImplementedException();
        }

        public void DeleteStation(int id)
        {
            throw new NotImplementedException();
        }

        public void AddCustomer(Customer c)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int id)
        {
            List<Customer> customersList = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            Customer c = customersList.Find(cust => cust.Id == id);
            if (CheckCustomer(customersList,id))
                return c; 
            else
                throw new IdNotFoundException(id, "customer");
        }

        public IEnumerable<Customer> GetListOfCustomers()
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of customer</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool CheckCustomer(List<Customer>lst ,int id)
        {
            return lst.Exists(cust => cust.Id == id);
        }

        public void AddDrone(Drone d)
        {
            throw new NotImplementedException();
        }

        public void SendDroneToCharge(int id, int id2)
        {
            throw new NotImplementedException();
        }

        public void ReleaseDroneFromCharge(int id)
        {
            throw new NotImplementedException();
        }

        public Drone GetDrone(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> GetListOfDrones()
        {
            throw new NotImplementedException();
        }

        public double[] GetDronePowerConsumption()
        {
            throw new NotImplementedException();
        }

        public void DeleteDrone(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneCharge> GetListOfBusyChargeSlots()
        {
            throw new NotImplementedException();
        }

        public int AddParcel(Parcel p)
        {
            throw new NotImplementedException();
        }

        public void AssignParcelToDrone(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public void CollectParcelByDrone(int id)
        {
            throw new NotImplementedException();
        }

        public void SupplyDeliveryToCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetListOfParcels()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetParcelsByPredicate(Predicate<Parcel> predicate)
        {
            throw new NotImplementedException();
        }

        public void DeleteParcel(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
