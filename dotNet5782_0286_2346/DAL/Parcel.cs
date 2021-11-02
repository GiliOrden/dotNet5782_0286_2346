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
            public int Id { get; set; }//property
            public int SenderId { get; set; }//property
            public int TargetId { get; set; }//property
            public WeightCategories Weight { get; set; }//property
            public Priorities Priority{ get; set; }//property
            public int DroneId { get; set; }//property
            public DateTime Requested { get; set; }//property
            public DateTime Scheduled { get; set; }//property
            public DateTime PickedUp { get; set; }//property
            public DateTime Delivered { get; set; }//property
            public override string ToString()//Print all the fields (Override the Object's 'ToString()')
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
