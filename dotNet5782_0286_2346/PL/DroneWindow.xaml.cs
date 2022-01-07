using BL;
using BlApi;
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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL droneWindowBL;
        DroneForList drone;
        public DroneWindow(ref IBL bl)//first constructor for adding
        {
            droneWindowBL = bl;
            InitializeComponent();
            updateButton.Visibility = Visibility.Collapsed;
            DroneStatusComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.DroneStatuses));
            MaxWeightComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.WeightCategories));
            stationsListBox.ItemsSource = bl.GetListOfBaseStations();
            stationsListBox.SelectedValuePath = "ID";
            idTextBox.TextChanged += addButton_isEnable;
            modelTextBox.TextChanged += addButton_isEnable;
            MaxWeightComboBox.SelectionChanged += addButton_isEnable;
            stationsListBox.SelectionChanged += addButton_isEnable;
        }

        public DroneWindow(ref IBL bl,  DroneForList selectedDrone)//second constructor for update
        {
            droneWindowBL = bl;
            drone = selectedDrone;           
            InitializeComponent();
            MaxWeightComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.WeightCategories));
            DroneStatusComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.DroneStatuses));
            droneWindowGrid.DataContext = selectedDrone;
            addButton.Visibility = Visibility.Collapsed;
            stationsListBox.Visibility = Visibility.Collapsed;
            chooseStationTextBox.Visibility = Visibility.Collapsed;
            idTextBox.IsEnabled = false;
            MaxWeightComboBox.IsEnabled = false;
            if (selectedDrone.DroneStatus == EnumsBL.DroneStatuses.Available)
            {
                sendToChargeButton.Visibility = Visibility.Visible;
                sendDroneToDeliveryButton.Visibility = Visibility.Visible;
            }
            if (selectedDrone.DroneStatus == EnumsBL.DroneStatuses.Maintenance)
            {
                releaseDroneFromChargeButton.Visibility = Visibility.Visible;
            }
            if (selectedDrone.DroneStatus == EnumsBL.DroneStatuses.OnDelivery)
            {
                if (droneWindowBL.GetParcel(selectedDrone.IdOfTheDeliveredParcel).CollectionTime == null)
                {
                    collectParcelButton.Visibility = Visibility.Visible;
                }
                else if (droneWindowBL.GetParcel(selectedDrone.IdOfTheDeliveredParcel).DeliveryTime == null)
                {
                    supplyParcelButton.Visibility = Visibility.Visible;
                }
            }

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addButton_isEnable(object sender, RoutedEventArgs e)
        {
            if (idTextBox.Text.Length != 0 && modelTextBox != null && MaxWeightComboBox != null && stationsListBox.SelectedItem != null)
                addButton.IsEnabled = true;
        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idOfStation;
                drone = new();
                drone.Location = new();
                drone.Id = int.Parse(idTextBox.Text);
                drone.Model = modelTextBox.Text;
                drone.MaxWeight = (EnumsBL.WeightCategories?)MaxWeightComboBox.SelectedItem;
                StationForList station = (BO.StationForList)stationsListBox.SelectedItem;
                idOfStation = station.ID;
                droneWindowBL.AddDrone(drone, idOfStation);
                MessageBox.Show("The drone was successfully added");
                Close();
                new DroneListWindow(ref  droneWindowBL).Show();
            }
            catch (ExistIdException)
            {
                idTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show($"The drone id is already existed,\nPlease check this data field","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }

        private void TextBoxOnlyNumbersPreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;

            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;

            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
             || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            //allow control system keys
            if (Char.IsControl(c)) return;

            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox

            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)//this is a func for 'click' event
        {

                droneWindowBL.UpdateDrone(drone.Id, modelTextBox.Text);
                MessageBox.Show($"The drone was successfully updated","Success",MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                new DroneWindow(ref droneWindowBL, droneWindowBL.GetDroneForList(drone.Id)).Show();

        }
        private void sendToChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                droneWindowBL.SendDroneToCharge(drone.Id);
                MessageBox.Show("The drone was sent to charging");
                this.Close();
                new DroneWindow(ref droneWindowBL, droneWindowBL.GetDroneForList(drone.Id)).Show();
            }
            catch (NoBatteryException)
            {
                MessageBox.Show($"The drone id is already existed,\nPlease check this data field", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void releaseDroneFromChargeButton_Click(object sender, RoutedEventArgs e)
        {
            droneWindowBL.ReleaseDroneFromCharge(drone.Id, DateTime.Now);
            MessageBox.Show("The drone was released from charging");
            this.Close();
            new DroneWindow(ref droneWindowBL, droneWindowBL.GetDroneForList(drone.Id)).Show();
        }

        private void sendDroneToDeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                droneWindowBL.AssignParcelToDrone(drone.Id);
                MessageBox.Show("The drone was assigned to parcel!");
                this.Close();
                new DroneWindow(ref droneWindowBL, droneWindowBL.GetDroneForList(drone.Id)).Show();
            }
            catch (NoBatteryException)
            {
                MessageBox.Show("The drone battery is to low,\nPlease send it to charge","Error",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(DroneMaxWeightIsLowException)
            {
                MessageBox.Show("The maximum weight that the drone can carry is not compatible with any package", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void collectParcelButton_Click(object sender, RoutedEventArgs e)//doesn't need exeption either, the id have chacked in 'sendDroneToDelivery'
        {
          droneWindowBL.CollectParcelByDrone(drone.Id);
          MessageBox.Show("The parcel was collected successfully","Success",MessageBoxButton.OK,MessageBoxImage.Information);
          this.Close();
          new DroneWindow(ref droneWindowBL, droneWindowBL.GetDroneForList(drone.Id)).Show();
        }

        private void supplyParcelButton_Click(object sender, RoutedEventArgs e)//doesn't need exeption either, the id have chacked in 'sendDroneToDelivery'
        {
          droneWindowBL.SupplyDeliveryToCustomer(drone.Id);
          MessageBox.Show("The drone was supplied to customer", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
          this.Close();
          new DroneWindow(ref droneWindowBL, droneWindowBL.GetDroneForList(drone.Id)).Show();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var selectedItem in stationsListBox.SelectedItems) { }

        }

        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
          
        }


    }
}