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

                    droneForList.BatteryStats = rand.NextDouble() * ( , 100);
                }

                //If the drone is in maintenance, its location will be drawn between the existing stations
                else if (droneForList.DroneStatus==EnumsBL.DroneStatuses.Maintenance)
                {
                    int index = rand.Next(dl.GetListOfBaseStations().Count());
                    droneForList.Location.Longitude = dl.GetListOfBaseStations().ElementAt(index).Longitude;
                    droneForList.Location.Latitude = dl.GetListOfBaseStations().ElementAt(index).Latitude;    
                    droneForList.BatteryStats = rand.Next(21);
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
        public void addCustomer(int id, string name, string phone, IBL.BO.Location location)
        {
            IDAL.DO.Customer customer = new();
            customer.Id = id;
            customer.Name = name; 
            customer.Phone = phone;
            customer.Latitude = location.Latitude;
            customer.Longitude = location.Longitude;
            dl.AddCustomer(customer);
        }

        public void addParcel(int senderId, int receiverId, IBL.BO.EnumsBL.WeightCategories weight, IBL.BO.EnumsBL.Priorities property)
        {
            IDAL.DO.Parcel parcel = new();
            parcel.SenderId = senderId;
            parcel.TargetId = receiverId;
            parcel.Weight = (IDAL.DO.WeightCategories)weight;
            parcel.Priority = (IDAL.DO.Priorities)property;
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
            dl.SetCustomer(id, customer);//where is the UpdateCustomer function?
        }

        public void SendingDroneForCharging(IBL.BO.Drone drone)
        {
            if (drone.DroneStatus == EnumsBL.DroneStatuses.Available)
            {
                IDAL.DO.Station minDistanceStation;
                double minDis = 1000000;
                foreach (IDAL.DO.Station s in dl.GetListOfAvailableChargingStations())
                {
                    double distance = DistanceBetweenPlaces(s.Longitude, s.Latitude, drone.Location.Longitude, drone.Location.Latitude);
                    
                    if ((distance < minDis) &&(s.ChargeSlots>0))
                    {
                        minDis = distance;
                        minDistanceStation = s;
                    }
                }      
                if(minDis != 1000000)
                {
                    if (emptyDronePowerConsumption * minDis < drone.BatteryStatus)
                    {
                        drone.BatteryStatus -= (int)(emptyDronePowerConsumption * minDis);
                        drone.Location = minDistanceStation.LocationOfStation;//need to change station for BL kind?
                        drone.DroneStatus = EnumsBL.DroneStatuses.Maintenance;
                        dl.GetBaseStation(minDistanceStation.Id).ChargeSlots -= 1;//it's by value, build a help function or change station for BL kind
                        .Add( new DroneInCharging() {droneInCharging.Id=drone.Id, droneInCharging.BatteryStatus = drone.BatteryStatus });//what the name of the list??


                    }
                    else
                    {
                        throw new ExceptionsBL.NoBatteryException(drone.Id);
                    }
                }

            }
           
        }
        public void CollectingParcelByDrones(Drone drone)
        {




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
