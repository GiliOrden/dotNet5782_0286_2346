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
        Random rand = new Random(DateTime.Now.Millisecond);
        public BL()//ctor
        {
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
            int index;
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
                    droneForList.Battery = rand.Next((int)(minCharge+1), 100);
                }

                //If the drone is in maintenance, its location will be drawn between the existing stations
                else if (droneForList.DroneStatus==EnumsBL.DroneStatuses.Maintenance)
                {
                    index = rand.Next(dl.GetListOfBaseStations().Count());
                    droneForList.Location.Longitude = dl.GetListOfBaseStations().ElementAt(index).Longitude;
                    droneForList.Location.Latitude = dl.GetListOfBaseStations().ElementAt(index).Latitude;    
                    droneForList.Battery = rand.Next(21);
                }

                else //if the drone is available
                {
                    //Its location will be raffled off among customers who have packages provided to them
                    IEnumerable<int>customersWhoHaveParcelsDeliveredToThem = from cust in GetListOfCustomers()
                                                               where cust.ReceivedParcels!=0
                                                               select cust.ID;
                    index = rand.Next(customersWhoHaveParcelsDeliveredToThem.Count());
                    droneForList.Location.Longitude = dl.GetCustomer(customersWhoHaveParcelsDeliveredToThem.ElementAt(index)).Longitude;
                    droneForList.Location.Latitude = dl.GetCustomer(customersWhoHaveParcelsDeliveredToThem.ElementAt(index)).Latitude;
                    getClosestStation(dl.GetCustomer(customersWhoHaveParcelsDeliveredToThem.ElementAt(index)),ref minDistance);
                    minCharge = minDistance * emptyDronePowerConsumption;
                    droneForList.Battery = rand.Next((int)(minCharge + 1), 100);
                }
                  
                dronesBL.Add(droneForList);
            }
        }


        private static double Radians(double x)
        {
            return x * Math.PI / 180;
        }

        // cos(d) = sin(φА)·sin(φB) + cos(φА)·cos(φB)·cos(λА − λB),
        //  where φА, φB are latitudes and λА, λB are longitudes
        // Distance = d * R
        private static double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
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
