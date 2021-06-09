using KDSingleManager.Models;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KDSingleManager.UserControls
{
    /// <summary>
    /// Interaction logic for SkladkiView.xaml
    /// </summary>
    public partial class SkladkiView : UserControl
    {
        AppContext _context;
        private CollectionViewSource zusViewSource;
        public SkladkiView()
        {
            InitializeComponent();
            _context = MainWindow._context;
            zusViewSource = (CollectionViewSource)FindResource(nameof(zusViewSource));
            GetMonthYears();
            ShowInfo().Wait();
        }

        private async Task ShowInfo()
        {
            _context.Database.EnsureCreated();
            _context.Skladki.Load();
            zusViewSource.Source = _context.Skladki.Local.ToObservableCollection();
            db_ZusList.ItemsSource = await _context.Skladki.ToListAsync();
        }

        private void GetMonthYears()
        {
            var empty = new ComboBoxItem() { Content = null, Height = 20 };

            for (int i = 2018; i < DateTime.Now.Year + 2; i++)
            {
                var y = new ComboBoxItem();
                y.Content = i;
                cb_Years.Items.Add(y);
            }
            cb_Years.Items.Add(empty);


            for (int i = 1; i < 13; i++)
            {
                var m = new ComboBoxItem();
                m.Content = i.ToString("D2");
                cb_Months.Items.Add(m);
            }
            cb_Months.Items.Add(new ComboBoxItem() { Content = null, Height = 20 });
        }
        private async Task UpdateQuery()
        {
            int res = 0;
            int.TryParse(cb_Months.Text, out res);

            IQueryable<Models.Skladka> query = _context.Skladki;

            if (cb_Months.SelectedItem != null && cb_Months.SelectionBoxItem.ToString() != "" && !string.IsNullOrEmpty(cb_Months.SelectedItem.ToString()))
            { query = query.Where(x => x.ZaOkresMonth == int.Parse(cb_Months.Text)); }
            if (!string.IsNullOrWhiteSpace(cb_Years.Text) && cb_Months.SelectionBoxItem.ToString() != "")
            { query = query.Where(x => x.ZaOkresYear == int.Parse(cb_Years.Text)); }

            //MessageBox.Show(query.ToString());
            db_ZusList.ItemsSource = await query.ToListAsync();


            //MessageBox.Show(cb_Months.Text.ToString());

        }

        private async void cb_Date_SelectionChanged(object sender, EventArgs e)
        {
            await UpdateQuery();
            //decimal total = 0m;
            //foreach (Skladka item in db_ZusList.Items)
            //{
            //    total += ((Skladka)item).Wartość;
            //}


            tb_SumOfContributions.Text = "100";
        }

        private void db_ZusList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var s = ((Skladka)db_ZusList.SelectedItem).Subcontractor;
            WinNewSubcontractor wns = new WinNewSubcontractor(s);
            wns.Show();
        }
    }
}