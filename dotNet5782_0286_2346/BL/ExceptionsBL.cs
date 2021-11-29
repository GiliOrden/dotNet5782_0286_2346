using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{

    class ExceptionsBL : Exception
    {
        
            [Serializable]
            public class NoBatteryException: Exception//when trying to add an id of station/drone/parcel and it allready exists 
            {
                public int ID;
                public string EntityName;
                public NoBatteryException(int id) : base() { ID = id; }
                public NoBatteryException(int id,  string message) : base(message) { ID = id; }
                public NoBatteryException(int id,  string message, Exception inner) : base(message, inner) { ID = id;  }

                public override string ToString() => base.ToString() + $"The drone Id: {ID} does not have enough battery to be sent to a base station for charging";
            }



        }
}
