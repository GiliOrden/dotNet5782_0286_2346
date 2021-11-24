using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO

    {
        public static class Tools
        {
            public static string ToStringProperty<T>(this T t)
            {
                string str = "";
                foreach (PropertyInfo item in t.GetType().GetProperties())
                    str += "\n" + item.Name +
                ": " + item.GetValue(t, null);
                return str;
            }

           /* public static string ToSexagesimal<T>(this T t)
            {
                string str = "";
                foreach(PropertyInfo item in t.GetType().GetProperties())
                {
                    if (item.Name=="Longitude")
                    {

                        string longitude, latitude;
                        double absValOfDegree = Math.Abs(item.GetValue());
                        double minute = (absValOfDegree - (int)absValOfDegree) * 60;//a formula that converts the decimal value of a coordinate to it sexagesimal value
                        longitude = string.Format("{0}° {1}\' {2}\" {3}", (int)Longitude, (int)(minute), (int)Math.Round((minute - (int)minute) * 60), Longitude < 0 ? "S" : "N");
                        absValOfDegree = Math.Abs(Latitude);
                        minute = (absValOfDegree - (int)absValOfDegree) * 60;
                        latitude = string.Format("{0}° {1}\' {2}\" {3}", (int)Latitude, (int)(minute), (int)Math.Round((minute - (int)minute) * 60), Latitude < 0 ? "W" : "E");
                    }
                }

            }*/

        }
    }
}
