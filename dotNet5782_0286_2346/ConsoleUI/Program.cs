﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace ConsoleUI
{
    class Program
    {
        enum MenuOptions { Exit, Add, Update, Display, ShowList, FindDistance }
        enum AddOptions { AddDrone=1, AddStation, AddCustomer,AddParcel}
        enum UpdateOptions { AssignParcelToDrone=1, CollectParcelByDrone, SupplyDeliveryToCustomer,SendDroneToCharge,ReleaseDroneFromCharge}
        enum DisplayOptions { BaseStationDisplay=1,DroneDisplay,CustomerDisplay,ParcelDisplay}
        enum DisplayListsOptions { BaseStationList=1, DroneList, CustomerList, ParcelList, ParselsNotAssociatedWithDrones, StationsWithAvailableChargings }
        enum FindDistances { CustomerDistance=1, StationDistance }

        static DalObject.DalObject mydal;

        static void Main(string[] args)
        {
            mydal = new DalObject.DalObject();
            MenuOptions mo;
            int userChoise;
            Console.WriteLine("press 1 to add an item");
            Console.WriteLine("press 2 to update an item");
            Console.WriteLine("press 3 to display an item");
            Console.WriteLine("press 4 to display a list of specific item");
            Console.WriteLine("Press 5 to find a distance of base or customer from a coordinate");
            Console.WriteLine("press 0 to exit");
            int.TryParse(Console.ReadLine(), out userChoise);
            mo = (MenuOptions)userChoise;
            while (mo!=MenuOptions.Exit)
            {
                switch (mo)
                {
                    case MenuOptions.Add:
                        AddingOptions();
                        break;
                    case MenuOptions.Update:
                        UpdatingOptions();
                        break;
                    case MenuOptions.Display:
                        DisplayObject();
                        break;
                    case MenuOptions.ShowList:
                        DisplayingListsOptions();
                        break;
                    case MenuOptions.FindDistance:
                        FindingDistance();
                        break;
                    case MenuOptions.Exit:

                        break;
                    default:
                        Console.WriteLine("Wrong number");
                        break;
                }
                int.TryParse(Console.ReadLine(), out userChoise);
                mo = (MenuOptions)userChoise;
            }
            Console.WriteLine("End of service");
            return;
        }
        public static void AddingOptions()
        {
            int ans;
            AddOptions add;
            Console.WriteLine("Press 1 to add drone");
            Console.WriteLine("Press 2 to add station");
            Console.WriteLine("Press 3 to add customer");
            Console.WriteLine("Press 4 to add parcel");
            int.TryParse(Console.ReadLine( ),out ans);
            add = (AddOptions)ans;
            switch (add)
            {
                case AddOptions.AddDrone:
                    Console.WriteLine("Enter ID, model, maximum weight(Press enter after each one of them)");
                    Drone d = new();
                    int.TryParse(Console.ReadLine(),out ans);
                    d.Id = ans;
                    d.Model = Console.ReadLine();
                    int.TryParse(Console.ReadLine(), out ans);
                    d.MaxWeight = (WeightCategories)ans;
                    d.Status = DroneStatuses.Available;
                    d.Battery = 100;
                    DalObject.DalObject.AddDrone(d);
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
                    DalObject.DalObject.AddStation(s);
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
                    DalObject.DalObject.AddCustomer(c);
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
                    DalObject.DalObject.AddParcel(p);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// the function
        /// </summary>
        public static void UpdatingOptions()
        {
            
            UpdateOptions update;
            int userChoise;
            int id1,id2;
            int.TryParse(Console.ReadLine(),out userChoise);
            update = (UpdateOptions)userChoise;
            switch (update)
            {
                case UpdateOptions.AssignParcelToDrone:
                    Console.WriteLine("Please enter the ID of the parcel and the ID of the drone");
                    int.TryParse(Console.ReadLine(), out id1);
                    int.TryParse(Console.ReadLine(), out id2);
                    DalObject.DalObject.AssignParcelToDrone(id1, id2);
                        break;
                case UpdateOptions.CollectParcelByDrone:
                    Console.WriteLine("Please enter the parcel ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    DalObject.DalObject.CollectParcelByDrone(id1);
                    break;
                case UpdateOptions.SupplyDeliveryToCustomer:
                    Console.WriteLine("Please enter the parcel ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    DalObject.DalObject.SupplyDeliveryToCustomer(id1);
                    break;
                case UpdateOptions.SendDroneToCharge:
                    Console.WriteLine("Please enter the drone ID ");
                    int.TryParse(Console.ReadLine(), out id1);
                    Console.WriteLine("Please enter the station ID from the list of stations");
                    DalObject.DalObject.ListOfAvailableChargingStations();
                    int.TryParse(Console.ReadLine(), out id2);
                    DalObject.DalObject.SendDroneToCharge(id1, id2);
                    break;
                case UpdateOptions.ReleaseDroneFromCharge:
                    Console.WriteLine("Please enter the drone ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    DalObject.DalObject.ReleaseDroneFromCharge(id1);
                    break;
                default:
                    break;
            }
        }
        public static void DisplayObject()
        {
            int ans;
            DisplayOptions show;
            int id;
            int.TryParse(Console.ReadLine(), out ans);
            show = (DisplayOptions)ans;
            switch (show)
            {
                case DisplayOptions.BaseStationDisplay:
                    Console.WriteLine("Please enter the staion ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(DalObject.DalObject.DisplayBaseStation(id)); 
                    break;
                case DisplayOptions.DroneDisplay:
                    Console.WriteLine("Please enter the Drone ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(DalObject.DalObject.DisplayDrone(id));
                    break;
                case DisplayOptions.CustomerDisplay:
                    Console.WriteLine("Please enter the customer ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(DalObject.DalObject.DisplayCustomer(id));
                    break;
                case DisplayOptions.ParcelDisplay:
                    Console.WriteLine("Please enter the parcel ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(DalObject.DalObject.DisplayParcel(id));
                    break;
                default:
                    break;
            }
        }
        
        public static void DisplayingListsOptions()
        {
            int ans;
            int.TryParse(Console.ReadLine(), out ans);
            DisplayListsOptions choise = (DisplayListsOptions)ans;
            switch (choise)
            {
                case DisplayListsOptions.BaseStationList:
                    foreach (Station allStaions in DalObject.DalObject.ListOfBaseStations())
                        Console.WriteLine(allStaions.ToString());
                    break;
                case DisplayListsOptions.DroneList:
                    foreach (Drone allDrones in DalObject.DalObject.ListOfDrones())
                        Console.WriteLine(allDrones.ToString());
                     break;
                case DisplayListsOptions.CustomerList:
                    foreach (Customer customer in DalObject.DalObject.ListOfCustomers())
                        Console.WriteLine(customer.ToString());
                     break;
                case DisplayListsOptions.ParcelList:
                    foreach (Parcel allParcel in DalObject.DalObject.ListOfParcels())
                        Console.WriteLine(allParcel.ToString());
                    break;
                case DisplayListsOptions.ParselsNotAssociatedWithDrones:
                    foreach (Parcel parcel in DalObject.DalObject.ListOfNotAssociatedParsels())
                        Console.WriteLine(parcel.ToString());
                     break;
                case DisplayListsOptions.StationsWithAvailableChargings:
                    foreach (Station station in DalObject.DalObject.ListOfAvailableChargingStations())
                        Console.WriteLine(station.ToString());
                    break;
                default:
                    Console.WriteLine("Wrong number");
                    break;
            }
        }

        public static void FindingDistance()
        {
            int ans,longitude,latitude,id;
            string name;
            bool check;
            int.TryParse(Console.ReadLine(), out ans);
            FindDistances choise = (FindDistances)ans;
            switch (choise)
            {
                case FindDistances.CustomerDistance:
                    Console.WriteLine("Enter the location (longitude & latitude) ");
                    check=int.TryParse(Console.ReadLine(), out longitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    check = int.TryParse(Console.ReadLine(), out latitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    Console.WriteLine("Enter your name");
                    name = Console.ReadLine();
                    Customer c = DalObject.DalObject.DistanceFromCustomer(longitude, latitude, name);
                    Console.WriteLine($"The distunce is:{Math.Sqrt(Math.Pow(c.Longitude - longitude, 2)) + (Math.Pow(c.Latitude - latitude, 2))}");
                    break;
                case FindDistances.StationDistance:
                    Console.WriteLine("Enter the location (longitude & latitude) ");
                    check = int.TryParse(Console.ReadLine(), out longitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    check = int.TryParse(Console.ReadLine(), out latitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    Console.WriteLine("Enter the station's Id");
                    int.TryParse(Console.ReadLine(), out id);
                    Station s=DalObject.DalObject.DistanceFromStation(longitude, latitude, id);
                    Console.WriteLine($"The distunce is:{Math.Sqrt(Math.Pow(s.Longitude - longitude, 2)) + (Math.Pow(s.Latitude - latitude, 2))}");
                    break;
                default:
                    break;
            }
        }
       
    }
}
