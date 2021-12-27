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
        private IBL.IBL droneListWindowBL;
        ObservableCollection<IBL.BO.DroneForList> dronesObservableCollection;
        

        private void statusSelectorAndWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem != null && WeightSelector.SelectedItem != null)
            {
                DroneListView.ItemsSource = droneListWindowBL.GetDronesByPredicate(drone => drone.DroneStatus == (IBL.BO.EnumsBL.DroneStatuses)StatusSelector.SelectedItem && drone.MaxWeight == (IBL.BO.EnumsBL.WeightCategories)WeightSelector.SelectedItem);
            }
            else if (StatusSelector.SelectedItem != null)
            {
                DroneListView.ItemsSource = droneListWindowBL.GetDronesByPredicate(drone => drone.DroneStatus == (IBL.BO.EnumsBL.DroneStatuses)StatusSelector.SelectedItem);
            }
            else if (WeightSelector.SelectedItem != null)
            {

                DroneListView.ItemsSource = droneListWindowBL.GetDronesByPredicate(drone => drone.MaxWeight == (IBL.BO.EnumsBL.WeightCategories)WeightSelector.SelectedItem);
            }

            DroneListView.Items.Refresh();
        }


        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)//update drone
        {
            IBL.BO.DroneForList drone = DroneListView.SelectedItem as IBL.BO.DroneForList;
            if (drone != null)
            {
                DroneWindow dw = new DroneWindow(ref droneListWindowBL,ref drone);
                dw.ShowDialog();
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
