using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaulStovell.TrialBalance.UserInterface.Validation {
    /// <summary>
    /// Interaction logic for ErrorMessageList.xaml
    /// </summary>
    public partial class ErrorMessageList : System.Windows.Controls.UserControl {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ErrorMessageList() {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the ErrorProvider that this ErrorMessageList control uses to populate it's list.
        /// </summary>
        public ErrorProvider ErrorProvider {
            get { return (ErrorProvider)this.GetValue(ErrorProviderProperty); }
            set { this.SetValue(ErrorProviderProperty, value); }
        }

        public static readonly DependencyProperty ErrorProviderProperty =
            DependencyProperty.Register("ErrorProvider", typeof(ErrorProvider), typeof(ErrorMessageList), new UIPropertyMetadata(null,
                ErrorProviderProperty_PropertyChanged));

        private static void ErrorProviderProperty_PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e) {
            ErrorMessageList errorMessageList = dependencyObject as ErrorMessageList;
            if (errorMessageList != null) {
                if (errorMessageList.ErrorProvider == null) {
                    errorMessageList._errorListItemControl.ItemsSource = null;
                } else {
                    errorMessageList._errorListItemControl.ItemsSource = errorMessageList.ErrorProvider.Errors;
                }
            }
        }

    }
}