using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DalObject.DataSource;
using IDAL;
namespace DalObject
{
    public partial class DalObject : IDal
    {

        /// <summary>
        /// Adding station element to the stations list
        /// </summary>
        /// <param name="s">  element ,Station type, we adding the list</param>
        public void AddStation(Station s)
        {
            if (!checkStation(s.Id))
                throw new IDAL.DO.ExistIdException(s.Id, "station");
            stations.Add(s);
        }

        /// <summary>
        /// this function returns a specific station from the list
        /// </summary>
        /// <param name="id">the station id</param>
        /// <returns>Station element</returns>
        public Station GetBaseStation(int id)
        {
            if (!checkStation(id))
                throw new IDAL.DO.IdNotFoundException(id, "station");
            Station s = DataSource.stations.Find(stat => stat.Id == id);
            return s;
        }

        /// <summary>
        ///  this function returns list of stations 
        /// </summary>
        /// <returns>list of stations </returns>
        public IEnumerable<Station> GetListOfBaseStations()
        {
            return from baseStation in stations
            select baseStation;
        }


        /// <summary>
        /// this function returns list of all the available charging stations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetListOfStationsWithAvailableChargeSlots()
        {
            return from Station baseStation in stations
                   where baseStation.ChargeSlots != 0
                   select baseStation;
        }
        public void DeleteStation (int id)
        {
            if (!checkStation(id))
                throw new IdNotFoundException(id, "station");
            DataSource.stations.RemoveAll(station => station.Id == id);
        }
        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of station</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkStation(int id)
        {
            return DataSource.stations.Any(sta => sta.Id ==id);
        }


    }
}