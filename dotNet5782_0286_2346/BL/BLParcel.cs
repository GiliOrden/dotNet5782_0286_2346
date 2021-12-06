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
        public void addParcel(Parcel p)
        {
            try
            {
                IDAL.DO.Parcel parcel = new();
                parcel.SenderId = p.Sender.Id;
                parcel.TargetId = p.Receiver.Id;
                parcel.Weight = (IDAL.DO.WeightCategories)p.Weight;
                parcel.Priority = (IDAL.DO.Priorities)p.Priority;
                parcel.DroneId = 0;//supposed to be null
                parcel.Requested = DateTime.Now;
                parcel.Scheduled = default;
                parcel.PickedUp = default;
                parcel.Delivered = default;
                dl.AddParcel(parcel);
            }
            catch (IDAL.DO.ExistIdException ex)
            {
                throw new IBL.BO.ExistIdException(ex.ID, ex.EntityName);
            }
        }


        public void CollectingParcelByDrones(int id)
        {
            int parcelId;
            foreach (DroneForList d in dronesBL)
            {
                if (d.Id == id)
                {
                    parcelId = d.IdOfTheDeliveredParcel;
                    if (dl.GetParcel(parcelId).PickedUp == DateTime.MinValue)//true=the parcel wasn't collected yet
                    {
                        Location senderLocation = new();
                        senderLocation.Latitude = dl.GetCustomer(dl.GetParcel(parcelId).SenderId).Latitude;
                        senderLocation.Longitude = dl.GetCustomer(dl.GetParcel(parcelId).SenderId).Longitude;
                        d.Battery -= DistanceBetweenPlaces(senderLocation.Longitude, senderLocation.Latitude, d.Location.Longitude, d.Location.Latitude) * emptyDronePowerConsumption;
                        d.Location = senderLocation;
                        dl.CollectParcelByDrone(parcelId);
                    }
                    else
                        throw new DroneCanNotCollectParcelException(id, parcelId);
                }
            }
        }

        public void SupplyDeliveryToCustomer(int droneId)
        {
            foreach (DroneForList d in dronesBL)
            {
                if (d.Id == droneId)
                {
                    if ((dl.GetParcel(d.IdOfTheDeliveredParcel).PickedUp != DateTime.MinValue) && (dl.GetParcel(d.IdOfTheDeliveredParcel).Delivered == DateTime.MinValue))
                    {//the parcel picked up but have not reached its destination
                        Location targetLocation = new();
                        targetLocation.Latitude = dl.GetCustomer(dl.GetParcel(d.IdOfTheDeliveredParcel).TargetId).Latitude;
                        targetLocation.Longitude = dl.GetCustomer(dl.GetParcel(d.IdOfTheDeliveredParcel).TargetId).Longitude;
                        double consumption = dronePowerConsumption[(int)dl.GetParcel(d.IdOfTheDeliveredParcel).Weight + 1];
                        d.Battery -= DistanceBetweenPlaces(targetLocation.Longitude, targetLocation.Latitude, d.Location.Longitude, d.Location.Latitude) * consumption;
                        d.Location = targetLocation;
                        d.DroneStatus = EnumsBL.DroneStatuses.Available;
                        dl.SupplyDeliveryToCustomer(d.IdOfTheDeliveredParcel);//update the 'delivery' time in parcel for now
                    }
                    else
                        throw new DroneCanNotSupplyDeliveryToCustomerException(droneId, d.IdOfTheDeliveredParcel);
                }
            }
        }

        public IEnumerable<IBL.BO.ParcelForList> GetListOfDParcelsThatHaveNotYetBeenAssignedToDrone()
        {
            return from parcel in dl.GetListOfNotAssociatedParcels()
                   select new IBL.BO.ParcelForList
                   {
                       Id = parcel.Id,
                       SenderName = dl.GetCustomer(parcel.SenderId).Name,
                       ReceiverName = dl.GetCustomer(parcel.TargetId).Name,
                       Weight = (EnumsBL.WeightCategories)parcel.Weight,
                       Priority = (EnumsBL.Priorities)parcel.Priority,
                       ParcelStatus = EnumsBL.ParcelStatuses.Defined
                   };
        }
        public IEnumerable<IBL.BO.ParcelForList> GetListOfParcels()
        {
            IEnumerable<IBL.BO.ParcelForList> parcels =
                from parcel in dl.GetListOfParcels()
                select new IBL.BO.ParcelForList
                {
                    Id = parcel.Id,
                    SenderName = dl.GetCustomer(parcel.SenderId).Name,
                    ReceiverName = dl.GetCustomer(parcel.TargetId).Name,
                    Weight = (EnumsBL.WeightCategories)parcel.Weight,
                    Priority = (EnumsBL.Priorities)parcel.Priority,
                };
            foreach (ParcelForList parc in parcels)
            {
                if (dl.GetParcel(parc.Id).Scheduled == default(DateTime))
                    parc.ParcelStatus = EnumsBL.ParcelStatuses.Defined;
                else if (dl.GetParcel(parc.Id).PickedUp == default(DateTime))
                    parc.ParcelStatus = EnumsBL.ParcelStatuses.Associated;
                else if (dl.GetParcel(parc.Id).Delivered == default(DateTime))
                    parc.ParcelStatus = EnumsBL.ParcelStatuses.Collected;
                else if (dl.GetParcel(parc.Id).Delivered != default(DateTime))
                    parc.ParcelStatus = EnumsBL.ParcelStatuses.Delivered;
            }
            return parcels;
        }
        public Parcel GetParcel(int id)
        {
            Parcel p = new();
            ParcelAtCustomer sender = new();
            ParcelAtCustomer recipient = new();
            CustomerInParcel senderOtherSide = new();
            CustomerInParcel recipientOtherSide = new();
            IDAL.DO.Parcel p2 = dl.GetParcel(id);
            p.Id = p2.Id;
            p.Weight = (EnumsBL.WeightCategories)p2.Weight;
            p.Priority = (EnumsBL.Priorities)p2.Priority;
            p.ParcelCreationTime = p2.Requested;
            p.AssociationTime = p2.Scheduled;
            p.CollectionTime = p2.PickedUp;
            p.DeliveryTime = p2.Delivered;
            sender.Id = recipient.Id = p.Id;
            sender.Weight = recipient.Weight = (EnumsBL.WeightCategories)p.Weight;
            sender.Priority = recipient.Priority = (EnumsBL.Priorities)p.Priority;
            sender.Status = recipient.Status = StatusOfParcel(p.Id);
            senderOtherSide.ID = p2.TargetId;
            recipientOtherSide.ID = p2.SenderId;
            senderOtherSide.Name = dl.GetCustomer(senderOtherSide.ID).Name;
            recipientOtherSide.Name = dl.GetCustomer(recipientOtherSide.ID).Name;
            sender.OtherSide = senderOtherSide;
            recipient.OtherSide = recipientOtherSide;
            p.Sender = sender;
            p.Receiver = recipient;
            if (p.AssociationTime != DateTime.MinValue)
            {
                DroneForParcel drone = new();
                foreach (DroneForList d in dronesBL)
                {
                    if (d.IdOfTheDeliveredParcel == id)
                    {
                        drone.Id = d.Id;
                        drone.Battery = d.Battery;
                        drone.Location = d.Location;
                    }
                }
                p.Drone = drone;
            }
            return p;
        }

        private IEnumerable<ParcelAtCustomer> GetParcelsIntendedToME(int cstId)
        {
            return from sic in dl.GetParcelsAtCustomerByPredicate(sic => sic.TargetId == cstId)
                   let prc = dl.GetParcel(sic.Id)
                   select new ParcelAtCustomer()
                   {
                       Id = prc.Id,
                       Weight = (EnumsBL.WeightCategories)prc.Weight,
                       Priority = (EnumsBL.Priorities)prc.Priority,
                       Status = StatusOfParcel(prc.Id),
                       OtherSide = OtherSideCustomerInParcel(sic.Id, cstId)
                   };
        }
        private IEnumerable<ParcelAtCustomer> GetParcelsFromMe(int cstId)
        {
            return from sic in dl.GetParcelsAtCustomerByPredicate(sic => sic.SenderId == cstId)
                   let prc = dl.GetParcel(sic.Id)
                   select new ParcelAtCustomer()
                   {
                       Id = prc.Id,
                       Weight = (EnumsBL.WeightCategories)prc.Weight,
                       Priority = (EnumsBL.Priorities)prc.Priority,
                       Status = StatusOfParcel(prc.Id),
                       OtherSide = OtherSideCustomerInParcel(sic.Id, cstId)
                   };
        }
        private EnumsBL.ParcelStatuses StatusOfParcel(int parcelId)
        {
            IDAL.DO.Parcel p = dl.GetParcel(parcelId);
            if (p.Scheduled == DateTime.MinValue)//only definited!
                return EnumsBL.ParcelStatuses.Defined;
            if (p.PickedUp == DateTime.MinValue)//PickedUp==null, the parcel did not picked up
                return EnumsBL.ParcelStatuses.Delivered;
            if (p.Delivered == DateTime.MinValue)
                return EnumsBL.ParcelStatuses.Collected;
            return EnumsBL.ParcelStatuses.Associated;
        }

        private CustomerInParcel OtherSideCustomerInParcel(int parcelId, int customerId)
        {
            IDAL.DO.Parcel p = dl.GetParcel(parcelId);
            CustomerInParcel customerInParcel = new();
            if (p.SenderId == customerId)
            {
                customerInParcel.ID = p.TargetId;
                customerInParcel.Name = dl.GetCustomer(p.TargetId).Name;
            }
            else//p.TargetId== customerId
            {
                customerInParcel.ID = p.SenderId;
                customerInParcel.Name = dl.GetCustomer(p.SenderId).Name;
            }
            return customerInParcel;
        }

    };


}
