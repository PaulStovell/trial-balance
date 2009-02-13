using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Input;

namespace PaulStovell.TrialBalance.UserInterface.Providers {
    public class StatusMessageProvider : DependencyObject {
        private static readonly StatusMessageProvider _instance = new StatusMessageProvider();

        private StatusMessageProvider() {

        }

        public static StatusMessageProvider Instance {
            get { return _instance; }
        }

        public static string GetStatusMessage(DependencyObject obj) {
            return (string)obj.GetValue(StatusMessageProperty);
        }

        public static void SetStatusMessage(DependencyObject obj, string value) {
            obj.SetValue(StatusMessageProperty, value);
        }

        public static readonly DependencyProperty StatusMessageProperty =
            DependencyProperty.RegisterAttached("StatusMessage", typeof(string), typeof(StatusMessageProvider), new UIPropertyMetadata(string.Empty,
            StatusMessageProperty_PropertyChanged));

        private static void StatusMessageProperty_PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e) {
            FrameworkElement element = dependencyObject as FrameworkElement;
            if (element != null) {
                element.MouseEnter += new MouseEventHandler(Element_MouseEnter);
                element.GotFocus += new RoutedEventHandler(Element_GotFocus);
            }
        }

        private static void Element_GotFocus(object sender, RoutedEventArgs e) {
            SetMessage(sender);
        }

        private static void Element_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            SetMessage(sender);
        }

        private static void SetMessage(object sender) {
            string statusMessage = null;

            DependencyObject dependencyObject = sender as DependencyObject;
            if (dependencyObject != null) {
                statusMessage = GetStatusMessage(dependencyObject);
                if (statusMessage != null && statusMessage.Length > 0) {
                    Instance.CurrentStatusMessage = statusMessage;
                }
            }
        }

        public static readonly DependencyProperty CurrentStatusMessageProperty =
            DependencyProperty.Register("CurrentStatusMessage", typeof(string), typeof(StatusMessageProvider), new PropertyMetadata(string.Empty));

        public string CurrentStatusMessage {
            get { return (string)StatusMessageProvider.Instance.GetValue(CurrentStatusMessageProperty); }
            set { StatusMessageProvider.Instance.SetValue(CurrentStatusMessageProperty, value); }
        }
    }
}
