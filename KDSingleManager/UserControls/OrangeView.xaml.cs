using KDSingleManager.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KDSingleManager;

namespace KDSingleManager.UserControls
{
    /// <summary>
    /// Interaction logic for Bank Payments.xaml
    /// </summary>
    public partial class OrangeView : UserControl
    {
        AppContext _context;
        public OrangeView()
        {
            InitializeComponent();
            _context = MainWindow._context;
        }

        private void btn_ReadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string fp = string.Empty;
            if (ofd.ShowDialog() == true)
            {
                fp = ofd.FileName;
            }

            var content = string.Empty;

            try
            {
                content = System.IO.File.ReadAllText(fp, CodePagesEncodingProvider.Instance.GetEncoding(1250));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            List<Subcontractor> _subcontractors = _context.Subcontractors.ToList();

            var records = content/*.Replace(" ", "").Replace(".", "")*/.Split("\r\n")
                 .Select(x => x.Split(";"))
                 .Where(x => x.Length > 2)
                 .Where(x => decimal.TryParse(x[2].Replace(" ", "").Replace("€", ""), out decimal amount))
                 .Select(_toClass);
            //.ToList();

            //var subcons = _context.Subcontractors.ToList();


            //    records.ToList().ForEach(x => x.Exists = _context.Subcontractors.Any(((WinZusProcessor.Normalize(x.FirstName.ToLower()).ToString().Contains(WinZusProcessor.Normalize(item[2].ToLower()).ToString()) ||
            //        WinZusProcessor.Normalize(x.LastName.ToLower()).ToString().Contains(WinZusProcessor.Normalize(item[2].ToLower()).ToString()))
            //&& (WinZusProcessor.Normalize(x.FirstName.ToLower()).ToString().Contains(WinZusProcessor.Normalize(item[1].ToLower()).ToString()) ||
            //WinZusProcessor.Normalize(x.LastName.ToLower()).Contains(WinZusProcessor.Normalize(item[1].ToLower())))));

            string doImportu = string.Empty;
            try
            {
                foreach (PaymentsList item in records)
                {
                    doImportu += prepareRowInfo(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\n\r" + ex.InnerException);
            }



            dg_Subcons.ItemsSource = records;
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(sfd.FileName, doImportu, CodePagesEncodingProvider.Instance.GetEncoding(1250));
            }
        }

        private string prepareRowInfo(PaymentsList row)
        {
            string result = string.Empty;

            result = string.Format($"{row.Name};{row.AccNr.Replace(" ", "")};{row.AccNr[..2]};;70249000050000453078025525;83249000050000460057540205;02/2021;{row.Amount};EUR{Environment.NewLine}");

            return result;
        }

        public class PaymentsList
        {
            public string Name { get; set; }
            public string AccNr { get; set; }
            public decimal Amount { get; set; }
            public bool Exists { get; set; }
        }

        private readonly Func<string[], PaymentsList> _toClass = x => new PaymentsList
        {
            Name = x[0],
            AccNr = x[1],
            Amount = decimal.Parse(x[2].Replace(" ", "").Replace("€", "")),
        };
    }
}
