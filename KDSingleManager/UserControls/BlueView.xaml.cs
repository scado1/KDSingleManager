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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KDSingleManager.UserControls
{
    /// <summary>
    /// Interaction logic for BlueView.xaml
    /// </summary>
    public partial class BlueView : UserControl
    {
        AppContext _context;
        private CollectionViewSource subconViewSource;
        public BlueView()
        {
            InitializeComponent();
            _context = MainWindow._context;
            subconViewSource = (CollectionViewSource)FindResource(nameof(subconViewSource));
            ShowInfo();
        }

        private void ShowInfo()
        {
            _context.Database.EnsureCreated();
            _context.Subcontractors.Load();
            subconViewSource.Source = _context.Subcontractors.Local.ToObservableCollection();
        }
    }
}
