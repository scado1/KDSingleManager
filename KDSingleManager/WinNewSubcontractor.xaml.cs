using KDSingleManager.Models;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for WinNewSubcontractor.xaml
    /// </summary>
    public partial class WinNewSubcontractor : Window
    {
        private AppContext _context = MainWindow._context;
        private Subcontractor _subcontractor;
        private CollectionViewSource defZusViewSource;
        public WinNewSubcontractor()
        {
            InitializeComponent();
            //  this.DataContext = this;
            defZusViewSource = (CollectionViewSource)FindResource(nameof(defZusViewSource));
            _context.DefinicjeSkladek.Load();
            defZusViewSource.Source = _context.DefinicjeSkladek.Local.ToObservableCollection();
            dg_DefSkladek.ItemsSource = _context.DefinicjeSkladek.Local.ToBindingList();
            CalcPrzejscia();
        }
        public WinNewSubcontractor(Subcontractor s)
        {
            InitializeComponent();
            _subcontractor = s;
            tb_FirstName.Text = s.FirstName;
            tb_LastName.Text = s.LastName;
            dp_Zalozenie.SelectedDate = DateTime.Parse(s.DataZalozenia);
        }

        //public int Convert(object value)
        //{
        //    var r = int.Parse(tb_Value.Text);
        //    return r * 2;
        //}

        private void btn_AddNewWorker_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb_FirstName.Text) && !string.IsNullOrWhiteSpace(tb_LastName.Text))
            {
                Subcontractor s = new Subcontractor()
                {
                    FirstName = tb_FirstName.Text,
                    LastName = tb_LastName.Text,
                    DataZalozenia = DateTime.Parse(dp_Zalozenie.SelectedDate.ToString()).ToShortDateString()
                };

                _context.Subcontractors.Add(s);
                _context.SaveChanges();
                tb_FirstName.Text = tb_LastName.Text = string.Empty;
            }
            //MainWindow._context.Subcontractors.
        }
        //private void tb_Value_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    decimal d;
        //    bool c = decimal.TryParse(tb_Value.Text, out d);

        //    if (!string.IsNullOrWhiteSpace(tb_Value.Text) && c)
        //    {
        //        tb_Value_Copy.Text = (d * 0.5m).ToString();
        //    }
        //}

        private void seedZUS_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.SeedDefSkladek();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void dp_Zalozenie_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalcPrzejscia();
        }

        private void CalcPrzejscia()
        {
            if (!String.IsNullOrWhiteSpace( dp_Zalozenie.SelectedDate.ToString()))
            {
                var x = dp_Zalozenie.SelectedDate;

                dp_przejscieNaMaly.SelectedDate =  (DateTime.Parse(x.ToString()).Day != 1 ? (DateTime.Parse(x.ToString().ToString()).AddMonths(7)) : (DateTime.Parse(x.ToString().ToString()).AddMonths(6))) ;
                dp_przejscieNaDuzy.SelectedDate = (DateTime.Parse(x.ToString()).AddMonths(30));
            }
        }
    }
}
