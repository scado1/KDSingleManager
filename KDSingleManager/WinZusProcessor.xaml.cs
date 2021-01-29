using CsvHelper;
using KDSingleManager.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
    /// Interaction logic for WinZusProcessor.xaml
    /// </summary>
    public partial class WinZusProcessor : Window
    {
        AppContext _context;

        public WinZusProcessor()
        {
            InitializeComponent();
            _context = MainWindow._context;
            GetMonthsYears();
        }

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
            MessageBox.Show(cb_Months.Text);
            GenerateZus();
        }

        private void GenerateZus()
        {
            List<Skladka> skladki = new List<Skladka>();

            string filePath = @"C:\Users\Horsh\Desktop\Kek\KD Building\Manager\ZUS11.CSV";
            List<Subcontractor> records = new List<Subcontractor>();
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = ";";
                    csv.Configuration.PrepareHeaderForMatch = (h, i) => Regex.Replace(h, @"\s", string.Empty);
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var record = new Subcontractor
                        {
                            LastName = csv.GetField("Surname"),
                            FirstName = csv.GetField("Name"),
                            NIP = csv.GetField("NIP")
                        };
                        records.Add(record);
                    }
                }
            }
            foreach (var rec in records)
            {
                try
                {
                    var sc = _context.Subcontractors.Local.ToObservableCollection()
                        .Where(x => x.NIP == rec.NIP ||
                        (Normalize(x.FirstName.ToLower()).Contains(Normalize(rec.FirstName.ToLower()))
                    && Normalize(x.LastName.ToLower()).Contains(Normalize(rec.LastName.ToLower())))).First();
                    sc.Zusy.Add(new ZUS().AddSkladka());
                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format($"{ex.ToString()} - {rec.FirstName} {rec.LastName}"));
                }
            }
        }
        private static IEnumerable<Subcontractor> FindCollection(Subcontractor dbSubc, Subcontractor docSubc, out IEnumerable<Subcontractor> res)
        {
            res = new List<Subcontractor>()
                .Where(x => x.FirstName.Contains(docSubc.FirstName)).ToList();

            return res;
        }

        /// <summary>
        /// Remove Accents/Diacritics from a String
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string Normalize(string text)
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
    }
}
