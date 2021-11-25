using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
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
        List<Drone> drones = new List<Drone>();
        IEnumerable<IDAL.DO.Drone> dalDrones;
        public BL()//ctor
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            dl = new DalObject.DalObject();
            dalDrones = dl.GetListOfDrones();
            dronePowerConsumption = dl.getDronePowerConsumption();
            emptyDronePowerConsumption = dronePowerConsumption[0];
            lightWeightCarrierPowerConsumption = dronePowerConsumption[1];
            mediumWeightCarrierPowerConsumption = dronePowerConsumption[2];
            heavyWeightCarrierPowerConsumption = dronePowerConsumption[3];
            chargingRatePerHour = dronePowerConsumption[4];
            foreach(var drone in dalDrones)
            {
                drones.Add(new DroneForList()
                { 
                  Id = drone.Id,
                  Model = drone.Model,
                  MaxWeight=(EnumsBL.WeightCategories)drone.MaxWeight,
                    DroneStatus = (dl.GetListOfParcels().Any(Parcel => Parcel.Id == drone.Id)) ? EnumsBL.DroneStatuses.OnDelivery : rand.Next((int)EnumsBL.DroneStatuses.Maintance),
                  BatteryStatus =
                  
        public int NumberOfTheDeliveredParcel { get; set; }
        public Location Location { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }

        public void ReceiveNewCustomer(int id, string name, string phone, IBL.BO.Location location)
        {
            IDAL.DO.Customer customer = new();
            customer.Id = id;
            customer.Name = name;
            customer.Phone = phone;
            customer.Latitude = location.Latitude;
            customer.Longitude = location.Longitude;
            dl.AddCustomer(customer);
        }

        public void ReceiveNewParcel(int senderId, int receiverId, IBL.BO.EnumsBL.WeightCategories weight, IBL.BO.EnumsBL.Priorities property)
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

        public void UpdatingCustomerData(int id, string name, string phone) //is there a chance the function will get only 2 values?
        {                                                                                          
            IDAL.DO.Customer customer = dl.GetCustomer(id);
            if (name != null)
                customer.Name = name;
            if (phone != null)
                customer.Phone = phone;
            dl.SetCustomer(id, customer);
        }

        public void SendingDroneForCharging(int id)
        {
            IDAL.DO.Drone drone = dl.GetDrone(id);
            if(drone.Status== IDAL.DO.Statuses.Available)
            {

            }
        }
    };
                    
           
}
