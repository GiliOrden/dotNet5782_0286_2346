
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal IBL bl = BlFactory.GetBl();

        public MainWindow()
        {
            InitializeComponent();
            
        }
        ~MainWindow()
        {

        }
        private void ShowDronesList_Click(object sender, RoutedEventArgs e)
        {
            DroneListWindow dw = new DroneListWindow(ref bl) ;
            dw.Show();
        }


        private void baseStationsButton_Click(object sender, RoutedEventArgs e)
        {
            BaseStaionsListWindow bw = new BaseStaionsListWindow(ref bl);
            bw.ShowDialog();
        }

        private void customersButton_Click(object sender, RoutedEventArgs e)
        {
            CustomersListWindow cw = new CustomersListWindow(ref bl);
            cw.ShowDialog();
        }
        private void ShowParcelsList_Click(object sender, RoutedEventArgs e)
        {
            ParcelListWindow pw = new ParcelListWindow(ref bl);
            pw.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("Closing called");
            foreach (DroneForList drone in bl.GetDronesByPredicate(drone=>drone.DroneStatus==EnumsBL.DroneStatuses.Maintenance))
            {
                bl.ReleaseDroneFromCharge(drone.Id, DateTime.Now);
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
