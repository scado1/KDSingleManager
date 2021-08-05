using KDSingleManager.Models;
using KDSingleManager.ZUSY;
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

namespace KDSingleManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static AppContext _context = new AppContext();
        private CollectionViewSource subconViewSource;

        public MainWindow()
        {
            InitializeComponent();
            subconViewSource = (CollectionViewSource)FindResource(nameof(subconViewSource));
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();

            _context.Subcontractors.Load();
            _context.SaveChanges();
            subconViewSource.Source = _context.Subcontractors.Local.ToObservableCollection();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _context.Dispose();
        }

        private void btn_OpenPage_Click(object sender, RoutedEventArgs e)
        {
            WinNewSubcontractor wns = new WinNewSubcontractor();
            wns.Show();
        }
        private void dg_SubconList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var s = (Subcontractor)dg_SubconList.SelectedItem;
            WinNewSubcontractor wns = new WinNewSubcontractor(s);
            wns.Show();
        }

        /// <summary>
        /// cascade delete for Subcontractors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dg_SubconList.SelectedItem != null)
            {
                Subcontractor w = (Subcontractor)dg_SubconList.SelectedItem;
                List<Przejscie> przejscia = _context.Przejscia.Where(y => y.Subcontractor.Id == w.Id).ToList();

                if (_context.WynagrKonta.Any(x => x.Subcontractor == w))
                {
                    var konto = _context.WynagrKonta.Where(x => x.Subcontractor == w).First();
                    konto.Subcontractor = null;
                    _context.WynagrKonta.Update(konto);
                }

                _context.RemoveRange(przejscia);
                _context.Subcontractors.Remove(w);

                _context.SaveChanges();
                subconViewSource.Source = _context.Subcontractors.Local.ToObservableCollection();
            }
        }

        public static void SeedDefSkladek()
        {
            List<DefSkladki> defSkladki = new List<DefSkladki>();
            // ZUS 2020
            defSkladki.Add(new DefSkladki()
            {
                Symbol = "0510 2020",
                Nazwa = "Duży",
                Year = 2020,
                ZUS51 = 992.30m,
                ZUS52 = 362.34m,
                ZUS53 = 76.84m
            });
            defSkladki.Add(new DefSkladki()
            {
                Symbol = "0540 2020",
                Nazwa = "Ulga",
                Year = 2020,
                ZUS51 = 0m,
                ZUS52 = 362.34m,
                ZUS53 = 0m
            });
            defSkladki.Add(new DefSkladki()
            {
                Symbol = "0570 2020",
                Nazwa = "Mały",
                Year = 2020,
                ZUS51 = 246.80m,
                ZUS52 = 362.34m,
                ZUS53 = 0m
            });
            // ZUS 2021
            defSkladki.Add(new DefSkladki()
            {
                Symbol = "0510 2021",
                Nazwa = "Duży",
                Year = 2021,
                ZUS51 = 998.37m,
                ZUS52 = 381.81m,
                ZUS53 = 77.31m
            });
            defSkladki.Add(new DefSkladki()
            {
                Symbol = "0540 2021",
                Nazwa = "Ulga",
                Year = 2021,
                ZUS51 = 0m,
                ZUS52 = 381.81m,
                ZUS53 = 0m
            });
            defSkladki.Add(new DefSkladki()
            {
                Symbol = "0570 2021",
                Nazwa = "Mały",
                Year = 2021,
                ZUS51 = 265.78m,
                ZUS52 = 381.81m,
                ZUS53 = 0m
            });

            var x = _context.DefinicjeSkladek.Select(x => x.Nazwa == "mały");
            foreach (var item in defSkladki)
            {
                _context.DefinicjeSkladek.Add(item);
            }
            _context.SaveChanges();
        }

        private void btn_AddZus_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btn_mi_OpenZusList_Click(object sender, RoutedEventArgs e)
        {
            WinZusProcessor wzp = new WinZusProcessor();
            wzp.Show();
        }

        private void NewInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var chosen = (Subcontractor)dg_SubconList.SelectedItem;

                WinNewInvoice wni = new WinNewInvoice(chosen);
                wni.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\n" + ex.InnerException);
            }
        }

        private void UserControl_Click(object sender, RoutedEventArgs e)
        {
            UserControllers uc = new UserControllers();
            uc.Show();
        }
    }
    public enum StanRozliczenia
    {
        Wprowadzony,
        Zaplacony,
        ZaplaconyIDoliczony,
        PayByHimself
    }
}

