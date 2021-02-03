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
    /// Interaction logic for WinNewInvoice.xaml
    /// </summary>
    public partial class WinNewInvoice : Window
    {
        public WinNewInvoice()
        {
            InitializeComponent();
        }

        /*Przychód KD:  Faktura BE
         *              ZUS current month
         *              Tax current month ↑
         *              
         *              AddCosts
         *              Tax previous months
         * */
    }
}
