﻿using System;
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

        public void UpdateCustomer(int id, string name, string phone)
        {
            if (!dl.GetListOfCustomers().Any(cus => cus.Id == id))
                throw new IdNotFoundException(id, "customer");
            IDAL.DO.Customer customer = dl.GetCustomer(id);
                if (name != "")
                    customer.Name = name;
                if (phone != "")
                    customer.Phone = phone;
                dl.DeleteCustomer(id);
                dl.AddCustomer(customer);
         }

        public IEnumerable<IBL.BO.CustomerForList> GetListOfCustomers()
        {
            IEnumerable<IBL.BO.CustomerForList> customers =
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
        private CustomerForList getCustomerForList(IDAL.DO.Customer customer)
        {
            CustomerForList customerForList = new();
            customerForList.ID = customer.Id;
            customerForList.Name = customer.Name;
            customerForList.Phone = customer.Phone;
            foreach (IDAL.DO.Parcel parc in dl.GetListOfParcels())
            {
                if (parc.SenderId == customerForList.ID)
                {
                    if (parc.Delivered != default(DateTime))
                        customerForList.SentAndDeliveredParcels++;
                    else
                        customerForList.SentButNotDeliveredParcels++;
                }
                else if (parc.TargetId == customerForList.ID)
                {
                    if (parc.Delivered != default(DateTime))
                        customerForList.ReceivedParcels++;
                    else
                        customerForList.OnTheWayToCustomerParcels++;
                }
            }
            return customerForList;
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


