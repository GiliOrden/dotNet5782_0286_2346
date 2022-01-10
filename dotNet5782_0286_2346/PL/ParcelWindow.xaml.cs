using BlApi;
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
            senderListBox.ItemsSource = bl.GetListOfCustomers();
            receiverListBox.ItemsSource= bl.GetListOfCustomers();
            senderListBox.SelectionChanged += addButton_isEnable;
            receiverListBox.SelectionChanged += addButton_isEnable;
            priorityComboBox.SelectionChanged += addButton_isEnable;
            weightComboBox.SelectionChanged += addButton_isEnable;
        }

        private void addButton_isEnable(object sender, RoutedEventArgs e)
        {
            if (weightComboBox.SelectedItem != null && priorityComboBox.SelectedItem != null && receiverListBox.SelectedItem != null && senderListBox.SelectedItem != null)
                AddParcel.IsEnabled = true;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            //int idOfSender, idOfReceiver;
            //new ObservableCollection<BO.ParcelForList>
            BO.Parcel parcel = new();
            parcel.Weight = (BO.EnumsBL.WeightCategories)weightComboBox.SelectedItem;
            parcel.Priority = (BO.EnumsBL.Priorities)priorityComboBox.SelectedItem;
            parcel.Sender = new();
            BO.CustomerForList cust= (BO.CustomerForList)senderListBox.SelectedItem;
            parcel.Sender.ID = cust.ID;
            parcel.Sender.Name = cust.Name;
            parcel.Receiver = new();
            BO.CustomerForList cust2 = (BO.CustomerForList)receiverListBox.SelectedItem;
            parcel.Receiver.ID = cust2.ID;
            parcel.Receiver.Name = cust2.Name;
            parc.AddParcel(parcel);
            MessageBox.Show("The parcel was successfully added");
            Close();

            //var ParcelCollection = new ObservableCollection<BO.ParcelForList>(parc.GetListOfParcels());
            //ParcelListWindow.parcelForListDataGrid.DataContext = ParcelCollection;
            //new ParcelListWindow(ref parc).Show();
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
