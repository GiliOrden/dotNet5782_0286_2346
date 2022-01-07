using BlApi;
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
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for CustomersListWindow.xaml
    /// </summary>
    public partial class CustomersListWindow : Window
    {
        IBL bL;
        public CustomersListWindow(ref IBL bl)
        {
            InitializeComponent();
            bL = bl;
            customerForListDataGrid.DataContext = bL.GetListOfCustomers();
        }

        private void customerForListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomerForList customer = customerForListDataGrid.SelectedItem as CustomerForList;
            if (customer != null)
            {
                CustomerWindow cw = new CustomerWindow(ref bL, customer.ID);
                cw.ShowDialog();
            }
        }

        private void addCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow cw = new CustomerWindow(ref bL);
            cw.ShowDialog();
        }
    }
}
