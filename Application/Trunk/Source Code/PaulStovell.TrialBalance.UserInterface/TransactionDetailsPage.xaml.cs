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
using System.ComponentModel;
using PaulStovell.TrialBalance.UserInterface.Converters;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// Interaction logic for TransactionDetailsWizardPage.xaml
    /// </summary>
    public partial class TransactionDetailsPage : WizardPage {
        private ChangeCoordinator<Transaction> _transactionChangeCoordinator;
        private Workbook _workbook;

        public TransactionDetailsPage(Workbook workbook) {
            InitializeComponent();
            this.Cancelling += new CancelEventHandler(Page_Cancelling);

            ((AccountConverter)this.Resources["_accountConverter"]).Workbook = workbook;
        }

        public ChangeCoordinator<Transaction> Transaction {
            get { return _transactionChangeCoordinator; }
            set 
            {
                _transactionChangeCoordinator = value;
                if (value == null) {
                    this.DataContext = null;
                } else {
                    this.DataContext = value.EditableItem;
                }
            }
        }

        /// <summary>
        /// Gets or sets the workbook that is used for this page.
        /// </summary>
        public Workbook Workbook {
            get { return _workbook; }
            set { _workbook = value; }
        }

        /// <summary>
        /// Called when the "Cancel" button is clicked.
        /// </summary>
        private void Page_Cancelling(object sender, CancelEventArgs e) {
            if (this.Transaction.EditableItem.IsDirty) {
                MessageBoxResult result = MessageBox.Show("There are unsaved changes for this transaction. If you elect to cancel now your changes will be lost. \r\n\r\nAre you sure you wish to cancel?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No) {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Called when the Save button is clicked.
        /// </summary>
        public override void Next() {
            if (_errorProvider.Validate() == false) {
                //_errorProvider.GetFirstInvalidElement().Focus();
            } else {
                this.OnReturn(new WizardPageReturnEventArgs(this.Transaction, false));
            }
        }
    }
}