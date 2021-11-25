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
        #endregion

        #region Drone
        //void addDrone(BO.Drone drone);
        void SendingDroneForCharging(int id);
        #endregion

        #region Location
        #endregion

        #region Customer
        BO.Customer ReceiveNewCustomer(int id, string name, int phone, BO.Location location);
        BO.Customer UpdatingCustomerData(int id, string name, int phone);
        #endregion

        #region Parcel
        BO.Parcel ReceiveNewParcel(int senderId, int receiverId, BO.EnumsBL.WeightCategories weight, BO.EnumsBL.Priorities property);
        #endregion




    }
}
