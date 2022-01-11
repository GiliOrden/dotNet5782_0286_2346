
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
        private readonly string configPath = "BatteryAndRowNumbers.xml";

        public void AddStation(Station s)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            if (checkStation(stations,s.Id))
                throw new DO.ExistIdException(s.Id, "station");
            stations.Add(s);
            XMLTools.SaveListToXMLSerializer(stations, stationsPath);
        }

        public Station GetBaseStation(int id)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            if (checkStation(stations, id))
                throw new DO.ExistIdException(id, "station");
            Station s = stations.Find(stat => stat.Id == id);
            return s;
        }

        public IEnumerable<Station> GetListOfBaseStations()
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            return from baseStation in stations
                   select baseStation;
        }

        public IEnumerable<Station> GetStationsByPredicate(Predicate<Station> predicate)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            return from stat in stations
                   where predicate(stat)
                   select stat;
        }

        public void DeleteStation(int id)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            if (checkStation(stations, id))
                throw new DO.ExistIdException(id, "station");
            stations.RemoveAll(station => station.Id == id);
            XMLTools.SaveListToXMLSerializer(stations, stationsPath);
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
            List<Drone> dronesList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            if (!checkDrone(dronesList,id))
                throw new IdNotFoundException(id, "drone");
            dronesList.RemoveAll(dron => dron.Id == id);
            XMLTools.SaveListToXMLSerializer(dronesList, dronesPath);
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
            List<DroneCharge> droneChargesList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargesPath);
            return from DroneCharge in droneChargesList
                   select DroneCharge;
        }

        public int AddParcel(Parcel p)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            XDocument doc = XDocument.Load("BatteryAndRowNumbers.xml");
            XElement codeOfParcel = doc.Descendants("codeOfParcel").First();
            int code = (int)codeOfParcel;
            p.Id = code;
            parcels.Add(p);
            code++;
            codeOfParcel.Value=code.ToString();
            doc.Save("BatteryAndRowNumbers.xml");
            XMLTools.SaveListToXMLSerializer(parcels, parcelsPath);
            return p.Id;
        }

        public void AssignParcelToDrone(int parcelId, int droneId)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            List<Drone> dronesList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            if (!checkDrone(dronesList,droneId))
                throw new DO.IdNotFoundException(droneId, "drone");
            Drone d = dronesList.Find(drone => drone.Id == droneId);
            if (!checkParcel(parcels,parcelId))
                throw new DO.IdNotFoundException(parcelId, "parcel");
            foreach (Parcel parcel in parcels)
            {
                if (parcel.Id == parcelId)
                {
                    Parcel p = parcel;
                    p.DroneId = d.Id;
                    p.Scheduled = DateTime.Now;
                    parcels.Add(p);
                    parcels.Remove(parcel);
                    break;
                }
            }
            XMLTools.SaveListToXMLSerializer(parcels, parcelsPath);
        }

        public void CollectParcelByDrone(int id)
        {
            Parcel p,parcel;
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            if (!checkParcel(parcels,id))
                throw new DO.IdNotFoundException(id, "parcel");
            parcel = parcels.Find(parcel => parcel.Id == id);
            p = parcel;
            p.PickedUp = DateTime.Now;
            parcels.Remove(parcel);
            parcels.Add(p);
            XMLTools.SaveListToXMLSerializer(parcels, parcelsPath);
        }

        public void SupplyDeliveryToCustomer(int id)
        {
            Parcel parcel1, parcel2;
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            if (!checkParcel(parcels,id))
                throw new DO.IdNotFoundException(id, "parcel");
            parcel2 = parcels.Find(parcel => parcel.Id == id);
            parcel1 = parcel2;
            parcel1.Delivered = DateTime.Now;
            parcels.Remove(parcel2);
            parcels.Add(parcel1);
            XMLTools.SaveListToXMLSerializer(parcels, parcelsPath);
        }

        public Parcel GetParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            if (!checkParcel(parcels,id))
                throw new DO.IdNotFoundException(id, "parcel");
            Parcel p =parcels.Find(parc => parc.Id == id);
            return p;
        }

        public IEnumerable<Parcel> GetListOfParcels()
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            return from Parcel parc in parcels
                   select parc;
        }

        public IEnumerable<Parcel> GetParcelsByPredicate(Predicate<Parcel> predicate)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            return from pac in parcels
                   where predicate(pac)
                   select pac;
        }

        public void DeleteParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            if (!checkParcel(parcels,id))
                throw new DO.IdNotFoundException(id, "parcel");
            parcels.RemoveAll(parc => parc.Id == id);
            XMLTools.SaveListToXMLSerializer(parcels, parcelsPath);

        }
        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of customer</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkParcel(List<Parcel> lst, int id)
        {
           return lst.Exists(parc => parc.Id == id);
        }
        #endregion
    }
}
