﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        public class DroneInCharging
        {
            public int Id { get; set; }
            public int BatteryStatus { get; set; }
            public override string ToString()
            {
                return this.ToStringProperty();
            }


        }
    }
}