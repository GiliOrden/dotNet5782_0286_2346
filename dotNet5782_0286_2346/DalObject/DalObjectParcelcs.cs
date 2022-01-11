using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dal.DataSource;
using DalApi;
using System.Runtime.CompilerServices;
namespace Dal
{
    sealed partial class DalObject : IDal
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
            if (!checkDrone(droneId))
                throw new DO.IdNotFoundException(droneId, "drone");
            Drone d = Dal.DataSource.drones.Find(drone => drone.Id == droneId);
            if (!checkParcel(parcelId))
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
        }

        /// <summary>
        /// updating of collection parcel  by drone.( update the time of picking up for now)
        /// </summary>
        /// <param name="id">the id of parcel</param>
        public void CollectParcelByDrone(int id)
        {
            Parcel parcel1,parcel2;
            if (!checkParcel(id))
                throw new DO.IdNotFoundException(id, "parcel");
            parcel2 = parcels.Find(parcel => parcel.Id == id);
            parcel1 = parcel2;
            parcel1.PickedUp = DateTime.Now;
            parcels.Remove(parcel2);
            parcels.Add(parcel1);
        }

        /// <summary>
        /// updating of supplying delivery to customer,( update the 'delivered' time of parcel for now)
        /// </summary>
        /// <param name="id">the id of parcel</param>
        public void SupplyDeliveryToCustomer(int id)
        {
            Parcel parcel1, parcel2;
            if (!checkParcel(id))
                throw new DO.IdNotFoundException(id, "parcel");
            parcel2 = parcels.Find(parcel => parcel.Id == id);
            parcel1 = parcel2;
            parcel1.Delivered = DateTime.Now;
            parcels.Remove(parcel2);
            parcels.Add(parcel1);
        }


        /// <summary>
        ///  this function returns a specific parcel from the list 
        /// </summary>
        /// <param name="id">the parcel id</param>
        /// <returns>Parcel element</returns>
        public Parcel GetParcel(int id)
        {
            if (!checkParcel(id))
                throw new DO.IdNotFoundException(id, "parcel");
            Parcel p =Dal.DataSource.parcels.Find(parc=>parc.Id==id);
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
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of parcel</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkParcel(int id)
        {
            return Dal.DataSource.parcels.Any(parc => parc.Id == id);
        }

      

        public IEnumerable<Parcel> GetParcelsByPredicate(Predicate<Parcel> predicate)
        {
            return from pac in parcels
                   where predicate(pac)
                   select pac;
        }
        /// <summary>
        /// this function deletes a parcel from the list
        /// </summary>
        /// <param name="id">the parcel's id</param>
        public void DeleteParcel(int id)
        {
            if (!checkParcel(id))
                throw new DO.IdNotFoundException(id, "parcel");
            foreach (Parcel parcel in parcels)
            {
                if (parcel.Id == id)
                {
                   parcels.Remove(parcel);
                    break;
                }
            }
        }


    }
}
