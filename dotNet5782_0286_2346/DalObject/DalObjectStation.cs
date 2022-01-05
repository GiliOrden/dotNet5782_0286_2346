using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dal.DataSource;
using System.Runtime.CompilerServices;
namespace Dal
{
     sealed partial class DalObject : IDal
    {

        /// <summary>
        /// Adding station element to the stations list
        /// </summary>
        /// <param name="s">  element ,Station type, we adding the list</param>
        public void AddStation(Station s)
        {
            if (checkStation(s.Id))
                throw new DO.ExistIdException(s.Id, "station");
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
                throw new DO.IdNotFoundException(id, "station");
            Station s = Dal.DataSource.stations.Find(stat => stat.Id == id);
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
        
        
       
        public void DeleteStation (int id)
        {
            if (!checkStation(id))
                throw new IdNotFoundException(id, "station");
           Dal.DataSource.stations.RemoveAll(station => station.Id == id);
        }
        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of station</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkStation(int id)
        {
            return Dal.DataSource.stations.Any(sta => sta.Id ==id);
        }


        public IEnumerable<Station> GetStationsByPredicate(Predicate<Station> predicate)
        {
            return from stat in stations
                   where predicate(stat)
                   select stat;
        }


    }
}