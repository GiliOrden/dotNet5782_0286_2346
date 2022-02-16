using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
        [Serializable]
        public class ExistIdException : Exception//when trying to add an id of station/drone/parcel and it allready exists 
        {
            public int ID;
            public string EntityName;
            public ExistIdException(int id, string entity) : base() { ID = id; EntityName = entity; }
            public ExistIdException(int id, string entity, string message) : base(message) { ID = id; EntityName = entity; }
            public ExistIdException(int id, string entity, string message, Exception inner) : base(message, inner) { ID = id; EntityName = entity; }
            protected ExistIdException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        public override string ToString() => base.ToString() + $"The {EntityName} Id: {ID} ,is already exists.";
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
            protected IdNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        public override string ToString() => base.ToString() + $",The {EntityName} Id:{ID} isn't found";
        }
    [Serializable]
    public class UserNotFoundException : Exception//When trying to look for a user and he does not exist 
    {
        public string Password;
        public string UserName;
        public UserNotFoundException(string password, string entity) : base() { Password = password; UserName = entity; }
        public UserNotFoundException(string password, string entity, string message) : base(message) { Password = password; UserName = entity; }
        public UserNotFoundException(string password, string entity, string message, Exception innerException) : base(message, innerException)
        { Password = password; UserName = entity; }
        protected UserNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
        public override string ToString() => base.ToString() + $"The user{UserName} Pass:{Password} isn't found";
    }
    [Serializable]
    public class ExistUserException : Exception//when trying to add a user and he allready exists  
    {
        public string Password;
        public string UserName;
        public ExistUserException(string password, string entity) : base() { Password=password ; UserName = entity; }
        public ExistUserException(string password, string entity, string message) : base(message) { Password = password; UserName = entity; }
        public ExistUserException(string password, string entity, string message, Exception innerException) : base(message, innerException)
        { Password = password; UserName = entity; }
        protected ExistUserException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
        public override string ToString() => base.ToString() + $"The user name {UserName} or the password {Password}are already exists in the system.";
    }
    [Serializable]
    public class XMLFileLoadCreateException : Exception
    {
        private string filePath;
        private string v;
        private Exception ex;

        public XMLFileLoadCreateException()
        {
        }

        public XMLFileLoadCreateException(string message) : base(message)
        {
        }

        public XMLFileLoadCreateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public XMLFileLoadCreateException(string filePath, string v, Exception ex)
        {
            this.filePath = filePath;
            this.v = v;
            this.ex = ex;
        }

        protected XMLFileLoadCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
