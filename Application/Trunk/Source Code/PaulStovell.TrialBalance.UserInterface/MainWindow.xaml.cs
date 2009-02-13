using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Media;
using System.Reflection;
using PaulStovell.TrialBalance.DomainModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using PaulStovell.Common;
using System.ComponentModel;
using PaulStovell.TrialBalance.UserInterfaceProcesses;
using System.Collections.Specialized;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// The main window of the application, containing the Account List.
    /// </summary>
    public partial class MainWindow : BaseWindow {
        private Workbook _workbook;
        private string _filterText = string.Empty;
        private AccountGroupFilter _assetsGroupFilter;
        private AccountGroupFilter _liabilitiesGroupFilter;
        private AccountGroupFilter _equityGroupFilter;
        private AccountGroupFilter _revenueGroupFilter;
        private AccountGroupFilter _expensesGroupFilter;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow(Workbook workbook) {
            InitializeComponent();
            this.EnableWindowDragging();
            this.EnableGlass(new Thickness(14, 130, 14, 14));
            
            this.Workbook = workbook;
        }

        /// <summary>
        /// Gets or sets the current workbook for this window.
        /// </summary>
        public Workbook Workbook {
            get { return _workbook; }
            set { 
                _workbook = value;
                this.DataContext = _workbook;
                if (_workbook != null) {
                    BuildAccountGroupFilters();
                    _workbook.PropertyChanged += 
                        delegate(object sender, PropertyChangedEventArgs e) {
                            if (e.PropertyName == "BusinessName") {
                                UpdateTitle();
                            }
                        };
                    UpdateTitle();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in the Filter text box.
        /// </summary>
        public string FilterText {
            get { return _filterText ?? string.Empty; }
            set {
                _filterText = value ?? string.Empty;
                _assetsGroupFilter.FilterText = _filterText;
                _liabilitiesGroupFilter.FilterText = _filterText;
                _equityGroupFilter.FilterText = _filterText;
                _revenueGroupFilter.FilterText = _filterText;
                _expensesGroupFilter.FilterText = _filterText;
                if (_filterText.Length > 0) {
                    // Force the account groups to expand
                    _assetsExpander.IsExpanded = true;
                    _liabilitiesExpander.IsExpanded = true;
                    _equityExpander.IsExpanded = true;
                    _revenueExpander.IsExpanded = true;
                    _expensesExpander.IsExpanded = true;
                }
                this.OnPropertyChanged(new PropertyChangedEventArgs("FilterText"));
            }
        }

        /// <summary>
        /// Initializes the filters for the different account types.
        /// </summary>
        private void BuildAccountGroupFilters() {
            this.Resources["Local_AssetsGroupFilter"] = _assetsGroupFilter = new AccountGroupFilter(this.Workbook, AccountType.Asset);
            this.Resources["Local_LiabilitiesGroupFilter"] = _liabilitiesGroupFilter = new AccountGroupFilter(this.Workbook, AccountType.Liability);
            this.Resources["Local_EquityGroupFilter"] = _equityGroupFilter = new AccountGroupFilter(this.Workbook, AccountType.Equity);
            this.Resources["Local_RevenueGroupFilter"] = _revenueGroupFilter = new AccountGroupFilter(this.Workbook, AccountType.Revenue);
            this.Resources["Local_ExpensesGroupFilter"] = _expensesGroupFilter = new AccountGroupFilter(this.Workbook, AccountType.Expense);
        }

        /// <summary>
        /// Updates the title of the application. It will usually be "[Business Name] - TrialBalance".
        /// </summary>
        private void UpdateTitle() {
            string title = "TrialBalance";
            if (this.Workbook != null) {
                title = this.Workbook.BusinessName + " - TrialBalance";
            }
            this.Title = title;
        }

        /// <summary>
        /// Called when the Back button for the period view is clicked.
        /// </summary>
        private void PeriodBackButton_Clicked(object sender, RoutedEventArgs e) {
            using (new DisposableMouseCursor()) {
                this.Workbook.CurrentPeriod = this.Workbook.CurrentPeriod.GetPrevious();
            }
        }

        /// <summary>
        /// Called when the Next button for the period view is clicked.
        /// </summary>
        private void PeriodForwardButton_Clicked(object sender, RoutedEventArgs e) {
            using (new DisposableMouseCursor()) {
                this.Workbook.CurrentPeriod = this.Workbook.CurrentPeriod.GetNext();
            }
        }

        /// <summary>
        /// Called when the Add Account button is clicked.
        /// </summary>
        private void AddAccountButton_Clicked(object sender, RoutedEventArgs e) {
            AccountDetailsDialog.ShowNew(this.Workbook);
        }

        /// <summary>
        /// Called when the Add Account button is clicked.
        /// </summary>
        private void AddTransactionButton_Clicked(object sender, RoutedEventArgs e) {
            
        }

        /// <summary>
        /// Called when the Add Account button is clicked.
        /// </summary>
        private void WorkbookDetailsButton_Clicked(object sender, RoutedEventArgs e) {
            WorkbookDetailsDialog.ShowNew(this.Workbook);
        }

        /// <summary>
        /// Called when an account in the Account List is clicked.
        /// </summary>
        private void Account_Clicked(object sender, MouseEventArgs e) {
            FrameworkElement item = sender as FrameworkElement;
            if (item != null && item.Tag != null) {
                Account account = item.Tag as Account;
                if (account != null) {
                    AccountLedgerWindow.Show(account);
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Called when an account in the Account List is clicked.
        /// </summary>
        private void Account_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Space) {
                FrameworkElement item = sender as FrameworkElement;
                if (item != null && item.Tag != null) {
                    Account account = item.Tag as Account;
                    if (account != null) {
                        AccountLedgerWindow.Show(account);
                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Called when a key is pressed in the Filter TextBox. We'll use this to trap the Enter key to open the topmost account.
        /// </summary>
        private void FilterTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                TransactionCollectionView topmostItem = null;
                foreach (Expander expander in _accountsStackPanel.Children) {
                    if (expander.IsExpanded) {
                        AccountGroupFilter filter = expander.DataContext as AccountGroupFilter;
                        if (filter.Count > 0) {
                            topmostItem = filter[0];
                            break;
                        }
                    }
                }
                if (topmostItem != null) {
                    AccountLedgerWindow.Show(topmostItem.ParentAccount);
                }
            }
        }

        /// <summary>
        /// Called just before a key is pressed in the Filter TextBox. We'll use this to capture the Down arrow key and move focus to 
        /// the topmost item in the account list.
        /// </summary>
        private void FilterTextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Down) {
                _accountsStackPanel.Children[0].Focus();
            }
        }

        /// <summary>
        /// Called when one of the Expander toggle buttons for the different accounts are clicked.
        /// </summary>
        private void ToggleExpansion(object sender, RoutedEventArgs e) {
            FrameworkElement control = sender as FrameworkElement;
            if (control != null) {
                Expander expander = control.Tag as Expander;
                if (expander != null) {
                    expander.IsExpanded = !expander.IsExpanded;
                }
            }
        }

        /// <summary>
        /// Called just before a text key is pressed on the window and unhandled anywhere else. We'll use this
        /// as an opportunity to forward key presses onto the FilterTextBox.
        /// </summary>
        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            if (!_filterTextBox.IsFocused) {
                Keyboard.Focus(_filterTextBox);
            }
        }
    }
}