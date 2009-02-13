using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Collections.ObjectModel;
using PaulStovell.Common;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// A read-only view over an accounts transaction collection.
    /// </summary>
    public class TransactionCollectionView : FilterView<Transaction>, IComparable {
        private Account _parentAccount;
        private Period _accountingPeriod;
        private BalanceType? _transactionBalanceType;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="account">The account that the view applies over.</param>
        /// <param name="period">The accounting period that the view is limited to.</param>
        public TransactionCollectionView(Account account, Period period)
            : this(account, period, null) {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="account">The account that the view applies over.</param>
        /// <param name="period">The accounting period that the view is limited to.</param>
        /// <param name="balanceType">The BalanceType that the view is limited to (can be null).</param>
        public TransactionCollectionView(Account account, Period period, BalanceType? balanceType) {
            this.AccountingPeriod = period;
            this.TransactionBalanceType = balanceType;
            this.ParentAccount = account;
            this.AutoSort = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="account">The account that the view applies over.</param>
        /// <param name="periodProvider">A publisher that allows the view to bind to a specific accounting period.</param>
        public TransactionCollectionView(Account account, ICurrentPeriodProvider periodProvider) : this(account, periodProvider, null) {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="account">The account that the view applies over.</param>
        /// <param name="periodProvider">An interface that allows the view to bind to a specific accounting period.</param>
        /// <param name="balanceType">The BalanceType that the view is limited to (can be null).</param>
        public TransactionCollectionView(Account account, ICurrentPeriodProvider periodProvider, BalanceType? balanceType) {
            this.AccountingPeriod = periodProvider.CurrentPeriod;
            this.TransactionBalanceType = balanceType;
            this.ParentAccount = account;
            this.AutoSort = true;

            periodProvider.CurrentPeriodChanged += new PeriodEventHandler(PeriodProvider_CurrentPeriodChanged);
        }

        /// <summary>
        /// Called to determine whether to include a given transaction in this view.
        /// </summary>
        /// <param name="item">The transaction to filter.</param>
        /// <returns>True if the transaction should be included in the view, otherwise false.</returns>
        protected override bool FilterItem(Transaction item) {
            bool result = true;

            result = (item.Date >= this.AccountingPeriod.StartDate && item.Date <= this.AccountingPeriod.EndDate);

            if (result == true) {
                if (this.TransactionBalanceType != null) {
                    BalanceType balanceType = (item.DebitAccount == this.ParentAccount) ? BalanceType.Debit : BalanceType.Credit;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the account that the view applies over.
        /// </summary>
        public Account ParentAccount {
            get { return _parentAccount; }
            protected set {
                _parentAccount = value;
                if (_parentAccount != null) {
                    this.ParentAccount.Transactions.CollectionChanged += new NotifyCollectionChangedEventHandler(UnderlyingTransactions_CollectionChanged);
                    Rebuild(this.ParentAccount.Workbook.FetchTransactions(this.ParentAccount));
                }
            }
        }

        private void UnderlyingTransactions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            base.ProcessChanges(e);
        }

        /// <summary>
        /// Gets the accounting period that the view is limited to.
        /// </summary>
        public Period AccountingPeriod {
            get { return _accountingPeriod; }
            protected set { 
                _accountingPeriod = value;
                if (this.ParentAccount != null) {
                    Rebuild(this.ParentAccount.Workbook.FetchTransactions(this.ParentAccount));
                }
            }
        }

        /// <summary>
        /// Gets the BalanceType that the view is limited to (can be null).
        /// </summary>
        public BalanceType? TransactionBalanceType {
            get { return _transactionBalanceType; }
            protected set { _transactionBalanceType = value; }
        }

        /// <summary>
        /// Gets the opening balance for this view.
        /// </summary>
        public Balance OpeningBalance {
            get {
                return this.ParentAccount.Transactions.GetBalance(this.AccountingPeriod.StartDate, this.ParentAccount);
            }
        }

        /// <summary>
        /// Gets the closing balance for this view.
        /// </summary>
        public Balance ClosingBalance {
            get {
                return this.ParentAccount.Transactions.GetBalance(this.AccountingPeriod.EndDate, this.ParentAccount);
            }
        }

        /// <summary>
        /// Raises the CollectionChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
            // Remind any listeners that the count, opening balance or closing balance may have changed
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("OpeningBalance"));
            OnPropertyChanged(new PropertyChangedEventArgs("ClosingBalance"));

            base.OnCollectionChanged(e);
        }

        /// <summary>
        /// Compares one <see cref="T:TransactionCollectionView"/> to another by comparing the accounts.
        /// </summary>
        /// <param name="obj">The <see cref="T:TransactionCollectionView"/> to compare against.</param>
        /// <returns>Returns a value indicating whether this <see cref="T:TransactionCollectionView"/> should be listed before or after the given 
        /// <see cref="T:TransactionCollectionView"/>.</returns>
        public int CompareTo(object obj) {
            int result = 0;

            TransactionCollectionView rhs = obj as TransactionCollectionView;
            if (rhs != null) {
                result = this.ParentAccount.CompareTo(rhs.ParentAccount);
            }

            return result;
        }

        /// <summary>
        /// Called when the CurrentPeriod of an ICurrentPeriodProvider passed in the constructor changes.
        /// </summary>
        private void PeriodProvider_CurrentPeriodChanged(object sender, PeriodEventArgs e) {
            this.AccountingPeriod = e.Period;
        }

        public override void ItemPropertyChanged(Transaction item, string propertyName) {
            if (propertyName == "Date") {
                base.ItemPropertyChanged(item, propertyName);
            }
        }
    }
}
