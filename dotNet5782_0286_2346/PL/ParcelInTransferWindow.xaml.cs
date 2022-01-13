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
    /// Interaction logic for ParcelInTransferWindow.xaml
    /// </summary>
    public partial class ParcelInTransferWindow : Window
    {
        ParcelInTransfer parcel;
        public ParcelInTransferWindow(ref IBL bL ,int id)
        {
            InitializeComponent();
            parcel= bL.GetDrone(id).ParcelInTransfer;
            parcelInTransferGrid.DataContext = parcel;
            IsOnTheWayCheckBox.IsChecked = parcel.OnTheWay;
            IsOnTheWayCheckBox.IsEnabled = false;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
