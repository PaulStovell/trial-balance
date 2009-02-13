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
using PaulStovell.TrialBalance.DomainModel;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// Interaction logic for AccountTypePage.xaml
    /// </summary>
    public partial class AccountTypePage : WizardPage {
        private Workbook _workbook;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountTypePage() {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="workbook">The workbook used to create the accounts.</param>
        public AccountTypePage(Workbook workbook) : this() {
            this.Workbook = workbook;
        }

        /// <summary>
        /// Gets or sets the workbook used to create the accounts.
        /// </summary>
        public Workbook Workbook {
            get { return _workbook; }
            set { _workbook = value; }
        }

        /// <summary>
        /// Called when one of the command buttons is clicked.
        /// </summary>
        private void AccountTypeButton_Clicked(object sender, RoutedEventArgs e) {
            FrameworkElement button = sender as FrameworkElement;
            if (button != null) {
                AccountType accountType = (AccountType)button.Tag;

                AccountDetailsPage detailsPage = new AccountDetailsPage();
                detailsPage.AccountChangeCoordinator = this.Workbook.CreateAccount(accountType);
                detailsPage.Return += new WizardPageReturnEventHandler(DetailsPage_Return);
                this.NavigationService.Navigate(detailsPage);
            }
        }

        private void DetailsPage_Return(object sender, WizardPageReturnEventArgs e) {
            this.OnReturn(e);
        }
    }
}