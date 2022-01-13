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
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        
        IBL droneListWindowBL;
        public DroneListWindow(ref IBL bl)
        {
            InitializeComponent();
            droneListWindowBL = bl;
            droneForListDataGrid.DataContext = bl.GetListOfDrones();
            droneForListDataGrid.IsReadOnly=true;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(EnumsBL.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(EnumsBL.WeightCategories));
        }

        private void statusSelectorAndWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem != null && WeightSelector.SelectedItem != null)
            {
                droneForListDataGrid.ItemsSource = droneListWindowBL.GetDronesByPredicate(drone => drone.DroneStatus == (EnumsBL.DroneStatuses)StatusSelector.SelectedItem && drone.MaxWeight == (EnumsBL.WeightCategories)WeightSelector.SelectedItem);
            }
            else if (StatusSelector.SelectedItem != null)
            {
                droneForListDataGrid.ItemsSource = droneListWindowBL.GetDronesByPredicate(drone => drone.DroneStatus == (EnumsBL.DroneStatuses)StatusSelector.SelectedItem);
            }
            else if (WeightSelector.SelectedItem != null)
            {
                droneForListDataGrid.ItemsSource = droneListWindowBL.GetDronesByPredicate(drone => drone.MaxWeight == (EnumsBL.WeightCategories)WeightSelector.SelectedItem);
            }

            droneForListDataGrid.Items.Refresh();
        }


        private void DroneView_SelectionChanged(object sender, SelectionChangedEventArgs e)//update drone
        {
            DroneForList drone = droneForListDataGrid.SelectedItem as DroneForList;
            
            if (drone != null)
            {
                DroneWindow dw = new DroneWindow(ref droneListWindowBL, drone.Id);
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

        private void GroupByStatus_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(droneForListDataGrid.DataContext);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("DroneStatus");
            view.GroupDescriptions.Add(groupDescription);
        }

        
    }
}