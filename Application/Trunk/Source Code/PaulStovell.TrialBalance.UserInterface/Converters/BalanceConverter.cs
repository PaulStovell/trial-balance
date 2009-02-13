using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using PaulStovell.TrialBalance.DomainModel;
using System.Windows.Media;

namespace PaulStovell.TrialBalance.UserInterface.Converters {
    [ValueConversion(typeof(Balance), typeof(string))]
    [ValueConversion(typeof(Balance), typeof(Brush))]
    public class BalanceConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {

            object result = null;

            parameter = parameter ?? string.Empty;
            bool showZeroMode = parameter.ToString().ToLower().Contains("showzero");
            bool asyncLoadMode = parameter.ToString().ToLower().Contains("async") && value == null;
            bool creditOnlyMode = parameter.ToString().ToLower().Contains("creditonly") && value != null;
            bool debitOnlyMode = parameter.ToString().ToLower().Contains("debitonly") && value != null;

            if (!showZeroMode)
            {
                showZeroMode = !creditOnlyMode && !debitOnlyMode;
            }

            if (targetType == typeof(Brush)) {
                if (asyncLoadMode) {
                    result = new SolidColorBrush(Color.FromRgb(0xBB, 0xBB, 0xBB));
                } else {
                    result = Brushes.Black;
                }
            } else {
                result = string.Empty;

                if (asyncLoadMode) {
                    result = "Loading...";
                } else if (value != null) {
                    Balance b = (Balance)value;

                    if (b.Magnitude != 0M || showZeroMode)
                    {
                        if (!creditOnlyMode && !debitOnlyMode) {
                            result = b.ToString();
                        }

                        if (creditOnlyMode && b.BalanceType == BalanceType.Credit) {
                            result = b.Magnitude.ToString("c");
                        } else if (debitOnlyMode && b.BalanceType == BalanceType.Debit) {
                            result = b.Magnitude.ToString("c");
                        }
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            object result = null;
            if (value is string) {
                parameter = parameter ?? string.Empty;
                bool creditOnlyMode = parameter.ToString().ToLower().Contains("creditonly") && value != null;
                bool debitOnlyMode = parameter.ToString().ToLower().Contains("debitonly") && value != null;

                BalanceType? type = null;
                if (creditOnlyMode) {
                    type = BalanceType.Credit;
                } else {
                    type = BalanceType.Debit;
                }

                Balance resultBalance = null;
                if (Balance.TryParse(value.ToString(), type, out resultBalance)) {
                    result = resultBalance;
                }
                    
            }
            return result;
        }
    }
}
