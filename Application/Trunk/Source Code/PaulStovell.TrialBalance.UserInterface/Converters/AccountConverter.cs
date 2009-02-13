using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using PaulStovell.TrialBalance.DomainModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows;

namespace PaulStovell.TrialBalance.UserInterface.Converters {
    [ValueConversion(typeof(Account), typeof(string))]
    [ValueConversion(typeof(string), typeof(Account))]
    public class AccountConverter : DependencyObject, IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            object result = string.Empty;

            if (this.Workbook == null) {
                // Although not needed for this method, we'll asset it here because the ConvertBack method needs 
                // the workbook.
                Debug.Fail("The Workbook must be supplied as a parameter to this converter.");
            }

            if (targetType == typeof(string)) {
                Account account = value as Account;
                if (account != null) {
                    result = account.Name;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            object result = null;

            if (this.Workbook == null) {
                Debug.Fail("The Workbook must be supplied as a parameter to this converter.");
            }

            if (this.Workbook != null) {
                if (value != null && value.ToString().Length > 0) {
                    // Try to load an account by name
                    if (result == null) {
                        result = this.Workbook.FetchAccountByAccountName(value.ToString());
                    }
                }
            }

            return result;
        }

        public Workbook Workbook {
            get { return (Workbook)GetValue(WorkbookProperty); }
            set { SetValue(WorkbookProperty, value); }
        }

        public static readonly DependencyProperty WorkbookProperty =
            DependencyProperty.Register("Workbook", typeof(Workbook), typeof(AccountConverter), new UIPropertyMetadata(null));
    }
}
