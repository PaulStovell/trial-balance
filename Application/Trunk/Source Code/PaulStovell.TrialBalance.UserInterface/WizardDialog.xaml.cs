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
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Windows.Media.Animation;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// Interaction logic for WizardWindow.xaml
    /// </summary>
    public partial class WizardDialog : BaseWindow {

        private bool _isReturning = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="startPage">The first page of the wizard to show.</param>
        public WizardDialog(WizardPage startPage) {
            InitializeComponent();

            this.EnableWindowDragging();
            this.EnableGlass(new Thickness(28, 28, 28, 100));

            startPage.Return += new WizardPageReturnEventHandler(StartPage_Return);
            _wizardPageFrame.Navigating += new NavigatingCancelEventHandler(WizardPageFrame_Navigating);
            _wizardPageFrame.Navigated += new NavigatedEventHandler(WizardPageFrame_Navigated);

            this.Closing += new System.ComponentModel.CancelEventHandler(WizardWindow_Closing);

            this.CurrentPage = startPage;
        }

        public event WizardPageReturnEventHandler Return;

        public void StartPage_Return(object sender, WizardPageReturnEventArgs e) {
            _isReturning = true;
            this.Close();
            this.OnReturn(e);
        }

        protected virtual void OnReturn(WizardPageReturnEventArgs e) {
            if (this.Return != null) {
                this.Return(this, e);
            }
        }

        public WizardPage CurrentPage {
            get {
                return _wizardPageFrame.Content as WizardPage;
            }
            set {
                _wizardPageFrame.Navigate(value);
            }
        }

        private void NextButton_Clicked(object sender, RoutedEventArgs e) {
            if (this.CurrentPage != null) {
                this.CurrentPage.Next();
            }
        }

        private void BackButton_Clicked(object sender, RoutedEventArgs e) {
            if (this.CurrentPage != null) {
                this.CurrentPage.Back();
            }
        }

        private void CancelButton_Clicked(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void WizardWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (!_isReturning) {
                _cancelButton.Focus();
                bool cancel = true;
                if (this.CurrentPage != null) {
                    cancel = this.CurrentPage.AskBeforeCancel();
                }

                if (cancel) {
                    this.OnReturn(new WizardPageReturnEventArgs(null, true));
                } else {
                    e.Cancel = true;
                }
            }
        }

        private void WizardPageFrame_Navigated(object sender, NavigationEventArgs e) {
            DoubleAnimation fadeOut = new DoubleAnimation();
            fadeOut.To = 1.00;
            fadeOut.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            _wizardPageFrame.BeginAnimation(Frame.OpacityProperty, fadeOut);
        }

        private void WizardPageFrame_Navigating(object sender, NavigatingCancelEventArgs e) {
            DoubleAnimation fadeOut = new DoubleAnimation();
            fadeOut.To = 0.00;
            fadeOut.Duration = new Duration(TimeSpan.FromMilliseconds(100));

            _wizardPageFrame.BeginAnimation(Frame.OpacityProperty, fadeOut);
        }
    }
}