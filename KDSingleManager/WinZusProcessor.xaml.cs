using KDSingleManager.Models;
using KDSingleManager.Processors;
using KDSingleManager.ZUSY;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for WinZusProcessor.xaml
    /// </summary>
    public partial class WinZusProcessor : Window
    {
        AppContext _context;

        List<string[]> dataFile = new List<string[]>();



        List<Subcontractor> subcontractors = new List<Subcontractor>();
        Dictionary<int, string> dataZUS = new Dictionary<int, string>();

        List<(int, string)> tDataZus = new List<(int, string)>();
        public WinZusProcessor()
        {
            InitializeComponent();
            _context = MainWindow._context;
            GetMonthsYears();
        }
        /// <summary>
        /// populate comboBox with months and years
        /// </summary>
        private void GetMonthsYears()
        {
            for (int i = 2018; i < DateTime.Now.Year + 2; i++)
            {
                var y = new ComboBoxItem();
                y.Content = i;
                cb_Years.Items.Add(y);
            }

            for (int i = 1; i < 13; i++)
            {
                var m = new ComboBoxItem();
                m.Content = i.ToString("D2");
                cb_Months.Items.Add(m);
            }
        }

        private void btn_ProcessZUS_Click(object sender, RoutedEventArgs e)
        {
            CheckExistanceFromList();
        }

        private void CheckExistanceFromList()
        {
            string fp = Environment.MachineName.ToLower() == "horsh-w10-11" ? @"C:\Users\Horsh\Desktop\Kek\KD Building\ZUS\ZUS_test\ZUS_01_short.csv" : @"C:\Users\dbasa\Desktop\ZUS\ZUSdoZap1.csv";

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                fp = ofd.FileName;
            }
            MessageBox.Show(fp);

            List<string> recs = new List<string>();

            string content = System.IO.File.ReadAllText(fp, CodePagesEncodingProvider.Instance.GetEncoding(1250));
            dataFile = content.Replace(" ", "").Replace(".", "").Split("\r\n")
                 .Skip(1)
                 .Select(x => x.Split(";"))
                 .Where(x => x.Length > 1)
                 .ToList();

            List<Subcontractor> _subcontractors = _context.Subcontractors.ToList();


            foreach (var item in dataFile)
            {
                dataZUS = new Dictionary<int, string>();
                var exists = _subcontractors.Any(x => x.NIP == item[3] ||
                        ((Normalize(x.FirstName.ToLower()).ToString().Contains(Normalize(item[2].ToLower()).ToString()) || Normalize(x.LastName.ToLower()).ToString().Contains(Normalize(item[2].ToLower()).ToString()))
                        && (Normalize(x.FirstName.ToLower()).ToString().Contains(Normalize(item[1].ToLower()).ToString()) || Normalize(x.LastName.ToLower()).Contains(Normalize(item[1].ToLower())))));
                if (exists)
                {
                    Subcontractor subcontractor = _subcontractors.Where(x => x.NIP == item[3] ||
                       ((Normalize(x.FirstName.ToLower()).Contains(Normalize(item[2].ToLower())) || Normalize(x.LastName.ToLower()).Contains(Normalize(item[2].ToLower())))
                       && (Normalize(x.FirstName.ToLower()).Contains(Normalize(item[1].ToLower())) || Normalize(x.LastName.ToLower()).Contains(Normalize(item[1].ToLower()))))).First();

                    subcontractors.Add(subcontractor);
                    if (item.Length > 4)
                    {
                        if (!string.IsNullOrWhiteSpace(item[5]))
                            dataZUS.Add(subcontractor.Id, item[5]);
                    }

                }
                else
                {
                    MessageBoxResult result = MessageBox.Show($"{item[2]} {item[1]} - brak w bazie danych.\nChcesz utworzyć?", "Brak pracownika", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                    if (result == MessageBoxResult.Yes)
                    {
                        Subcontractor s = new Subcontractor()
                        {
                            FirstName = item[2],
                            LastName = item[1],
                            NIP = item[3],

                            DataZalozenia = DateTime.Now.ToString()
                        };
                        WinNewSubcontractor wns = new WinNewSubcontractor(s);
                        wns.ShowDialog();
                        MessageBox.Show($"{s.FirstName}: {_context.Subcontractors.Any(x => x.NIP == s.NIP)}");

                        subcontractors.Add(s);

                        if (item.Length > 4)
                        {
                            if (!string.IsNullOrWhiteSpace(item[5]))
                                dataZUS.Add(s.Id, item[5]);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            dg_ContentZUS.ItemsSource = subcontractors;
        }

        private void OnClick()
        {

        }

        //private void CreateSubcontractor(object s)
        //{
        //    Thread.Sleep(TimeSpan.FromSeconds(5));
        //    this.Dispatcher.BeginInvoke(new Action(() =>
        //    {
        //        WinNewSubcontractor wns = new WinNewSubcontractor((Subcontractor)s);
        //        wns.Show();
        //    }));
        //}

        /// <summary>
        /// Remove Accents/Diacritics from a String
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Normalize(string text)
        {
            var x = text.Replace(" ", "").Replace("ł", "l").Normalize(NormalizationForm.FormD);
            string res = string.Empty;
            foreach (var c in x)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    res += (c);
                }
            }
            return res;
        }


        private void btn_GenerateZUS_Click(object sender, RoutedEventArgs e)
        {
            string fp = string.Empty;

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == true)
                {
                    fp = ofd.FileName;
                }
                MessageBox.Show(fp);

                List<string> recs = new List<string>();
                string content = System.IO.File.ReadAllText(fp);
                // List<Subcontractor> subcontractors = new List<Subcontractor>();

                List<Subcontractor> _subcontractors = _context.Subcontractors.ToList();


                var records = content.Replace(" ", "").Replace(".", "").Split("\r\n")
                     .Skip(1)
                     .Select(x => x.Split(";"))
                     .ToList();


                Processor proc = new Processor();


                //subcontractors.ForEach(s => MessageBox.Show(s.FullName));
                List<Skladka> skladki = new List<Skladka>();

                subcontractors.ForEach(s => proc.AddZus(s, int.Parse(cb_Months.Text), int.Parse(cb_Years.Text)));

                var skl = _context.Skladki.Where(x => x.ZaOkresMonth == int.Parse(cb_Months.Text)).Where(x => x.ZaOkresYear == int.Parse(cb_Years.Text)).ToList();

                foreach (var item in skl)
                {
                    skladki.Add(item);
                }
                string result = string.Empty;
                ESkladka eskl = new ESkladka();
                foreach (var item in skladki)
                {
                    result += string.Format($"{item.Subcontractor.FirstName};{item.Subcontractor.LastName};{item.Subcontractor.FullName};{item.Subcontractor.NIP};{item.Subcontractor.DataZalozenia};{(_context.ESkladki.Where(x => x.Subcontractor == item.Subcontractor).First()).Konto};" +
                        $"{item.Wartość}{Environment.NewLine}");
                }

                string sfp = string.Empty;
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == true)
                {
                    System.IO.File.WriteAllText(sfd.FileName, result, CodePagesEncodingProvider.Instance.GetEncoding(1250));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\r\n" + ex.InnerException);
            }
        }
    }
}
