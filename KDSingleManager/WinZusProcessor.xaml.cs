using System;
using System.Collections.Generic;
using System.Globalization;
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
        public static readonly DependencyProperty MonthsProperty = DependencyProperty.Register(
        "Months",
        typeof(List<string>),
        typeof(MainWindow),
        new PropertyMetadata(new CultureInfo("pl-PL").DateTimeFormat.MonthNames.Take(12).ToList()));

        public List<string> Months
        {
            get
            {
                return (List<string>)this.GetValue(MonthsProperty);
            }

            set
            {
                this.SetValue(MonthsProperty, value);
            }
        }
        public WinZusProcessor()
        {
            InitializeComponent();
        }
    }

    [ValueConversion(typeof(List<string>), typeof(List<string>))]
    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //get the list of months from the object sent in    
            List<string> months = (List<string>)value;

            //manipulate the data index starts at 0 so add 1 and set to 2 decimal places
            if (months != null && months.Count > 0)
            {
                for (int x = 0; x < months.Count; x++)
                {
                    months[x] = (x + 1).ToString("D2") + " - " + months[x];
                }
            }

            return months;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
