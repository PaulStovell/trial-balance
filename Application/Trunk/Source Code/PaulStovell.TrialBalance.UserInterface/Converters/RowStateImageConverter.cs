using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Data;

namespace PaulStovell.TrialBalance.UserInterface.Converters {
    [ValueConversion(typeof(GridRowState), typeof(ImageSource))]
    public class RoWStateImageConverter : IValueConverter {

        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture) {

            if (value is GridRowState) {
                if ((GridRowState)value == GridRowState.New) {
                    return (ImageSource)Application.Current.Resources["Image_NewItem"];
                } else if ((GridRowState)value == GridRowState.Invalid) {
                    return (ImageSource)Application.Current.Resources["Image_Warning"];
                }
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
