using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using PaulStovell.TrialBalance.DomainModel;
using System.Windows.Media;

namespace PaulStovell.TrialBalance.UserInterface.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    [ValueConversion(typeof(string), typeof(int))]
    public class NumberConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int result = 0;
            if (value != null && value.ToString().Trim().Length > 0)
            {
                int.TryParse(value.ToString(), out result);
            }

            return result;
        }
    }
}
