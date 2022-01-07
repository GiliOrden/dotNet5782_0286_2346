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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        IBL bL;
        Customer customer;
        public CustomerWindow(ref IBL bl,int id)
        {
            InitializeComponent();
            bL = bl;
            customer=bL.GetCustomer(id);
            customerGrid.DataContext = customer;
            parcelsfromCustomerDataGrid.DataContext = customer.ListOfParcelsFromMe;
            parcelsIntendedToCustomerDataGrid.DataContext = customer.ListOfParcelsIntendedToME;
            addButton.Visibility = Visibility.Collapsed;
        }
        public CustomerWindow(ref IBL bl)
        {
            InitializeComponent();
            bL = bl;
            parcelsfromCustomerDataGrid.Visibility = Visibility.Collapsed;
            parcelsIntendedToCustomerDataGrid.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Collapsed;
           
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                customer = new();
                customer.Location = new();
                customer.Id = int.Parse(idTextBox.Text);
                customer.Name = nameTextBox.Text;
                customer.Phone = phoneTextBox.Text;
                customer.Location.Longitude =double.Parse(longitudeTextBox.Text);
                customer.Location.Latitude = double.Parse(latitudeTextBox.Text);
                bL.AddCustomer(customer);
                MessageBox.Show("The customer was successfully added");
            }
            catch(IdNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
