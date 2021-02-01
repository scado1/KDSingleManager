using CsvHelper;
using KDSingleManager.Models;
using KDSingleManager.Processors;
using KDSingleManager.ZUSY;
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

        WinNewSubcontractor wns = new WinNewSubcontractor();

        List<Subcontractor> subcontractors = new List<Subcontractor>();
        Dictionary<int, string> dataZUS = new Dictionary<int, string>();

        List<(int, string)> tDataZus = new List<(int, string)>();
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
            //MessageBox.Show(cb_Months.Text);
            CheckExistanceFromList();
        }

        private void CheckExistanceFromList()
        {
            string fp = @"C:\Users\dbasa\Desktop\ZUS\ZUSdoZap1.csv";
            //string fp = @"C:\Users\Horsh\Desktop\Kek\KD Building\ZUS\ZUS_test\ZUS_01_short.csv";

            List<string> recs = new List<string>();

            //Read file and split
            string content = System.IO.File.ReadAllText(fp);
            dataFile = content.Replace(" ", "").Replace(".", "").Split("\r\n")
                 .Skip(1)
                 .Select(x => x.Split(";"))
                 .ToList();


            //    subcontractors = new List<Subcontractor>();

            //Get exisitng subcontractors from AppContext(db)
            List<Subcontractor> _subcontractors = _context.Subcontractors.ToList();


            foreach (var item in dataFile)
            {
                var exists = _subcontractors.Any(x => x.NIP == item[3] ||
                        ((Normalize(x.FirstName.ToLower()).ToString().Contains(Normalize(item[2].ToLower()).ToString()) || Normalize(x.LastName.ToLower()).ToString().Contains(Normalize(item[2].ToLower()).ToString()))
                        && (Normalize(x.FirstName.ToLower()).ToString().Contains(Normalize(item[1].ToLower()).ToString()) || Normalize(x.LastName.ToLower()).Contains(Normalize(item[1].ToLower())))));
                if (exists)
                {
                    Subcontractor subcontractor = _subcontractors.Where(x => x.NIP == item[3] ||
                       ((Normalize(x.FirstName.ToLower()).Contains(Normalize(item[2].ToLower())) || Normalize(x.LastName.ToLower()).Contains(Normalize(item[2].ToLower())))
                       && (Normalize(x.FirstName.ToLower()).Contains(Normalize(item[1].ToLower())) || Normalize(x.LastName.ToLower()).Contains(Normalize(item[1].ToLower()))))).First();

                    subcontractors.Add(subcontractor);

                    dataZUS.Add(subcontractor.Id, item[5]);

                    MessageBox.Show("exists");
                }
                else
                {
                    //MessageBox.Show($"{item[2]} {item[1]} does not appear in DB");

                    MessageBoxResult result = MessageBox.Show($"{item[2]} {item[1]} does not appear in DB\nChcesz utworzyć?", "Brak pracownika", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                    if (result == MessageBoxResult.Yes)
                    {
                        Subcontractor s = new Subcontractor()
                        {
                            FirstName = item[2],
                            LastName = item[1],
                            NIP = item[3],

                            DataZalozenia = DateTime.Now.ToString()
                        };

                        //WinNewSubcontractor wns = new WinNewSubcontractor((Subcontractor)s);
                        Monitor.TryEnter(wns);
                        wns.Show();

                        //Thread thread = new Thread(CreateSubcontractor);

                        //thread.Start();
                    }
                    else
                    {
                        continue;
                    }
                }

            }

            dg_ContentZUS.ItemsSource = subcontractors;


            //  System.IO.File.WriteAllText(fp.Replace("doZap1.csv", "converted1.csv"), records);
        }

        private void OnClick()
        {

        }

        private void CreateSubcontractor(object s)
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                WinNewSubcontractor wns = new WinNewSubcontractor((Subcontractor)s);
                wns.Show();
            }));
        }
        //private void GenerateZus()
        //{
        //    List<Skladka> skladki = new List<Skladka>();

        //    string filePath;// = @"C:\Users\Horsh\Desktop\Kek\KD Building\Manager\ZUS11.CSV";
        //    List<Subcontractor> records = new List<Subcontractor>();
        //    using (var reader = new StreamReader(filePath))
        //    {
        //        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //        {
        //            csv.Configuration.Delimiter = ";";
        //            csv.Configuration.PrepareHeaderForMatch = (h, i) => Regex.Replace(h, @"\s", string.Empty);
        //            csv.Read();
        //            csv.ReadHeader();
        //            while (csv.Read())
        //            {
        //                var record = new Subcontractor
        //                {
        //                    LastName = csv.GetField("Surname"),
        //                    FirstName = csv.GetField("Name"),
        //                    NIP = csv.GetField("NIP")
        //                };
        //                records.Add(record);
        //            }
        //        }
        //    }
        //    foreach (var rec in records)
        //    {
        //        try
        //        {
        //            var sc = _context.Subcontractors.Local.ToObservableCollection()
        //                .Where(x => x.NIP == rec.NIP ||
        //                (Normalize(x.FirstName.ToLower()).Contains(Normalize(rec.FirstName.ToLower()))
        //            && Normalize(x.LastName.ToLower()).Contains(Normalize(rec.LastName.ToLower())))).First();
        //            sc.Zusy.Add(new ZUS().AddSkladka());
        //            _context.SaveChanges();

        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(string.Format($"{ex.ToString()} - {rec.FirstName} {rec.LastName}"));
        //        }
        //    }
        //}
        //private static IEnumerable<Subcontractor> FindCollection(Subcontractor dbSubc, Subcontractor docSubc, out IEnumerable<Subcontractor> res)
        //{
        //    res = new List<Subcontractor>()
        //        .Where(x => x.FirstName.Contains(docSubc.FirstName)).ToList();

        //    return res;
        //}

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


        private void btn_GenerateZUS_Click(object sender, RoutedEventArgs e)
        {
            // string fp = @"C:\Users\Horsh\Desktop\Kek\KD Building\ZUS\ZUS_test\ZUS_01_short.csv";
            // string fp = @"C:\Users\Horsh\Desktop\Kek\KD Building\ZUS\ZUS_test\ZUS_01_short.csv";
            string fp = @"C:\Users\dbasa\Desktop\ZUS\ZUSdoZap1.csv";


            List<string> recs = new List<string>();
            string content = System.IO.File.ReadAllText(fp);
            // List<Subcontractor> subcontractors = new List<Subcontractor>();

            List<Subcontractor> _subcontractors = _context.Subcontractors.ToList();

            // var check ;

            var records = content.Replace(" ", "").Replace(".", "").Split("\r\n")
                 .Skip(1)
                 .Select(x => x.Split(";"))
                 .ToList();

            Processor proc = new Processor();


            subcontractors.ForEach(s => MessageBox.Show(s.FullName));
            subcontractors.ForEach(s => proc.AddZus(s));

        }

        //private void AddZus(Subcontractor s)
        //{
        //    try
        //    {
        //        DateTime today = DateTime.Now;

        //        Skladka skl = new Skladka();
        //        skl.Data = today.ToShortDateString();
        //        skl.ZaOkresYear = int.Parse(cb_Years.Text);
        //        skl.ZaOkresMonth = int.Parse(cb_Months.Text);
        //        skl.Data = DateTime.Now.ToString();

        //        skl.Opis = dataZUS.Where(x => x.Key == s.Id).Select(x => x.Value).FirstOrDefault();
        //        skl.Stan = String.IsNullOrEmpty(skl.Opis) ? 1 : 0;

        //        ISimpleZUS simpleZUS = new SimpleZUS();
        //        IZUS zUS = Processor.AddZus(s);
        //        //simpleZUS.AddZUS(s);

        //        skl.DefSkladka = (DefSkladki)zUS;
        //        skl.Wartość = skl.DefSkladka.GetWartosc();
        //        skl.Subcontractor = s;
        //        //s.ZUSy.Add(skl);

        //        _context.Skladki.Add(skl);
        //        _context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(string.Format($"{ex.ToString()} \n {ex.InnerException}"));
        //    }
        //}
    }
}
