using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    public abstract class CollectionView<T> : IEnumerable<T>, INotifyCollectionChanged, INotifyPropertyChanged {
        private Predicate<T> _filterPredicate;
        private List<T> _innerList;
        private object _innerListLock = new object();

        public CollectionView() {

        }

        public CollectionView(Transaction t) {
            
        }

        /// <summary>
        /// Gets the inner list of items that are visible in this filter.
        /// </summary>
        protected List<Transaction> InnerList {
            get {
                if (_innerList == null) {
                    lock (_innerListLock) {
                        if (_innerList == null) {
                            _innerList = new List<Transaction>();
                        }
                    }
                }
                return _innerList;
            }
        }

        /// <summary>
        /// Gets the number of items in this view.
        /// </summary>
        public int Count {
            get { return this.InnerList.Count; }
        }

        protected abstract bool Filter(T item);

        /// <summary>
        /// Copies transactions from the underlying parent account's transactions to this collection.
        /// </summary>
        protected void SynchroniseCollections() {
            List<Transaction> transactionsToAdd = new List<Transaction>();
            foreach (Transaction t in this.ParentAccount.Transactions) {
                if (_filterPredicate(t)) {
                    transactionsToAdd.Add(t);
                }
            }

            this.InnerList.AddRange(transactionsToAdd);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, transactionsToAdd));

            this.SortView();
        }

        /// <summary>
        /// Sorts the items in this view.
        /// </summary>
        protected void SortView() {
            this.InnerList.Sort();
        }

        /// <summary>
        /// Called when the CollectionChanged event of the underlying transaction collection changes.
        /// </summary>
        private void UnderlyingTransactions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            List<NotifyCollectionChangedEventArgs> bubbleArgs = new List<NotifyCollectionChangedEventArgs>();

            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    // Items were added
                    List<Transaction> transactionsToAdd = new List<Transaction>();
                    foreach (Transaction t in e.NewItems) {
                        if (_filterPredicate(t)) {
                            transactionsToAdd.Add(t);
                            bubbleArgs.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, t));
                        }
                    }

                    this.InnerList.AddRange(transactionsToAdd);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Transaction t in e.OldItems) {
                        if (this.InnerList.Contains(t)) {
                            this.InnerList.Remove(t);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (Transaction t in e.OldItems) {
                        if (this.InnerList.Contains(t) && !_filterPredicate(t)) {
                            this.InnerList.Remove(t);
                        }
                    }
                    foreach (Transaction t in e.NewItems) {
                        if (!this.InnerList.Contains(t) && _filterPredicate(t)) {
                            this.InnerList.Add(t);
                        }
                    }
                    bubbleArgs.Add(e);
                    break;

                default:
                    Debug.Fail("Collection changed action of " + e.Action.ToString() + " was not expected.");
                    break;
            }

            this.SortView();
            foreach (NotifyCollectionChangedEventArgs bubbleArg in bubbleArgs) {
                this.OnCollectionChanged(bubbleArg);
            }
        }

        /// <summary>
        /// Occurs when the items in this view change.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when a property on this collection view changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets an enumerator for the items in this view.
        /// </summary>
        /// <returns>An enumerator for the items in the view.</returns>
        public IEnumerator<Transaction> GetEnumerator() {
            return InnerList.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the items in this view.
        /// </summary>
        /// <returns>An enumerator for the items in the view.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return InnerList.GetEnumerator();
        }

        /// <summary>
        /// Gets the index of a given transaction in this view.
        /// </summary>
        /// <param name="item">The item to get the index for.</param>
        /// <returns>The index of the given item, or -1 if the item does not exist in the collection.</returns>
        public int IndexOf(Transaction item) {
            return this.InnerList.IndexOf(item);
        }

        /// <summary>
        /// Gets the transaction at a given index in this view.
        /// </summary>
        /// <param name="index">The index to get the transaction at.</param>
        /// <returns>The transaction at the given index.</returns>
        public Transaction this[int index] {
            get {
                return this.InnerList[index];
            }
        }

        /// <summary>
        /// Gets a value indicating whether the view contains a given transaction.
        /// </summary>
        /// <param name="item">The transaction to look for.</param>
        /// <returns>True if the view contains the given transaction, otherwise false.</returns>
        public bool Contains(Transaction item) {
            return this.InnerList.Contains(item);
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
            // Remind any listeners that the count, opening balance or closing balance may have changed
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("OpeningBalance"));
            OnPropertyChanged(new PropertyChangedEventArgs("ClosingBalance"));

            if (this.CollectionChanged != null) {
                this.CollectionChanged(this, e);
            }
        }
    }
}
