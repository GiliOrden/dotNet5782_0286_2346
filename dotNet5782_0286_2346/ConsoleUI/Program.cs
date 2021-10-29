using System;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {

        static void Main(string[] args)
        {

            DalObject.DalObject d1 = new DalObject.DalObject();//it will produce the data base and the methods that related to it
            Drone d = new Drone();
            d.MaxWeight = WeightCategories.Heavy;

        
            // d1.AddDrone(d);
            //print menue according to user instructions
        enum MenuOptions {  Add, Update, ShowOne, ShowList, Exit }
        enum AddOptions { Exit, AddDrone, AddStation, AddCustomer }//sub-menue
        void PrintMenue()
        {
            MenuOptions mo;
            string userChoise = Console.ReadLine();
            mo = (MenuOptions)int.Parse(userChoise);
            /* int opt;
             bool b = int.TryParse(Console.ReadLine(), out opt);
             mo = (MenuOptions)opt;the lecturer offer*/
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



        
        
        
        public  void AddingOptions()
        {
            AddOptions add;
            string userChoise = Console.ReadLine();
            add = (AddOptions)int.Parse(userChoise);
            
            switch (add)
            {
                case AddOptions.Exit:

                    break;
                case AddOptions.AddDrone:
                   
                    break;
                case AddOptions.AddStation:
                    DalObject.AddStation(stations);
                    break;
                case AddOptions.AddCustomer:
                    NewMethod();
                    break;

                default:

                    break;
            }

            static void NewMethod()
            {
                AddCustomer(customers);
            }
        }
    
    }
    
}
