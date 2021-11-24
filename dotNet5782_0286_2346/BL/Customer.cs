﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Phone { get; set; }
            public Location Location { get; set; }
            //פה צריכה להיות רשימת חבילות אצל לקוח -מהלקוח
            //פה צריכה להיות רשימת חבילות אצל לקוח -אל הלקוח
            public override string ToString()
            {
                return this.ToStringProperty();
            }
        }
    }
}