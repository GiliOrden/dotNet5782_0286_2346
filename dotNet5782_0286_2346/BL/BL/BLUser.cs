using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using System.Runtime.CompilerServices;
using static BO.EnumsBL;

namespace BL
{
    sealed partial class BL : IBL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(User u)
        {
           DO.User user = new();
            try
            {
                user.Name = u.Name;
                user.Password = u.Password;
                user.Status = (DO.UserStatuses)u.Status;
                dl.AddUser(user);
            }
            catch (DO.ExistUserException ex)
            {
                throw new ExistUserException(ex.Password, ex.UserName,ex.ToString());
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetListOfUsers()
        {
            IEnumerable<User> users =
                (IEnumerable<User>)(from user in dl.GetListOfUsers()
                select user);
            return users;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.User GetUser(string name,string password)
        {
            User u = new BO.User();
            try
            {
                DO.User u1 = dl.GetUser(name,password);
                u.Name = u1.Name;
                u.Password = u1.Password;
                u.Status = (EnumsBL.UserStatuses)u1.Status;
            }
            catch (DO.UserNotFoundException ex)
            {
                throw new UserNotFoundException(ex.Password,ex.UserName);
            }
            return u;
        }
    };
}