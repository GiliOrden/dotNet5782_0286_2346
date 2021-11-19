using System;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }//property
            public string Model { get; set; }//property
            public WeightCategories MaxWeight { get; set; }//property
            public DroneStatuses Status { get; set; }//property
            public double Battery { get; set; }//property
            public override string ToString()//Print all the fields (Override the Object's 'ToString()')
            {
                return this.ToStringProperty();
            }
        }
    }
}
