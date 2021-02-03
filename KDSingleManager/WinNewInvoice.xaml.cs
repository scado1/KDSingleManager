using KDSingleManager.Processors;
using System;
using System.Collections.Generic;
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

        private async void dp_InvoceDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(dp_InvoceDate.SelectedDate.ToString());
            DateTime selectedDate = DateTime.Parse(dp_InvoceDate.SelectedDate.ToString());
            var exR = await GetExRate(selectedDate.ToString());
            tb_ExRate.Text = exR.GetRate().ToString();
            tb_ExRate_date.Text = exR.GetDate();
        }

        private async Task<ExRate> GetExRate(string date)
        {
            ExRate exR = new ExRate();

            NBPProcessor nbp = new NBPProcessor();
            exR = await nbp.getRate(date);
            return exR;

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
