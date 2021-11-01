using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace ConsoleUI
{
    class Program
    {
        enum MenuOptions { Add, Update, Display, ShowList, FindDistance, Exit }
        enum AddOptions { AddDrone, AddStation, AddCustomer,AddParcel, Exit }
        enum UpdateOptions { AssignParcelToDrone, CollectParcelByDrone, SupplyDeliveryToCustomer,SendDroneToCharge,ReleaseDroneFromCharge}
        enum DisplayOptions { BaseStationDisplay,DroneDisplay,CustomerDisplay,ParcelDisplay}
        enum DisplayListsOptions { BaseStationList, DroneList, CustomerList, ParcelList, ParselsNotAssociatedWithDrones, StationsWithAvailableChargings }
        enum FindDistances { CustomerDistance, StationDistance }

        static DalObject.DalObject d1 = new DalObject.DalObject();
        public static void AddingOptions()
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
        /// <summary>
        /// the function
        /// </summary>
        public static void UpdatingOptions()
        {
            
            UpdateOptions update;
            int userChoise;
            int id1,id2;
            int.TryParse(Console.ReadLine(),out userChoise);//i change it because they wrote to use tryParse
            update = (UpdateOptions)userChoise;
            switch (update)
            {
                case UpdateOptions.AssignParcelToDrone://question:Should it look like this?with 2 id that are sent as parameters?
                    Console.WriteLine("Please enter the ID of the parcel and the ID of the drone");
                    int.TryParse(Console.ReadLine(), out id1);
                    int.TryParse(Console.ReadLine(), out id2);
                    d1.AssignParcelToDrone(id1, id2);
                        break;
                case UpdateOptions.CollectParcelByDrone:
                    Console.WriteLine("Please enter the parcel ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    d1.CollectParcelByDrone(id1);
                    break;
                case UpdateOptions.SupplyDeliveryToCustomer:
                    Console.WriteLine("Please enter the parcel ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    d1.SupplyDeliveryToCustomer(id1);
                    break;
                case UpdateOptions.SendDroneToCharge:
                    Console.WriteLine("Please enter the drone ID ");
                    int.TryParse(Console.ReadLine(), out id1);
                    Console.WriteLine("Please enter the station ID from the list of stations");
                    d1.ListOfAvailableChargingStations();
                    int.TryParse(Console.ReadLine(), out id2);
                    d1.SendDroneToCharge(id1, id2);
                    break;
                case UpdateOptions.ReleaseDroneFromCharge:
                    Console.WriteLine("Please enter the drone ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    d1.ReleaseDroneFromCharge(id1);
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
            DisplayOptions.TryParse(Console.ReadLine(), out ans);
            show = (DisplayOptions)ans;
            switch (show)
            {
                case DisplayOptions.BaseStationDisplay:
                    Console.WriteLine("Please enter the staion ID");
                    int.TryParse(Console.ReadLine(), out id);
                    d1.DisplayBaseStation(id);
                    break;
                case DisplayOptions.DroneDisplay:
                    Console.WriteLine("Please enter the Drone ID");
                    int.TryParse(Console.ReadLine(), out id);
                    d1.DisplayDrone(id);
                    break;
                case DisplayOptions.CustomerDisplay:
                    Console.WriteLine("Please enter the customer ID");
                    int.TryParse(Console.ReadLine(), out id);
                    d1.DisplayCustomer(id);
                    break;
                case DisplayOptions.ParcelDisplay:
                    Console.WriteLine("Please enter the parcel ID");
                    int.TryParse(Console.ReadLine(), out id);
                    d1.DisplayParcel(id);
                    break;
                default:
                    break;
            }
        }

        public static void DisplayingListsOptions()
        {
            int ans;
            DisplayListsOptions.TryParse(Console.ReadLine(), out ans);
            DisplayListsOptions choise = (DisplayListsOptions)ans;
            switch (choise)
            {
                case DisplayListsOptions.BaseStationList:
                    d1.ListOfBaseStations();
                    break;
                case DisplayListsOptions.DroneList:
                    d1.ListOfDrones();
                    break;
                case DisplayListsOptions.CustomerList:
                    d1.ListOfCustomers();
                    break;
                case DisplayListsOptions.ParcelList:
                    d1.ListOfParcels();
                    break;
                case DisplayListsOptions.ParselsNotAssociatedWithDrones:
                    d1.ListOfNotAssociatedParsels();
                    break;
                case DisplayListsOptions.StationsWithAvailableChargings:
                    d1.ListOfAvailableChargingStations();
                    break;
                default:
                    break;
            }
        }

        public static void FindingDistance()
        {
            int ans,longitude,latitude,id;
            string name;
            bool check;
            FindDistances.TryParse(Console.ReadLine(), out ans);
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
                    d1.DistanceFromCustomer(longitude, latitude, name);
                    break;
                case FindDistances.StationDistance:
                    Console.WriteLine("Enter the location (longitude & latitude) ");
                    check = int.TryParse(Console.ReadLine(), out longitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    check = int.TryParse(Console.ReadLine(), out latitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    Console.WriteLine("Enter the station's Id");
                    int.TryParse(Console.ReadLine(), out id);
                    d1.DistanceFromStation(longitude, latitude, id);
                    break;
                default:
                    break;
            }
        }

            static void Main(string[] args)
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

                    break;
            }
            Console.ReadLine();
        }
    }
}
