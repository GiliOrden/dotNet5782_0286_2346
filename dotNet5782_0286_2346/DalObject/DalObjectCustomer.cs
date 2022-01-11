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
        public void AddCustomer(Customer c)
        {
            if(checkCustomer(c.Id))
                throw new  ExistIdException(c.Id, "customer");
            customers.Add(c);
        }

        public Customer GetCustomer(int id)
        {
            if (!checkCustomer(id))
                throw new IdNotFoundException(id, "customer");
            Customer c = customers.Find(cust=>cust.Id==id);
            return c;
        }

        /// <summary>
        /// the function returns list of customers
        /// </summary>
        public IEnumerable<Customer>GetListOfCustomers()
        {
            return from customer in customers
                   select customer;
        }

        /// <summary>
        /// the function check an ID
        /// </summary>
        /// <param name="id">ID of customer</param:>
        /// <returns>true if the id exists in the list otherwise it returns false </returns>
        private bool checkCustomer(int id)
        {
            return customers.Any(cust => cust.Id == id);
        }

        public void DeleteCustomer(int id)
        {
           if (!checkCustomer(id))
                throw new IdNotFoundException(id, "customer");
            customers.RemoveAll(cus => cus.Id == id);
        }

        
    }
}

