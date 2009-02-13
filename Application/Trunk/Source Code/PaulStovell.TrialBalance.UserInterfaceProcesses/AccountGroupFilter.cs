using System;
using System.Collections.Generic;
using System.Text;
using PaulStovell.TrialBalance.DomainModel;
using System.Collections.ObjectModel;
using PaulStovell.Common;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.UserInterfaceProcesses {
    public class AccountGroupFilter : FilterView<TransactionCollectionView> {
        private string _filterText;
        private string _filterTextLowercase = string.Empty;
        private AccountTypeFilter _filter;

        public AccountGroupFilter(Workbook workbook, AccountType accountType) {
            _filter = new AccountTypeFilter(workbook, accountType);
            _filter.CollectionChanged += new NotifyCollectionChangedEventHandler(Filter_CollectionChanged);
            this.Rebuild(_filter);
        }

        public string FilterText {
            get { return _filterText ?? string.Empty; }
            set {
                _filterText = value ?? string.Empty;
                _filterTextLowercase = _filterText.ToLower();
                Rebuild(_filter);
            }
        }

        private void Filter_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Reset) {
                this.Rebuild(_filter);
            } else {
                this.ProcessChanges(e);
            }
        }

        protected override bool FilterItem(TransactionCollectionView item) {
            bool result = false;

            if (item.ParentAccount.Name.ToLower().Contains(_filterTextLowercase)) {
                result = true;
            }

            return result;
        }

        public bool HasItems {
            get { return this.Count > 0; }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
            base.OnCollectionChanged(e);
            this.OnPropertyChanged(new PropertyChangedEventArgs("HasItems"));
        }

        private class AccountTypeFilter : FilterView<TransactionCollectionView> {
            private Workbook _workbook;
            private AccountType _accountType;

            public AccountTypeFilter(Workbook workbook, AccountType accountType) {
                _accountType = accountType;
                _workbook = workbook;

                this.AutoSort = true;
                this.Workbook.AccountSaved += new AccountEventHandler(Workbook_AccountSaved);
                this.Workbook.PropertyChanged += new PropertyChangedEventHandler(Workbook_PropertyChanged);

                ReloadAccounts();
            }

            public AccountType AccountType {
                get { return _accountType; }
            }

            public Workbook Workbook {
                get { return _workbook; }
            }

            private void Workbook_PropertyChanged(object sender, PropertyChangedEventArgs e) {
                if (e.PropertyName == "CurrentPeriod") {
                    ReloadAccounts();
                }
            }

            public void ReloadAccounts() {
                BindableCollection<Account> accounts = this.Workbook.FetchAccounts();
                List<TransactionCollectionView> views = new List<TransactionCollectionView>();
                foreach (Account account in accounts) {
                    views.Add(new TransactionCollectionView(account, this.Workbook.CurrentPeriod));
                }
                this.Rebuild(views);
            }

            private void Workbook_AccountSaved(object sender, AccountEventArgs e) {
                if (!this.ContainsViewForAccount(e.Account)) {
                    TransactionCollectionView view = new TransactionCollectionView(e.Account, this.Workbook.CurrentPeriod);
                    NotifyCollectionChangedEventArgs eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, view);
                    this.ProcessChanges(eventArgs);
                }
            }

            protected override bool FilterItem(TransactionCollectionView item) {
                return item != null && item.ParentAccount.AccountType == this.AccountType;
            }

            private bool ContainsViewForAccount(Account account) {
                bool result = false;
                foreach (TransactionCollectionView tcv in this) {
                    if (tcv.ParentAccount == account) {
                        result = true;
                        break;
                    }
                }
                return result;
            }
        }
    }
}
