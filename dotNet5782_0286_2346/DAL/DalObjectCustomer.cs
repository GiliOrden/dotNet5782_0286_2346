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
        public void AddCustomer(Customer c)
        {
            if(checkCustomer(c.Id))
                throw new IDAL.DO.ExistIdException(c.Id, "customer");
            customers.Add(c);
        }

        public Customer GetCustomer(int id)
        {
            if (!checkCustomer(id))
                throw new IDAL.DO.IdNotFoundException(id, "customer");
            Customer c = DataSource.customers.Find(cust=>cust.Id==id);
            return c;
        }

        /// <summary>
        /// the function returns list of customers
        /// </summary>
        public IEnumerable<Customer>GetListOfCustomers()
        {
            return from customer in DataSource.customers
                   select customer;
        }

        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of customer</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkCustomer(int id)
        {
            return DataSource.customers.Any(cust => cust.Id == id);
        }

        public void DeleteCustomer(int id)
        {
            foreach(Customer c in customers)
            {
                if (c.Id == id)
                    customers.Remove(c);
                break;
            }
        }
        
    }
}

