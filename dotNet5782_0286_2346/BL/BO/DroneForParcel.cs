﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneForParcel
    {
        public int Id { get; set; }
        public double Battery { get; set; }
        public Location Location { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}