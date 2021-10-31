using System;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {
        

        static void Main(string[] args)
        { }
            DalObject.DalObject d1 = new DalObject.DalObject();//it will produce the data base and the methods that related to it
           // print menue according to user instructions
             enum MenuOptions { Add, Update, ShowOne, ShowList, Exit }
        enum AddOptions { AddDrone, AddStation, AddCustomer, Exit }
                                                            //sub-menu
    
             void PrintMenue()
            {
            MenuOptions mo;
            int userChoise;
            int.TryParse(Console.ReadLine(), out userChoise);
            mo = (MenuOptions)userChoise;
            switch (mo)
            {
                case MenuOptions.Add:
                    AddingOptions();
                    break;
                case MenuOptions.Update:

                    break;
                case MenuOptions.ShowOne:


                    break;
                case MenuOptions.ShowList:

                    break;

                case MenuOptions.Exit:

                    break;
                default:

                    break;
            }
            Console.ReadLine();

        }


    

    
        public void AddingOptions()
        {
            AddOptions add;
            string userChoise = Console.ReadLine();
            add = (AddOptions)int.Parse(userChoise);
            switch (add)
            {
                case AddOptions.AddDrone:
                    
                    break;
                case AddOptions.AddStation:
                    Console.WriteLine("Enter ID, name, chargeSlots, Longitude and  Latitude of the station (Press enter after each one of them)");
                   Station s= new Station()
                    {

                        Id = int.Parse(Console.ReadLine()),
                        Name = Console.ReadLine(),
                        ChargeSlots = int.Parse(Console.ReadLine()),
                        Longitude = double.Parse(Console.ReadLine()),
                        Latitude = double.Parse(Console.ReadLine())
                    };
                  d1.AddStation(s);
                    break;
                case AddOptions.AddCustomer:
                    break;
                case AddOptions.Exit:
                    break;
                default:
                    break;
            }
        }

    }
}