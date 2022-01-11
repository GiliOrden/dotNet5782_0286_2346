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
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        IBL bL;
        Station station;
        public StationWindow(ref IBL bl,int id)
        {            
            InitializeComponent();
            bL = bl;
            station = bL.GetBaseStation(id);
            stationGrid.DataContext = station;
            droneInChargingDataGrid.DataContext = station.DroneInChargingList;
            idTextBox.IsEnabled = false;
            addButton.Visibility = Visibility.Collapsed;
        }
        public StationWindow(ref IBL bl)
        {
            InitializeComponent();
            bL = bl;
            droneInChargingDataGrid.Visibility=Visibility.Collapsed;
            dronesLabel.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Collapsed;
            addButton.IsEnabled = false;
            idTextBox.TextChanged += addButton_isEnable;
            nameTextBox.TextChanged += addButton_isEnable;
            chargeSlotsTextBox.TextChanged += addButton_isEnable;
            longitudeTextBox.TextChanged += addButton_isEnable;
            latitudeTextBox.TextChanged += addButton_isEnable;
        }
        private void addButton_isEnable(object sender, RoutedEventArgs e)
        {
            if (idTextBox.Text.Length != 0 && nameTextBox.Text != null && chargeSlotsTextBox.Text != null && longitudeTextBox.Text != null && latitudeTextBox.Text != null)
                addButton.IsEnabled = true;
            else
                addButton.IsEnabled = false;
        }
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(chargeSlotsTextBox.Text, out id);
            bL.UpdateBaseStation(station.ID, nameTextBox.Text, id);
            MessageBox.Show("The station is updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            new CustomerWindow(ref bL,station.ID).Show();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idOfStation,chargeSlots;
                double longitude, latitude;
                station = new();
                station.Location = new();               
                int.TryParse(idTextBox.Text,out idOfStation);
                station.ID=idOfStation;
                station.Name = nameTextBox.Text;
                int.TryParse(chargeSlotsTextBox.Text, out chargeSlots);
                station.ChargeSlots = chargeSlots;
                double.TryParse(longitudeTextBox.Text, out longitude);
                station.Location.Longitude = longitude;
                double.TryParse(latitudeTextBox.Text, out latitude);
                station.Location.Latitude = latitude;
                bL.AddBaseStation(station);
                MessageBox.Show("The station was successfully added");
                Close();
                new BaseStaionsListWindow(ref bL).Show();
            }
            catch (ExistIdException)
            {
               idTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show($"The station id is already existed,\nPlease check this data field", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new MainWindow().Show();
        }

        private void droneInChargingDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneInCharging drone = droneInChargingDataGrid.SelectedItem as DroneInCharging;
            new DroneWindow(ref bL, drone.Id).Show();
        }
    }
}