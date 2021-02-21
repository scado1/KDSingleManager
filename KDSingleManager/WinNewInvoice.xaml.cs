using KDSingleManager.Models;
using KDSingleManager.Processors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private static AppContext _context = new AppContext();
        private CollectionViewSource subconViewSource;

        private ICollectionView _subconCollectionView;
        private Subcontractor _subcontractor;
        private CollectionViewSource skladkiViewSource;
        private static readonly Regex _regex = new Regex("[^0-9.,-]+");
        public WinNewInvoice(Subcontractor s)
        {
            InitializeComponent();
            _context = MainWindow._context;
            _subcontractor = s;

            _context.Subcontractors.Load();
            _context.Skladki.Load();

            _subconCollectionView = CollectionViewSource.GetDefaultView(_subcontractor);
            subconViewSource = (CollectionViewSource)FindResource(nameof(subconViewSource));

            subconViewSource.Source = _context.Subcontractors.Local.ToObservableCollection();

            dg_Skladki.ItemsSource = _context.Skladki.Where(x => x.Subcontractor == _subcontractor).ToList();
            //   subconViewSource.View = _context.Subcontractors.Local.ToObservableCollection().Equals(_subcontractor);
            //_context.Subcontractors.Local.ToObservableCollection();
            skladkiViewSource = (CollectionViewSource)FindResource(nameof(skladkiViewSource));
            this.Title += _subcontractor.FullName;
        }

        public bool SubconFilter(object item)
        {
            Subcontractor subcon = item as Subcontractor;
            return subcon.Id == _subcontractor.Id;
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

        private void btn_Calculate_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(tb_TotalAmount.Text) && !_regex.IsMatch(tb_TotalAmount.Text))
                {
                    var rate = decimal.Parse(cb_TaxRate.SelectedValue.ToString().TrimEnd(new char[] { '%', ' ' })) / 100m;

                    var tax = decimal.Parse(tb_TotalAmount.Text) * rate;

                    MessageBox.Show(tax.ToString("c"));
                }
                else
                {
                    MessageBox.Show("bad");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tb_TotalAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

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
