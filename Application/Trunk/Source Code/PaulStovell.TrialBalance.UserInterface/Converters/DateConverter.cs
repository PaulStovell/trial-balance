using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using PaulStovell.TrialBalance.DomainModel;
using System.Windows.Media;
using System.Globalization;

namespace PaulStovell.TrialBalance.UserInterface.Converters {
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {

            object result = null;

            if (targetType == typeof(string)) {
                if (value is DateTime) {
                    DateTime date = ((DateTime)value);
                    if (date == DateTime.MinValue) {
                        result = string.Empty;
                    } else {
                        result = date.Date.ToString("dd MMM yyyy");
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            object result = null;
            if (value is string) {
                DateTime newDate = DateTime.Now.Date;
                if (value.ToString() == string.Empty) {
                    result = DateTime.MinValue;
                } else if (DateTime.TryParse(value.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.None, out newDate)) {
                    result = newDate.Date;
                }
            }
            return result;
        }
    }
}
