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
            drones.Add(d);
        }


        /// <summary>
        /// updating of sending drone to staion for charging
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <param name="id2">the station id</param>
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
                    d.Status = DroneStatuses.Maintenance;
                    dc.DroneId = d.Id;
                    dc.StationId = id2;
                    drones.Add(d);
                    drones.Remove(drone);
                    break;
                }
                droneCharges.Add(dc);
            }
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
                    d.Battery = 100;
                    d.Status = DroneStatuses.Available;
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
            Drone d = new Drone();
            foreach (Drone drone in drones)
            {
                if (drone.Id == id)
                {
                    return drone;
                }
            }
            return d;
        }

 
        /// <summary>
        /// this function returns list of drone
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetListOfDrones()
        {
            List<Drone> d = new List<Drone>();
            for (int i = 0; i < drones.Count; i++)
                d.Add(drones[i]);
            return d;
        }
    }
}
