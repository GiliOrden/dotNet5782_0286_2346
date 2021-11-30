using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface IBL
    {
        #region BaseStation
        //void addBaseStation(BO.Station station);
        public BO.Station GetBaseStation(int id);
        #endregion

        #region Drone
        //void addDrone(BO.Drone drone);
        //public void SendingDroneForCharging(int id);
        public void SendingDroneForCharging(BO.Drone drone);
        public void CollectingParcelByDrones(int droneId);
        public void SupplyDeliveryToCustomer(int droneId);
        public BO.Drone GetDrone(int id);
        #endregion

        #region Location
        #endregion

        #region Customer
        public void ReceiveNewCustomer(int id, string name, string phone, BO.Location location);
        public void UpdatingCustomerData(int id, string name, string phone);
        public BO.Customer Customer(int id);
        #endregion

        #region Parcel
        public void addParcel(BO.Parcel p);
        public BO.Parcel Parcel(int id);
        #endregion




    }
}
