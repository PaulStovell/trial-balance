using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using PaulStovell.TrialBalance.DomainModel;
using System.Windows.Media;

namespace PaulStovell.TrialBalance.UserInterface.Converters {
    [ValueConversion(typeof(decimal), typeof(string))]
    [ValueConversion(typeof(string), typeof(decimal))]
    public class DecimalConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            object result = value;
            if (value is decimal) {
                result = ((decimal)value).ToString("n2", culture);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            string inputString = (value ?? string.Empty).ToString();

            decimal result = 0;
            decimal.TryParse(inputString, System.Globalization.NumberStyles.Any, culture, out result);

            return result;
        }
    }
}
