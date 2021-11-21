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
        /// Adding customer element to the customers list
        /// </summary>
        /// <param name="p">element ,Parcel tipe, we adding the list</param>
        public int AddParcel(Parcel p)
        {
            p.Id = Config.CodeOfParcel++;
            parcels.Add(p);
            return p.Id;
        }
        /// <summary>
        /// updating of assign parcel to the drone which will deliver it
        /// </summary>
        /// <param name="parcelId">the id of parcel</param>
        /// <param name="droneId">the id of drone</param>
        public void AssignParcelToDrone(int parcelId, int droneId)
        {
            foreach (Drone drone in drones)
            {
                if (drone.Id == droneId)
                {
                    Drone d = drone;
                    drones.Add(d);
                    drones.Remove(drone);
                    break;
                }
            }

            foreach (Parcel parcel in parcels)
            {
                if (parcel.Id == parcelId)
                {
                    Parcel p = parcel;
                    p.DroneId = drones[drones.Count - 1].Id;
                    p.Scheduled = DateTime.Now;
                    parcels.Add(p);
                    parcels.Remove(parcel);
                    break;
                }
            }
        }
        /// <summary>
        /// updating of collection parcel  by drone
        /// </summary>
        /// <param name="id">the id of parcel</param>
        public void CollectParcelByDrone(int id)
        {
            Parcel p;
            foreach (Parcel parcel in parcels)
            {
                if (parcel.Id == id)
                {
                    p = parcel;
                    p.PickedUp = DateTime.Now;
                    parcels.Add(p);
                    parcels.Remove(parcel);
                    break;
                }
            }

        }
        /// <summary>
        /// updating of supplying delivery to customer
        /// </summary>
        /// <param name="id">the id of parcel</param>
        public void SupplyDeliveryToCustomer(int id)
        {
            Parcel p;
            foreach (Parcel parcel in parcels)
            {
                if (parcel.Id == id)
                {
                    p = parcel;
                    p.Delivered = DateTime.Now;
                    foreach (Drone drone in drones)
                    {
                        if (drone.Id == p.DroneId)
                        {
                            Drone d = drone;
                            drones.Add(d);
                            drones.Remove(drone);
                            break;
                        }
                    }
                    parcels.Add(p);
                    parcels.Remove(parcel);
                    break;
                }
            }
        }


        /// <summary>
        ///  this function returns a specific parcel from the list 
        /// </summary>
        /// <param name="id">the parcel id</param>
        /// <returns>Parcel element</returns>
        public Parcel GetParcel(int id)
        {
            Parcel p = new Parcel();
            foreach (Parcel parcel in parcels)
            {
                if (parcel.Id == id)
                {
                    return parcel;
                }
            }
            return p;
        }

        /// <summary>
        /// this function returns list of parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetListOfParcels()
        {
            List<Parcel> p = new List<Parcel>();
            for (int i = 0; i < parcels.Count; i++)
                p.Add(parcels[i]);
            return p;
        }
        /// <summary>
        /// this function returns list of all the parsels which aren't associated to drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetListOfNotAssociatedParsels()
        {
            List<Parcel> p = new List<Parcel>();
            foreach (Parcel parcel in parcels)
            {
                if (parcel.DroneId == 0)
                    p.Add(parcel);
            }
            return p;
        }


    }
}
