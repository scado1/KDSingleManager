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

        string fp = string.Empty;

        List<Subcontractor> subcontractors = new List<Subcontractor>();
        Dictionary<int, string> dataZUS = new Dictionary<int, string>();

        List<(int, string)> tDataZus = new List<(int, string)>();
        public WinZusProcessor()
        {
            InitializeComponent();
            _context = MainWindow._context;
            GetMonthsYears();
            GetSubcons();

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
        private void GetSubcons()
        {
            cb_Subcons.ItemsSource = _context.Subcontractors.Local.OrderBy(x => x.LastName).ToList();
            cb_Subcons.DisplayMemberPath = "FullName";
            //cb_Subcons.
        }
        private void btn_ProcessZUS_Click(object sender, RoutedEventArgs e)
        {
            CheckExistanceFromList();
        }

        private void CheckExistanceFromList()
        {
            fp = Environment.MachineName.ToLower() == "horsh-w10-11" ? @"C:\Users\Horsh\Desktop\Kek\KD Building\ZUS\ZUS_test\ZUS_01_short.csv" : @"C:\Users\dbasa\Desktop\ZUS\ZUSdoZap1.csv";

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
                    if (item.Length > 5)
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

                        if (item.Length > 5)
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
                if (fp == string.Empty)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    if (ofd.ShowDialog() == true)
                    {
                        fp = ofd.FileName;
                    }
                }
                MessageBox.Show(fp);

                List<string> recs = new List<string>();
                string content = System.IO.File.ReadAllText(fp);
                List<Subcontractor> _subcontractors = _context.Subcontractors.ToList();
                List<string[]> records = content.Replace(" ", "").Replace(".", "").Split("\r\n")
                     .Skip(1)
                     .Select(x => x.Split(";"))
                     .ToList();
                subcontractors.ForEach(s => AddZusPayment(s));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\r\n" + ex.InnerException);
            }
        }

        private void btn_addSingleContribution_Click(object sender, RoutedEventArgs e)
        {
            AddZusPayment((Subcontractor)cb_Subcons.SelectedItem);

        }
        private void AddZusPayment(Subcontractor s)
        {
            List<Skladka> skladki = new List<Skladka>();
            Processor proc = new Processor();

            proc.AddZus(s, int.Parse(cb_Months.Text), int.Parse(cb_Years.Text));

            var skl = _context.Skladki.Where(x => x.ZaOkresMonth == int.Parse(cb_Months.Text)).Where(x => x.ZaOkresYear == int.Parse(cb_Years.Text)).ToList();

            foreach (var item in skl)
            {
                skladki.Add(item);
            }
            string result = string.Empty;
            ESkladka eskl = new ESkladka();

            skladki.ForEach(s => result += GetPaymentInfo(s));

            //string sfp = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, result, CodePagesEncodingProvider.Instance.GetEncoding(1250));
            }
        }

        public string GetPaymentInfo(Skladka skl)
        {
            string result = string.Empty;
            result = $@"110,{(DateTime.Today.ToString("yyyyMMdd"))},{(int)(skl.Wartość * 100)},11401140,0,""70114011400000354247001001"",""{(_context.ESkladki.Where(x => x.Subcontractor == skl.Subcontractor).First()).Konto}"",{skl.Subcontractor.FullName}||"",""ZAKŁAD UBEZPIECZEŃ SPOŁECZNYCH"",0,{((_context.ESkladki.Where(x => x.Subcontractor == skl.Subcontractor).First()).Konto.Substring(2, 8))},""{skl.Subcontractor.NIP}|{skl.Subcontractor.FullName}|S{string.Format($"{cb_Years.Text}{cb_Months.Text}")}01|"","""","""",""51"", ""REF:""{Environment.NewLine}";
            return result;
        }

    }
}
