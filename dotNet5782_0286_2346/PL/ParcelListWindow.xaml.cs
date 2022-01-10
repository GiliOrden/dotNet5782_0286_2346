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
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        IBL parc;
        public ParcelListWindow(ref IBL bl)
        {
            
            InitializeComponent();
            parc = bl;
            parcelForListDataGrid.DataContext = parc.GetListOfParcels();
            WeightSelect.ItemsSource= Enum.GetValues(typeof(BO.EnumsBL.WeightCategories));
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

        private void WeightSelectComboBox(object sender, SelectionChangedEventArgs e)
        {
           if(WeightSelect.SelectedItem != null && ParcelStatusSelect.SelectedItem != null)
                 parcelForListDataGrid.DataContext = parc.GetParcelsByPredicate((BO.EnumsBL.WeightCategories)WeightSelect.SelectedItem, (BO.EnumsBL.ParcelStatuses)ParcelStatusSelect.SelectedItem);
           else if (WeightSelect.SelectedItem != null)
                parcelForListDataGrid.DataContext = parc.GetParcelsByPredicate((BO.EnumsBL.WeightCategories)WeightSelect.SelectedItem, null);
           else if(ParcelStatusSelect.SelectedItem != null)
                parcelForListDataGrid.DataContext = parc.GetParcelsByPredicate(null,(BO.EnumsBL.ParcelStatuses)ParcelStatusSelect.SelectedItem);
            parcelForListDataGrid.Items.Refresh();
        }

        private void UpdateParcel(object sender, RoutedEventArgs e)
        {
            BO.ParcelForList p = parcelForListDataGrid.SelectedItem as BO.ParcelForList;
            if (p != null)
            {
                BO.Parcel p2= new BO.Parcel();
                p2 = parc.GetParcel(p.Id);
                ParcelWindow pw = new ParcelWindow(ref parc, p2);
                pw.ShowDialog();
            }
        }

        private void AddParcel(object sender, RoutedEventArgs e)
        {
            ParcelWindow pw = new ParcelWindow(ref parc);
            pw.ShowDialog();

        }

        //private void ParcelView_SelectionChanged(object sender, SelectionChangedEventArgs e)//using without button
        //{

        //    BO.ParcelForList p = parcelForListDataGrid.SelectedItem as BO.ParcelForList;
        //    if (p != null)
        //    {
        //        BO.Parcel p2 = new BO.Parcel();
        //        p2 = parc.GetParcel(p.Id);
        //        ParcelWindow pw = new ParcelWindow(p2);
        //        pw.ShowDialog();
        //    }
        //}
    }
}
