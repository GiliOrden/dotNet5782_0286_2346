using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    class ProgramBL
    {

        enum MenuOptions { Exit, Add, Update, Display, ShowList, FindDistance }
        enum AddOptions { AddDrone = 1, AddStation, AddCustomer, AddParcel }
        enum UpdateOptions { AssignParcelToDrone = 1, CollectParcelByDrone, SupplyDeliveryToCustomer, SendDroneToCharge, ReleaseDroneFromCharge }
        enum DisplayOptions { BaseStationDisplay = 1, DroneDisplay, CustomerDisplay, ParcelDisplay }
        enum DisplayListsOptions { BaseStationList = 1, DroneList, CustomerList, ParcelList, ParselsNotAssociatedWithDrones, StationsWithAvailableChargings }
        enum FindDistances { CustomerDistance = 1, StationDistance }


        /// <summary>
        /// the main function -prints menu
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            IBL.IBL bl = new BL.BL();
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
            while (mo != MenuOptions.Exit)
            {
                try
                {
                    switch (mo)
                    {
                        case MenuOptions.Add:
                            AddingOptions(ref bl);
                            break;
                        case MenuOptions.Update:
                            UpdatingOptions(ref bl );
                            break;
                        case MenuOptions.Display:
                            GetObject(ref bl);
                            break;
                        case MenuOptions.ShowList:
                            DisplayingListsOptions(ref bl);
                            break;
                        case MenuOptions.FindDistance:
                            FindingDistance(ref bl);
                            break;
                        case MenuOptions.Exit:
                            Console.WriteLine("End of service");
                            break;
                        default:
                            Console.WriteLine("Wrong number");
                            break;

                    }
                }
                catch (Exceptions.ExistIdException e)
                {

                }
                catch (Exceptions.IdNotFoundException e)
                {

                }
                catch (Exceptions.NameNotFoundException e)
                {

                }
                Console.WriteLine("press 1 to add an item");
                Console.WriteLine("press 2 to update an item");
                Console.WriteLine("press 3 to display an item");
                Console.WriteLine("press 4 to display a list of specific item");
                Console.WriteLine("Press 5 to find a distance of base or customer from a coordinate");
                Console.WriteLine("press 0 to exit");
                int.TryParse(Console.ReadLine(), out userChoise);
                mo = (MenuOptions)userChoise;
            }
            return;
        }
        /// <summary>
        /// the function adds items according to user request
        /// </summary>
        public static void AddingOptions(ref IBL.IBL bl)
        {
            int ans;
            double ans2;
            AddOptions add;
            Console.WriteLine("Press 1 to add drone");
            Console.WriteLine("Press 2 to add station");
            Console.WriteLine("Press 3 to add customer");
            Console.WriteLine("Press 4 to add parcel");
            int.TryParse(Console.ReadLine(), out ans);
            add = (AddOptions)ans;
            switch (add)
            {
                case AddOptions.AddDrone:
                    Console.WriteLine("Enter Manufacturer's serial number & model(Press enter after each one of them)");
                    DroneForList d = new();
                    int.TryParse(Console.ReadLine(), out ans);
                    d.Id = ans;
                    d.Model = Console.ReadLine();
                    Console.WriteLine("Choose maximum weight:0 for Light,1 for Medium,2 for Heavy");
                    int.TryParse(Console.ReadLine(), out ans);
                    d.MaxWeight = (EnumsBL.WeightCategories)ans;
                    Console.WriteLine("Enter Station ID for initial charging");
                    int.TryParse(Console.ReadLine(), out ans);
                    bl.AddDrone(d,ans);
                    Console.WriteLine("Succeeded");
                    break;
                case AddOptions.AddStation:
                    Console.WriteLine("Enter ID, name,Location, chargeSlots(Press enter after each one of them)");
                    Station s = new Station();
                    int.TryParse(Console.ReadLine(), out ans);
                    s.ID = ans;
                    s.Name = Console.ReadLine();
                    double.TryParse(Console.ReadLine(), out ans2);
                    s.Location.Longitude = ans2;
                    double.TryParse(Console.ReadLine(), out ans2);
                    s.Location.Latitude = ans2;
                    int.TryParse(Console.ReadLine(), out ans);
                    s.ChargeSlots = ans;
                    bl.AddBaseStation(s);
                    Console.WriteLine("Succeeded");
                    break;
                case AddOptions.AddCustomer:
                    Console.WriteLine("Enter id , name , phone and location (Press enter after each one of them)");
                    IBL.BO.Customer c = new IBL.BO.Customer();
                    int.TryParse(Console.ReadLine(), out ans);
                    c.Id = ans;
                    c.Name = Console.ReadLine();
                    c.Phone = Console.ReadLine();
                    double.TryParse(Console.ReadLine(), out ans2);
                    c.Location.Longitude = ans2;
                    double.TryParse(Console.ReadLine(), out ans2);
                    c.Location.Latitude = ans2;
                    bl.AddCustomer(c);
                    break;
                case AddOptions.AddParcel:
                    Console.WriteLine("Enter senderId and targetId (Press enter after each one of them)");
                    Parcel p = new Parcel();
                    int.TryParse(Console.ReadLine(), out ans);
                    p.SenderId = ans;
                    int.TryParse(Console.ReadLine(), out ans);
                    p.TargetId = ans;
                    Console.WriteLine("Choose maximum weight:0 for Light,1 for Medium,2 for Heavy");
                    int.TryParse(Console.ReadLine(), out ans);
                    p.Weight = (WeightCategories)ans;
                    Console.WriteLine("Choose priority: 0 for Regular, 1 for Fast, 2 for Emergency ");
                    int.TryParse(Console.ReadLine(), out ans);
                    p.Priority = (Priorities)ans;
                    p.DroneId = 0;
                    p.Requested = DateTime.Now;
                    dl.AddParcel(p);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// the function updates items according to user request
        /// </summary>
        public static void UpdatingOptions(ref IBL.IBL bl)
        {
            UpdateOptions update;
            int userChoise;
            int id1, id2;
            Console.WriteLine("Press 1 to assign drone to parcel");
            Console.WriteLine("Press 2 to collect parcel by drone");
            Console.WriteLine("Press 3 to supply delivery to customer");
            Console.WriteLine("Press 4 to send drone to charge");
            Console.WriteLine("Press 5 to release drone from charging");
            int.TryParse(Console.ReadLine(), out userChoise);
            update = (UpdateOptions)userChoise;
            switch (update)
            {
                case UpdateOptions.AssignParcelToDrone:
                    Console.WriteLine("Please enter the ID of the parcel and the ID of the drone");
                    int.TryParse(Console.ReadLine(), out id1);
                    int.TryParse(Console.ReadLine(), out id2);
                    dl.AssignParcelToDrone(id1, id2);
                    break;
                case UpdateOptions.CollectParcelByDrone:
                    Console.WriteLine("Please enter the parcel ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    dl.CollectParcelByDrone(id1);
                    break;
                case UpdateOptions.SupplyDeliveryToCustomer:
                    Console.WriteLine("Please enter the parcel ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    dl.SupplyDeliveryToCustomer(id1);
                    break;
                case UpdateOptions.SendDroneToCharge:
                    Console.WriteLine("Please enter the drone ID ");
                    int.TryParse(Console.ReadLine(), out id1);
                    Console.WriteLine("Please enter the station ID from the list of stations");
                    dl.GetListOfStationsWithAvailableChargeSlots();
                    int.TryParse(Console.ReadLine(), out id2);
                    dl.SendDroneToCharge(id1, id2);
                    break;
                case UpdateOptions.ReleaseDroneFromCharge:
                    Console.WriteLine("Please enter the drone ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    dl.ReleaseDroneFromCharge(id1);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// the function display an object according to user request
        /// </summary>
        public static void GetObject(ref IBL.IBL bl)
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
                    Console.WriteLine(dl.GetBaseStation(id));
                    break;
                case DisplayOptions.DroneDisplay:
                    Console.WriteLine("Please enter the Drone ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(dl.GetDrone(id));
                    break;
                case DisplayOptions.CustomerDisplay:
                    Console.WriteLine("Please enter the customer ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(dl.GetCustomer(id));
                    break;
                case DisplayOptions.ParcelDisplay:
                    Console.WriteLine("Please enter the parcel ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(dl.GetParcel(id));
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// the function show lists of items according to user request
        /// </summary>
        public static void DisplayingListsOptions(ref IBL.IBL bl)
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
                    foreach (Station stations in dl.GetListOfBaseStations())
                        Console.WriteLine(stations.ToString());
                    break;
                case DisplayListsOptions.DroneList:
                    foreach (Drone drones in dl.GetListOfDrones())
                        Console.WriteLine(drones.ToString());
                    break;
                case DisplayListsOptions.CustomerList:
                    foreach (Customer customer in dl.GetListOfCustomers())
                        Console.WriteLine(customer.ToString());
                    break;
                case DisplayListsOptions.ParcelList:
                    foreach (Parcel parcel in dl.GetListOfParcels())
                        Console.WriteLine(parcel.ToString());
                    break;
                case DisplayListsOptions.ParselsNotAssociatedWithDrones:
                    foreach (Parcel parcel in dl.GetListOfNotAssociatedParcels())
                        Console.WriteLine(parcel.ToString());
                    break;
                case DisplayListsOptions.StationsWithAvailableChargings:
                    foreach (Station station in dl.GetListOfStationsWithAvailableChargeSlots())
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
        public static void FindingDistance(ref IBL.IBL bl)
        {
            int ans, longitude, latitude, id;
            bool check;
            Console.WriteLine("Press 1 to check a customer's distance from a coordinate");
            Console.WriteLine("Press 2 to check a stationws distance from a coordinate");
            int.TryParse(Console.ReadLine(), out ans);
            FindDistances choise = (FindDistances)ans;
            switch (choise)
            {
                case FindDistances.CustomerDistance:
                    Console.WriteLine("Enter the location (longitude & latitude) ");
                    check = int.TryParse(Console.ReadLine(), out longitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    check = int.TryParse(Console.ReadLine(), out latitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    Console.WriteLine("Enter the ID of the customer");
                    int.TryParse(Console.ReadLine(), out id);
                    Customer c = dl.GetCustomer(id);
                    Console.WriteLine($"The distance from the customer:{Math.Sqrt(Math.Pow(c.Longitude - longitude, 2)) + (Math.Pow(c.Latitude - latitude, 2))}");
                    break;
                case FindDistances.StationDistance:
                    Console.WriteLine("Enter the location (longitude & latitude) ");
                    check = int.TryParse(Console.ReadLine(), out longitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    check = int.TryParse(Console.ReadLine(), out latitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    Console.WriteLine("Enter the station's Id");
                    int.TryParse(Console.ReadLine(), out id);
                    Station s = dl.GetBaseStation(id);
                    Console.WriteLine($"The distance from the station is:{Math.Sqrt(Math.Pow(s.Longitude - longitude, 2)) + (Math.Pow(s.Latitude - latitude, 2))}");
                    break;
                default:
                    break;
            }
        }

    }
}
