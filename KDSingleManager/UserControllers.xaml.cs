using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KDSingleManager
{
    /// <summary>
    /// Interaction logic for UserControllers.xaml
    /// </summary>
    public partial class UserControllers : Window
    {
        public UserControllers()
        {
            InitializeComponent();
        }

        private void RedView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new UserControls.RedView();
        }

        private void BlueView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new UserControls.BlueView();
        }

        private void OrangeView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new UserControls.OrangeView();
        }
        private void ZusView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new UserControls.SkladkiView();
        }
    }
}
