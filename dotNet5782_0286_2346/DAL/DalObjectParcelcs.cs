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
            if (checkParcel(p.Id))
                throw new IDAL.DO.ExistIdException(p.Id,"parcel");
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
            if (!checkDrone(droneId))
                throw new IDAL.DO.IdNotFoundException(droneId, "drone");
            Drone d = DataSource.drones.Find(drone => drone.Id == droneId);
            if (!checkParcel(parcelId))
                 throw new IDAL.DO.IdNotFoundException(parcelId, "parcel");
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
        }

        /// <summary>
        /// updating of collection parcel  by drone.( update the time of picking up for now)
        /// </summary>
        /// <param name="id">the id of parcel</param>
        public void CollectParcelByDrone(int id)
        {
            Parcel p;
            if (!checkParcel(id))
                throw new IDAL.DO.IdNotFoundException(id, "parcel");
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
        /// updating of supplying delivery to customer,( update the 'delivered' time of parcel for now)
        /// </summary>
        /// <param name="id">the id of parcel</param>
        public void SupplyDeliveryToCustomer(int id)
        {
            Parcel p;
            if (!checkParcel(id))
                throw new IDAL.DO.IdNotFoundException(id, "parcel");
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
                            drone.status = avilable;
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
            if (!checkParcel(id))
                throw new IDAL.DO.IdNotFoundException(id, "parcel");
            Parcel p =DataSource.parcels.Find(parc=>parc.Id==id);
            return p;
        }

        /// <summary>
        /// this function returns list of parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetListOfParcels()
        {
            return from Parcel parc in parcels
                   select parc;
        }

        /// <summary>
        /// this function returns list of all the parsels which aren't associated to drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetListOfNotAssociatedParsels()
        {
            return from parc in parcels
                   where parc.DroneId == 0
                   select parc;
        }
        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of parcel</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkParcel(int id)
        {
            return DataSource.parcels.Any(parc => parc.Id == id);
        }
    }
}
