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

namespace KDSingleManager.UserControls
{
    /// <summary>
    /// Interaction logic for RenumerationView.xaml
    /// </summary>
    public partial class RenumerationView : UserControl
    {
        AppContext _context;
        private CollectionViewSource RenumerationViewSource;
        public RenumerationView()
        {
            InitializeComponent();
            _context = MainWindow._context;
            RenumerationViewSource = (CollectionViewSource)FindResource(nameof(RenumerationViewSource));
            ShowInfo();
        }
        private void ShowInfo()
        {
            _context.Przejscia.Load();
            dg_Subcons.ItemsSource = _context.Przejscia.Local.ToBindingList().Where(x=> x.Subcontractor != null);
            
            
        }
    }
}
