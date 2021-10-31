using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace ConsoleUI
{
    class p
    {
        enum MenuOptions { Add, Update, ShowOne, ShowList, Exit }
        enum AddOptions { AddDrone, AddStation, AddCustomer,AddParcel, Exit }

        enum UpdateOptions { AssignParcelToDrone, CollectParcelByDrone, SupplyDeliveryToCustomer,SendDroneToCharge,ReleaseDroneFromCharge,Exit }
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
                    AddingOptions();
                    break;
                case MenuOptions.Update:
                    UpdatingOptions();
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
            int ans;
            AddOptions add;
            int.TryParse(Console.ReadLine( ),out ans);
            add = (AddOptions)ans;
            switch (add)
            {
                case AddOptions.AddDrone:
                    Console.WriteLine("Enter ID, model, maximum weight(Press enter after each one of them)");
                    Drone d = new();
                    d.Id = int.Parse(Console.ReadLine());
                    d.Model = Console.ReadLine();
                    
                    WeightCategories.TryParse(Console.ReadLine(), out ans);
                    d.MaxWeight = (WeightCategories)ans;
                    WeightCategories.TryParse(Console.ReadLine(), out ans);
                    d.Status = (DroneStatuses)ans;
                    d.Battery = 100;
                    d1.AddDrone(d);
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
                    Console.WriteLine("Enter ID, name, Phone, Longitude and  Latitude of the customer (Press enter after each one of them)");
                    Customer c=new Customer()
                    {
                        Id = int.Parse(Console.ReadLine()),
                        Name = Console.ReadLine(),
                        Phone = Console.ReadLine(),
                        Longitude = double.Parse(Console.ReadLine()),
                        Latitude = double.Parse(Console.ReadLine()),
                    };
                    d1.AddCustomer(c);
                    break;
                case AddOptions.AddParcel:
                    Console.WriteLine("Enter ID, senderId, targetId, weight and priority of your parcel(Press enter after each one of them)");
                    Parcel p = new Parcel();
                    p.Id = int.Parse(Console.ReadLine());
                    p.SenderId = int.Parse(Console.ReadLine());
                    p.TargetId = int.Parse(Console.ReadLine());
                    WeightCategories.TryParse(Console.ReadLine(), out ans);
                    p.Weight = (WeightCategories)ans;
                    Priorities.TryParse(Console.ReadLine(), out ans);
                    p.Priority = (Priorities)ans;
                    p.DroneId = 0;
                    p.Requested = DateTime.Now;
                    d1.AddParcel(p);
                    break;
                case AddOptions.Exit:
                    return;
                default:
                    break;
            }
        }

        public static void UpdatingOptions()
        {
            
            UpdateOptions update;
            int userChoise;
            int.TryParse(Console.ReadLine(),out userChoise);//i change it because they wrote to use tryParse
            update = (UpdateOptions)userChoise;
            switch (update)
            {
                case UpdateOptions.AssignParcelToDrone:
                    Console.WriteLine("Please enter the parcel Id");
                    int.TryParse(Console.ReadLine(), out userChoise);  
                    foreach(Parcel parcel in parcels)
                    {

                    }
                        break;
                case UpdateOptions.CollectParcelByDrone:
                    break;
                case UpdateOptions.SupplyDeliveryToCustomer:
                    break;
                case UpdateOptions.SendDroneToCharge:
                    break;
                case UpdateOptions.ReleaseDroneFromCharge:
                    break;
                case UpdateOptions.Exit:
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