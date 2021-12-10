using IBL;
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
        enum UpdateOptions { UpdateDroneData = 1, UpdateStationData, UpdateCustomerData, SendDroneToCharge, ReleaseDroneFromCharge, AssignParcelToDrone, CollectParcelByDrone, SupplyParcelByDrone }
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
            Console.WriteLine("Press 5 to find a distance of base station or customer from a coordinate");
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
                            UpdatingOptions(ref bl);
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
                catch (IBL.BO.ExistIdException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (IBL.BO.IdNotFoundException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (IBL.BO.NoBatteryException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (IBL.BO.DroneCanNotCollectParcelException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (IBL.BO.DroneCanNotSupplyDeliveryToCustomerException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (IBL.BO.DroneMaxWeightIsLowException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (IBL.BO.DroneStatusException ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine();
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
                    d.Location = new Location();
                    int.TryParse(Console.ReadLine(), out ans);
                    d.Id = ans;
                    d.Model = Console.ReadLine();
                    Console.WriteLine("Choose maximum weight:0 for Light,1 for Medium,2 for Heavy");
                    int.TryParse(Console.ReadLine(), out ans);
                    d.MaxWeight = (EnumsBL.WeightCategories)ans;
                    Console.WriteLine("Enter Station ID for initial charging");
                    int.TryParse(Console.ReadLine(), out ans);
                    bl.AddDrone(d, ans);
                    Console.WriteLine("Succeeded");
                    break;
                case AddOptions.AddStation:
                    Console.WriteLine("Enter ID, name,Location, chargeSlots(Press enter after each one of them)");
                    Station s = new Station();
                    s.Location = new Location();
                    s.DroneInChargingList = new List<DroneInCharging>();
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
                    Customer c = new Customer();
                    int.TryParse(Console.ReadLine(), out ans);
                    c.Id = ans;
                    c.Name = Console.ReadLine();
                    c.Phone = Console.ReadLine();
                    double.TryParse(Console.ReadLine(), out ans2);
                    c.Location = new();
                    c.Location.Longitude = ans2;
                    double.TryParse(Console.ReadLine(), out ans2);
                    c.Location.Latitude = ans2;
                    bl.AddCustomer(c);
                    Console.WriteLine("Succeeded");
                    break;
                case AddOptions.AddParcel:
                    Console.WriteLine("Enter senderId , targetId  (Press enter after each one of them)");
                    Parcel p = new Parcel();
                    int.TryParse(Console.ReadLine(), out ans);
                    p.Sender = new();
                    p.Sender.Id = ans;
                    int.TryParse(Console.ReadLine(), out ans);
                    p.Receiver = new();
                    p.Receiver.Id = ans;
                    Console.WriteLine("Choose maximum weight:0 for Light,1 for Medium,2 for Heavy");
                    int.TryParse(Console.ReadLine(), out ans);
                    p.Weight = (EnumsBL.WeightCategories)ans;
                    Console.WriteLine("Choose priority: 0 for Regular, 1 for Fast, 2 for Emergency ");
                    int.TryParse(Console.ReadLine(), out ans);
                    p.Priority = (EnumsBL.Priorities)ans;
                    bl.AddParcel(p);
                    Console.WriteLine("Succeeded");
                    break;
                default:
                    Console.WriteLine("Wrong number");
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
            int id1, numOfChargeSlots;
            double chargingTime;
            string name, phone;
            Console.WriteLine("Press 1 to update drone data");
            Console.WriteLine("Press 2 to update station data");
            Console.WriteLine("Press 3 to update customer data");
            Console.WriteLine("Press 4 to send drone to charge");
            Console.WriteLine("Press 5 to release drone from charging");
            Console.WriteLine("Press 6 to assign parcel to drone");
            Console.WriteLine("Press 7 to collect parcel by drone");
            Console.WriteLine("Press 8 to supply parcel by drone");
            int.TryParse(Console.ReadLine(), out userChoise);
            update = (UpdateOptions)userChoise;
            switch (update)
            {
                case UpdateOptions.UpdateDroneData:
                    Console.WriteLine("Please enter the drone ID and the new model");
                    int.TryParse(Console.ReadLine(), out id1);
                    name = Console.ReadLine();
                    bl.UpdateDrone(id1, name);
                    Console.WriteLine("Succeeded");
                    break;
                case UpdateOptions.UpdateStationData:
                    Console.WriteLine(@"Please enter the station ID and details to update (name Of Station\total number of charge slots)");
                    int.TryParse(Console.ReadLine(), out id1);
                    name = Console.ReadLine();
                    int.TryParse(Console.ReadLine(), out numOfChargeSlots);
                    bl.UpdateBaseStation(id1, name, numOfChargeSlots);
                    Console.WriteLine("Succeeded");
                    break;
                case UpdateOptions.UpdateCustomerData:
                    Console.WriteLine("Please enter ID, a new name and/or phone number");
                    int.TryParse(Console.ReadLine(), out id1);
                    name = Console.ReadLine();
                    phone = Console.ReadLine();
                    bl.UpdateCustomer(id1, name, phone);
                    Console.WriteLine("Succeeded");
                    break;
                case UpdateOptions.SendDroneToCharge:
                    Console.WriteLine("Please enter the drone ID ");
                    int.TryParse(Console.ReadLine(), out id1);
                    bl.SendDroneToCharge(id1);
                    Console.WriteLine("Succeeded");
                    break;
                case UpdateOptions.ReleaseDroneFromCharge:
                    Console.WriteLine("Please enter the drone ID and the time of charging in hours");
                    int.TryParse(Console.ReadLine(), out id1);
                    double.TryParse(Console.ReadLine(), out chargingTime);
                    bl.ReleaseDroneFromCharge(id1, chargingTime);
                    Console.WriteLine("Succeeded");
                    break;
                case UpdateOptions.AssignParcelToDrone:
                    Console.WriteLine("Please enter the ID of the drone");
                    int.TryParse(Console.ReadLine(), out id1);
                    bl.AssignParcelToDrone(id1);
                    Console.WriteLine("Succeeded");
                    break;
                case UpdateOptions.CollectParcelByDrone:
                    Console.WriteLine("Please enter the drone ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    bl.CollectingParcelByDrones(id1);
                    Console.WriteLine("Succeeded");
                    break;
                case UpdateOptions.SupplyParcelByDrone:
                    Console.WriteLine("Please enter the drone ID");
                    int.TryParse(Console.ReadLine(), out id1);
                    bl.SupplyDeliveryToCustomer(id1);
                    Console.WriteLine("Succeeded");
                    break;
                default:
                    Console.WriteLine("Wrong number");
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
                    Console.WriteLine(bl.GetBaseStation(id));
                    foreach (var item in bl.GetBaseStation(id).DroneInChargingList)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine();
                    Console.WriteLine("Succeeded");
                    break;
                case DisplayOptions.DroneDisplay:
                    Console.WriteLine("Please enter the Drone ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(bl.GetDrone(id));
                    Console.WriteLine("Succeeded");
                    break;
                case DisplayOptions.CustomerDisplay:
                    Console.WriteLine("Please enter the customer ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(bl.GetCustomer(id));
                    foreach (var item in bl.GetCustomer(id).ListOfParcelsFromMe)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine();
                    foreach (var item in bl.GetCustomer(id).ListOfParcelsIntendedToME)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine();
                    Console.WriteLine("Succeeded");
                    break;
                case DisplayOptions.ParcelDisplay:
                    Console.WriteLine("Please enter the parcel ID");
                    int.TryParse(Console.ReadLine(), out id);
                    Console.WriteLine(bl.GetParcel(id));
                    Console.WriteLine("Succeeded");
                    break;
                default:
                    Console.WriteLine("Wrong number");
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
                    foreach (StationForList station in bl.GetListOfBaseStations())
                        Console.WriteLine(station.ToString());
                    Console.WriteLine("Succeeded");
                    break;
                case DisplayListsOptions.DroneList:
                    foreach (DroneForList drone in bl.GetListOfDrones())
                        Console.WriteLine(drone.ToString());
                    Console.WriteLine("Succeeded");
                    break;
                case DisplayListsOptions.CustomerList:
                    foreach (CustomerForList customer in bl.GetListOfCustomers())
                        Console.WriteLine(customer.ToString());
                    Console.WriteLine("Succeeded");
                    break;
                case DisplayListsOptions.ParcelList:
                    foreach (ParcelForList parcel in bl.GetListOfParcels())
                        Console.WriteLine(parcel.ToString());
                    Console.WriteLine("Succeeded");
                    break;
                case DisplayListsOptions.ParselsNotAssociatedWithDrones:
                    foreach (ParcelForList parcel in bl.GetListOfNotAssociatedParcels())
                        Console.WriteLine(parcel.ToString());
                    Console.WriteLine("Succeeded");
                    break;
                case DisplayListsOptions.StationsWithAvailableChargings:
                    foreach (StationForList station in bl.GetListOfStationsWithAvailableChargeSlots())
                        Console.WriteLine(station.ToString());
                    Console.WriteLine("Succeeded");
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
                    Customer c = bl.GetCustomer(id);
                    Console.WriteLine($"The distance from the customer:{BL.BL.DistanceBetweenPlaces(c.Location.Longitude, c.Location.Latitude, longitude, latitude)}");
                    break;//
                case FindDistances.StationDistance:
                    Console.WriteLine("Enter the location (longitude & latitude) ");
                    check = int.TryParse(Console.ReadLine(), out longitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    check = int.TryParse(Console.ReadLine(), out latitude);
                    if (!check) Console.WriteLine("Write only with numbers");
                    Console.WriteLine("Enter the station's Id");
                    int.TryParse(Console.ReadLine(), out id);
                    Station s = bl.GetBaseStation(id);
                    Console.WriteLine($"The distance from the station is:{BL.BL.DistanceBetweenPlaces(s.Location.Longitude, s.Location.Latitude, longitude, latitude)}");
                    break;
                default:
                    Console.WriteLine("Wrong number");
                    break;
            }


        }
    }
}
