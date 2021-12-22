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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.IBL droneListWindowBL;
        public DroneListWindow(ref IBL.IBL bl)
        {
            InitializeComponent();
            droneListWindowBL = bl;
            DroneListView.ItemsSource = droneListWindowBL.GetListOfDrones();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.EnumsBL.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.EnumsBL.WeightCategories));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           ( StatusSelector.SelectedItem as IBL.BO.EnumsBL.DroneStatuses)
        
        DroneListView.ItemsSource = droneListWindowBL.GetDronesByPredicate(drone => drone.DroneStatus == StatusSelector.SelectedItem as IBL.BO.EnumsBL.DroneStatuses));
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
