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
        public DroneWindow(ref IBL.IBL bl)//first constructor
        {
            InitializeComponent();
            updateButton.Visibility = Visibility.Collapsed;
        }

        public DroneWindow(ref IBL.BO.DroneForList drone)//second constructor
        {
            InitializeComponent();
            addButton.Visibility = Visibility.Collapsed;
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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
