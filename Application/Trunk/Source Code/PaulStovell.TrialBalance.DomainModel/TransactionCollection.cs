using System;
using System.Collections.Generic;
using System.Text;
using PaulStovell.Common;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents a collection of Transactions.
    /// </summary>
    public class TransactionCollection : IEnumerable<Transaction>, INotifyCollectionChanged, INotifyPropertyChanged {
        private List<Transaction> _innerList;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TransactionCollection() {

        }

        /// <summary>
        /// Occurs when a property on the collection changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when a this collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets the inner collection of transactions.
        /// </summary>
        protected List<Transaction> InnerList {
            get {
                if (_innerList == null) {
                    _innerList = new List<Transaction>();
                }
                return _innerList;
            }
        }

        /// <summary>
        /// Gets the balance of all transactions in the view at a given date.
        /// </summary>
        /// <param name="date">The date to get all transactions up to.</param>
        /// <param name="account">The account to get the balance for.</param>
        /// <returns></returns>
        public Balance GetBalance(DateTime date, Account account) {
            Balance result = new Balance(0, account.DefaultBalanceType);

            foreach (Transaction t in this.InnerList) {
                if (t.Date <= date.Date) {
                    result += t.GetBalance(account);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the index of a given item.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>The index of the item if found, otherwise -1.</returns>
        public int IndexOf(Transaction item) {
            return this.InnerList.IndexOf(item);
        }

        /// <summary>
        /// Gets the transaction at the given index.
        /// </summary>
        /// <param name="index">The index to get the item at.</param>
        /// <returns>The transaction at the given index.</returns>
        public Transaction this[int index] {
            get {
                return this.InnerList[index];
            }
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="raiseEvent">Whether or not to raise a CollectionChanged event.</param>
        internal void InnerAdd(Transaction item, bool raiseEvent) {
            if (!this.InnerList.Contains(item)) {
                this.InnerList.Add(item);

                this.InnerList.Sort();

                if (raiseEvent) {
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                }
            }
        }

        /// <summary>
        /// Adds an item to this collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        internal void Add(Transaction item) {
            this.InnerAdd(item, true);
        }


        /// <summary>
        /// Adds a range of transactions to this collection.
        /// </summary>
        /// <param name="transactions">An enumerable list of transactions to add.</param>
        internal void AddRange(IEnumerable<Transaction> transactions) {
            if (transactions != null) {
                foreach (Transaction t in transactions) {
                    this.InnerAdd(t, false);
                }
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, transactions));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains a given transaction.
        /// </summary>
        /// <param name="item">The transaction to look for.</param>
        /// <returns>True if the collection contains the given transaction, otherwise false.</returns>
        public bool Contains(Transaction item) {
            return this.InnerList.Contains(item);
        }

        /// <summary>
        /// Gets the count of all items in this collection.
        /// </summary>
        public int Count {
            get { return this.InnerList.Count; }
        }

        /// <summary>
        /// Removes a given transaction from the collection.
        /// </summary>
        /// <param name="item">The transaction to attempt to remove.</param>
        /// <returns>True if the item was removed, or false if it was not found.</returns>
        internal bool Remove(Transaction item) {
            bool result = this.InnerList.Remove(item);

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));

            return result;
        }

        /// <summary>
        /// Gets an enumerator for this collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<Transaction> GetEnumerator() {
            return this.InnerList.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for this collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the CollectionChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
            if (this.CollectionChanged != null) {
                this.CollectionChanged(this, e);
            }
        }
    }
}
