using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DalObject.DataSource;
using IDAL;
namespace DalObject
{
    public partial class DalObject : IDal
    {
        /// <summary>
       /// the function recieves a drone as parameter and adds it to the list of drones if its id is already existed it throws "ExistIdException"
      /// </summary>
      /// <param name="d"></param>
        public void AddDrone(Drone d)
        {
            if (checkDrone(d.Id))
                throw new IDAL.DO.ExistIdException(d.Id, "drone");
            drones.Add(d);
        }

        /// <summary>
        /// The function returns an array of data on power consumption and the rate of charge in drones
        /// </summary>
        /// <returns></returns>
        public double[] GetDronePowerConsumption()
        {
            double[] status = new double[5];
            status[0]=Config.EmptyDronePowerConsumption;
            status[1] = Config.LightWeightCarrierPowerConsumption;
            status[2]= Config.MediumWeightCarrierPowerConsumption;
            status[3]=Config. HeavyWeightCarrierPowerConsumption;
            status[4] =Config.ChargingRatePerHour;
            return status;
        }

        /// <summary>
        /// the function send drone to charge
        /// </summary>
        /// <param name="idDrone">drone id</param>
        /// <param name="idStation">station id</param>
        public void SendDroneToCharge(int idDrone, int idStation)
        {
            if (!checkDrone(idDrone))
                throw new IDAL.DO.IdNotFoundException(idDrone, "drone");
            if(!checkStation(idStation))
                throw new IDAL.DO.IdNotFoundException(idStation, "station");

            DroneCharge dc = new DroneCharge();
            Station s;         

            dc.DroneId =idDrone;
            dc.StationId = idStation;
            droneCharges.Add(dc);
            foreach (Station station in stations)
            {
                if (station.Id == idStation)
                {
                    s = station;
                    s.ChargeSlots = s.ChargeSlots - 1;
                    stations.Add(s);
                    stations.Remove(station);
                    break;
                }
            }
        }

        /// <summary>
        /// updating of releasing drone from Charging station
        /// </summary>
        /// <param name="id">the drone id</param>
        public void ReleaseDroneFromCharge(int id)
        {
            if (!DataSource.drones.Any(dron => dron.Id == id))
                throw new IDAL.DO.IdNotFoundException(id, "drone");
            Drone d;
            foreach (Drone drone in drones)//unnecessary cause the status isnt here
            {
                if (drone.Id == id)
                {
                    d = drone;
                    drones.Add(d);
                    drones.Remove(drone);
                    break;
                }
            }

            foreach (DroneCharge charger in droneCharges)
            {
                Station s;
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
                            break;
                        }
                    }
                    droneCharges.Remove(charger);
                }      
            }
        }

        /// <summary>
        ///  this function returns a specific drone from the list
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <returns>Drone element</returns>
        public Drone GetDrone(int id)
        {
            if (!checkDrone(id))
                throw new IDAL.DO.IdNotFoundException(id, "drone");
            Drone d = DataSource.drones.Find(drone => drone.Id == id);
            return d;
        }

 
        /// <summary>
        /// this function returns list of drone
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetListOfDrones()
        {
            return from drone in DataSource.drones
                   select drone; ;
        }

        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of drone</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        public bool checkDrone(int id)
        {
            return DataSource.drones.Any(drone=>drone.Id == id);
        }
    }
}
