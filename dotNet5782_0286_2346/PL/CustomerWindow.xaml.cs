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
            idTextBox.IsEnabled = false;
            longitudeTextBox.IsEnabled = false;
            latitudeTextBox.IsEnabled = false;
        }
        public CustomerWindow(ref IBL bl)
        {
            InitializeComponent();
            bL = bl;
            parcelsfromCustomerDataGrid.Visibility = Visibility.Collapsed;
            parcelsIntendedToCustomerDataGrid.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Collapsed;
            parcelsFromCustomerLabel.Visibility = Visibility.Collapsed;
            parcelsIntendedToCustomerLabel.Visibility = Visibility.Collapsed;
            addButton.IsEnabled = false;
            idTextBox.TextChanged += addButton_isEnable;
            nameTextBox.TextChanged += addButton_isEnable;
            phoneTextBox.TextChanged += addButton_isEnable;
            longitudeTextBox.TextChanged += addButton_isEnable;
            latitudeTextBox.TextChanged += addButton_isEnable;
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
                MessageBox.Show("The customer was successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                new CustomersListWindow(ref bL).ShowDialog();
            }
            catch(IdNotFoundException )
           {
                idTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show($"The station id is already existed,\nPlease check this data field", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;
                int.TryParse(idTextBox.Text, out id);
                bL.UpdateCustomer(id, nameTextBox.Text, phoneTextBox.Text);
                MessageBox.Show( "The customer is updated", "Success",MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                new CustomerWindow(ref bL, customer.Id).Show();
            }
            catch(IdNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addButton_isEnable(object sender, TextChangedEventArgs e)
        {
            if (idTextBox.Text.Length == 9 && nameTextBox.Text != null && phoneTextBox.Text != null && longitudeTextBox.Text != null && latitudeTextBox.Text != null)
                addButton.IsEnabled = true;
            else
                addButton.IsEnabled = false;
        }
        internal void TextBoxOnlyNumbersPreviewKeyDown(object sender, KeyEventArgs e)//i dont know why i should write it twice
        {
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;

            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;

            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
             || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            //allow control system keys
            if (Char.IsControl(c)) return;

            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox

            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        }
    }
}
