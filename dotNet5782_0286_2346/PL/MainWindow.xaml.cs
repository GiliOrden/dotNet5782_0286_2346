using IBL.BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        private IBL.IBL bl;
        public MainWindow()
        {
            InitializeComponent();
            bl = new BL.BL();
        }

        

        private void ShowDronesList_Click(object sender, RoutedEventArgs e)
        {
            DroneListWindow dw = new DroneListWindow(ref bl);
            dw.Show();
        }


    }
}
