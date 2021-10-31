using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DalObject;
namespace ConsoleUI
{
    class p
    {
        enum MenuOptions { Add, Update, ShowOne, ShowList, Exit }
        enum AddOptions { AddDrone, AddStation, AddCustomer, Exit }

        DalObject.DalObject d1 = new DalObject.DalObject();
        void PrintMenue()
        {
            MenuOptions mo;
            int userChoise;
            int.TryParse(Console.ReadLine(), out userChoise);
            mo = (MenuOptions)userChoise;
            switch (mo)
            {
                case MenuOptions.Add:

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
                    Station s = new Station()
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
        static void Main(string[] args)
        {
        }
    }
}