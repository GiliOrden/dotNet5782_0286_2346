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
                          Id:{Id}
                          Model:{Model}
                          MaxWheight:{MaxWeight}
                          Status:{Status}
                          Battery: {Battery }";
            }
        }
    }
}
