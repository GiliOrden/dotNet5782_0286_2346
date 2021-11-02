using System;
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
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority{ get; set; }
            public int DroneId { get; set; }
            public DateTime Requested { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }
            public override string ToString()
            {
                
                return @$"Parcel
                          Id:        {Id,-15}
                          SenderId:  {SenderId,-15} 
                          TargetId:  {TargetId,-15}
                          Weight:    {Weight,-15}
                          Priority:  {Priority,-15}
                          DroneId:   {DroneId,-15} 
                          Requested: {Requested,-15}
                          Scheduled: {Scheduled,-15}
                          PickedUp:  {PickedUp,-15}
                          Delivered: {Delivered,-15}";

            }
        }
    }
}
