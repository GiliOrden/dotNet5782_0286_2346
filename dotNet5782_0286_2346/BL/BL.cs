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
        List<DroneForList> drones = new List<DroneForList>();
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
                droneForList.DroneStatus = (dl.GetListOfParcels().Any(Parcel => Parcel.DroneId == drone.Id)) ? EnumsBL.DroneStatuses.OnDelivery : (EnumsBL.DroneStatuses)rand.Next(2);//Random value between available and maintenance

                if (droneForList.DroneStatus == EnumsBL.DroneStatuses.OnDelivery)
                {
                    IDAL.DO.Parcel parcel = dl.GetListOfParcels().FirstOrDefault(parc => parc.DroneId == droneForList.Id);
                    IDAL.DO.Customer sender = dl.GetCustomer(parcel.SenderId);

                    //If the package was associated but not collected - location will be at the station closest to the sender
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
                  
                drones.Add(droneForList);
            }
        }

        public void addBaseStation(Station station)
        {
            IDAL.DO.Station dalStation=new();
            dalStation.Id = station.ID;
            dalStation.Name = station.Name;
            dalStation.Longitude = station.Location.Longitude;
            dalStation.Latitude = station.Location.Latitude;
            dalStation.ChargeSlots = station.AvailableChargeSlots;
            dl.AddStation(dalStation);
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

        public void SendDroneToChargeBL(int id)
        {
            foreach (DroneForList drone in drones)
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
                        throw new ExceptionsBL.NoBatteryException(drone.Id);
                    }
                }
            }
        }




        public void CollectingParcelByDronesBL(int id)
        {
            int parcelId;
            foreach(DroneForList d in drones)
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
                        throw new ExceptionsBL.DroneCanNotCollectParcelException(id, parcelId);
                }
             }
        }

        public void SupplyDeliveryToCustomerBL(int droneId)
        {
            foreach (DroneForList d in drones)
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
                        throw new ExceptionsBL.DroneCanNotSupplyDeliveryToCustomerException(droneId, d.IdOfTheDeliveredParcel);
                }
            }
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
