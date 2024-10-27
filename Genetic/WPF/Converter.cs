using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace WPF
{
    public class CityNConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            return value.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int cityN = (int)value;
            return cityN;
        }
    }

    public class GenCountConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int gencount = (int)value;
            return gencount;
        }
    }
    public class MaxPopConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int maxpop = (int)value;
            return maxpop;
        }
    }

}
