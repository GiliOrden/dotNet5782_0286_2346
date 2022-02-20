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
    /// Interaction logic for CustomerInterfaceWindow.xaml
    /// </summary>
    public partial class CustomerInterfaceWindow : Window
    {
        IBL bl;
        User user;
        Customer customer;
        public CustomerInterfaceWindow(ref IBL bL ,User u)
        {
            InitializeComponent();
            bl = bL;
            user = u;
            customer = bl.GetCustomer(bl.GetListOfCustomers().FirstOrDefault(cust => cust.Name == u.Name).ID);
            customerGrid.DataContext = customer;
            userNameLabel.DataContext = u.Name;
            ListOfParcels.DataContext = bl.GetListOfCustomerParcels(customer);
            ListOfParcels.Visibility = Visibility.Hidden;
        }

        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            ParcelWindow pw = new(ref bl, user);
            pw.Show();
        }

        private void showParcelsButton_Click(object sender, RoutedEventArgs e)
        {
            ListOfParcels.Visibility = Visibility.Visible;
        }
    }
}
