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
  
        public void AddDrone(Drone d)
        {
            if (DataSource.drones.Any(dron => dron.Id == d.Id))
                throw new IDAL.DO.Exceptions.ExistIdException(d.Id, "drone");
            drones.Add(d);
        }
        public double[] GetDronePowerConsumption()
        {
            double[] status = new double[5];
            status[0]=Config.EmptyDronePowerConsumption;//should it be public?
            status[1] = Config.LightWeightCarrierPowerConsumption;
            status[2]= Config.MediumWeightCarrierPowerConsumption;
            status[3]=Config. HeavyWeightCarrierPowerConsumption;
            status[4] =Config.ChargingRatePerHour;
            return status;
        }

        public void SendDroneToCharge(int id, int id2)//there is problem with the function
        {
            Drone d;
            Station s;
            DroneCharge dc = new DroneCharge();
            foreach (Drone drone in drones)
            {
                if (drone.Id == id)
                {
                    d = drone;
                    dc.DroneId = d.Id;
                    dc.StationId = id2;
                    drones.Add(d);
                    drones.Remove(drone);
                    break;
                }
                droneCharges.Add(dc);
            }
            if (!DataSource.drones.Any(dron => dron.Id == id))
                throw new IDAL.DO.Exceptions.IdNotFoundException(id, "drone");
            foreach (Station station in stations)
            {
                if (station.Id == id2)
                {
                    s = station;
                    s.ChargeSlots = s.ChargeSlots - 1;
                    stations.Add(s);
                    stations.Remove(station);
                    break;
                }
            }
            if (!DataSource.stations.Any(sta => sta.Id == id2))
                throw new IDAL.DO.Exceptions.IdNotFoundException(id2, "station");
        }
        /// <summary>
        /// updating of releasing drone from Charging station
        /// </summary>
        /// <param name="id">the drone id</param>
        public void ReleaseDroneFromCharge(int id)
        {
            Drone d;
            foreach (Drone drone in drones)
            {
                if (drone.Id == id)
                {
                    d = drone;
                    drones.Add(d);
                    drones.Remove(drone);
                    break;
                }
            }
            if (!DataSource.drones.Any(dron => dron.Id == id))
                throw new IDAL.DO.Exceptions.IdNotFoundException(id, "drone");
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
                    if (!DataSource.stations.Any(sta => sta.Id == id))
                        throw new IDAL.DO.Exceptions.IdNotFoundException(id, "station");
                }
                droneCharges.Remove(charger);
            }
        }

        /// <summary>
        ///  this function returns a specific drone from the list
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <returns>Drone element</returns>
        public Drone GetDrone(int id)
        {
            if (!DataSource.drones.Any(dron => dron.Id == id))
                throw new IDAL.DO.Exceptions.IdNotFoundException(id, "drone");
            foreach (Drone drone in drones)
            {
                if (drone.Id == id)
                {
                    return drone;
                }
            }
            Drone d = new();
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
    }
}
