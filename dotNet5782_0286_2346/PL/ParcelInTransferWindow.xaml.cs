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
    /// Interaction logic for ParcelInTransferWindow.xaml
    /// </summary>
    public partial class ParcelInTransferWindow : Window
    {
       
        public ParcelInTransferWindow(ref IBL bL ,int id)
        {
            InitializeComponent();
            parcelInTransferGrid.DataContext = bL.GetDrone(id).ParcelInTransfer;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
