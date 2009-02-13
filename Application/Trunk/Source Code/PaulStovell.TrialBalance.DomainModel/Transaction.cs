using System;
using System.Collections.Generic;
using PaulStovell.Common;
using PaulStovell.Common.BindingFramework;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents a single transaction against two accounts.
    /// </summary>
    public class Transaction : AccountingDomainObject, IComparable<Transaction> {
        private Guid _transactionID;
        private DateTime _date;
        private string _particulars;
        private Account _debitAccount;
        private Account _creditAccount;
        private decimal _value;

        /// <summary>
        /// Constructor.
        /// </summary>
        internal Transaction() {
            this.TransactionID = Guid.NewGuid();
            this.Date = DateTime.Now;
        }

        /// <summary>
        /// Raised by the PropertyChanged event when the TransactionID property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs TransactionIDPropertyChangedEventArgs = new PropertyChangedEventArgs("TransactionID");

        /// <summary>
        /// Raised by the PropertyChanged event when the Date property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs DatePropertyChangedEventArgs = new PropertyChangedEventArgs("Date");

        /// <summary>
        /// Raised by the PropertyChanged event when the Particulars property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs ParticularsPropertyChangedEventArgs = new PropertyChangedEventArgs("Particulars");

        /// <summary>
        /// Raised by the PropertyChanged event when the CreditAccount property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs CreditAccountPropertyChangedEventArgs = new PropertyChangedEventArgs("CreditAccount");

        /// <summary>
        /// Raised by the PropertyChanged event when the DebitAccount property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs DebitAccountPropertyChangedEventArgs = new PropertyChangedEventArgs("DebitAccount");

        /// <summary>
        /// Raised by the PropertyChanged event when the Value property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs ValuePropertyChangedEventArgs = new PropertyChangedEventArgs("Value");

        /// <summary>
        /// Gets a unique identifier for this transaction.
        /// </summary>
        public Guid TransactionID {
            get { return _transactionID; }
            internal set {
                if (_transactionID != value) {
                    _transactionID = value;
                    NotifyChanged(TransactionIDPropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Gets or sets the date the transaction took place in.
        /// </summary>
        [Auditable("Transaction Date")]
        public DateTime Date {
            get { return _date.Date; }
            set {
                if (_date != value) {
                    _date = value.Date;
                    NotifyChanged(DatePropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Gets or sets some details about this transaction. This is usually a comment about what the transaction was for.
        /// </summary>
        [Auditable("Particulars")]
        public string Particulars {
            get { return (_particulars ?? string.Empty).Trim(); }
            set {
                if (_particulars != value) {
                    _particulars = (value ?? string.Empty).Trim();
                    NotifyChanged(ParticularsPropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Gets or sets the account that will be debited by this transaction.
        /// </summary>
        public Account DebitAccount {
            get { return _debitAccount; }
            set {
                _debitAccount = value;
                NotifyChanged(DebitAccountPropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets or sets the account that will be credited by this transaction.
        /// </summary>
        public Account CreditAccount {
            get { return _creditAccount; }
            set {
                _creditAccount = value;
                NotifyChanged(CreditAccountPropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets or sets the value of the transaction.
        /// </summary>
        [Auditable("Transaction Value")]
        public decimal Value {
            get { return _value; }
            set {
                _value = value;
                NotifyChanged(ValuePropertyChangedEventArgs);
            }
        }

        public BalanceType GetBalanceType(Account parentAccount) {
            if (parentAccount == this.DebitAccount) {
                return BalanceType.Debit;
            } else if (parentAccount == this.CreditAccount) {
                return BalanceType.Credit;
            } else {
                throw new ArgumentException(string.Format("Account {0} is not the credit account nor the debit account for this transaction.", parentAccount.ToString()));
            }
        }

        /// <summary>
        /// Override this method to add rules to this transaction.
        /// </summary>
        protected override List<Rule> CreateRules() {
            List<Rule> rules = base.CreateRules();
            rules.Add(new SimpleRule("TransactionID", "The Transaction ID cannot be an empty GUID.", delegate { return this.TransactionID != Guid.Empty; }));
            rules.Add(new SimpleRule("Particulars", "Some particulars describing this transaction are required.", delegate { return this.Particulars.Length > 0; }));
            rules.Add(new SimpleRule("DebitAccount", "The debit account for this transaction is required.", delegate { return this.DebitAccount != null; }));
            rules.Add(new SimpleRule("CreditAccount", "The credit account for this transaction is required.", delegate { return this.CreditAccount != null; }));
            rules.Add(new SimpleRule("Value", "The value of a transaction can not be zero.", delegate { return this.Value > 0; }));
            rules.Add(new SimpleRule("Date", "A valid date must be supplied for this transaction.", delegate { return this.Date != DateTime.MinValue.Date; }));
            return rules;
        }

        /// <summary>
        /// Compares this transaction to another given transaction to determine the sort order.
        /// </summary>
        /// <param name="other">The transaction to compare this transaction to.</param>
        public int CompareTo(Transaction other) {
            int result = this.Date.CompareTo(other.Date);
            if (result == 0) {
                result = this.CreatedDate.CompareTo(other.CreatedDate);
                if (result == 0) {
                    result = this.Particulars.CompareTo(other.Particulars);
                    if (result == 0) {
                        result = this.GetHashCode().CompareTo(other.GetHashCode());
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Copies the values of a given transaction to this transaction.
        /// </summary>
        /// <param name="targetObject">The source transaction.</param>
        public override void CopyTo(object targetObject) {
            base.CopyTo(targetObject);

            Transaction transaction = targetObject as Transaction;
            if (transaction != null) {
                transaction.DebitAccount = this.DebitAccount;
                transaction.CreditAccount = this.CreditAccount;
                transaction.Date = this.Date;
                transaction.Particulars = this.Particulars;
                transaction._transactionID = this.TransactionID;
                transaction.Value = this.Value;
            }
        }

        public Balance GetBalance(Account account) {
            BalanceType type = this.GetBalanceType(account);
            return new Balance(this.Value, type);
        }
    }
}