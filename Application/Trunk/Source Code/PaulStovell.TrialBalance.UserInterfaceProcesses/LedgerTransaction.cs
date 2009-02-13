using System;
using System.Collections.Generic;
using System.Text;
using PaulStovell.TrialBalance.DomainModel;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.UserInterfaceProcesses {
    public sealed class LedgerTransaction : INotifyPropertyChanged, IEditableObject {
        private ChangeCoordinator<Transaction> _changeCoordinator;
        private Transaction _wrappedTransaction;
        private Account _ownerAccount;
        private Workbook _workbook;

        public LedgerTransaction(Transaction wrappedTransaction, Account ownerAccount, Workbook workbook) {
            _wrappedTransaction = wrappedTransaction;
            _ownerAccount = ownerAccount;
            _workbook = workbook;
            _wrappedTransaction.PropertyChanged += new PropertyChangedEventHandler(WrappedTransaction_PropertyChanged);
        }

        public LedgerTransaction(Transaction wrappedTransaction, ChangeCoordinator<Transaction> changeCoordinator, Account ownerAccount, Workbook workbook) {
            _wrappedTransaction = wrappedTransaction;
            _ownerAccount = ownerAccount;
            _workbook = workbook;
            _changeCoordinator = changeCoordinator;
            _wrappedTransaction.PropertyChanged += new PropertyChangedEventHandler(WrappedTransaction_PropertyChanged);
            _changeCoordinator.EditableItem.PropertyChanged += new PropertyChangedEventHandler(WrappedTransaction_PropertyChanged);
        }

        public event LedgerTransactionSavingEventHandler Saving;

        public DateTime Date {
            get { return GetVisibleTransaction().Date.Date; }
            set {
                GetVisibleTransaction().Date = value.Date;
            }
        }

        public string Particulars {
            get { return GetVisibleTransaction().Particulars ?? string.Empty; }
            set {
                GetVisibleTransaction().Particulars = value ?? string.Empty;
            }
        }

        public Account OwnerAccount {
            get { return _ownerAccount; }
        }

        public Account AffectedAccount {
            get {
                if (GetVisibleTransaction().DebitAccount == this.OwnerAccount) {
                    return GetVisibleTransaction().CreditAccount;
                } else {
                    return GetVisibleTransaction().DebitAccount; 
                }
            }
            set {
                if (GetVisibleTransaction().DebitAccount == this.OwnerAccount) {
                    GetVisibleTransaction().CreditAccount = value;
                } else {
                    GetVisibleTransaction().DebitAccount = value;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("AffectedAccount"));
            }
        }

        public Balance CreditBalance {
            get { return GetVisibleTransaction().GetBalance(this.OwnerAccount); }
            set {
                value = value ?? new Balance();
                GetVisibleTransaction().Value = value.Magnitude;
                Account affected = this.AffectedAccount;
                if (value.BalanceType == BalanceType.Credit) {
                    GetVisibleTransaction().CreditAccount = _ownerAccount;
                    GetVisibleTransaction().DebitAccount = affected;
                } else {
                    GetVisibleTransaction().DebitAccount = _ownerAccount;
                    GetVisibleTransaction().CreditAccount = affected;
                }
                this.OnPropertyChanged(new PropertyChangedEventArgs("DebitBalance"));
                this.OnPropertyChanged(new PropertyChangedEventArgs("CreditBalance"));
            }
        }

        public Balance DebitBalance {
            get { return this.CreditBalance; }
            set { this.CreditBalance = value; }
        }

        private Transaction GetVisibleTransaction() {
            if (_changeCoordinator == null) {
                return _wrappedTransaction;
            } else {
                return _changeCoordinator.EditableItem;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e) {
            if (PropertyChanged != null) {
                PropertyChanged(this, e);
            }
        }

        public void BeginEdit() {
            if (_changeCoordinator == null) {
                _changeCoordinator = _workbook.AcquireChangeCoordinator(_wrappedTransaction);
                _changeCoordinator.EditableItem.PropertyChanged += new PropertyChangedEventHandler(WrappedTransaction_PropertyChanged);
            }
        }

        public void CancelEdit() {
            _changeCoordinator.EditableItem.PropertyChanged -= new PropertyChangedEventHandler(WrappedTransaction_PropertyChanged);
            _changeCoordinator = null;
            this.OnPropertyChanged(new PropertyChangedEventArgs("Date"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Particulars"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("OwnerAccount"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("AffectedAccount"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("CreditBalance"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("DebitBalance"));
        }

        public void EndEdit() {
            if (_changeCoordinator != null) {
                if (_changeCoordinator.EditableItem.IsDirty) {
                    LedgerTransactionSavingEventArgs e = new LedgerTransactionSavingEventArgs(this);
                    e.Cancel = false;
                    OnSaving(e);
                    if (e.Cancel == false) {
                        _changeCoordinator.EditableItem.PropertyChanged -= new PropertyChangedEventHandler(WrappedTransaction_PropertyChanged);
                        _changeCoordinator.PushChanges();
                        _changeCoordinator = null;
                    } else {
                        CancelEdit();
                        return;
                    }
                }
                _changeCoordinator = null;
            }
        }

        private void WrappedTransaction_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            this.OnPropertyChanged(e);
        }

        private void OnSaving(LedgerTransactionSavingEventArgs e) {
            if (this.Saving != null) {
                this.Saving(this, e);
            }
        }
    }
}
