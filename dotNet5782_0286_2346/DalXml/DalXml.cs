
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
        string droneChargesPath = @"DroneChargesXml.xml"; //XElement
        string dronesPath = @"DronesXml.xml"; //XMLSerializer
        string customersPath = @"CustomersXml.xml"; //XMLSerializer
        string parcelsPath = @"ParcelsXml.xml"; //XMLSerializer
        string stationsPath = @"StationsXml.xml"; //XMLSerializer
        private readonly string configPath = "config.xml";

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
        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of drone</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkStation(List<Station> lst, int id)
        {
            return lst.Exists(station => station.Id == id);
        }

        public void AddCustomer(Customer c)
        {
            XElement customersRootElem = XMLTools.LoadListFromXMLElement(customersPath);

            XElement cus = (from customer in customersRootElem.Elements()
                             where int.Parse(customer.Element("Id").Value) == c.Id
                             select customer).FirstOrDefault();

            if (cus != null)
                throw new ExistIdException(c.Id, "customer");

            XElement personElem = new XElement("Customer",
                                  new XElement("Id", c.Id.ToString()),
                                  new XElement("Name", c.Name),
                                  new XElement("Phone", c.Phone),
                                  new XElement("Longitude", c.Longitude.ToString()),
                                  new XElement("Latitude", c.Latitude.ToString()));

            customersRootElem.Add(personElem);

            XMLTools.SaveListToXMLElement(customersRootElem, customersPath);
        }

        public Customer GetCustomer(int id)
        {
            XElement customersRootElem = XMLTools.LoadListFromXMLElement(customersPath);

            Customer c = (from cus in customersRootElem.Elements()
                        where int.Parse(cus.Element("Id").Value) == id
                        select new Customer()
                        {
                            Id = Int32.Parse(cus.Element("Id").Value),
                            Name = cus.Element("Name").Value,
                            Phone = cus.Element("Phone").Value,
                            Longitude = Int32.Parse(cus.Element("Longitude").Value),
                            Latitude = Int32.Parse(cus.Element("Latitude").Value)
                        }
                        ).FirstOrDefault();

            if (!CheckCustomer(id))
                throw new IdNotFoundException(id, "customer");

            return c;
        }

        public IEnumerable<Customer> GetListOfCustomers()
        {
            XElement customersRootElem = XMLTools.LoadListFromXMLElement(customersPath);

            return (from cus in customersRootElem.Elements()
                    select new Customer()
                    {
                        Id = Int32.Parse(cus.Element("Id").Value),
                            Name = cus.Element("Name").Value,
                            Phone = cus.Element("Phone").Value,
                            Longitude = Int32.Parse(cus.Element("Longitude").Value),
                            Latitude = Int32.Parse(cus.Element("Latitude").Value)
                    }
                   );
        }

        public void DeleteCustomer(int id)
        {
            XElement customersRootElem = XMLTools.LoadListFromXMLElement(customersPath);

            XElement cus = (from c in customersRootElem.Elements()
                            where int.Parse(c.Element("Id").Value) == id
                            select c).FirstOrDefault();

            if (cus != null)
            {
                cus.Remove(); //<==>   Remove per from personsRootElem

                XMLTools.SaveListToXMLElement(customersRootElem, customersPath);
            }
            else
                throw new IdNotFoundException(id, "customer");
        }

        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of customer</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool CheckCustomer(int id)
        {
            XElement customersRootElem = XMLTools.LoadListFromXMLElement(customersPath);
            XElement cus = (from c in customersRootElem.Elements()
                            where int.Parse(c.Element("Id").Value) == id
                            select c).FirstOrDefault();
            if (cus != null)
                return true;
            return false;
            
        }

        public void AddDrone(Drone d)
        {
           List<Drone> dronesList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
           if (checkDrone(dronesList,d.Id))
                throw new ExistIdException(d.Id, "drone");
            dronesList.Add(d);
            XMLTools.SaveListToXMLSerializer(dronesList, dronesPath);
        }

        public void SendDroneToCharge(int idDrone, int idStation)
        {
            DroneCharge dc = new DroneCharge();
            List<Drone> dronesList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            List<Station> stationsList = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            List<DroneCharge> droneChargesList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);
            if (!checkDrone(dronesList,idDrone))
                throw new DO.IdNotFoundException(idDrone, "drone");
            if (!checkStation(stationsList,idStation))
                throw new DO.IdNotFoundException(idStation, "station");
            dc.DroneId = idDrone;
            dc.StationId = idStation;
            dc.StartOfCharging = DateTime.Now;
            droneChargesList.Add(dc);
            XMLTools.SaveListToXMLSerializer(droneChargesList, droneChargesPath);
            Station s = stationsList.Find(stat => stat.Id == idStation);
            s.ChargeSlots--;
            stationsList.RemoveAll(stat => stat.Id == idStation);
            stationsList.Add(s);
            XMLTools.SaveListToXMLSerializer(stationsList, stationsPath);
        }

        public void ReleaseDroneFromCharge(int id)
        {
            List<Drone> dronesList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            List<Station> stationsList = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            List<DroneCharge> droneChargesList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);
            if (!checkDrone(dronesList,id))
                throw new DO.IdNotFoundException(id, "drone");
            Station s;
            foreach (DroneCharge charger in droneChargesList)
            {
                if (charger.DroneId == id)
                {
                    foreach (Station station in stationsList)
                    {
                        if (charger.StationId == station.Id)
                        {
                            s = station;
                            s.ChargeSlots++;
                            stationsList.Add(s);
                            stationsList.Remove(station);
                            droneChargesList.Remove(charger);
                            break;
                        }
                    }
                    break;
                }
            }
            XMLTools.SaveListToXMLSerializer(stationsList, stationsPath);
            XMLTools.SaveListToXMLSerializer(droneChargesList, droneChargesPath);
        }

        public Drone GetDrone(int id)
        {
            List<Drone> dronesList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            Drone drone = dronesList.Find(drone => drone.Id == id);
            if (checkDrone(dronesList, id))
                return drone;
            else
                throw new IdNotFoundException(id, "drone");
        }

        public IEnumerable<Drone> GetListOfDrones()
        {
            List<Drone> dronesList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            return from drone in dronesList
                   select drone; ;
        }

        public double[] GetDronePowerConsumption()
        {
            return XMLTools.LoadListFromXMLElement(configPath).Element("BatteryUsages").Elements()
                .Select(e => Convert.ToDouble(e.Value)).ToArray();
        }

        public void DeleteDrone(int id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of drone</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkDrone(List<Drone> lst, int id)
        {
            return lst.Exists(drone => drone.Id == id);
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
