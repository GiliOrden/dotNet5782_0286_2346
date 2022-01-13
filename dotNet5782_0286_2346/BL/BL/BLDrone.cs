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
        public void AddDrone(BO.DroneForList drone, int idOfStation)
        {
            try
            {
                DO.Drone dalDrone = new();
                dalDrone.Id = drone.Id;
                dalDrone.Model = drone.Model;
                dalDrone.MaxWeight = (DO.WeightCategories)drone.MaxWeight;
                dl.AddDrone(dalDrone);
                if (dl.GetBaseStation(idOfStation).ChargeSlots == 0)
                    throw new NoAvailableChargeSlots(idOfStation);
                dl.SendDroneToCharge(dalDrone.Id, idOfStation);
                drone.Battery = rand.Next(20, 41);
                drone.DroneStatus = BO.EnumsBL.DroneStatuses.Maintenance;
                drone.Location = GetBaseStation(idOfStation).Location;
                dronesBL.Add(drone); 
            }
            catch (DO.ExistIdException ex)
            {
                throw new ExistIdException(ex.ID, ex.EntityName);
            }
            catch (DO.IdNotFoundException ex)
            {
                dl.DeleteDrone(drone.Id);
                throw new IdNotFoundException(ex.ID, ex.EntityName);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int id, string newModel)
        {
            lock (dl)
            {
                try
                {
                    DO.Drone d = dl.GetDrone(id);
                    dronesBL.Find(drone => drone.Id == id).Model = newModel;
                    d.Model = newModel;
                    dl.DeleteDrone(id);
                    dl.AddDrone(d);
                }
                catch (DO.IdNotFoundException ex)
                {
                    throw new IdNotFoundException(ex.ID, ex.EntityName);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneFromCharge(int id, DateTime releaseTime)
        {
            BO.DroneForList drone = dronesBL.FirstOrDefault(drone => drone.Id == id);
            if (drone == null)
                throw new IdNotFoundException(id, "drone");
            if (drone.DroneStatus != BO.EnumsBL.DroneStatuses.Maintenance)
                throw new DroneStatusException(id, "in maintenance");
            TimeSpan chargingTime = dl.GetListOfBusyChargeSlots().FirstOrDefault(dc => dc.DroneId == id).StartOfCharging - releaseTime;
            drone.Battery = drone.Battery + chargingTime.TotalMinutes/60 * chargingRatePerHour;
            if (drone.Battery > 100)
                drone.Battery = 100;
            drone.DroneStatus = BO.EnumsBL.DroneStatuses.Available;
            dl.ReleaseDroneFromCharge(id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AssignParcelToDrone(int idOfDrone)
        {
            int idOfParcel = 0;
            BO.DroneForList drone = dronesBL.FirstOrDefault(drone => drone.Id == idOfDrone);
            double distance;
            double minDistance = 100000;

            if (drone == null)
                throw new IdNotFoundException(idOfDrone, "drone");
            if (drone.DroneStatus != BO.EnumsBL.DroneStatuses.Available)
                throw new DroneStatusException(idOfDrone, "available");

            IEnumerable<DO.Parcel> parcelsThatDroneCanTransfer =
                from parc in dl.GetParcelsByPredicate(p=>p.DroneId == null)
                where checkSufficientPowerToTransmission(drone, parc) == true
                select parc;
            if (parcelsThatDroneCanTransfer.Count() == 0)//if the drone can't transfer any parcel because is low battery
                throw new NoBatteryException(idOfDrone);

            IEnumerable<DO.Parcel> parcelsWithTheHighestPriority = parcelsThatDroneCanTransfer.Where(parc => parc.Priority == DO.Priorities.Emergency);
            if (parcelsWithTheHighestPriority.Count() == 0)//if there aren't parcels with emergency priority that the drone can transfer
            {
                parcelsWithTheHighestPriority = parcelsThatDroneCanTransfer.Where(parc => parc.Priority == DO.Priorities.Fast);
                if (parcelsWithTheHighestPriority.Count() == 0)
                    parcelsWithTheHighestPriority = parcelsThatDroneCanTransfer;
            }

            IEnumerable<DO.Parcel> parcelsWithMaxWeightPossibleToDrone = getParcelsWithMaxWeightPossibleToDrone(parcelsWithTheHighestPriority, idOfDrone);
            if (parcelsWithMaxWeightPossibleToDrone.Count() == 0)
            {
                if (parcelsWithTheHighestPriority.ElementAt(0).Priority == DO.Priorities.Emergency)//if there are parcels with emergency priority that the drone has enough power to carry bat there weight is too heavy
                {
                    parcelsWithMaxWeightPossibleToDrone = parcelsThatDroneCanTransfer.Where(parc => parc.Priority == DO.Priorities.Fast);
                    parcelsWithMaxWeightPossibleToDrone = getParcelsWithMaxWeightPossibleToDrone(parcelsWithTheHighestPriority, idOfDrone);
                    if (parcelsWithMaxWeightPossibleToDrone.Count() == 0)
                    {
                        parcelsWithMaxWeightPossibleToDrone = parcelsThatDroneCanTransfer.Where(parc => parc.Priority == DO.Priorities.Regular);
                        parcelsWithMaxWeightPossibleToDrone = getParcelsWithMaxWeightPossibleToDrone(parcelsWithTheHighestPriority, idOfDrone);
                    }
                }
                else if (parcelsWithTheHighestPriority.ElementAt(0).Priority == DO.Priorities.Fast)
                {
                    parcelsWithMaxWeightPossibleToDrone = parcelsThatDroneCanTransfer.Where(parc => parc.Priority == DO.Priorities.Regular);
                    parcelsWithMaxWeightPossibleToDrone = getParcelsWithMaxWeightPossibleToDrone(parcelsWithTheHighestPriority, idOfDrone);
                }
                if (parcelsWithMaxWeightPossibleToDrone.Count() == 0)//if there are no parcels that the drone can transfer becuse its max weight
                    throw new DroneMaxWeightIsLowException(idOfDrone);
            }

            foreach (DO.Parcel parcel in parcelsWithMaxWeightPossibleToDrone)
            {
                distance = DistanceBetweenPlaces(drone.Location.Longitude, drone.Location.Latitude, dl.GetCustomer(parcel.SenderId).Longitude, dl.GetCustomer(parcel.SenderId).Latitude);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    idOfParcel = parcel.Id;
                }
            }

            dl.AssignParcelToDrone(idOfParcel, idOfDrone);
            drone.DroneStatus = BO.EnumsBL.DroneStatuses.OnDelivery;
            drone.IdOfTheDeliveredParcel = idOfParcel;
        }

        /// <summary>
        /// the function receives drone and parcel and check if the drone has enough power to carry the parcel to destination
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="parcel"></param>
        /// <returns>true if the drone has enough power otherwise it returns false</returns>
        private bool checkSufficientPowerToTransmission(BO.DroneForList drone, DO.Parcel parcel)
        {
            double minDistance = 100000;
            double minCharge = 0;
            getClosestStation(dl.GetCustomer(parcel.TargetId), ref minDistance);
            double way = DistanceBetweenPlaces(drone.Location.Longitude, drone.Location.Latitude, dl.GetCustomer(parcel.SenderId).Longitude, dl.GetCustomer(parcel.SenderId).Latitude)
            + DistanceBetweenPlaces(dl.GetCustomer(parcel.SenderId).Longitude, dl.GetCustomer(parcel.SenderId).Latitude, dl.GetCustomer(parcel.TargetId).Longitude, dl.GetCustomer(parcel.TargetId).Latitude)
            + minDistance;
            if (parcel.Weight == DO.WeightCategories.Light)
                minCharge = lightWeightCarrierPowerConsumption * way;
            else if (parcel.Weight == DO.WeightCategories.Medium)
                minCharge = mediumWeightCarrierPowerConsumption * way;
            else if (parcel.Weight == DO.WeightCategories.Heavy)
                minCharge = heavyWeightCarrierPowerConsumption * way;
            return minCharge < drone.Battery;
        }

        /// <summary>
        /// recieves list of parcel and drone's id and returnes parcels that the drone can carry their weight
        /// </summary>
        /// <param name="parcels">list of parcel</param>
        /// <param name="id">drone's id</param>
        /// <returns>IEnumerable<DO.Parcel> parcels that the drone can carry their weight </returns>
        private IEnumerable<DO.Parcel> getParcelsWithMaxWeightPossibleToDrone(IEnumerable<DO.Parcel> parcels, int id)
        {
            IEnumerable<DO.Parcel> parcelsWithMaxWeightPossibleToDrone =
                 from parc in parcels
                 where parc.Weight == dl.GetDrone(id).MaxWeight
                 select parc;
            if (parcelsWithMaxWeightPossibleToDrone.Count() == 0)
            {
                parcelsWithMaxWeightPossibleToDrone =
                from parc in parcels
                where parc.Weight < dl.GetDrone(id).MaxWeight
                select parc;
            }
            return parcelsWithMaxWeightPossibleToDrone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendDroneToCharge(int id)
        {
            BO.Drone drone = GetDrone(id);
            if (drone.DroneStatus == BO.EnumsBL.DroneStatuses.Available)
            {
                DO.Station closestStation = new();
                double minDis = 1000000;              
                foreach (DO.Station s in dl.GetStationsByPredicate(stat => stat.ChargeSlots > 0))
                {
                    double distance = DistanceBetweenPlaces(s.Longitude, s.Latitude, drone.Location.Longitude, drone.Location.Latitude);
                    if (distance < minDis)
                    {
                        minDis = distance;
                        closestStation = s;
                    }
                }
                
                if (emptyDronePowerConsumption * minDis < drone.Battery)
                {
                    BO.DroneForList d = new();
                    d.Id = drone.Id;
                    d.Model = drone.Model;
                    d.MaxWeight = drone.MaxWeight;
                    d.Battery = drone.Battery - emptyDronePowerConsumption * minDis;
                    d.Location = new();
                    d.Location.Latitude = closestStation.Latitude;
                    d.Location.Longitude = closestStation.Longitude;
                    d.DroneStatus = BO.EnumsBL.DroneStatuses.Maintenance;
                    dl.SendDroneToCharge(drone.Id, closestStation.Id);
                    dronesBL.RemoveAll(dr => dr.Id == id);
                    dronesBL.Add(d);
                }
                else
                {
                    throw new NoBatteryException(drone.Id);
                }
            }
            else
                throw new DroneStatusException(id, "available");

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BO.DroneForList> GetListOfDrones()
        {
            return from drone in dronesBL
                   select drone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BO.DroneForList> GetDronesByPredicate(Predicate<BO.DroneForList> predicate)
        {
            return from drone in dronesBL
                   where predicate(drone)
                   select drone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Drone GetDrone(int id)
        {
            BO.Drone d = new();
            if (!dronesBL.Any(drone => drone.Id == id))
                throw new IdNotFoundException(id, "drone");
            BO.DroneForList d2 = dronesBL.Find(drone => drone.Id == id);
            d.Id = d2.Id;
            d.Model = d2.Model;
            d.MaxWeight = d2.MaxWeight;
            d.Battery = d2.Battery;
            d.DroneStatus = d2.DroneStatus;
            d.Location = new BO.Location();
            d.Location = d2.Location;
            if (d.DroneStatus == BO.EnumsBL.DroneStatuses.OnDelivery)
            {
                BO.ParcelInTransfer parcelInTransfer = new();
                parcelInTransfer.Sender = new();
                parcelInTransfer.Receiver = new();
                DO.Parcel p = dl.GetParcel(d2.IdOfTheDeliveredParcel);
                parcelInTransfer.Id = p.Id;
                if (p.PickedUp == null)
                    parcelInTransfer.OnTheWay = false;
                else
                    parcelInTransfer.OnTheWay = true;
                parcelInTransfer.Weight = (BO.EnumsBL.WeightCategories)p.Weight;
                parcelInTransfer.Priority = (BO.EnumsBL.Priorities)p.Priority;
                double lat1 = dl.GetCustomer(p.SenderId).Latitude;
                parcelInTransfer.Source = new BO.Location();
                parcelInTransfer.Source.Latitude = lat1;
                double lon1 = dl.GetCustomer(p.SenderId).Longitude;
                parcelInTransfer.Source.Longitude = lon1;
                double lat2 = dl.GetCustomer(p.TargetId).Latitude;
                parcelInTransfer.Destination = new BO.Location();
                parcelInTransfer.Destination.Latitude = lat2;
                double lon2 = dl.GetCustomer(p.TargetId).Longitude;
                parcelInTransfer.Destination.Longitude = lon2;
                parcelInTransfer.TransportDistance = DistanceBetweenPlaces(lon1, lat1, lon2, lat2);
                parcelInTransfer.Sender.ID=p.SenderId;
                parcelInTransfer.Sender.Name = dl.GetCustomer(p.SenderId).Name;
                parcelInTransfer.Receiver.ID = p.TargetId;
                parcelInTransfer.Receiver.Name = dl.GetCustomer(p.TargetId).Name;
                d.ParcelInTransfer = parcelInTransfer;
            }

            return d;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]

        public BO.DroneForList GetDroneForList(int id)
        {
            return dronesBL.Find(drone => drone.Id == id);
        }

    }
}
