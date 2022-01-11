using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    sealed partial class BL : IBL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddBaseStation(BO.Station station)
        {
            DO.Station dalStation = new();
            dalStation.Id = station.ID;
            dalStation.Name = station.Name;
            dalStation.Longitude = station.Location.Longitude;
            dalStation.Latitude = station.Location.Latitude;
            dalStation.ChargeSlots = station.ChargeSlots;
            try
            {
                dl.AddStation(dalStation);
            }
            catch (DO.ExistIdException ex)
            {
                throw new ExistIdException(ex.ID, ex.EntityName);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStation(int id, string name="", int numOfChargeSlots=0)
        {
            try
            {
                DO.Station s = dl.GetBaseStation(id);
                if (name != "")
                    s.Name = name;
                if (numOfChargeSlots !=0)
                {
 
                    foreach (DO.DroneCharge droneCharge in dl.GetListOfBusyChargeSlots())
                    {
                        if (droneCharge.StationId == id)
                            numOfChargeSlots--;
                    }
                    s.ChargeSlots = numOfChargeSlots;                    
                }
                dl.DeleteStation(id);
                dl.AddStation(s);
            }
            catch (DO.IdNotFoundException ex)
            {
                throw new IdNotFoundException(ex.ID, ex.EntityName);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BO.StationForList> GetListOfBaseStations()
        {
            IEnumerable<BO.StationForList> stationsBO =
                from station in dl.GetListOfBaseStations()
                select new BO.StationForList
                {
                    ID = station.Id,
                    Name = station.Name,
                    AvailableChargingPositions = station.ChargeSlots,
                    InaccessibleChargingPositions=getNumberOfInaccessibleChargingSlots(station.Id)
                };
            return stationsBO;
        }

        /// <summary>
        /// the function calculates and returns the number of inaccessible charging slots in a specific station
        /// </summary>
        /// <param name="id">the station's ID</param>
        /// <returns>number of inaccessible charging slots in a specific station</returns>
        private int getNumberOfInaccessibleChargingSlots(int id)
        {
            int numOfInaccessibleChargingSlots = 0;
            foreach (DO.DroneCharge droneCharger in dl.GetListOfBusyChargeSlots())
            {
                if (droneCharger.StationId == id)
                    numOfInaccessibleChargingSlots++;
            }
            return numOfInaccessibleChargingSlots;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BO.StationForList> GetListOfStationsWithAvailableChargeSlots()
        {
            IEnumerable<BO.StationForList> stationsWithAvailableChargeSlots =
            from station in dl.GetStationsByPredicate(t=>t.ChargeSlots != 0)
            select new BO.StationForList
            {
                ID = station.Id,
                Name = station.Name,
                AvailableChargingPositions = station.ChargeSlots,
                InaccessibleChargingPositions = getNumberOfInaccessibleChargingSlots(station.Id)
            };
            return stationsWithAvailableChargeSlots;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Station GetBaseStation(int id)
        {
            BO.Station s = new();
            try
            {
                BO.DroneInCharging droneInCharging = new();
                DO.Station sDal = dl.GetBaseStation(id);
                s.ID = sDal.Id;
                s.Name = sDal.Name;
                s.Location = new BO.Location();
                s.Location.Latitude = sDal.Latitude;
                s.Location.Longitude = sDal.Longitude;
                s.ChargeSlots = sDal.ChargeSlots;
                s.DroneInChargingList = GetdronesInChargingPerStation(id, s.Location);
            }
            catch (DO.IdNotFoundException ex)
            {
                throw new IdNotFoundException(ex.ID, "station");
            }

            return s;
        }

        /// <summary>
        /// the function receives customer and distance and returns the ID of the station that is closest to the customer and the distance to it
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="minDistance"></param>
        /// <returns> the ID of the station that is closest to the customer</returns>
        private int getClosestStation(DO.Customer customer, ref double minDistance)
        {
            int idOfStation = 0;
            double distance;
            minDistance = 10000;
            foreach (DO.Station baseStation in dl.GetListOfBaseStations())
            {
                distance = DistanceBetweenPlaces(baseStation.Longitude, baseStation.Latitude, customer.Longitude, customer.Latitude);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    idOfStation = baseStation.Id;
                }
            }
            return idOfStation;
        }

        /// <summary>
        /// the function calculates and returns a list of drones in charging in a specific base station
        /// </summary>
        /// <param name="id">baseStation's ID</param>
        /// <param name="location">baseStation's location</param>
        /// <returns>list of drones in charging in a specific base station</returns>
        private IEnumerable<BO.DroneInCharging> GetdronesInChargingPerStation(int id, BO.Location location)//id and location of a base station 
        {
            var drones=from d in GetDroneInChargingByPredicate(d => d.Location.Longitude == location.Longitude && d.Location.Latitude == location.Latitude&&d.DroneStatus==BO.EnumsBL.DroneStatuses.Maintenance)
                   let dronesInCharging = GetDrone(d.Id)
                   select new BO.DroneInCharging()
                   {
                       Id = dronesInCharging.Id,
                       Battery = dronesInCharging.Battery
                   };
            return drones;
        }

        /// <summary>
        /// the function calculates and returns all the drones in charging at a specific station
        /// </summary>
        /// <param name="predicate">check if the drone's location is int the specific station and its status is in maintenance </param>
        /// <returns>collection of drones by predicate</returns>
        private IEnumerable<BO.DroneForList> GetDroneInChargingByPredicate(Predicate<BO.DroneForList> predicate)
        {
            return from drone in dronesBL
                   where predicate(drone)
                   select drone;
        }


    };
}


