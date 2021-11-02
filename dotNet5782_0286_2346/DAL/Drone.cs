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
