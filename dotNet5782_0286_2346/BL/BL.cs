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
        internal double chargingRatePerHour;
        internal double emptyDronePowerConsumption;
        internal double lightWeightCarrierPowerConsumption;
        internal double mediumWeightCarrierPowerConsumption;
        internal double heavyWeightCarrierPowerConsumption;
        double[] dronePowerConsumption;
        IDal dl;
        List<DroneForList> dronesBL = new List<DroneForList>();
        IEnumerable<IDAL.DO.Drone> dalDrones;

        public BL()//ctor
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            dl = new DalObject.DalObject();
            dalDrones = dl.GetListOfDrones();
            dronePowerConsumption = dl.GetDronePowerConsumption();
            emptyDronePowerConsumption = dronePowerConsumption[0];
            lightWeightCarrierPowerConsumption = dronePowerConsumption[1];
            mediumWeightCarrierPowerConsumption = dronePowerConsumption[2];
            heavyWeightCarrierPowerConsumption = dronePowerConsumption[3];
            chargingRatePerHour = dronePowerConsumption[4];
            double way;
            double minDistance = 10000;
            double minCharge=0;
            foreach (var drone in dalDrones)
            {
                DroneForList droneForList = new DroneForList();
                droneForList.Id = drone.Id;
                droneForList.Model = drone.Model;
                droneForList.MaxWeight = (EnumsBL.WeightCategories)drone.MaxWeight;

                //If there are parcels that have not yet been delivered but the drone has already been associated the drone status is OnDelivery
                droneForList.DroneStatus = (dl.GetListOfParcels().Any(parc => parc.DroneId == drone.Id && parc.Delivered==default(DateTime))) ? EnumsBL.DroneStatuses.OnDelivery : (EnumsBL.DroneStatuses)rand.Next(2);//Random value between available and maintenance
                
                if (droneForList.DroneStatus == EnumsBL.DroneStatuses.OnDelivery)
                {
                    IDAL.DO.Parcel parcel = dl.GetListOfParcels().FirstOrDefault(parc => parc.DroneId == droneForList.Id && parc.Delivered == default(DateTime));
                    IDAL.DO.Customer sender = dl.GetCustomer(parcel.SenderId);
                    IDAL.DO.Customer receiver = dl.GetCustomer(parcel.TargetId);
                    //If the parcel was associated but not collected - location will be at the station closest to the sender
                    if (parcel.PickedUp== default(DateTime))
                    {
                        droneForList.Location.Longitude = dl.GetBaseStation(getClosestStation(sender,ref minDistance)).Longitude;
                        droneForList.Location.Latitude = dl.GetBaseStation(getClosestStation(sender, ref minDistance)).Latitude;
                    }

                    //If the package was collected  but wasn't delivered- location will be at the sender
                    if (parcel.PickedUp != default(DateTime) && parcel.Delivered == default(DateTime))
                    {
                        droneForList.Location.Longitude=sender.Longitude;
                        droneForList.Location.Latitude = sender.Latitude;
                    }
    
                    if (parcel.PickedUp == default(DateTime))
                    {
                       way= DistanceBetweenPlaces(sender.Longitude, sender.Latitude, dl.GetBaseStation(getClosestStation(sender, ref minDistance)).Longitude, dl.GetBaseStation(getClosestStation(sender, ref minDistance)).Latitude)
                          + DistanceBetweenPlaces(sender.Longitude, sender.Latitude, receiver.Longitude, receiver.Latitude) +
                          DistanceBetweenPlaces(receiver.Longitude,receiver.Latitude, dl.GetBaseStation(getClosestStation(receiver, ref minDistance)).Longitude, dl.GetBaseStation(getClosestStation(receiver, ref minDistance)).Latitude);
                    } 
                    else
                    {
                       getClosestStation(receiver, ref minDistance);
                       way = DistanceBetweenPlaces(sender.Longitude, sender.Latitude, receiver.Longitude, receiver.Latitude) + minDistance;
                    }                      
                    if (parcel.Weight == IDAL.DO.WeightCategories.Light)
                        minCharge = lightWeightCarrierPowerConsumption * way;
                    else if (parcel.Weight == IDAL.DO.WeightCategories.Medium)
                        minCharge = mediumWeightCarrierPowerConsumption * way;
                    else if (parcel.Weight == IDAL.DO.WeightCategories.Heavy)
                        minCharge = heavyWeightCarrierPowerConsumption * way;
                    droneForList.Battery = rand.Next((int)minCharge + 1, 100);
                }

                //If the drone is in maintenance, its location will be drawn between the existing stations
                else if (droneForList.DroneStatus==EnumsBL.DroneStatuses.Maintenance)
                {
                    int index = rand.Next(dl.GetListOfBaseStations().Count());
                    droneForList.Location.Longitude = dl.GetListOfBaseStations().ElementAt(index).Longitude;
                    droneForList.Location.Latitude = dl.GetListOfBaseStations().ElementAt(index).Latitude;    
                    droneForList.Battery = rand.Next(21);
                }

                else //if the drone is available
                {
                  //  מיקומו יוגרל בין לקוחות שיש חבילות שסופקו להם

                }
                  
                dronesBL.Add(droneForList);
            }
        }
        /*ם יש חבילהות שעוד לא סופקו אך הרחפן כבר שויך
○	מצב הרחפן יהיה כמבצע משלוח
○	מיקום הרחפן יהיה כדלקמן:
■	אם החבילה שויכה אך לא נאספה - מיקום יהיה בתחנה הקרובה לשולח
■	אם החבילה נאספה אך עוד לא סופקה - מיקום הרחפן יהיה במיקום השולח
○	מצב סוללה יוגרל בין טעינה מינימלית שתאפשר לרחפן לבצע את המשלוח ולהגיע לטעינה לתחנה הקרובה ליעד המשלוח לבין טעינה מלאה
●	אם הרחפן לא מבצע משלוח
○	מצבו יוגרל בין תחזוקה לפנוי
●	אם הרחפן בתחזוקה
○	מיקומו יוגרל בין תחנות התחנות הקיימות
○	מצב סוללה יוגרל בין 0% ל-20%
●	אם הרחפן פנוי
○	מיקומו יוגרל בין לקוחות שיש חבילות שסופקו להם
○	מצב סוללה יוגרל בין טעינה מינימלית שתאפשר לו להגיע לתחנה הקרובה לטעינה לבין טעינה מלאה
*/      private int getClosestStation(IDAL.DO.Customer customer,ref double minDistance)//help method
        {
            int idOfStation=0;
            double distance;
            foreach (IDAL.DO.Station baseStation in dl.GetListOfBaseStations())
            {
                distance = DistanceBetweenPlaces(baseStation.Longitude, baseStation.Latitude, customer.Longitude, customer.Latitude);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    idOfStation = baseStation.Id;
                }
            }
            return idOfStation;
        }
        public void addBaseStation(Station station)
        {
            IDAL.DO.Station dalStation=new();
            dalStation.Id = station.ID;
            dalStation.Name = station.Name;
            dalStation.Longitude = station.Location.Longitude;
            dalStation.Latitude = station.Location.Latitude;
            dalStation.ChargeSlots = station.ChargeSlots;
            try 
            { 
                dl.AddStation(dalStation);
            }
            catch (IDAL.DO.ExistIdException ex)
            {
                throw new IBL.BO.ExistIdException(ex.ID, ex.EntityName);
            }
        }

        public void addDrone(DroneForList drone,int idOfStation)
        {
            try 
            {
                IDAL.DO.Drone dalDrone = new();
                dalDrone.Id = drone.Id;
                dalDrone.Model = drone.Model;
                dalDrone.MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight;
                dl.AddDrone(dalDrone);
                //Sends the drone for initial charging
                dl.SendDroneToCharge(dalDrone.Id, idOfStation);
                dronesBL.Add(drone);//in the main i should fill its extra fields  
            }
            catch(IDAL.DO.ExistIdException ex)
            {
                throw new IBL.BO.ExistIdException(ex.ID, ex.EntityName);
            }  
            catch(IDAL.DO.IdNotFoundException ex)
            {
                dl.RemoveDrone(drone.Id);
                throw new IBL.BO.IdNotFoundException(ex.ID, ex.EntityName);
            }
        }

        public void UpdateDrone(int id,string newModel)
        {
            if (!dl.checkDrone(id))
                throw new IBL.BO.IdNotFoundException(id, "drone");
            dronesBL.Find(drone => drone.Id == id).Model = newModel;   
            IDAL.DO.Drone d = dl.GetDrone(id);
            d.Model = newModel;
            dl.RemoveDrone(id);
            dl.AddDrone(d);
        }
        //I'm going to delete this note
        //Note that the total numOfChargeSlots is the sum of the number of available positions and the number of skimmers in the charge
        public void UpdateBaseStation(int id,string name,int numOfChargeSlots)
        {
            if(!dl.checkStation(id))
              throw new IBL.BO.IdNotFoundException(id, "station");
            IDAL.DO.Station s = dl.GetBaseStation(id);
            if (name != " ")
                s.Name = name;
            if(numOfChargeSlots!=-1)
            {
              //need to continue
            }
        }
        public void ReleaseDroneFromCharge(int id,)
        {

        }
        public void AssignParcelToDrone(int idOfDrone)
        {

        }

        public IEnumerable<IBL.BO.StationForList> GetListOfBaseStations()
        {
            return from station in dl.GetListOfBaseStations()
                   select new IBL.BO.StationForList
                   {
                       ID = station.Id,
                       Name = station.Name,
                       AvailableChargingPositions=,
                       In
                   };
        }
        public void addCustomer(Customer c)
        {
            IDAL.DO.Customer customer = new();
            customer.Id = c.Id;
            customer.Name = c.Name; 
            customer.Phone = c.Phone;
            customer.Latitude = c.Location.Latitude;
            customer.Longitude = c.Location.Longitude;
            dl.AddCustomer(customer);
        }

        public void addParcel(Parcel p)
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

        public void UpdatinCustomerData(int id, string name, string phone) //is there a chance the function will get only 2 values?
        {                                                                                          
            IDAL.DO.Customer customer = dl.GetCustomer(id);
            if (name != null)
                customer.Name = name;
            if (phone != null)
                customer.Phone = phone;
            dl.UpdateCustomer(customer);//where is the UpdateCustomer function?
        }

        public void SendDroneToCharge(int id)
        {
            foreach (DroneForList drone in dronesBL)
            {
                if ((drone.Id == id)&& (drone.DroneStatus == EnumsBL.DroneStatuses.Available))
                {
                    IDAL.DO.Station minDistanceStation=new();
                    double minDis = 1000000;
                    foreach (IDAL.DO.Station s in dl.GetListOfAvailableChargingStations())
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




        public void CollectingParcelByDrones(int id)
        {
            int parcelId;
            foreach(DroneForList d in dronesBL)
            {
                if (d.Id == id)
                {
                    parcelId = d.IdOfTheDeliveredParcel;
                    if (dl.GetParcel(parcelId).PickedUp == DateTime.MinValue)//true=the parcel wasn't collected yet
                    {
                        Location senderLocation = new();
                        senderLocation.Latitude = dl.GetCustomer(dl.GetParcel(parcelId).SenderId).Latitude;
                        senderLocation.Longitude = dl.GetCustomer(dl.GetParcel(parcelId).SenderId).Longitude;
                        d.Battery-=DistanceBetweenPlaces(senderLocation.Longitude, senderLocation.Latitude, d.Location.Longitude, d.Location.Latitude)* emptyDronePowerConsumption;
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
                if (d.Id == droneId) {
                    if ((dl.GetParcel(d.IdOfTheDeliveredParcel).PickedUp != DateTime.MinValue) && (dl.GetParcel(d.IdOfTheDeliveredParcel).Delivered == DateTime.MinValue))
                    {//the parcel picked up but have not reached its destination
                        Location targetLocation = new();
                        targetLocation.Latitude = dl.GetCustomer(dl.GetParcel(d.IdOfTheDeliveredParcel).TargetId).Latitude;
                        targetLocation.Longitude = dl.GetCustomer(dl.GetParcel(d.IdOfTheDeliveredParcel).TargetId).Longitude;
                        double consumption= dronePowerConsumption[(int)dl.GetParcel(d.IdOfTheDeliveredParcel).Weight + 1];
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

        public Station GetBaseStation(int id)//i didn't finished!
        {
            Station s = new();
            DroneInCharging droneInCharging = new();
            IDAL.DO.Station sDal = dl.GetBaseStation(id);
            s.ID = sDal.Id;
            s.Name = sDal.Name;
            s.Location.Latitude = sDal.Latitude;
            s.Location.Longitude = sDal.Longitude;
            s.ChargeSlots = sDal.ChargeSlots;
            List<DroneInCharging> droneInChargingList=new();
            foreach (DroneForList d in drones)
            {
                if (s.Location == d.Location)//the drone and the station have the same location(if a drone in a station it have to be in charging?)
                {
                    droneInCharging.Id = d.Id;
                    droneInCharging.Battery = d.Battery;
                    droneInChargingList.Add(droneInCharging);
                }
            }
            s.DroneInChargingList = droneInChargingList;
            return s;
        }

        public Drone GetDrone(int id)
        {
            Drone d = new();
            ParcelInTransfer parcelInTransfer = new();
            ParcelAtCustomer sender = new();
            ParcelAtCustomer recipient = new();
            CustomerInParcel senderOtherSide = new();
            CustomerInParcel recipientOtherSide = new();
            foreach (DroneForList d2 in drones)
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
                        parcelInTransfer.Source.Latitude = lat1;
                        double lon1 = dl.GetCustomer(p.SenderId).Longitude;
                        parcelInTransfer.Source.Longitude = lon1;
                        double lat2 = dl.GetCustomer(p.TargetId).Latitude;
                        parcelInTransfer.Destination.Latitude = lat2;
                        double lon2 = dl.GetCustomer(p.TargetId).Longitude;
                        parcelInTransfer.Destination.Longitude = lon2;
                        parcelInTransfer.TransportDistance = DistanceBetweenPlaces(lon1, lat1, lon2, lat2);
                        sender.Id = recipient.Id = p.Id;
                        sender.Weight = recipient.Weight = (EnumsBL.WeightCategories)p.Weight;
                        sender.Priority = recipient.Priority = (EnumsBL.Priorities)p.Priority;

                        if (p.Scheduled == DateTime.MinValue)//only definited!
                            sender.Status = recipient.Status = EnumsBL.ParcelStatuses.Defined;
                        else
                        {
                            if (p.PickedUp == DateTime.MinValue)//PickedUp==null, the parcel did not picked up
                                sender.Status = recipient.Status = EnumsBL.ParcelStatuses.Supplied;
                            else
                            {
                                if (p.Delivered == DateTime.MinValue)
                                    sender.Status = recipient.Status = EnumsBL.ParcelStatuses.Collected;
                                else
                                    sender.Status = recipient.Status = EnumsBL.ParcelStatuses.Associated;
                            }

                        }
                        senderOtherSide.ID = p.TargetId;
                        recipientOtherSide.ID = p.SenderId;
                        senderOtherSide.Name = dl.GetCustomer(senderOtherSide.ID).Name;
                        recipientOtherSide.Name = dl.GetCustomer(recipientOtherSide.ID).Name;
                        sender.OtherSide = senderOtherSide;
                        recipient.OtherSide = recipientOtherSide;
                        parcelInTransfer.Sender = sender;
                        parcelInTransfer.Recipient = recipient;
                    }
                    d.ParcelInTransfer = parcelInTransfer;
                }
            }
           return d;
        }

        public Customer GetCustomer(int id)//i did not finish
        {
            Customer c = new();
            IDAL.DO.Customer c2 = dl.GetCustomer(id);
            IEnumerable<ParcelAtCustomer> listOfParcelsFromMe = new IEnumerable<ParcelAtCustomer>();//problem with IEnumerable?
            c.Id = id;
            c.Name = c2.Name;
            c.Phone = c2.Phone;
            c.Location.Latitude = c2.Latitude;
            c.Location.Longitude = c2.Longitude;
            
            return c;

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

            if (p2.Scheduled == DateTime.MinValue)//only definited!
                sender.Status = recipient.Status = EnumsBL.ParcelStatuses.Defined;
            else
            {
                if (p2.PickedUp == DateTime.MinValue)//PickedUp==null, the parcel did not picked up
                    sender.Status = recipient.Status = EnumsBL.ParcelStatuses.Supplied;
                else
                {
                    if (p2.Delivered == DateTime.MinValue)
                        sender.Status = recipient.Status = EnumsBL.ParcelStatuses.Collected;
                    else
                        sender.Status = recipient.Status = EnumsBL.ParcelStatuses.Associated;
                }

            }
            senderOtherSide.ID = p2.TargetId;
            recipientOtherSide.ID = p2.SenderId;
            senderOtherSide.Name = dl.GetCustomer(senderOtherSide.ID).Name;
            recipientOtherSide.Name = dl.GetCustomer(recipientOtherSide.ID).Name;
            sender.OtherSide = senderOtherSide;
            recipient.OtherSide = recipientOtherSide;
            p.Sender = sender;
            p.Receiver = recipient;
            if(p.AssociationTime != DateTime.MinValue)
            {
                DroneForParcel drone = new();
                foreach (DroneForList d in drones)
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




        internal static double Radians(double x)
        {
            return x * Math.PI / 180;
        }
        // cos(d) = sin(φА)·sin(φB) + cos(φА)·cos(φB)·cos(λА − λB),
        //  where φА, φB are latitudes and λА, λB are longitudes
        // Distance = d * R
        internal static double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
        {
            double R = 6371; // km

            double sLat1 = Math.Sin(Radians(lat1));
            double sLat2 = Math.Sin(Radians(lat2));
            double cLat1 = Math.Cos(Radians(lat1));
            double cLat2 = Math.Cos(Radians(lat2));
            double cLon = Math.Cos(Radians(lon1) - Radians(lon2));

            double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;

            double d = Math.Acos(cosD);

            double dist = R * d;

            return dist;
        }
    };
                    
           
}
