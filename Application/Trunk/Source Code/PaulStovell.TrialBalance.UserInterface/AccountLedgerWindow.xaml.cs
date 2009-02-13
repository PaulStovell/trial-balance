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
using PaulStovell.TrialBalance.DomainModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using PaulStovell.TrialBalance.UserInterfaceProcesses;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// Interaction logic for LedgerWindow.xaml
    /// </summary>
    public partial class AccountLedgerWindow : BaseWindow {
        private static readonly Dictionary<Account, AccountLedgerWindow> _openWindows = new Dictionary<Account, AccountLedgerWindow>();
        private Workbook _workbook;

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="workbook">The workbook that this ledger window uses.</param>
        /// <param name="account">The account to display in the window.</param>
        private AccountLedgerWindow(Account account) {
            InitializeComponent();
            this.EnableWindowDragging();
            this.EnableGlass(new Thickness(7, 70, 7, 7));

            this.Account = account;

            this.Loaded += new RoutedEventHandler(AccountLedgerWindow_Loaded);
        }

        /// <summary>
        /// Gets the account being displayed in this window.
        /// </summary>
        public Account Account {
            get {
                return this.DataContext as Account;
            }
            private set {
                this.DataContext = value;

                if (value != null) {
                    if (value.Name.Length > 0) {
                        this.Title = value.Name + " - Account Ledger";
                    }
                }
            }
        }

        /// <summary>
        /// A static method to show the window. 
        /// </summary>
        /// <param name="account">The account to edit.</param>
        /// <returns>The newly created ledger window.</returns>
        public static AccountLedgerWindow Show(Account account) {
            AccountLedgerWindow result = null;
            if (_openWindows.ContainsKey(account)) {
                result = _openWindows[account];
                result.Activate();
            } else {
                result = new AccountLedgerWindow(account);
                result.Closed += new EventHandler(Window_Closed);
                result.Show();
                _openWindows[account] = result;
            }
            return result;
        }

        /// <summary>
        /// Called when the window is closed to remove it from the open windows collection.
        /// </summary>
        private static void Window_Closed(object sender, EventArgs e) {
            AccountLedgerWindow window = sender as AccountLedgerWindow;
            if (window != null) {
                if (_openWindows.ContainsKey(window.Account)) {
                    _openWindows.Remove(window.Account);
                }
            }
        }

        /// <summary>
        /// Called when the window initially loads.
        /// </summary>
        private void AccountLedgerWindow_Loaded(object sender, RoutedEventArgs e) {
            LedgerTransactionTransformedCollection datasource = new LedgerTransactionTransformedCollection(new TransactionCollectionView(this.Account, this.Account.Workbook), this.Account.Workbook);
            datasource.LedgerTransactionSaving += new LedgerTransactionSavingEventHandler(DataSource_LedgerTransactionSaving);
            _transactionsItemsControl.ItemsSource = datasource;
        }

        /// <summary>
        /// Called when a LedgerTransaction is saved.
        /// </summary>
        private void DataSource_LedgerTransactionSaving(object sender, LedgerTransactionSavingEventArgs e) {
            if (!this.Account.Workbook.CurrentPeriod.IncludesDate(e.LedgerTransaction.Date)) {
                MessageBoxResult mbr = MessageBox.Show("The date for this transaction lies outside of the current accounting period. If you proceed, the transaction will not be visible on this ledger. \r\n\r\nAre you sure you wish to proceed?",
                    "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (mbr == MessageBoxResult.Cancel) {
                    e.Cancel = true;
                }
            }
        }

        private void DataRow_GotFocus(object sender, RoutedEventArgs e) {
            if (e.OriginalSource is Border) {
                MessageBox.Show("GotFocus");
            }
        }

        private void DataRow_LostFocus(object sender, RoutedEventArgs e) {
            if (e.OriginalSource is Border) {
                MessageBox.Show("LostFocus");
            }
        }
    }
}