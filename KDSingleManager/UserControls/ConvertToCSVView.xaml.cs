using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using KDSingleManager.Models;
using KDSingleManager.Processors;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KDSingleManager.UserControls
{
    /// <summary>
    /// Interaction logic for ConvertToCSVView.xaml
    /// </summary>
    public partial class ConvertToCSVView : UserControl
    {
        AppContext _context;
        private XLWorkbook wb;
        public ConvertToCSVView()
        {
            InitializeComponent();
            _context = MainWindow._context;
        }

        private void btn_ReadFile_Click(object sender, RoutedEventArgs e)
        {
            List<InvoiceExport> invoices = new List<InvoiceExport>();

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "excel| *xlsx",
                InitialDirectory = @"W:\KD Building\Do przekazania od Michała\PPE\2021"
            };
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                try
                {
                    wb = new XLWorkbook(ofd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("zamknij plik \r\n" + ex.ToString());
                    return;
                }
                finally
                {
                    foreach (IXLWorksheet item in wb.Worksheets)
                    {
                        cb_Worksheets.Items.Add(item.Name);
                    }
                }

            }
        }

        private void cb_Worksheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
