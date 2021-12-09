using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {

        public void AddCustomer(Customer c)
        {
            IDAL.DO.Customer customer = new();
            try
            {
                customer.Id = c.Id;
                customer.Name = c.Name;
                customer.Phone = c.Phone;
                customer.Latitude = c.Location.Latitude;
                customer.Longitude = c.Location.Longitude;
                dl.AddCustomer(customer);
            }
            catch (IDAL.DO.ExistIdException ex)
            {
                throw new IBL.BO.ExistIdException(ex.ID, ex.EntityName);
            }

        }

        public void UpdateCustomer(int id, string name, string phone) //is there a chance the function will get only 2 values?
        {
            try
            {
                IDAL.DO.Customer customer = dl.GetCustomer(id);
                if (name != "")
                    customer.Name = name;
                if (phone != "")
                    customer.Phone = phone;
                dl.DeleteCustomer(id);
                dl.AddCustomer(customer);
            }
            catch (IDAL.DO.IdNotFoundException ex)
            {
                throw new IBL.BO.IdNotFoundException(ex.ID, ex.EntityName);
            }
        }
        public IEnumerable<IBL.BO.CustomerForList> GetListOfCustomers()
        {
            IEnumerable<IBL.BO.CustomerForList> customers =
                from customer in dl.GetListOfCustomers()
                select new IBL.BO.CustomerForList
                {
                    ID = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                };
            foreach (CustomerForList cust in customers)
            {
                foreach (IDAL.DO.Parcel parc in dl.GetListOfParcels())
                {
                    if (parc.SenderId == cust.ID)
                    {
                        if (parc.Delivered != default(DateTime))
                            cust.SentAndDeliveredParcels++;
                        else
                            cust.SentButNotDeliveredParcels++;
                    }
                    else if (parc.TargetId == cust.ID)
                    {
                        if (parc.Delivered != default(DateTime))
                            cust.ReceivedParcels++;
                        else
                            cust.OnTheWayToCustomerParcels++;
                    }
                }
            }
            return customers;
        }

        public Customer GetCustomer(int id)
        {
            Customer c = new Customer();
            try
            {
                IDAL.DO.Customer c2 = dl.GetCustomer(id);
                c.Id = id;
                c.Name = c2.Name;
                c.Phone = c2.Phone;
                c.Location = new Location();
                c.Location.Latitude = c2.Latitude;
                c.Location.Longitude = c2.Longitude;
                
                c.ListOfParcelsFromMe = GetParcelsFromMe(id) ;
                c.ListOfParcelsIntendedToME = GetParcelsIntendedToME(id);
            }
            catch (IDAL.DO.IdNotFoundException ex)
            {
                throw new IdNotFoundException(ex.ID, "customer");
            }

            return c;

        }


    };
}


