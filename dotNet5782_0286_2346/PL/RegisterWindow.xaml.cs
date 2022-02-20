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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        IBL bL;
        Customer customer;
        User user;
        public RegisterWindow(ref IBL bl)
        {
            bL = bl;
            customer = new();
            InitializeComponent();           
            customerGrid.DataContext = customer;
            idTextBox.TextChanged += RegisterButton_isEnable2;
            nameTextBox.TextChanged += RegisterButton_isEnable2;
            phoneTextBox.TextChanged += RegisterButton_isEnable2;
            longitudeTextBox.TextChanged += RegisterButton_isEnable2;
            latitudeTextBox.TextChanged += RegisterButton_isEnable2;
            passwordTextBox.TextChanged += RegisterButton_isEnable2;
            RegisterButton.Click += RegisterButton_Click2;
            usernameTextBox.Visibility = Visibility.Hidden;
            userNameLabel.Visibility = Visibility.Hidden;
            statusLabel.Visibility = Visibility.Hidden;
            statusSelector.Visibility = Visibility.Hidden;
        }

        public RegisterWindow(ref IBL bl,ref User u)
        {
            bL = bl;
            InitializeComponent();
            customerGrid.Visibility = Visibility.Collapsed;
            statusSelector.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.CompanyStatuses));
            RegisterButton.Click += RegisterButton_Click1;
            statusSelector.SelectionChanged += StatusSelector_SelectionChanged;
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (usernameTextBox.Text!=" " && passwordTextBox.Text!=" " && statusSelector.SelectedItem != null)
                RegisterButton.IsEnabled = true;
        }
        private void RegisterButton_Click1(object sender, RoutedEventArgs e)
        {
            try
            {
                user = new();
                user.Name = usernameTextBox.Text;
                user.Password = passwordTextBox.Text;
                user.Status = (EnumsBL.UserStatuses)statusSelector.SelectedItem;
                bL.AddUser(user);
                MessageBox.Show("The user added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch(ExistUserException ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterButton_isEnable2(object sender, TextChangedEventArgs e)
        {
            if (idTextBox.Text.Length == 9 && nameTextBox.Text.Length != 0 && phoneTextBox.Text.Length != 0 && longitudeTextBox.Text.Length != 0 && latitudeTextBox.Text.Length != 0&&passwordTextBox.Text.Length!=0)
                RegisterButton.IsEnabled = true;
            else
                RegisterButton.IsEnabled = false;
        }
        private void RegisterButton_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                customer = new();
                user = new();
                customer.Location = new();
                customer.Id = int.Parse(idTextBox.Text);
                customer.Name = nameTextBox.Text;
                customer.Phone = phoneTextBox.Text;
                customer.Location.Longitude = double.Parse(longitudeTextBox.Text);
                customer.Location.Latitude = double.Parse(latitudeTextBox.Text);
                user.Name = customer.Name;
                user.Password = passwordTextBox.Text;
                user.Status = EnumsBL.UserStatuses.Customer;
                bL.AddCustomer(customer);
                bL.AddUser(user);
                MessageBox.Show("Hi! you are in,thanks for joining", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                CustomerInterfaceWindow ci = new(ref bL, user);
                ci.Show();
                this.Close();
            }
            catch (ExistIdException ex)
            {
                idTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(ExistUserException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error) ;
            }
        }
    }
}
