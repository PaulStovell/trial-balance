using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Specialized;
using PaulStovell.Common.BindingFramework;
using PaulStovell.Common;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// An abstract class that represents an account. There are five concrete account types - 
    /// Assets, Liabilities, Expenses, Revenue and Equity.
    /// </summary>
    public abstract class Account : AccountingDomainObject, IComparable {
        private Guid _accountID;
        private string _name;
        private string _description;
        private AccountType _accountType;
        private BalanceType _defaultBalanceType;
        private TransactionCollection _transactions;
        private object _transactionLock = new object();

        /// <summary>
        /// Internal constructor.
        /// </summary>
        protected Account(AccountType accountType, BalanceType defaultBalanceType)
            : base() {
            this.AccountID = Guid.NewGuid();
            this.AccountType = accountType;
            this.DefaultBalanceType = defaultBalanceType;
        }

        /// <summary>
        /// Raised by the PropertyChanged event when the AccountID property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs AccountIDPropertyChangedEventArgs = new PropertyChangedEventArgs("AccountID");

        /// <summary>
        /// Raised by the PropertyChanged event when the Name property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs NamePropertyChangedEventArgs = new PropertyChangedEventArgs("Name");

        /// <summary>
        /// Raised by the PropertyChanged event when the Description property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs DescriptionPropertyChangedEventArgs = new PropertyChangedEventArgs("Description");

        /// <summary>
        /// Gets the account ID for this account.
        /// </summary>
        public Guid AccountID {
            get { return _accountID; }
            internal set {
                this.AssertCanEdit();
                _accountID = value;
                NotifyChanged(AccountIDPropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets or sets the name of the account. Names are required and cannot be more than 50 characters in length.
        /// </summary>
        [Auditable("Account Name")]
        public string Name {
            get { return CleanString(_name); }
            set {
                this.AssertCanEdit();
                _name = CleanString(value);
                NotifyChanged(NamePropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets or sets a description of this account.
        /// </summary>
        [Auditable("Description")]
        public string Description {
            get { return CleanString(_description); }
            set {
                this.AssertCanEdit();
                _description = CleanString(value);
                NotifyChanged(DescriptionPropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets the default balance type for this account.
        /// </summary>
        public BalanceType DefaultBalanceType {
            get { return _defaultBalanceType; }
            protected set { _defaultBalanceType = value; }
        }

        /// <summary>
        /// Gets the type of account that this account represents.
        /// </summary>
        public AccountType AccountType {
            get { return _accountType; }
            protected set { _accountType = value; }
        }

        /// <summary>
        /// Gets a collection of transactions for this account.
        /// </summary>
        public TransactionCollection Transactions {
            get {
                if (_transactions == null) {
                    lock (_transactionLock) {
                        if (_transactions == null) {
                            _transactions = new TransactionCollection();
                            _transactions.AddRange(Workbook.FetchTransactions(this));
                        }
                    }
                }
                return _transactions;
            }
        }

        /// <summary>
        /// Gets a unique string representation of this account.
        /// </summary>
        /// <returns>A unique string representation of this asset.</returns>
        public override string ToString() {
            return this.GetType().Name + ": " + this.Name;
        }

        /// <summary>
        /// Override this method to add extra validation rules to this account.
        /// </summary>
        protected override List<Rule> CreateRules() {
            List<Rule> rules = base.CreateRules();
            rules.Add(new SimpleRule("AccountID", "The Account ID cannot be an empty GUID.", delegate { return this.AccountID != Guid.Empty; }));
            rules.Add(new SimpleRule("Name", "An account name is required.", delegate { return this.Name.Length != 0; }));
            rules.Add(new SimpleRule("Name", "Account names cannot be more than 50 characters in length.", delegate { return this.Name.Length <= 50; }));
            rules.Add(new UniqueAccountNameRule(this.Workbook, this));
            return rules;
        }

        /// <summary>
        /// Copies the properties of this object to those of a new object.
        /// </summary>
        /// <param name="targetObject">The object to copy the properties to.</param>
        public override void CopyTo(object targetObject) {
            base.CopyTo(targetObject);

            Account account = targetObject as Account;
            if (account != null) {
                account._transactions = this.Transactions;
                account.Name = this.Name;
                account.Description = this.Description;
                account._defaultBalanceType = this.DefaultBalanceType;
                account.AccountType = this.AccountType;
                account.AccountID = this.AccountID;
            }
        }

        /// <summary>
        /// Compares a given account with the current account.
        /// </summary>
        /// <param name="obj">The account to compare with.</param>
        /// <returns></returns>
        public int CompareTo(object obj) {
            int result = 0;

            Account rhs = obj as Account;
            if (rhs != null) {
                if (result == 0) {
                    result = this.Name.CompareTo(rhs.Name);
                }
            }

            return result;
        }
    }
}