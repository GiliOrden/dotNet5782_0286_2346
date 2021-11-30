﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }//property
            public int SenderId { get; set; }//property
            public int TargetId { get; set; }//property
            public WeightCategories Weight { get; set; }//property
            public Priorities Priority { get; set; }//property
            public int DroneId { get; set; }//property
            public DateTime Requested { get; set; }//property
            public DateTime Scheduled { get; set; }//property, assigned to drone
            public DateTime PickedUp { get; set; }//property, collected by drone
            public DateTime Delivered { get; set; }//property, brought to the destination
            public override string ToString()//Print all the fields (Override the Object's 'ToString()')
            {
                return this.ToStringProperty();
            }
        }
    }
}
