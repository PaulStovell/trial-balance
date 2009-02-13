using System;
using System.Data;
using System.Configuration;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.UserInterface {
    public class WizardPage : Page, INotifyPropertyChanged {
        public WizardPage() {

        }

        public static readonly DependencyProperty IsCancelEnabledProperty =
            DependencyProperty.Register("IsCancelEnabled", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(true));

        public static readonly DependencyProperty IsNextEnabledProperty =
            DependencyProperty.Register("IsNextEnabled", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(true));

        public static readonly DependencyProperty IsBackEnabledProperty =
            DependencyProperty.Register("IsBackEnabled", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(true));

        public static readonly DependencyProperty HasCancelButtonProperty =
            DependencyProperty.Register("HasCancelButton", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(true));

        public static readonly DependencyProperty HasNextButtonProperty =
            DependencyProperty.Register("HasNextButton", typeof(bool), typeof(WizardPage), new UIPropertyMetadata(true));

        public static readonly DependencyProperty NextButtonTextProperty =
            DependencyProperty.Register("NextButtonText", typeof(string), typeof(WizardPage), new UIPropertyMetadata("Next"));

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCancelEnabled {
            get { return (bool)GetValue(IsCancelEnabledProperty); }
            set { SetValue(IsCancelEnabledProperty, value); }
        }

        public bool IsNextEnabled {
            get { return (bool)GetValue(IsNextEnabledProperty); }
            set { SetValue(IsNextEnabledProperty, value); }
        }

        public bool IsBackEnabled {
            get { return (bool)GetValue(IsBackEnabledProperty); }
            set { SetValue(IsBackEnabledProperty, value); }
        }

        public bool HasCancelButton {
            get { return (bool)GetValue(HasCancelButtonProperty); }
            set { SetValue(HasCancelButtonProperty, value); }
        }

        public bool HasNextButton {
            get { return (bool)GetValue(HasNextButtonProperty); }
            set { SetValue(HasNextButtonProperty, value); }
        }

        public string NextButtonText {
            get { return (string)GetValue(NextButtonTextProperty); }
            set { SetValue(NextButtonTextProperty, value); }
        }

        public virtual void Next() {

        }

        public virtual void Back() {
            this.NavigationService.GoBack();
        }

        internal bool AskBeforeCancel() {
            CancelEventArgs args = new CancelEventArgs();
            this.OnCancelling(args);
            return !args.Cancel;
        }

        public event WizardPageReturnEventHandler Return;

        public event CancelEventHandler Cancelling;

        protected virtual void OnReturn(WizardPageReturnEventArgs e) {
            if (this.Return != null) {
                this.Return(this, e);
            }
        }

        protected virtual void OnCancelling(CancelEventArgs e) {
            if (this.Cancelling != null) {
                this.Cancelling(this, e);
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, e);
            }
        }
    }
}
