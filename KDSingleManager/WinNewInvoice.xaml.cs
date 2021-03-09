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
            _context.Renumerations.Load();

            _subconCollectionView = CollectionViewSource.GetDefaultView(_subcontractor);
            subconViewSource = (CollectionViewSource)FindResource(nameof(subconViewSource));

            subconViewSource.Source = _context.Subcontractors.Local.ToObservableCollection();

            dg_Skladki.ItemsSource = _context.Skladki.Where(x => x.Subcontractor == _subcontractor).ToList();
            dg_Renumerations.ItemsSource = _context.Renumerations.Where(x => x.Subcontractor == _subcontractor).ToList();
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
            DateTime selectedDate = dp_InvoceDate.SelectedDate.Value;
            //DateTime selectedDate = DateTime.Parse(dp_InvoceDate.SelectedDate.ToString());
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
                    MessageBox.Show(CalculateTax().ToString("c"));
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

        private decimal CalculateTax()
        {
            decimal tax = decimal.MinValue;

            if (!String.IsNullOrWhiteSpace(tb_TotalAmount.Text) && !_regex.IsMatch(tb_TotalAmount.Text))
            {
                var rate = decimal.Parse(cb_TaxRate.SelectedValue.ToString().TrimEnd(new char[] { '%', ' ' })) / 100m;

                tax = decimal.Parse(tb_TotalAmount.Text) * rate;

                //MessageBox.Show(tax.ToString("c"));
            }
            else
            {
                MessageBox.Show("bad");
            }
            return tax;
        }

        private void tb_TotalAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Renumeration r = new Renumeration();
                r.Subcontractor = _subcontractor;
                r.Data = DateTime.Now.ToShortDateString();
                r.Opis = tb_InvoiceNr.Text;
                r.Wartosc = decimal.Parse(tb_TotalAmount.Text);
                r.Tax = Math.Round(CalculateTax(), 2);
                r.ZaOkresMoth = DateTime.Parse(dp_IssueDate.SelectedDate.ToString()).Month;
                r.ZaOkresYear = DateTime.Parse(dp_IssueDate.SelectedDate.ToString()).Year;
                r.Stan = (int)StanRozliczenia.Wprowadzony;

                _context.Renumerations.Add(r);
                _context.SaveChanges();
                dg_Renumerations.ItemsSource = _context.Renumerations.Where(x => x.Subcontractor == _subcontractor).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\n" + ex.InnerException);
            }
        }

        private void btn_Pay_Click(object sender, RoutedEventArgs e)
        {
            PayTax();
        }
        private void PayTax()
        {
            try
            {
                Renumeration sel = (Renumeration)dg_Renumerations.SelectedItem;
                sel.Stan = (int)StanRozliczenia.Zaplacony;

                _context.Renumerations.Update(sel);
                _context.SaveChanges();
                dg_Renumerations.ItemsSource = _context.Renumerations.Where(x => x.Subcontractor == _subcontractor).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\n" + ex.InnerException);
            }
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
