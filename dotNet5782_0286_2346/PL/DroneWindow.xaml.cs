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
        IBL.IBL DroneWindowBL;
        DroneForList drone;
        public DroneWindow(ref IBL.IBL bl)//first constructor
        {
            DroneWindowBL = bl;
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

        public DroneWindow(ref IBL.BO.DroneForList drone)//second constructor
        {
            InitializeComponent();
            addButton.Visibility = Visibility.Collapsed;
            stationsListBox.Visibility = Visibility.Collapsed;
            chooseStationTextBox.Visibility = Visibility.Collapsed;
            idTextBox.IsEnabled = false;
            MaxWeightComboBox.IsEnabled = false;
            droneWindowGrid.DataContext = drone;
            DroneStatusComboBox.DataContext = drone.DroneStatus;
            MaxWeightComboBox.DataContext = drone.MaxWeight;
            longitudeTextBox.DataContext = drone.Location.Longitude;
            latitudeTextBox.DataContext = drone.Location.Latitude;
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
                idOfStation = int.Parse(stationsListBox.SelectedValuePath);
                DroneWindowBL.AddDrone(drone, idOfStation);
                MessageBox.Show("The drone was successfully added");
                
            }
            catch(ExistIdException ex)
            {
                idTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show(ex.Message);
            }

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
         
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
