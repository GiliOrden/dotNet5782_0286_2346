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
        enum MenuOptions { Exit, Add, Update, Display, ShowList, FindDistance }
        enum AddOptions { AddDrone=1, AddStation, AddCustomer,AddParcel}
        enum UpdateOptions { AssignParcelToDrone=1, CollectParcelByDrone, SupplyDeliveryToCustomer,SendDroneToCharge,ReleaseDroneFromCharge}
        enum DisplayOptions { BaseStationDisplay=1,DroneDisplay,CustomerDisplay,ParcelDisplay}
        enum DisplayListsOptions { BaseStationList=1, DroneList, CustomerList, ParcelList, ParselsNotAssociatedWithDrones, StationsWithAvailableChargings }
        enum FindDistances { CustomerDistance=1, StationDistance }

        static DalObject.DalObject mydal;
        /// <summary>
        /// the main function -prints menu
        /// </summary>
        /// <param name="args"></param>
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
                        Console.WriteLine("End of service");
                        break;
                    default:
                        Console.WriteLine("Wrong number");
                        break;
                }
                int.TryParse(Console.ReadLine(), out userChoise);
                mo = (MenuOptions)userChoise;
            }
            return;
        }
        /// <summary>
        /// the function adds items according to user request
        /// </summary>
        public static void AddingOptions()
        {
            int ans;
            double ans2;
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
                    Station s = new Station();
                    int.TryParse(Console.ReadLine(), out ans);
                    s.Id = ans;
                    s.Name = Console.ReadLine();
                    int.TryParse(Console.ReadLine(), out ans);
                    s.ChargeSlots = ans;
                    double.TryParse(Console.ReadLine(), out ans2);
                    s.Longitude = ans2;
                    double.TryParse(Console.ReadLine(), out ans2);
                    s.Latitude = ans2;
                    DalObject.DalObject.AddStation(s);
                    break;
                case AddOptions.AddCustomer:
                    Console.WriteLine("Enter ID, name, Phone, Longitude and  Latitude of the customer (Press enter after each one of them)");
                    Customer c = new Customer();
                    int.TryParse(Console.ReadLine(), out ans);
                    c.Id = ans;
                    c.Name = Console.ReadLine();
                    c.Phone = Console.ReadLine();
                    double.TryParse(Console.ReadLine(), out ans2);
                    c.Longitude = ans2;
                    double.TryParse(Console.ReadLine(), out ans2);
                    c.Latitude = ans2;
                    DalObject.DalObject.AddCustomer(c);
                    break;
                case AddOptions.AddParcel:
                    Console.WriteLine("Enter ID, senderId, targetId, weight and priority of your parcel(Press enter after each one of them)");
                    Parcel p = new Parcel();
                    int.TryParse(Console.ReadLine(),out ans);
                    p.Id = ans;
                    int.TryParse(Console.ReadLine(), out ans);
                    p.SenderId = ans;
                    int.TryParse(Console.ReadLine(), out ans);
                    p.TargetId = ans;
                    int.TryParse(Console.ReadLine(), out ans);
                    p.Weight = (WeightCategories)ans;
                    int.TryParse(Console.ReadLine(), out ans);
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
        /// the function updates items according to user request
        /// </summary>
        public static void UpdatingOptions()
        {
            UpdateOptions update;
            int userChoise;
            int id1,id2;
            Console.WriteLine("Press 1 to assign drone to parcel");
            Console.WriteLine("Press 2 to collect parcel by drone");
            Console.WriteLine("Press 3 to supply delivery to customer");
            Console.WriteLine("Press 4 to send drone to charge");
            Console.WriteLine("Press 5 to release drone from charging");
            int.TryParse(Console.ReadLine(),out userChoise);
            update = (UpdateOptions)userChoise;
            switch (update)
            {
                case UpdateOptions.AssignParcelToDrone:
                    Console.WriteLine(DalObject.DalObject.ListOfDrones());
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
        /// <summary>
        /// the function display an object according to user request
        /// </summary>
        public static void DisplayObject()
        {
            int ans;
            DisplayOptions show;
            int id;
            Console.WriteLine("press 1 to view details of a specific base station");
            Console.WriteLine("press 2 to view details of a specific drone");
            Console.WriteLine("press 3 to view details of a specific customer");
            Console.WriteLine("press 4 to view details of a specific parcel");
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
        /// <summary>
        /// the function show lists of items according to user request
        /// </summary>
        public static void DisplayingListsOptions()
        {
            int ans;
            Console.WriteLine("press 1 to view the list of base stations");
            Console.WriteLine("press 2 to view the list of the drones");
            Console.WriteLine("press 3 to view the list of the cutomers");
            Console.WriteLine("press 4 to view the list of the parcels");
            Console.WriteLine("press 5 to view the list of the parcels without drones");
            Console.WriteLine("press 6 to view the list of the stations with available charge slots");
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
        /// <summary>
        /// the function recieve(not as parameter) coordinates of any point and prints distance from any base or client
        /// </summary>
        public static void FindingDistance()
        {
            int ans,longitude,latitude,id;
            string name;
            bool check;
            Console.WriteLine("Press 1 to check a customer's distance from a coordinate");
            Console.WriteLine("Press 2 to check a stationws distance from a coordinate");
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
                    //calculates the distance between 2 coordinates and prints it
                    Console.WriteLine($"The distance is:{Math.Sqrt(Math.Pow(c.Longitude - longitude, 2)) + (Math.Pow(c.Latitude - latitude, 2))}");
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
                    //calculates the distance between 2 coordinates and prints it
                    Console.WriteLine($"The distance is:{Math.Sqrt(Math.Pow(s.Longitude - longitude, 2)) + (Math.Pow(s.Latitude - latitude, 2))}");
                    break;
                default:
                    break;
            }
        }
       
    }
}
