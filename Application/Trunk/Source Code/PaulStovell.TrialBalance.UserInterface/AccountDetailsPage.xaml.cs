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
using PaulStovell.Common;
using System.Collections.Specialized;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// Interaction logic for AccountDetailsWizardPage.xaml
    /// </summary>
    public partial class AccountDetailsPage : WizardPage {
        private ChangeCoordinator<Account> _ChangeCoordinator;
        private BindableCollection<BindableEnumerationValue> _availableCategories;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountDetailsPage() {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
            this.Cancelling += new CancelEventHandler(Page_Cancelling);
        }

        /// <summary>
        /// Gets or sets the editable account.
        /// </summary>
        public ChangeCoordinator<Account> AccountChangeCoordinator {
            get { return _ChangeCoordinator; }
            set {
                _ChangeCoordinator = value;
                if (value != null) {
                    this.DataContext = value.EditableItem;
                    // Create the list of available categories
                    this.AvailableCategories.Clear();
                    switch (this.AccountChangeCoordinator.EditableItem.AccountType) {
                        case AccountType.Asset:
                            this.AvailableCategories.AddRange(BindableEnumerationValue.BuildCollection(new AssetCategory()));
                            break;
                        case AccountType.Liability:
                            this.AvailableCategories.AddRange(BindableEnumerationValue.BuildCollection(new LiabilityCategory()));
                            break;
                        case AccountType.Revenue:
                            this.AvailableCategories.AddRange(BindableEnumerationValue.BuildCollection(new RevenueCategory()));
                            break;
                        case AccountType.Expense:
                            this.AvailableCategories.AddRange(BindableEnumerationValue.BuildCollection(new ExpenseCategory()));
                            break;
                    }
                    this.Title = this.AccountChangeCoordinator.EditableItem.AccountType.ToString() + " Details";
                    value.EditableItem.PropertyChanged += new PropertyChangedEventHandler(EditableItem_PropertyChanged);
                } else {
                    this.DataContext = null;
                }
            }
        }

        /// <summary>
        /// Gets the list of available cate
        /// </summary>
        public BindableCollection<BindableEnumerationValue> AvailableCategories {
            get {
                if (_availableCategories == null) {
                    _availableCategories = new BindableCollection<BindableEnumerationValue>();
                    _availableCategories.CollectionChanged += new NotifyCollectionChangedEventHandler(
                        delegate(object sender, NotifyCollectionChangedEventArgs e) {
                            this.OnPropertyChanged(new PropertyChangedEventArgs("HasCategories"));
                        });
                }
                return _availableCategories; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the categories combo box should be displayed.
        /// </summary>
        public bool HasCategories {
            get { return this.AvailableCategories.Count != 0; }
        }

        /// <summary>
        /// Called when the page loads.
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            _accountNumberTextBox.Focus();
        }

        /// <summary>
        /// Called when a property on the current account changes.
        /// </summary>
        private void EditableItem_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            // We have a special requirement for account numbers and assets. If the asset category is Current, the account number should start with a 3 (i.e., 3XX). 
            // If it's not a Current asset, the number should start with a 4 (i.e., 4XX).
            if (e.PropertyName == "Category") {
                Asset asset = this.AccountChangeCoordinator.EditableItem as Asset;
                if (asset != null) {
                    //if (asset.AccountNumber >= 400 && asset.AccountNumber <= 499 && asset.Category == AssetCategory.Current) {
                    //    asset.AccountNumber -= 100;
                    //} else if (asset.AccountNumber >= 300 && asset.AccountNumber <= 399 && asset.Category != AssetCategory.Current) {
                    //    asset.AccountNumber += 100;
                    //}
                }
            }
        }

        /// <summary>
        /// Called when the "Cancel" button is clicked.
        /// </summary>
        private void Page_Cancelling(object sender, CancelEventArgs e) {
            if (this.AccountChangeCoordinator.EditableItem.IsDirty) {
                MessageBoxResult result = MessageBox.Show("There are unsaved changes for this account. If you elect to cancel now your changes will be lost. \r\n\r\nAre you sure you wish to cancel?",
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
            if (this.AccountChangeCoordinator.EditableItem.IsValid) {
                _ChangeCoordinator.PushChanges();
                this.OnReturn(new WizardPageReturnEventArgs(this._ChangeCoordinator, false));
            }
        }

    }
}