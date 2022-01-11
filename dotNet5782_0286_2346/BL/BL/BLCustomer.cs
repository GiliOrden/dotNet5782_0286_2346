using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    sealed partial class BL : IBL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(BO.Customer c)
        {
            DO.Customer customer = new();
            try
            {
                customer.Id = c.Id;
                customer.Name = c.Name;
                customer.Phone = c.Phone;
                customer.Latitude = c.Location.Latitude;
                customer.Longitude = c.Location.Longitude;
                dl.AddCustomer(customer);
            }
            catch (ExistIdException ex)
            {
                throw new ExistIdException(ex.ID, ex.EntityName);
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(int id, string name, string phone)
        {
            if (!dl.GetListOfCustomers().Any(cus => cus.Id == id))
                throw new IdNotFoundException(id, "customer");
          lock (dl)
          { 
            DO.Customer customer = dl.GetCustomer(id);
            if (name != "")
            {

                customer.Name = name;
            }
            if (phone != "")
            {
                customer.Phone = phone;
            }
            dl.DeleteCustomer(id);
            dl.AddCustomer(customer);
          }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BO.CustomerForList> GetListOfCustomers()
        {
            IEnumerable<BO.CustomerForList> customers =
                from customer in dl.GetListOfCustomers()
                let cust= getCustomerForList(customer)
                select cust;
            return customers;
        }

        /// <summary>
        /// the function generates a customer for list according to data in the dalobject layer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private BO.CustomerForList getCustomerForList(DO.Customer customer)
        {
            BO.CustomerForList customerForList = new();
            customerForList.ID = customer.Id;
            customerForList.Name = customer.Name;
            customerForList.Phone = customer.Phone;
            foreach (DO.Parcel parc in dl.GetListOfParcels())
            {
                if (parc.SenderId == customerForList.ID)
                {
                    if (parc.Delivered != null)
                        customerForList.SentAndDeliveredParcels++;
                    else
                        customerForList.SentButNotDeliveredParcels++;
                }
                else if (parc.TargetId == customerForList.ID)
                {
                    if (parc.Delivered != null)
                        customerForList.ReceivedParcels++;
                    else
                        customerForList.OnTheWayToCustomerParcels++;
                }
            }
            return customerForList;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Customer GetCustomer(int id)
        {
            BO.Customer c = new BO.Customer();
            try
            {
                DO.Customer c2 = dl.GetCustomer(id);
                c.Id = id;
                c.Name = c2.Name;
                c.Phone = c2.Phone;
                c.Location = new BO.Location();
                c.Location.Latitude = c2.Latitude;
                c.Location.Longitude = c2.Longitude;
                c.ListOfParcelsFromMe = GetParcelsFromMe(c.Id);
                c.ListOfParcelsIntendedToME = GetParcelsIntendedToMe(c.Id);
            }
            catch (IdNotFoundException ex)
            {
                throw new IdNotFoundException(ex.ID, "customer");
            }

            return c;

        }
    };
}


