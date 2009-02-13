using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using PaulStovell.TrialBalance.DomainModel;

namespace PaulStovell.TrialBalance.UserInterface.Converters {
    [ValueConversion(typeof(Balance), typeof(int))]
    public class BalanceColumnNumberConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            int result = 0;

            if (value is Balance) {
                if (((Balance)value).BalanceType == BalanceType.Credit) {
                    result = 1;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new Exception("The method or operation is not implemented.");
        }
    }

}
