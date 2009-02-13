using System;
using System.Collections.Generic;
using System.Text;
using PaulStovell.TrialBalance.DomainModel;
using PaulStovell.Common;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.UserInterfaceProcesses {
    public sealed class LedgerTransactionTransformedCollection : TransformedCollection<Transaction, LedgerTransaction> {
        private TransactionCollectionView _transactions;
        private Workbook _workbook;

        public LedgerTransactionTransformedCollection(TransactionCollectionView transactions, Workbook workbook) {
            _transactions = transactions;
            _workbook = workbook;
            _transactions.CollectionChanged += new NotifyCollectionChangedEventHandler(Transactions_CollectionChanged);

            this.Rebuild(transactions);
        }

        public event LedgerTransactionSavingEventHandler LedgerTransactionSaving;

        private void Transactions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            ProcessChanges(e);
        }

        public override void Add(LedgerTransaction item) {

        }

        public override void Clear() {
            throw new NotSupportedException();
        }

        public override bool Remove(LedgerTransaction item) {
            return true;
        }

        protected override LedgerTransaction InnerWrapItem(Transaction sourceItem) {
            LedgerTransaction result = new LedgerTransaction(sourceItem, _transactions.ParentAccount, _workbook);
            result.Saving += new LedgerTransactionSavingEventHandler(Item_Saving);
            return result;
        }

        private void Item_Saving(object sender, LedgerTransactionSavingEventArgs e) {
            this.OnLedgerTransactionSaving(this, e);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
            base.OnCollectionChanged(e);
        }

        private void OnLedgerTransactionSaving(object sender, LedgerTransactionSavingEventArgs e) {
            if (this.LedgerTransactionSaving != null) {
                this.LedgerTransactionSaving(this, e);
            }
        }
    }
}
