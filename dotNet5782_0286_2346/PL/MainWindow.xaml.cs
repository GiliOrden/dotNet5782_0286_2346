
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
        User u;
        public MainWindow()
        {
            InitializeComponent();
            EnterButton.IsEnabled = false;           
            PasswordBox.PasswordChanged += EnterButtonIsEnable;
            UserNameTextBox.TextChanged += EnterButtonIsEnable;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow rw = new(ref bl);
            rw.ShowDialog();
            this.Close();
        }

        private void EnterButtonIsEnable(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password != "" && UserNameTextBox.Text != "")
                EnterButton.IsEnabled = true;
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                u = bl.GetUser(UserNameTextBox.Text, PasswordBox.Password.ToString());
                if(u.Status==EnumsBL.UserStatuses.Manager||u.Status==EnumsBL.UserStatuses.Worker)
                {
                    MenuWindow mw = new(ref bl);
                    mw.Show();
                }
                else { 
                       }
            }
            catch(UserNotFoundException ex)
            {
                PasswordBox.BorderBrush = Brushes.Red;
                UserNameTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
