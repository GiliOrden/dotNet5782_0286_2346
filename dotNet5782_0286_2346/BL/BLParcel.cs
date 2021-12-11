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
        public void AddParcel(Parcel p)
        {
           
                IDAL.DO.Parcel parcel = new();
                parcel.SenderId = p.Sender.ID;
                parcel.TargetId = p.Receiver.ID;
                parcel.Weight = (IDAL.DO.WeightCategories)p.Weight;
                parcel.Priority = (IDAL.DO.Priorities)p.Priority;
                parcel.DroneId = 0;//supposed to be null
                parcel.Requested = DateTime.Now;
                parcel.Scheduled = default;
                parcel.PickedUp = default;
                parcel.Delivered = default;
                dl.AddParcel(parcel);
          
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
                    //if the parcel picked up but have not reached its destination
                    if ((dl.GetParcel(d.IdOfTheDeliveredParcel).PickedUp !=default(DateTime)) && (dl.GetParcel(d.IdOfTheDeliveredParcel).Delivered == default(DateTime)))
                    {
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

        public IEnumerable<IBL.BO.ParcelForList> GetListOfNotAssociatedParcels()
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
             var parcels =
                from parcel in dl.GetListOfParcels()
                select new IBL.BO.ParcelForList
                {
                    Id = parcel.Id,
                    SenderName = dl.GetCustomer(parcel.SenderId).Name,
                    ReceiverName = dl.GetCustomer(parcel.TargetId).Name,
                    Weight = (EnumsBL.WeightCategories)parcel.Weight,
                    Priority = (EnumsBL.Priorities)parcel.Priority,
                };
            var listOfParcels=parcels.ToList();
            foreach (ParcelForList parc in listOfParcels)
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
            return listOfParcels;
        }
        public Parcel GetParcel(int id)
        {
            Parcel p = new();
            p.Sender= new();
            p.Receiver = new();
            p.Drone = new();
            p.Drone.Location = new();
            IDAL.DO.Parcel p2 = dl.GetParcel(id);
            p.Id = p2.Id;
            p.Weight = (EnumsBL.WeightCategories)p2.Weight;
            p.Priority = (EnumsBL.Priorities)p2.Priority;
            p.ParcelCreationTime = p2.Requested;
            p.AssociationTime = p2.Scheduled;
            p.CollectionTime = p2.PickedUp;
            p.DeliveryTime = p2.Delivered;
            p.Sender.ID=p2.SenderId;
            p.Sender.Name = dl.GetCustomer(p2.SenderId).Name;
            p.Receiver.ID = p2.TargetId;
            p.Receiver.Name = dl.GetCustomer(p2.TargetId).Name;
            if (p.AssociationTime != DateTime.MinValue)
            {
                foreach (DroneForList d in dronesBL)
                {
                    if (d.IdOfTheDeliveredParcel == id)
                    {
                        p.Drone.Id = d.Id;
                        p.Drone.Battery = d.Battery;
                        p.Drone.Location = d.Location;
                    }
                }
            }
            else
                p.Drone.Id = 0;
            return p;
        }

        /// <summary>
        /// the function recieves a customer's ID and returns all the parcels he recieved/will receive according to the current data
        /// </summary>
        /// <param name="cstId"></param>
        /// <returns></returns>
        private IEnumerable<ParcelAtCustomer> GetParcelsIntendedToME(int cstId)
        {
            return from parc in dl.GetParcelsAtCustomerByPredicate(parc => parc.TargetId == cstId)
                   let prc = dl.GetParcel(parc.Id)
                   select new ParcelAtCustomer()
                   {
                       Id = prc.Id,
                       Weight = (EnumsBL.WeightCategories)prc.Weight,
                       Priority = (EnumsBL.Priorities)prc.Priority,
                       Status = StatusOfParcel(prc.Id),
                       OtherSide = OtherSideCustomerInParcel(parc.Id, cstId)
                   };
        }

        /// <summary>
        /// the function recieves a customer's ID and returns all the parcels he sent/will send according to the current data
        /// </summary>
        /// <param name="cstId"></param>
        /// <returns>IEnumerable<ParcelAtCustomer></returns>
        private IEnumerable<ParcelAtCustomer> GetParcelsFromMe(int cstId)
        {
            return from parc in dl.GetParcelsAtCustomerByPredicate(parc => parc.SenderId == cstId)
                   let prc = dl.GetParcel(parc.Id)
                   select new ParcelAtCustomer() 
                   { 
                       Id = prc.Id,
                       Weight = (EnumsBL.WeightCategories)prc.Weight,
                       Priority = (EnumsBL.Priorities)prc.Priority,
                       Status = StatusOfParcel(prc.Id),
                       OtherSide = OtherSideCustomerInParcel(parc.Id, cstId)
                   };
        }

        /// <summary>
        /// the function recieves a parcel's ID and returns the parcel's status
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns>status of the parcel </returns>
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

        /// <summary>
        /// the function recieves a parcel's ID and customer's ID and returns the customer of the other side of the customer is received 
        /// as parameter sender-receiver/receiver-sender
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="customerId"></param>
        /// <returns>CustomerInParcel</returns>
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
