﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.EnumsBL;

namespace IBL
{

    namespace BO
    {
        public class ParcelInTransfer
        {
            public int Id { get; set; }
            public bool Status { get; set; }//no- Waiting for collection /yes- on the way to the destination
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelAtCustomer Sender { get; set; }
            public ParcelAtCustomer Recipient { get; set; }
            public Location Source { get; set; }
            public Location Destination { get; set; }
            public double TransportDistance { get; set; }

            public override string ToString()
            {
                return this.ToStringProperty();
            }

        }
    }