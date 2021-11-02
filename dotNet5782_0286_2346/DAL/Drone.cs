using System;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }
            public override string ToString()
            {
                return @$"Drone
                          Id:         {Id,-15}
                          Model:      {Model,-25}
                          MaxWheight: {MaxWeight,-15}
                          Status:     {Status,-15}
                          Battery:    {Battery,-15}";
            }
        }
    }
}
