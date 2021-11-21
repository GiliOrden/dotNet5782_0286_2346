
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
        /// <param name="s">  element ,Station tipe, we adding the list</param>
        public void AddStation(Station s)
        {
            if (DataSource.stations.Any(sta => sta.Id == s.Id))
                throw new IDAL.DO.Exceptions.ExistIdException(s.Id, "station");
            stations.Add(s);
        }

        /// <summary>
        /// this function returns a specific station from the list
        /// </summary>
        /// <param name="id">the station id</param>
        /// <returns>Station element</returns>
        public Station GetBaseStation(int id)
        {
            if (!DataSource.stations.Any(sta => sta.Id == id))
                throw new IDAL.DO.Exceptions.IdNotFoundException(id, "station");
            foreach (Station baseStaion in stations)
            {
                if (baseStaion.Id == id)
                {
                    return baseStaion;
                }
            }
            Station s=new();//the function doesn't supposed to come here
            return s;
        }



        /// <summary>
        ///  this function returns list of stations 
        /// </summary>
        /// <returns>list of stations </returns>
        public IEnumerable<Station> GetListOfBaseStations()
        {
            List<Station> s = new List<Station>();
            for (int i = 0; i < stations.Count; i++)
                s.Add(stations[i]);
            return s;
        }


        /// <summary>
        /// this function returns list of all the available charging stations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetListOfAvailableChargingStations()
        {
            List<Station> s = new List<Station>();
            foreach (Station baseStaion in stations)
            {
                if (baseStaion.ChargeSlots != 0)
                    s.Add(baseStaion);
            }
            return s;
        }

        /// <summary>
        ///  returns station element whose the id is its 
        /// </summary>
        /// <param name="id">id of station</param>
        /// <returns></returns>
        public Station DistanceFromStation(int id)
        {

            if (!DataSource.stations.Any(sta => sta.Id == id))
                throw new IDAL.DO.Exceptions.IdNotFoundException(id, "station");
            
            foreach (Station station in stations)
            {

                if (station.Id == id)
                {
                    return station;

                }
            }
            Station s = new Station();
            return s;
        }

    }
}