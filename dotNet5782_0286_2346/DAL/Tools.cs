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

            public static double FindDistance<T>(this T t,double longitue,double latitude)
            {
                return Math.Sqrt(Math.Pow(t.Longitude - longitude, 2)) + (Math.Pow(t.Latitude - latitude, 2)
            }

        }
    }
}
