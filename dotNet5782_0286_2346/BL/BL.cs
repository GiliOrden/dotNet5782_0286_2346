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
                if(droneForList.DroneStatus==EnumsBL.DroneStatuses.Maintenance)
                {
                    int location = rand.Next(dl.GetListOfBaseStations().Count());
                    droneForList.Location.Longitude = dl.GetListOfBaseStations()[location];
                    droneForList.BatteryStats = rand.Next(21);
                }
                  
                drones.Add(droneForList);
            }
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
            dl.SetCustomer(id, customer);
        }

        public void SendingDroneForCharging(IBL.BO.Drone drone)
        {
            IDAL.DO.Drone d2 =new();
            foreach (IDAL.DO.Drone d in dalDrones)
            {
                if (d.Id == drone.Id)
                {
                    if (d.Status == IDAL.DO.Statuses.Available)
                    {
                        foreach (IDAL.DO.Station s in dl.GetListOfAvailableChargingStations())
                        {










                        }
                           

                    }
                }
            }
            
        }
        public static double Radians(double x)
        {
            return x * Math.PI / 180;
        }
        // cos(d) = sin(φА)·sin(φB) + cos(φА)·cos(φB)·cos(λА − λB),
        //  where φА, φB are latitudes and λА, λB are longitudes
        // Distance = d * R
        public static double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
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
