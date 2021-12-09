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

        public void AddDrone(DroneForList drone, int idOfStation)
        {
            try
            {
                IDAL.DO.Drone dalDrone = new();
                dalDrone.Id = drone.Id;
                dalDrone.Model = drone.Model;
                dalDrone.MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight;
                dl.AddDrone(dalDrone);
                dl.SendDroneToCharge(dalDrone.Id, idOfStation);
                drone.Battery = rand.Next(20, 41);
                drone.DroneStatus = EnumsBL.DroneStatuses.Maintenance;
                drone.Location = GetBaseStation(idOfStation).Location;
                dronesBL.Add(drone); 
            }
            catch (IDAL.DO.ExistIdException ex)
            {
                throw new IBL.BO.ExistIdException(ex.ID, ex.EntityName);
            }
            catch (IDAL.DO.IdNotFoundException ex)
            {
                dl.DeleteDrone(drone.Id);
                throw new IBL.BO.IdNotFoundException(ex.ID, ex.EntityName);
            }
        }

        public void UpdateDrone(int id, string newModel)
        {
            try
            {
                IDAL.DO.Drone d = dl.GetDrone(id);
                dronesBL.Find(drone => drone.Id == id).Model = newModel;
                d.Model = newModel;
                dl.DeleteDrone(id);
                dl.AddDrone(d);
            }
            catch (IDAL.DO.IdNotFoundException ex)
            {
                throw new IBL.BO.IdNotFoundException(ex.ID, ex.EntityName);
            }
        }


        public void ReleaseDroneFromCharge(int id, double chargingTime)
        {
            DroneForList drone = dronesBL.FirstOrDefault(drone => drone.Id == id);
            if (drone == null)
                throw new IBL.BO.IdNotFoundException(id, "drone");
            if (drone.DroneStatus != EnumsBL.DroneStatuses.Maintenance)
                throw new IBL.BO.DroneStatusException(id, "in maintenance");

            drone.Battery = drone.Battery + chargingTime * chargingRatePerHour;
            if (drone.Battery > 100)
                drone.Battery = 100;
            drone.DroneStatus = EnumsBL.DroneStatuses.Available;
            dl.ReleaseDroneFromCharge(id);
        }

        public void AssignParcelToDrone(int idOfDrone)
        {

            int idOfParcel = 0;
            DroneForList drone = dronesBL.FirstOrDefault(drone => drone.Id == idOfDrone);
            double distance;
            double minDistance = 100000;

            if (drone == null)
                throw new IBL.BO.IdNotFoundException(idOfDrone, "drone");
            if (drone.DroneStatus != EnumsBL.DroneStatuses.Available)
                throw new IBL.BO.DroneStatusException(idOfDrone, "available");

            IEnumerable<IDAL.DO.Parcel> parcelsThatDroneCanTransfer =
                from parc in dl.GetListOfNotAssociatedParcels()
                where checkSufficientPowerToTransmission(drone, parc) == true
                select parc;
            if (parcelsThatDroneCanTransfer.Count() == 0)
                throw new IBL.BO.NoBatteryException(idOfDrone);

            IEnumerable<IDAL.DO.Parcel> parcelsWithTheHighestPriority = parcelsThatDroneCanTransfer.Where(parc => parc.Priority == IDAL.DO.Priorities.Emergency);
            if (parcelsWithTheHighestPriority.Count() == 0)
            {
                parcelsWithTheHighestPriority = parcelsThatDroneCanTransfer.Where(parc => parc.Priority == IDAL.DO.Priorities.Fast);
                if (parcelsWithTheHighestPriority.Count() == 0)
                    parcelsWithTheHighestPriority = parcelsThatDroneCanTransfer;
            }

            IEnumerable<IDAL.DO.Parcel> parcelsWithMaxWeightPossibleToDrone = getParcelsWithMaxWeightPossibleToDrone(parcelsWithTheHighestPriority, idOfDrone);
            if (parcelsWithMaxWeightPossibleToDrone.Count() == 0)
            {
                if (parcelsWithTheHighestPriority.ElementAt(0).Priority == IDAL.DO.Priorities.Emergency)
                {
                    parcelsWithMaxWeightPossibleToDrone = parcelsThatDroneCanTransfer.Where(parc => parc.Priority == IDAL.DO.Priorities.Fast);
                    parcelsWithMaxWeightPossibleToDrone = getParcelsWithMaxWeightPossibleToDrone(parcelsWithTheHighestPriority, idOfDrone);
                    if (parcelsWithMaxWeightPossibleToDrone.Count() == 0)
                    {
                        parcelsWithMaxWeightPossibleToDrone = parcelsThatDroneCanTransfer.Where(parc => parc.Priority == IDAL.DO.Priorities.Regular);
                        parcelsWithMaxWeightPossibleToDrone = getParcelsWithMaxWeightPossibleToDrone(parcelsWithTheHighestPriority, idOfDrone);
                    }
                }
                else if (parcelsWithTheHighestPriority.ElementAt(0).Priority == IDAL.DO.Priorities.Fast)
                {
                    parcelsWithMaxWeightPossibleToDrone = parcelsThatDroneCanTransfer.Where(parc => parc.Priority == IDAL.DO.Priorities.Regular);
                    parcelsWithMaxWeightPossibleToDrone = getParcelsWithMaxWeightPossibleToDrone(parcelsWithTheHighestPriority, idOfDrone);
                }
                if (parcelsWithMaxWeightPossibleToDrone.Count() == 0)
                    throw new IBL.BO.DroneMaxWeightIsLowException(idOfDrone);
            }
            foreach (IDAL.DO.Parcel parcel in parcelsWithMaxWeightPossibleToDrone)
            {
                distance = DistanceBetweenPlaces(drone.Location.Longitude, drone.Location.Latitude, dl.GetCustomer(parcel.SenderId).Longitude, dl.GetCustomer(parcel.SenderId).Latitude);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    idOfParcel = parcel.Id;
                }
            }

            dl.AssignParcelToDrone(idOfParcel, idOfDrone);
            drone.DroneStatus = EnumsBL.DroneStatuses.OnDelivery;
        }

        private bool checkSufficientPowerToTransmission(DroneForList drone, IDAL.DO.Parcel parcel)
        {
            double minDistance = 100000;
            double minCharge = 0;
            getClosestStation(dl.GetCustomer(parcel.TargetId), ref minDistance);
            double way = DistanceBetweenPlaces(drone.Location.Longitude, drone.Location.Latitude, dl.GetCustomer(parcel.SenderId).Longitude, dl.GetCustomer(parcel.SenderId).Latitude)
            + DistanceBetweenPlaces(dl.GetCustomer(parcel.SenderId).Longitude, dl.GetCustomer(parcel.SenderId).Latitude, dl.GetCustomer(parcel.TargetId).Longitude, dl.GetCustomer(parcel.TargetId).Latitude)
            + minDistance;
            if (parcel.Weight == IDAL.DO.WeightCategories.Light)
                minCharge = lightWeightCarrierPowerConsumption * way;
            else if (parcel.Weight == IDAL.DO.WeightCategories.Medium)
                minCharge = mediumWeightCarrierPowerConsumption * way;
            else if (parcel.Weight == IDAL.DO.WeightCategories.Heavy)
                minCharge = heavyWeightCarrierPowerConsumption * way;
            return minCharge < drone.Battery;
        }

        private IEnumerable<IDAL.DO.Parcel> getParcelsWithMaxWeightPossibleToDrone(IEnumerable<IDAL.DO.Parcel> parcels, int id)
        {
            IEnumerable<IDAL.DO.Parcel> parcelsWithMaxWeightPossibleToDrone =
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


        public void SendDroneToCharge(int id)
        {
            foreach (DroneForList drone in dronesBL)
            {
                if ((drone.Id == id) && (drone.DroneStatus == EnumsBL.DroneStatuses.Available))
                {
                    IDAL.DO.Station minDistanceStation = new();
                    double minDis = 1000000;
                    foreach (IDAL.DO.Station s in dl.GetListOfStationsWithAvailableChargeSlots())
                    {
                        double distance = DistanceBetweenPlaces(s.Longitude, s.Latitude, drone.Location.Longitude, drone.Location.Latitude);
                        if ((distance < minDis) && (s.ChargeSlots > 0))
                        {
                            minDis = distance;
                            minDistanceStation = s;
                        }
                    }
                    if (emptyDronePowerConsumption * minDis < drone.Battery)
                    {
                        drone.Battery -= emptyDronePowerConsumption * minDis;
                        drone.Location.Latitude = minDistanceStation.Latitude;
                        drone.Location.Longitude = minDistanceStation.Longitude;
                        drone.DroneStatus = EnumsBL.DroneStatuses.Maintenance;
                        dl.SendDroneToCharge(drone.Id, minDistanceStation.Id);

                    }
                    else
                    {
                        throw new NoBatteryException(drone.Id);
                    }
                }
            }
        }


        public IEnumerable<IBL.BO.DroneForList> GetListOfDrones()
        {
            return from drone in dronesBL
                   select drone;
        }

        public Drone GetDrone(int id)
        {
            Drone d = new();
            try
            {
                ParcelInTransfer parcelInTransfer = new();
                ParcelAtCustomer sender = new();
                ParcelAtCustomer recipient = new();
                CustomerInParcel senderOtherSide = new();
                CustomerInParcel recipientOtherSide = new();
                foreach (DroneForList d2 in dronesBL)
                {
                    if (d2.Id == id)
                    {
                        d.Id = d2.Id;
                        d.Model = d2.Model;
                        d.MaxWeight = d2.MaxWeight;
                        d.Battery = d2.Battery;
                        d.DroneStatus = d2.DroneStatus;
                        d.Location = d2.Location;
                        if (d.DroneStatus == EnumsBL.DroneStatuses.OnDelivery)
                        {
                            IDAL.DO.Parcel p = dl.GetParcel(id);
                            parcelInTransfer.Id = p.Id;
                            if (p.PickedUp == DateTime.MinValue)
                                parcelInTransfer.Status = false;
                            else
                                parcelInTransfer.Status = true;
                            parcelInTransfer.Weight = (EnumsBL.WeightCategories)p.Weight;
                            parcelInTransfer.Priority = (EnumsBL.Priorities)p.Priority;
                            double lat1 = dl.GetCustomer(p.SenderId).Latitude;
                            parcelInTransfer.Source = new Location();
                            parcelInTransfer.Source.Latitude = lat1;
                            double lon1 = dl.GetCustomer(p.SenderId).Longitude;
                            parcelInTransfer.Source.Longitude = lon1;
                            double lat2 = dl.GetCustomer(p.TargetId).Latitude;
                            parcelInTransfer.Destination = new Location();
                            parcelInTransfer.Destination.Latitude = lat2;
                            double lon2 = dl.GetCustomer(p.TargetId).Longitude;
                            parcelInTransfer.Destination.Longitude = lon2;
                            parcelInTransfer.TransportDistance = DistanceBetweenPlaces(lon1, lat1, lon2, lat2);
                            sender.Id = recipient.Id = p.Id;
                            sender.Weight = recipient.Weight = (EnumsBL.WeightCategories)p.Weight;
                            sender.Priority = recipient.Priority = (EnumsBL.Priorities)p.Priority;
                            sender.Status = recipient.Status = StatusOfParcel(p.Id);
                            senderOtherSide.ID = p.TargetId;
                            recipientOtherSide.ID = p.SenderId;
                            senderOtherSide.Name = dl.GetCustomer(senderOtherSide.ID).Name;
                            recipientOtherSide.Name = dl.GetCustomer(recipientOtherSide.ID).Name;
                            sender.OtherSide = senderOtherSide;
                            recipient.OtherSide = recipientOtherSide;
                            parcelInTransfer.Sender = sender;
                            parcelInTransfer.Receiver = recipient;
                        }
                        d.ParcelInTransfer = parcelInTransfer;
                    }
                }
            }
            catch (IDAL.DO.IdNotFoundException ex)
            {
                throw new IdNotFoundException(ex.ID, "drone");
            }

            return d;
        }
        private IEnumerable<DroneInCharging> GetdronesInChargingPerStation(int id, Location location)//id and location of a base station 
        {
            return from d in GetDroneInChargingByPredicate(d =>d.Location.Longitude == location.Longitude )
                   let dronesInCharging = GetDrone(d.Id)
                   select new DroneInCharging()
                   {
                       Id = dronesInCharging.Id,
                       Battery = dronesInCharging.Battery
                   };
        }
        private IEnumerable<DroneForList> GetDroneInChargingByPredicate(Predicate<DroneForList> predicate)
        {
            return from sic in dronesBL
                   where predicate(sic)
                   select sic;
        }
    }
}
