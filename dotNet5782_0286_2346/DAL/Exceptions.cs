using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    public class Exceptions : Exception
    {
        [Serializable]
        public class ExistIdException : Exception//when trying to add an id of station/drone/parcel and it allready exists 
        {
            public int ID;
            public string EntityName;
            public ExistIdException(int id, string entity) : base() { ID = id; EntityName = entity; }
            public ExistIdException(int id, string entity, string message) : base(message) { ID = id; EntityName = entity; }
            public ExistIdException(int id, string entity, string message, Exception inner) : base(message, inner) { ID = id; EntityName = entity; }

            public override string ToString() => base.ToString() + $"The {EntityName} Id: {ID} , already exists!";
        }

        [Serializable]
        public class IdNotFoundException : Exception//When trying to look for an id of station/drone/parcel and it does not exist 
        {
            public int ID;
            public string EntityName;
            public IdNotFoundException(int id, string entity) : base() { ID = id; EntityName = entity; }
            public IdNotFoundException(int id, string entity, string message) : base(message) { ID = id; EntityName = entity; }
            public IdNotFoundException(int id, string entity, string message, Exception innerException) : base(message, innerException)
              { ID = id; EntityName = entity; }
            public override string ToString() => base.ToString() + $",The {EntityName} Id:{ID} doesn't found";
        }
        public class NameNotFoundException : Exception//When trying to look for a name of station/drone/parcel and it does not exist 
        {
            public string Name;
            public string EntityName;
            public NameNotFoundException(string name, string entity) : base() { Name = name; EntityName = entity; }
            public NameNotFoundException(string name, string entity, string message) : base(message) { Name = name; EntityName = entity; }
            public NameNotFoundException(string name, string entity, string message, Exception innerException) : base(message, innerException)
            { Name = name; EntityName = entity; }
            public override string ToString() => base.ToString() + $",The {EntityName} name:{Name} doesn't found";
        }
    }
}
