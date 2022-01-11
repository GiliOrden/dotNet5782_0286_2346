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
        BO.Parcel parcel;
        public ParcelWindow(ref IBL bl, BO.Parcel p, BO.EnumsBL.ParcelStatuses parcelStatus)//update/delete parcel window
        {
            parc = bl;
            parcel = p;
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
            AddParcel.Visibility = Visibility.Collapsed;
            if (BO.EnumsBL.ParcelStatuses.Defined== parcelStatus)//the botton for delete parcel
            {
                DeleteParcel.IsEnabled = true;
                SupplyParcel.Visibility = Visibility.Collapsed;
                CollectParcel.Visibility = Visibility.Collapsed;
                ShowDrone.Visibility = Visibility.Collapsed;
            }
            else if (BO.EnumsBL.ParcelStatuses.Associated == parcelStatus)
            {
                CollectParcel.IsEnabled = true;
                SupplyParcel.Visibility = Visibility.Collapsed;
            }
           else if (BO.EnumsBL.ParcelStatuses.Collected == parcelStatus)
            {
                CollectParcel.Visibility = Visibility.Collapsed;
                SupplyParcel.IsEnabled = true;
            }
            else if (BO.EnumsBL.ParcelStatuses.Delivered == parcelStatus)
            {
                SupplyParcel.Visibility = Visibility.Collapsed;
                CollectParcel.Visibility = Visibility.Collapsed;
                ShowDrone.Visibility = Visibility.Collapsed;
            }
        }

        public ParcelWindow(ref IBL bl)//adding window
        {
            parc = bl;
            InitializeComponent();
            UpdateParcel.Visibility = Visibility.Collapsed;
            DeleteParcel.Visibility = Visibility.Collapsed;
            SupplyParcel.Visibility = Visibility.Collapsed;
            CollectParcel.Visibility = Visibility.Collapsed;
            ShowDrone.Visibility = Visibility.Collapsed;
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

            
        }  

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            //droneWindowBL.UpdateDrone(drone.Id, modelTextBox.Text);
            //MessageBox.Show($"The parcel was successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //this.Close();
            //new DroneWindow(ref droneWindowBL, droneWindowBL.GetDroneForList(drone.Id)).Show();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            parc.DeleteParcel(parcel.Id);
            MessageBox.Show("The parcel was successfully deleted");
            Close();
        }

        private void CollectButton_Click(object sender, RoutedEventArgs e)
        {
            parc.CollectParcelByDrone(parcel.Drone.Id);//problem associated
            MessageBox.Show("The parcel was successfully collected");
            Close();
        }

        private void SupplyButton_Click(object sender, RoutedEventArgs e)
        {
            parc.SupplyDeliveryToCustomer(parcel.Drone.Id);
            MessageBox.Show("The parcel was successfully supplied");
            Close();
        }

        private void ShowDroneButton_Click(object sender, RoutedEventArgs e) // need to fix the  DroneWindow to be for BO.Drone type. then it will work
        {
            BO.Drone drone = parc.GetDrone(parcel.Drone.Id);

            //if (drone != null)     
            //{
            //    DroneWindow dw = new DroneWindow(ref parc, drone);
            //    dw.ShowDialog();
            //    //Close();
            //}

        }

        private void ShowSenderButton_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            if (parcel != null)
            {
                id = parcel.Sender.ID;
                CustomerWindow cw = new CustomerWindow(ref parc, id);
                cw.ShowDialog();
            }
               
            else if (senderListBox.SelectedItem != null)
            {
                BO.CustomerForList cust = senderListBox.SelectedItem as BO.CustomerForList;
                id = cust.ID;
                CustomerWindow cw = new CustomerWindow(ref parc, id);
                cw.ShowDialog();
            }
            else
                MessageBox.Show("You didn't choose a customer");
           
        }

        private void ShowReceiverButton_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            if (parcel != null)
            {
                id = parcel.Receiver.ID;
                CustomerWindow cw = new CustomerWindow(ref parc, id);
                cw.ShowDialog();
            }

            else if (receiverListBox.SelectedItem != null)
            {
                BO.CustomerForList cust = receiverListBox.SelectedItem as BO.CustomerForList;
                id = cust.ID;
                CustomerWindow cw = new CustomerWindow(ref parc, id);
                cw.ShowDialog();
            }
            else
                MessageBox.Show("You didn't choose a customer");

        }
    
    }
}
