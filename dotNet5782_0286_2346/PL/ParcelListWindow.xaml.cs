
﻿using BlApi;
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
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        IBL bL;
        public ParcelListWindow(ref IBL bl)
        {
            InitializeComponent();
            bL = bl;
            parcelForListDataGrid.DataContext = bL.GetListOfParcels();
            WeightSelect.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.WeightCategories));
            ParcelStatusSelect.ItemsSource = Enum.GetValues(typeof(BO.EnumsBL.ParcelStatuses));
        }

        private void GroupBySender_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parcelForListDataGrid.DataContext);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("SenderName");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void GroupByReceiver_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(parcelForListDataGrid.DataContext);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("ReceiverName");
            view.GroupDescriptions.Add(groupDescription);
        }

        //private void WeightSelectComboBox(object sender, SelectionChangedEventArgs e)
        //{
        //    if (WeightSelect.SelectedItem != null && ParcelStatusSelect.SelectedItem != null)
        //        parcelForListDataGrid.DataContext = bL.GetParcelsByPredicate((BO.EnumsBL.WeightCategories)WeightSelect.SelectedItem, (BO.EnumsBL.ParcelStatuses)ParcelStatusSelect.SelectedItem);
        //    else if (WeightSelect.SelectedItem != null)
        //        parcelForListDataGrid.DataContext = bL.GetParcelsByPredicate((BO.EnumsBL.WeightCategories)WeightSelect.SelectedItem, null);
        //    else if (ParcelStatusSelect.SelectedItem != null)
        //        parcelForListDataGrid.DataContext = bL.GetParcelsByPredicate(null, (BO.EnumsBL.ParcelStatuses)ParcelStatusSelect.SelectedItem);
        //    parcelForListDataGrid.Items.Refresh();
        //}

        //private void UpdateParcel(object sender, RoutedEventArgs e)
        //{
        //    BO.ParcelForList p = parcelForListDataGrid.SelectedItem as BO.ParcelForList;
        //    if (p != null)
        //    {
        //        BO.Parcel p2 = new BO.Parcel();
        //        p2 = bL.GetParcel(p.Id);
        //        ParcelWindow pw = new ParcelWindow(ref bL, p2, p.ParcelStatus);
        //        pw.ShowDialog();
        //        parcelForListDataGrid.ItemsSource = null;
        //        parcelForListDataGrid.ItemsSource = bL.GetListOfParcels();//update the parcel collection in the parcelListWindow
        //    }
        //}

        private void AddParcel(object sender, RoutedEventArgs e)
        {
            ParcelWindow pw = new ParcelWindow(ref bL);
            pw.Show();
        }

        

        private void RefreshButton(object sender, RoutedEventArgs e)
        {
            ParcelListWindow pw = new ParcelListWindow(ref bL);
            pw.Show();
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Show_Item(object sender, RoutedEventArgs e)
        {
            BO.ParcelForList p = parcelForListDataGrid.SelectedItem as BO.ParcelForList;
            if (p != null)
            {
                BO.Parcel p2 = new BO.Parcel();
                p2 = bL.GetParcel(p.Id);
                ParcelWindow pw = new ParcelWindow(ref bL, p2, p.ParcelStatus);
                pw.ShowDialog();
                parcelForListDataGrid.ItemsSource = null;
                parcelForListDataGrid.ItemsSource = bL.GetListOfParcels();//update the parcel collection in the parcelListWindow

            }
        }

        private void StatusAndWeightSelectComboBox(object sender, SelectionChangedEventArgs e)
        {

            if (WeightSelect.SelectedItem != null && ParcelStatusSelect.SelectedItem != null)
                parcelForListDataGrid.DataContext = bL.GetParcelsByPredicate((BO.EnumsBL.WeightCategories)WeightSelect.SelectedItem, (BO.EnumsBL.ParcelStatuses)ParcelStatusSelect.SelectedItem);
            else if (WeightSelect.SelectedItem != null)
                parcelForListDataGrid.DataContext = bL.GetParcelsByPredicate((BO.EnumsBL.WeightCategories)WeightSelect.SelectedItem, null);
            else if (ParcelStatusSelect.SelectedItem != null)
                parcelForListDataGrid.DataContext = bL.GetParcelsByPredicate(null, (BO.EnumsBL.ParcelStatuses)ParcelStatusSelect.SelectedItem);
            parcelForListDataGrid.Items.Refresh();
        }
    }
}
