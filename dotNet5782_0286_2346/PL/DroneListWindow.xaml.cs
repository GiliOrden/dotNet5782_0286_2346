using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL droneListWindowBL;
        ObservableCollection<DroneForList> dronesObservableCollection;
        public DroneListWindow(ref IBL bl)
        {
            InitializeComponent();
            droneListWindowBL = bl;
            dronesObservableCollection = new ObservableCollection<DroneForList>(droneListWindowBL.GetListOfDrones());
            DroneListView.ItemsSource = dronesObservableCollection;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(EnumsBL.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(EnumsBL.WeightCategories));
        }

        private void statusSelectorAndWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem != null && WeightSelector.SelectedItem != null)
            {
                DroneListView.ItemsSource = droneListWindowBL.GetDronesByPredicate(drone => drone.DroneStatus == (EnumsBL.DroneStatuses)StatusSelector.SelectedItem && drone.MaxWeight == (EnumsBL.WeightCategories)WeightSelector.SelectedItem);
            }
            else if (StatusSelector.SelectedItem != null)
            {
                DroneListView.ItemsSource = droneListWindowBL.GetDronesByPredicate(drone => drone.DroneStatus == (EnumsBL.DroneStatuses)StatusSelector.SelectedItem);
            }
            else if (WeightSelector.SelectedItem != null)
            {
                DroneListView.ItemsSource = droneListWindowBL.GetDronesByPredicate(drone => drone.MaxWeight == (EnumsBL.WeightCategories)WeightSelector.SelectedItem);
            }

            DroneListView.Items.Refresh();
        }


        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)//update drone
        {
            DroneForList drone = DroneListView.SelectedItem as DroneForList;
            
            if (drone != null)
            {
                DroneWindow dw = new DroneWindow(ref droneListWindowBL, drone);
                dw.ShowDialog();
                Close();
            }
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)//add drone
        {
            DroneWindow dw = new DroneWindow(ref droneListWindowBL);
            dw.Show();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}