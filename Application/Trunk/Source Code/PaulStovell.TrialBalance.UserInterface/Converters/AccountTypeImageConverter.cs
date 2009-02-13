using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using PaulStovell.TrialBalance.DomainModel;
using System.Windows.Media;

namespace PaulStovell.TrialBalance.UserInterface.Converters {
    [ValueConversion(typeof(AccountType), typeof(ImageSource))]
    public class AccountTypeImageConverter : IValueConverter {
        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture) {

            if (value != null) {
                return Application.Current.Resources["Image_" + value.ToString()] as ImageSource;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture) {
            // we don't intend this to ever be called
            return null;
        }
    }
}
