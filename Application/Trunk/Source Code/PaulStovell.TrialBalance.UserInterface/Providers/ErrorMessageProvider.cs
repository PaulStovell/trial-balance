using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using PaulStovell.TrialBalance.UserInterface.Validation;
using System.Collections.Specialized;

namespace PaulStovell.TrialBalance.UserInterface.Providers
{
    public static class ErrorMessageProvider {

        public static string GetErrorMessage(DependencyObject obj) {
            return (string)obj.GetValue(ErrorMessageProperty);
        }

        public static void SetErrorMessage(DependencyObject obj, string value) {
            obj.SetValue(ErrorMessageProperty, value);
        }

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.RegisterAttached("ErrorMessage", typeof(string), typeof(ErrorMessageProvider), new FrameworkPropertyMetadata(
                null, new CoerceValueCallback(ErrorMessageProperty_CoerceValueCallback)));

        public static bool GetHasErrorMessage(DependencyObject obj) {
            return (bool)obj.GetValue(ErrorMessageProperty);
        }

        public static void SetHasErrorMessage(DependencyObject obj, bool value) {
            obj.SetValue(ErrorMessageProperty, value);
        }

        public static readonly DependencyPropertyKey HasErrorMessageKey =
            DependencyProperty.RegisterAttachedReadOnly("HasErrorMessage", typeof(bool), typeof(ErrorMessageProvider), new FrameworkPropertyMetadata(null, 
            new CoerceValueCallback(HasErrorMessageProperty_CoerceValueCallback)));

        public static readonly DependencyProperty HasErrorMessageProperty = HasErrorMessageKey.DependencyProperty;

        public static string GetErrorProperty(DependencyObject obj) {
            return (string)obj.GetValue(ErrorPropertyProperty);
        }

        public static void SetErrorProperty(DependencyObject obj, string value) {
            obj.SetValue(ErrorMessageProperty, value);
        }

        public static readonly DependencyProperty ErrorPropertyProperty =
            DependencyProperty.RegisterAttached("ErrorProperty", typeof(string), typeof(ErrorMessageProvider), new UIPropertyMetadata(string.Empty));


        public static ErrorProvider GetErrorProvider(DependencyObject obj) {
            return (ErrorProvider)obj.GetValue(ErrorProviderProperty);
        }

        public static void SetErrorProvider(DependencyObject obj, ErrorProvider value) {
            obj.SetValue(ErrorProviderProperty, value);
        }

        public static readonly DependencyProperty ErrorProviderProperty =
            DependencyProperty.RegisterAttached("ErrorProvider", typeof(ErrorProvider), typeof(ErrorMessageProvider), new UIPropertyMetadata(null, StupidDIckheadPropertyHCanged));

        private static void StupidDIckheadPropertyHCanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            ErrorProvider p = e.NewValue as ErrorProvider;
            p.Errors.CollectionChanged +=
                delegate(object sender, NotifyCollectionChangedEventArgs nce) {
                    o.CoerceValue(HasErrorMessageProperty);
                    o.CoerceValue(ErrorMessageProperty);
                };
        }

        private static object ErrorMessageProperty_CoerceValueCallback(DependencyObject dependencyObject, object value) {
            StringBuilder result = new StringBuilder();
            ErrorProvider errorProvider = GetErrorProvider(dependencyObject);
            string errorProperty = GetErrorProperty(dependencyObject);

            if (errorProvider != null) {
                foreach (PaulStovell.Common.BindingFramework.ValidationError error in errorProvider.Errors) {
                    bool appliesToProperty = false;
                    foreach (string propertyName in error.PropertyNames) {
                        if (propertyName == errorProperty) {
                            appliesToProperty = true;
                            break;
                        }
                    }

                    if (appliesToProperty) {
                        result.AppendLine(error.ErrorMessage);
                    }
                }
            }
            return result.ToString().TrimEnd();
        }

        private static object HasErrorMessageProperty_CoerceValueCallback(DependencyObject dependencyObject, object value) {
            bool result = false;

            string errorMessage = ErrorMessageProperty_CoerceValueCallback(dependencyObject, null).ToString();
            result = errorMessage != null && errorMessage.Length > 0;

            return result;
        }
    }
}
