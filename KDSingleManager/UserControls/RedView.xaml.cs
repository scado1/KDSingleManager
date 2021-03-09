using KDSingleManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RedView.xaml
    /// </summary>
    public partial class RedView : UserControl
    {
        AppContext _context;
        private CollectionViewSource subconAccountsViewSource;
        public RedView()
        {
            InitializeComponent();
            _context = MainWindow._context;
            subconAccountsViewSource = (CollectionViewSource)FindResource(nameof(subconAccountsViewSource));
            ShowInfo();
        }

        // TODO: comboBox subcontractors - search filter
        private void ShowInfo()
        {
            _context.Database.EnsureCreated();
            _context.Subcontractors.Load();
            _context.WynagrKonta.Load();
            subconAccountsViewSource.Source = _context.WynagrKonta.Local.ToObservableCollection();

            cb_Subcontractor.ItemsSource = _context.Subcontractors.ToList();

        }

        private void btn_AddWynagrKonto_Click(object sender, RoutedEventArgs e)
        {
            WynagrKonto kw = new WynagrKonto
            {
                Subcontractor = (Subcontractor)cb_Subcontractor.SelectedItem,
                Konto = tb_Konto.Text.Replace(" ", ""),
                Swift = tb_Swift.Text,
                Kraj = tb_Kraj.Text,
                PosiadaczRachunku = tb_PosiadaczRachunku.Text,
            };
            AddKontoWynagr(kw);
        }
        private void btn_ImportWynagrKont_Click(object sender, RoutedEventArgs e)
        {
            string fp = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                fp = ofd.FileName;
            }
            MessageBox.Show(fp);


            List<string> recs = new List<string>();

            string content = System.IO.File.ReadAllText(fp, CodePagesEncodingProvider.Instance.GetEncoding(1250));
            List<WynagrKonto> result = content.Split("\r\n")
                            .Select(_splitOnSemicolon)
                            .Select(_parseData).ToList();

            result.ForEach(x => _context.WynagrKonta.Add(x));
            _context.SaveChanges();
            //.Where(x => x.Konto != null);
            //.Select(x => x.PosiadaczRachunku);

            //result.ToList().ForEach(x => MessageBox.Show(x.PosiadaczRachunku));

        }

        private readonly Func<string, string[]> _splitOnSemicolon = x => x.Split(";");
        private readonly Func<string[], WynagrKonto> _parseData = x => new WynagrKonto
        {
            PosiadaczRachunku = x[0].ToString(),
            Konto = x[1].ToString(),
            Kraj = x[2].ToString(),
            Swift = x[3].ToString()
        };

        private void AddKontoWynagr(WynagrKonto kw)
        {
            _context.WynagrKonta.Add(kw);
            _context.SaveChanges();
        }

        private void tb_Konto_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                Regex regex = new Regex(@"^[a-zA-Z]{2}");
                Match match = regex.Match(tb_Konto.Text);

                while (match.Success)
                {
                    tb_Kraj.Text += Regex.Match(tb_Konto.Text, @"^[a-zA-Z]{2}").Value;
                    tb_Konto.Text = Regex.Replace(tb_Konto.Text, @"^[a-zA-Z]{2}", "");
                    match = match.NextMatch();
                }

                if (tb_Konto.Text.Length == 26)
                {
                    tb_Konto.Text = String.Format("{0:00 0000 0000 0000 0000 0000 0000}", (decimal.Parse(tb_Konto.Text)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.ToString()}\r\n{ex.InnerException}");
            }
        }
        private void cb_Subcontractor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_PosiadaczRachunku.Text = cb_Subcontractor.DisplayMemberPath;
        }

    }
}

