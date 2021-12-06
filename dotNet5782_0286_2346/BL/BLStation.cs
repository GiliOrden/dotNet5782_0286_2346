using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {

        public void AddBaseStation(Station station)
        {
            IDAL.DO.Station dalStation = new();
            dalStation.Id = station.ID;
            dalStation.Name = station.Name;
            dalStation.Longitude = station.Location.Longitude;
            dalStation.Latitude = station.Location.Latitude;
            dalStation.ChargeSlots = station.ChargeSlots;
            try
            {
                dl.AddStation(dalStation);
            }
            catch (IDAL.DO.ExistIdException ex)
            {
                throw new IBL.BO.ExistIdException(ex.ID, ex.EntityName);
            }
        }


        public void UpdateBaseStation(int id, string name="", int numOfChargeSlots=-1)
        {
            try
            {
                IDAL.DO.Station s = dl.GetBaseStation(id);
                if (name != "")
                    s.Name = name;
                if (numOfChargeSlots !=-1)
                {
                    foreach (IDAL.DO.DroneCharge droneCharge in dl.GetListOfBusyDroneCharges())
                    {
                        if (droneCharge.StationId == id)
                            numOfChargeSlots--;
                    }
                    s.ChargeSlots = numOfChargeSlots;
                    dl.DeleteStation(id);
                    dl.AddStation(s);
                }
            }
            catch (IDAL.DO.IdNotFoundException ex)
            {
                throw new IBL.BO.IdNotFoundException(ex.ID, ex.EntityName);
            }
        }



        public IEnumerable<IBL.BO.StationForList> GetListOfBaseStations()
        {
            IEnumerable<IBL.BO.StationForList> stationsBO =
                from station in dl.GetListOfBaseStations()
                select new IBL.BO.StationForList
                {
                    ID = station.Id,
                    Name = station.Name,
                    AvailableChargingPositions = station.ChargeSlots
                };
            foreach (StationForList station in stationsBO)
            {
                foreach (IDAL.DO.DroneCharge droneCharger in dl.GetListOfBusyChargeSlots())
                {
                    if (droneCharger.StationId == station.ID)
                        station.InaccessibleChargingPositions++;
                }
            }
            return stationsBO;
        }



        public IEnumerable<IBL.BO.StationForList> GetListOfStationsWithAvailableChargeSlots()
        {
            IEnumerable<IBL.BO.StationForList> stationsWithAvailableChargeSlots =
            from station in dl.GetListOfStationsWithAvailableChargeSlots()
            select new IBL.BO.StationForList
            {
                ID = station.Id,
                Name = station.Name,
                AvailableChargingPositions = station.ChargeSlots
            };
            foreach (StationForList station in stationsWithAvailableChargeSlots)
            {
                foreach (IDAL.DO.DroneCharge droneCharger in dl.GetListOfBusyChargeSlots())
                {
                    if (droneCharger.StationId == station.ID)
                        station.InaccessibleChargingPositions++;
                }
            }
            return stationsWithAvailableChargeSlots;
        }


        public Station GetBaseStation(int id)//i didn't finished!
        {
            Station s = new();
            try
            {
                DroneInCharging droneInCharging = new();
                IDAL.DO.Station sDal = dl.GetBaseStation(id);
                s.ID = sDal.Id;
                s.Name = sDal.Name;
                s.Location.Latitude = sDal.Latitude;
                s.Location.Longitude = sDal.Longitude;
                s.ChargeSlots = sDal.ChargeSlots;
                s.DroneInChargingList = GetdronesInChargingPerStation(id, s.Location);
            }
            catch (IDAL.DO.IdNotFoundException ex)
            {
                throw new IdNotFoundException(ex.ID, "station");
            }

            return s;
        }
        private int getClosestStation(IDAL.DO.Customer customer, ref double minDistance)//help method
        {
            int idOfStation = 0;
            double distance;
            minDistance = 10000;
            foreach (IDAL.DO.Station baseStation in dl.GetListOfBaseStations())
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

    };
}


