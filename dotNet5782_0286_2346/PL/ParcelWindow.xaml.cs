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

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        IBL parc;
        public ParcelWindow(ref IBL bl, BO.Parcel p)//update window
        {
            parc = bl;
            InitializeComponent();
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.Priorities));
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.WeightCategories));
            
            ListBoxItem item = new ListBoxItem();
            item.Content = p.Sender;
            senderListBox.Items.Add(item);
            ListBoxItem item2 = new ListBoxItem();
            item2.Content = p.Receiver;
            receiverListBox.Items.Add(item2);
            ListBoxItem item3 = new ListBoxItem();
            item3.Content = p.Drone;
            droneListBox.Items.Add(item3);

            gridOfParcel.DataContext = p;
        }

        public ParcelWindow(ref IBL bl)//adding window
        {
            parc = bl;
            InitializeComponent();
            UpdateParcel.Visibility = Visibility.Collapsed;
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.Priorities));
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.WeightCategories));
            //אמורות להופיע ID של המוכר והקונה לבחירה ובנוסף כל שאר האפשרויות צריכות להיות חסומות.
        }


        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            //droneWindowBL.UpdateDrone(drone.Id, modelTextBox.Text);
            //MessageBox.Show($"The parcel was successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //this.Close();
            //new DroneWindow(ref droneWindowBL, droneWindowBL.GetDroneForList(drone.Id)).Show();
        }
    }
}
