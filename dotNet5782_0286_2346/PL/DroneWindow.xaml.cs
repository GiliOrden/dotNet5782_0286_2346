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
using IBL.BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.IBL droneWindowBL;
        DroneForList drone;
        public DroneWindow(ref IBL.IBL bl)//first constructor for adding
        {
            droneWindowBL = bl;
            InitializeComponent();
            updateButton.Visibility = Visibility.Collapsed;
            DroneStatusComboBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.EnumsBL.DroneStatuses));
            MaxWeightComboBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.EnumsBL.WeightCategories));
            stationsListBox.ItemsSource = bl.GetListOfBaseStations();
            stationsListBox.SelectedValuePath = "ID";
            idTextBox.TextChanged += addButton_isEnable;
            modelTextBox.TextChanged += addButton_isEnable;
            MaxWeightComboBox.SelectionChanged += addButton_isEnable;
            stationsListBox.SelectionChanged += addButton_isEnable;
        }

        public DroneWindow(ref IBL.IBL bl, ref IBL.BO.DroneForList selectedDrone)//second constructor for update
        {
            droneWindowBL = bl;
            drone = selectedDrone;
            drone.Location = new();
            InitializeComponent();
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
            droneWindowGrid.DataContext = selectedDrone;
            DroneStatusComboBox.DataContext = selectedDrone.DroneStatus;
            MaxWeightComboBox.DataContext = selectedDrone.MaxWeight;
            longitudeTextBox.DataContext = selectedDrone.Location.Longitude;
            latitudeTextBox.DataContext = selectedDrone.Location.Latitude;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addButton_isEnable(object sender, RoutedEventArgs e)
        {
            if (idTextBox.Text.Length == 9 && modelTextBox != null && MaxWeightComboBox != null && stationsListBox.SelectedItem != null)
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
                StationForList station = (IBL.BO.StationForList)stationsListBox.SelectedItem;
                idOfStation = station.ID;
                droneWindowBL.AddDrone(drone, idOfStation);
                MessageBox.Show("The drone was successfully added");               
            }
            catch (ExistIdException ex)
            {
                idTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show(ex.Message);
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
            try
            {
                droneWindowBL.UpdateDrone(drone.Id, modelTextBox.Text);
            }
            catch (IdNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void sendToChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                droneWindowBL.SendDroneToCharge(drone.Id);
            }
            catch (IdNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void releaseDroneFromChargeButton_Click(object sender, RoutedEventArgs e)
        {
            droneWindowBL.ReleaseDroneFromCharge(drone.Id, DateTime.Now);
        }

        private void sendDroneToDeliveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                droneWindowBL.SendDroneToCharge(drone.Id);
            }
            catch (IdNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void collectParcelButton_Click(object sender, RoutedEventArgs e)//doesn't need exeption either, the id have chacked in 'sendDroneToDelivery'
        {
            droneWindowBL.CollectingParcelByDrones(drone.Id);
        }

        private void supplyParcelButton_Click(object sender, RoutedEventArgs e)//doesn't need exeption either, the id have chacked in 'sendDroneToDelivery'
        {
            droneWindowBL.SupplyDeliveryToCustomer(drone.Id);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var selectedItem in stationsListBox.SelectedItems) { }

        }

        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}    