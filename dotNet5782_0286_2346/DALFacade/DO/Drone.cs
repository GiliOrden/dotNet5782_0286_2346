using System;


    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }//property
            public string Model { get; set; }//property
            public WeightCategories MaxWeight { get; set; }

            public override string ToString()//Print all the fields (Override the Object's 'ToString()')
            {
                return this.ToStringProperty();
            }
        }
    }

