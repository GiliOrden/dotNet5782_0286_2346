using System;
using System.Collections.Generic;

using System.Linq;
using IBL.BO;
using IDAL;
namespace BL
{
    public partial class BL : IBL.IBL
    {
        internal double chargingRatePerHour;
        internal double emptyDronePowerConsumption;
        //should it be public?
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
            
            double minDistance=10000000;
            double distance;
            int idOfStation=0;

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

                    //If the parcel was associated but not collected - location will be at the station closest to the sender
                    if (parcel.PickedUp== default(DateTime))
                    {
                        foreach (IDAL.DO.Station baseStation in dl.GetListOfBaseStations())
                        {
                            distance = DistanceBetweenPlaces(baseStation.Longitude, baseStation.Latitude, sender.Longitude, sender.Latitude);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                idOfStation = baseStation.Id;
                            }
                        }
                        droneForList.Location.Longitude = dl.GetBaseStation(idOfStation).Longitude;
                        droneForList.Location.Latitude = dl.GetBaseStation(idOfStation).Latitude;
                    }

                    //If the package was collected  but wasn't delivered- location will be at the sender
                    if (parcel.PickedUp != default(DateTime) && parcel.Delivered == default(DateTime))
                    {
                        droneForList.Location.Longitude = sender.Longitude;
                        droneForList.Location.Latitude = sender.Latitude;
                    }

                    droneForList.BatteryStatus = rand.NextDouble() * ( , 100);
                }

                //If the drone is in maintenance, its location will be drawn between the existing stations
                else if (droneForList.DroneStatus==EnumsBL.DroneStatuses.Maintenance)
                {
                    int index = rand.Next(dl.GetListOfBaseStations().Count());
                    droneForList.Location.Longitude = dl.GetListOfBaseStations().ElementAt(index).Longitude;
                    droneForList.Location.Latitude = dl.GetListOfBaseStations().ElementAt(index).Latitude;    
                    droneForList.BatteryStatus = rand.Next(21);
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
*/
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

            }

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
                    if (emptyDronePowerConsumption * minDis < drone.BatteryStatus)
                    {
                        drone.BatteryStatus -= emptyDronePowerConsumption * minDis;
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
                        d.BatteryStatus-=DistanceBetweenPlaces(senderLocation.Longitude, senderLocation.Latitude, d.Location.Longitude, d.Location.Latitude)* emptyDronePowerConsumption;
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
                        d.BatteryStatus -= DistanceBetweenPlaces(targetLocation.Longitude, targetLocation.Latitude, d.Location.Longitude, d.Location.Latitude) * consumption;
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
            IDAL.DO.Station sDal = dl.GetBaseStation(id);
            s.ID = sDal.Id;
            s.Name = sDal.Name;
            s.Location.Latitude = sDal.Latitude;
            s.Location.Longitude = sDal.Longitude;
            s.ChargeSlots = sDal.ChargeSlots;
            foreach (IDAL.DO.DroneCharge droneCharge in droneCharges)//need to get the list droneCharges


            return s;
        }

        public Drone GetDrone(int id)//i didn't finished!
        {
            Drone d = new();
            ParcelInTransfer parcelInTransfer = new();
            foreach (DroneForList d2 in dronesBL)
            {
                if (d2.Id == id)
                {
                    d.Id = d2.Id;
                    d.Model = d2.Model;
                    d.MaxWeight = d2.MaxWeight;
                    d.BatteryStatus = d2.BatteryStatus;
                    d.DroneStatus = d2.DroneStatus;
                    d.Location = d2.Location;
                    foreach (IDAL.DO.Parcel p in dl.GetListOfParcels())
                    {
                        if (p.Id == d2.IdOfTheDeliveredParcel)
                        {

                        }
                    }
                    d.ParcelInTransfer = parcelInTransfer;
                }
            }
           

            return d;
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
