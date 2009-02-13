using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using PaulStovell.TrialBalance.DomainModel;
using System.Windows.Media;
using System.Windows;

namespace PaulStovell.TrialBalance.UserInterface.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class VisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = Visibility.Visible;
            if (value is bool) {
                if ((bool)value) {
                    visibility = Visibility.Visible;
                } else {
                    visibility = Visibility.Collapsed;
                }
            } else if (value is Balance) {
                visibility = Visibility.Hidden;
                if (((Balance)value).BalanceType == BalanceType.Credit && parameter.ToString().ToLower() == "cr") {
                    visibility = Visibility.Visible;
                } else if (((Balance)value).BalanceType == BalanceType.Debit && parameter.ToString().ToLower() == "dr") {
                    visibility = Visibility.Visible;
                }
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
