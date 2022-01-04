using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dal.DataSource;
using DalApi;
namespace Dal
{
    internal partial class DalObject : IDal
    {
        /// <summary>
        /// the function recieves a drone as parameter and adds it to the list of drones if its id is already existed it throws "ExistIdException"
        /// </summary>
        /// <param name="d"></param>
        public void AddDrone(Drone d)
        {
            if (checkDrone(d.Id))
                throw new DO.ExistIdException(d.Id, "drone");
            drones.Add(d);
        }

        /// <summary>
        /// The function returns an array of data on power consumption and the rate of charge in drones
        /// </summary>
        /// <returns></returns>
        public double[] GetDronePowerConsumption()
        {
            double[] status = new double[5];
            status[0] = Config.EmptyDronePowerConsumption;
            status[1] = Config.LightWeightCarrierPowerConsumption;
            status[2] = Config.MediumWeightCarrierPowerConsumption;
            status[3] = Config.HeavyWeightCarrierPowerConsumption;
            status[4] = Config.ChargingRatePerHour;
            return status;
        }

        /// <summary>
        /// the function send drone to charge
        /// </summary>
        /// <param name="idDrone">drone id</param>
        /// <param name="idStation">station id</param>
        public void SendDroneToCharge(int idDrone, int idStation)
        {
            DroneCharge dc = new DroneCharge();
            if (!checkDrone(idDrone))
                throw new DO.IdNotFoundException(idDrone, "drone");
            if (!checkStation(idStation))
                throw new DO.IdNotFoundException(idStation, "station");
            dc.DroneId = idDrone;
            dc.StationId = idStation;
            dc.StartOfCharging = DateTime.Now;
            droneCharges.Add(dc);
            Station s = Dal.DataSource.stations.Find(stat => stat.Id == idStation);
            s.ChargeSlots--;
            stations.RemoveAll(stat => stat.Id == idStation);
            stations.Add(s);
        }

        /// <summary>
        /// updating of releasing drone from Charging station
        /// </summary>
        /// <param name="id">the drone id</param>
        public void ReleaseDroneFromCharge(int id)
        {
            if (!Dal.DataSource.drones.Any(dron => dron.Id == id))
                throw new DO.IdNotFoundException(id, "drone");
            Station s;
            foreach (DroneCharge charger in droneCharges)
            {
                if (charger.DroneId == id)
                {
                    foreach (Station station in stations)
                    {
                        if (charger.StationId == station.Id)
                        {
                            s = station;
                            s.ChargeSlots++;
                            stations.Add(s);
                            stations.Remove(station);
                            droneCharges.Remove(charger);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void DeleteDrone(int id)
        {
            if (!checkDrone(id))
                throw new IdNotFoundException(id, "drone");
            Dal.DataSource.drones.RemoveAll(dron => dron.Id == id);
        }

        /// <summary>
        ///  this function returns a specific drone from the list
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <returns>Drone element</returns>
        public Drone GetDrone(int id)
        {
            if (!checkDrone(id))
                throw new DO.IdNotFoundException(id, "drone");
            Drone d = Dal.DataSource.drones.Find(drone => drone.Id == id);
            return d;
        }


        /// <summary>
        /// this function returns list of drone
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetListOfDrones()
        {
            return from drone in Dal.DataSource.drones
                   select drone; ;
        }

        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of drone</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkDrone(int id)
        {
            return Dal.DataSource.drones.Any(drone => drone.Id == id);
        }
    }
}
