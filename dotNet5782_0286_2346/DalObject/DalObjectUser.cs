using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dal.DataSource;
using DalApi;
using System.Runtime.CompilerServices;
namespace Dal
{
    sealed partial class DalObject : IDal
    {
        public void AddUser(User u)
        {
            if (checkUser(u.Name,u.Password))
                throw new ExistUserException(u.Password, u.Name);
           users.Add(u);
        }

        public User GetUser(string name,string password)
        {
            if (!checkUser(name,password))
                throw new UserNotFoundException(password,name);
            User  u= users.Find(user => user.Name==name);
            return u;
        }

        
        public IEnumerable<User> GetListOfUsers()
        {
            return from user in users
                   select user;
        }

        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of customer</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkUser(string name,string pass)
        {
            return users.Any(user => user.Name ==name||user.Password==pass);
        }

        public void DeleteUser(string name,string password)
        {

            if (!checkUser(name, password))
                throw new UserNotFoundException(password, name);
            users.RemoveAll(user=>user.Name==name);
        }


    }
}
