﻿using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for BaseStaionsListWindow.xaml
    /// </summary>
    public partial class BaseStaionsListWindow : Window
    {
        IBL bL;
        public BaseStaionsListWindow(ref IBL bl)
        {
            InitializeComponent();
            bL = bl;
            stationForListDataGrid.DataContext = bl.GetListOfBaseStations();
        }

        private void stationForListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationForList station = stationForListDataGrid.SelectedItem as StationForList;
            if (station != null)
            {
                new StationWindow(ref bL,station.ID).Show();               
            }           
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mw=new();
            mw.Show();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            StationWindow sw = new(ref bL);
            sw.Show();
        }

        private void groupByStationsWithAvailableChargeSlotesButton_Click(object sender, RoutedEventArgs e)
        {
            stationForListDataGrid.DataContext=from station in bL.GetListOfBaseStations()
                                                group station by  station.AvailableChargingPositions > 0 into gs
                                                select gs;
        }

        private void groupByNumberOfAvailableChargeSlotsButton_Click(object sender, RoutedEventArgs e)
        {

            //stationForListDataGrid.DataContext = from station in bL.GetListOfBaseStations()
            //                                     group station by station.AvailableChargingPositions into gs
            //                                     select gs;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(stationForListDataGrid.DataContext);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("AvailableChargingPositions");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new BaseStaionsListWindow(ref bL).Show();
        }
    }
}
