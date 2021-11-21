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
        /// Adding customer element to the customers list
        /// </summary>
        /// <param name="c">element ,Customer tipe, we adding the list</param>
        public void AddCustomer(Customer c)
        {
            if(DataSource.customers.Any(cust => cust.Id == c.Id))
                throw new IDAL.DO.Exceptions.ExistIdException(c.Id, "customer");
            customers.Add(c);
        }

        /// <summary>
        ///  this function returns a specific customer from the list
        /// </summary>
        /// <param name="id">the customer id</param>
        /// <returns>Customer element</returns>
        public Customer GetCustomer(int id)
        {
            if (!DataSource.customers.Any(cust => cust.Id == id))
                throw new IDAL.DO.Exceptions.IdNotFoundException(id, "customer");
            foreach (Customer customer in customers)
            {
                if (customer.Id == id)
                {
                    return customer;
                }
            }
            Customer c = new();
            return c;
        }

        /// <summary>
        /// the function returns the list of the customers
        /// </summary>
        /// <returns><"listOfCustomers">
        public IEnumerable<Customer>GetListOfCustomers()
        {
            return from customer in DataSource.customers
                   select customer;
        }

        /// <summary>
        /// returns customer element which the name is his 
        /// </summary>
        /// <param name="name">name of customer</param>
        /// <returns></returns>
        public Customer DistanceFromCustomer(string name)
        {
            if (!DataSource.customers.Any(cust => cust.Name == name))
                throw new IDAL.DO.Exceptions.NameNotFoundException(name, "customer");
            foreach (Customer customer in customers)
            {
                if (customer.Name == name)
                {
                    return customer;

                }
            }
            Customer c = new();
            return c;

        }
    }
}

