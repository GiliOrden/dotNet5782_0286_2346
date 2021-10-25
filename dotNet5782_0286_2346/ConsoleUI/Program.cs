﻿using System;
using IDAL.DO;
namespace ConsoleUI
{
    class Program
    {
        //print menue according to user instructions
        enum MenuOptions { Exit,ADD,Update,ShowOne,ShowList}
        enum AddOptions { Exit,AddDrone,AddStation}//sub-menue
        void PrintMenue()
        {
            MenuOptions mo;
            int opt;
            bool b = int.TryParse(Console.ReadLine(), out opt);
             mo = (MenuOptions)opt;
            switch (mo)
            {
                case MenuOptions.Exit:
                    break;
                case MenuOptions.ADD:
                    break;
                case MenuOptions.Update:
                    break;
                case MenuOptions.ShowOne:
                    break;
                case MenuOptions.ShowList:
                    break;
                default:
                    break;
            }
        }
        static void Main(string[] args)
        {

            DalObject.DalObject d1 = new DalObject.DalObject();//it will produce the data base
            Drone d = new Drone();
            d.MaxWeight = WeightCategories.Heavy;
            // d1.AddDrone(d);
        }
    }
}
