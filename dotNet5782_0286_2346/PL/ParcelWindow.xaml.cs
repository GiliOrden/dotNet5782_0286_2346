﻿using BlApi;
using BO;
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
        IBL bL;
        BO.Parcel parcel;
        public ParcelWindow(ref IBL bl, BO.Parcel p, BO.EnumsBL.ParcelStatuses parcelStatus)//update/delete parcel window
        {
            bL = bl;
            parcel = p;
            InitializeComponent();
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.Priorities));
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.WeightCategories));
            senderListBox.Items.Add(parcel.Sender);
            receiverListBox.Items.Add(parcel.Receiver);
            droneListBox.Items.Add(parcel.Drone);
            gridOfParcel.DataContext = p;
            AddParcelButton.Visibility = Visibility.Collapsed;
            if (BO.EnumsBL.ParcelStatuses.Defined == parcelStatus)//the botton for delete parcel wiil be abled
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
            bL = bl;
            InitializeComponent();
            priorityComboBox.IsEnabled = true;
            receiverListBox.IsEnabled = true;
            senderListBox.IsEnabled = true;
            weightComboBox.IsEnabled = true;
            DeleteParcel.Visibility = Visibility.Collapsed;
            SupplyParcel.Visibility = Visibility.Collapsed;
            CollectParcel.Visibility = Visibility.Collapsed;
            ShowDrone.Visibility = Visibility.Collapsed;
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.Priorities));
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.WeightCategories));
            senderListBox.ItemsSource = bl.GetListOfCustomers();
            receiverListBox.ItemsSource = bl.GetListOfCustomers();
            senderListBox.SelectionChanged += addButton_isEnable;
            receiverListBox.SelectionChanged += addButton_isEnable;
            priorityComboBox.SelectionChanged += addButton_isEnable;
            weightComboBox.SelectionChanged += addButton_isEnable;
        }
        public ParcelWindow(ref IBL bl,User user)//customer interface
        {
            bL = bl;
            InitializeComponent();
            priorityComboBox.IsEnabled = true;
            receiverListBox.IsEnabled = true;
            senderListBox.IsEnabled =true;//change
            weightComboBox.IsEnabled = true;
            DeleteParcel.Visibility = Visibility.Collapsed;
            SupplyParcel.Visibility = Visibility.Collapsed;
            CollectParcel.Visibility = Visibility.Collapsed;
            ShowDrone.Visibility = Visibility.Collapsed;
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.Priorities));
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.WeightCategories));
            ListBoxItem item = new ListBoxItem();
            item.Content = bl.GetListOfCustomers().FirstOrDefault(customer=>customer.Name==user.Name);
            senderListBox.Items.Add(item);
            receiverListBox.ItemsSource = bl.GetListOfCustomers();
            receiverListBox.SelectionChanged +=addMyParcelButton_isEnable;
            priorityComboBox.SelectionChanged +=addMyParcelButton_isEnable;
            weightComboBox.SelectionChanged +=addMyParcelButton_isEnable;
        }

        private void addButton_isEnable(object sender, RoutedEventArgs e)
        {
            if (weightComboBox.SelectedItem != null && priorityComboBox.SelectedItem != null && receiverListBox.SelectedItem != null && senderListBox.SelectedItem != null)
                AddParcelButton.IsEnabled = true;
        }
        private void addMyParcelButton_isEnable(object sender, RoutedEventArgs e)//for customer interface
        {
            if (weightComboBox.SelectedItem != null && priorityComboBox.SelectedItem != null && receiverListBox.SelectedItem != null )
                AddParcelButton.IsEnabled = true;
        }

        private void addParcelButton_Click(object sender, RoutedEventArgs e)
        {
            BO.Parcel parcel = new();
            parcel.Weight = (BO.EnumsBL.WeightCategories)weightComboBox.SelectedItem;
            parcel.Priority = (BO.EnumsBL.Priorities)priorityComboBox.SelectedItem;
            parcel.Sender = new();
            BO.CustomerForList cust = (BO.CustomerForList)senderListBox.SelectedItem;
            parcel.Sender.ID = cust.ID;
            parcel.Sender.Name = cust.Name;
            parcel.Receiver = new();
            BO.CustomerForList cust2 = (BO.CustomerForList)receiverListBox.SelectedItem;
            parcel.Receiver.ID = cust2.ID;
            parcel.Receiver.Name = cust2.Name;
            bL.AddParcel(parcel);
            MessageBox.Show("The parcel was successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            new ParcelListWindow(ref bL).Show();
            
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            bL.DeleteParcel(parcel.Id);
            MessageBox.Show("The parcel was successfully deleted");
            Close();
        }

        private void CollectButton_Click(object sender, RoutedEventArgs e)
        {
            bL.CollectParcelByDrone(parcel.Drone.Id);//problem associated
            MessageBox.Show("The parcel was successfully collected");
            Close();
        }

        private void SupplyButton_Click(object sender, RoutedEventArgs e)
        {
            bL.SupplyDeliveryToCustomer(parcel.Drone.Id);
            MessageBox.Show("The parcel was successfully supplied");
            Close();
        }

        private void ShowDroneButton_Click(object sender, RoutedEventArgs e) // need to fix the  DroneWindow to be for BO.Drone type. then it will work
        {
            BO.Drone drone = bL.GetDrone(parcel.Drone.Id);

            if (drone != null)
            {
                DroneWindow dw = new DroneWindow(ref bL, drone.Id);
                dw.ShowDialog();
                //Close();
            }

        }

        private void ShowSenderButton_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            if (parcel != null)
            {
                id = parcel.Sender.ID;
                CustomerWindow cw = new CustomerWindow(ref bL, id);
                cw.ShowDialog();
            }

            else if (senderListBox.SelectedItem != null)
            {
                BO.CustomerForList cust = senderListBox.SelectedItem as BO.CustomerForList;
                id = cust.ID;
                CustomerWindow cw = new CustomerWindow(ref bL, id);
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
                CustomerWindow cw = new CustomerWindow(ref bL, id);
                cw.ShowDialog();
            }

            else if (receiverListBox.SelectedItem != null)
            {
                BO.CustomerForList cust = receiverListBox.SelectedItem as BO.CustomerForList;
                id = cust.ID;
                CustomerWindow cw = new CustomerWindow(ref bL, id);
                cw.ShowDialog();
            }
            else
                MessageBox.Show("You didn't choose a customer");

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
