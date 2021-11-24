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
            if(DataSource.customers.Any(cust => cust.Id == c.Id))
                throw new IDAL.DO.Exceptions.ExistIdException(c.Id, "customer");
            customers.Add(c);
        }

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
            Customer c = new();//i think it shouldnt be here because of th try
            return c;
        }

        public IEnumerable<Customer>GetListOfCustomers()
        {
            return from customer in DataSource.customers
                   select customer;
        }

    
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
            Customer c = new();//i think it shouldnt be here because of th try
            return c;

        }
    }
}

