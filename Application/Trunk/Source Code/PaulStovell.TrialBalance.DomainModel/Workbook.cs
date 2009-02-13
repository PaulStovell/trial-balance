using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using PaulStovell.Common.BindingFramework;
using PaulStovell.Common;
using System.Globalization;
using System.Threading;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// The Workbook is the central object of the application, and is anagulous to the "Spreadsheet" in Excel. The Workbook contains 
    /// all accounts and transactions that the user has recorded for a given business.
    /// </summary>
    public class Workbook : DomainObject, ICurrentPeriodProvider {
        private Dictionary<Guid, Account> _loadedAccounts;
        private Dictionary<Guid, Transaction> _loadedTransactions;
        private DataProvider _dataProvider;
        private string _businessName;
        private string _legalName;
        private string _userName;
        private PeriodLength? _periodLength;
        private DateTime _periodStartDate;
        private Period _currentPeriod;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Workbook(DataProvider provider) {
            this.DataProvider = provider;
        }

        /// <summary>
        /// Raised by the PropertyChanged event when the CurrentPeriod property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs CurrentPeriodPropertyChangedEventArgs = new PropertyChangedEventArgs("CurrentPeriod");

        /// <summary>
        /// Raised by the PropertyChanged event when the PeriodLength property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs PeriodLengthPropertyChangedEventArgs = new PropertyChangedEventArgs("PeriodLength");

        /// <summary>
        /// Raised by the PropertyChanged event when the PeriodStartDate property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs PeriodStartDatePropertyChangedEventArgs = new PropertyChangedEventArgs("PeriodStartDate");

        /// <summary>
        /// Raised by the PropertyChanged event when the BusinessName property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs BusinessNamePropertyChangedEventArgs = new PropertyChangedEventArgs("BusinessName");
    
        /// <summary>
        /// Raised by the PropertyChanged event when the LegalName property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs LegalNamePropertyChangedEventArgs = new PropertyChangedEventArgs("LegalName");

        /// <summary>
        /// Raised by the PropertyChanged event when the UserName property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs UserNamePropertyChangedEventArgs = new PropertyChangedEventArgs("UserName");

        /// <summary>
        /// Occurs when the CurrentPeriod of this workbook changes.
        /// </summary>
        public event PeriodEventHandler CurrentPeriodChanged;

        /// <summary>
        /// Gets or sets the current accounting period.
        /// </summary>
        public Period CurrentPeriod {
            get {
                if (_currentPeriod == null) {
                    _currentPeriod = Period.CalculatePeriodForDate(DateTime.Now, this.PeriodStartDate, this.PeriodLength);
                }
                return _currentPeriod;
            }
            set {
                if (_currentPeriod != value) {
                    _currentPeriod = value;
                    NotifyChanged(CurrentPeriodPropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Gets or sets the business/organisaion name of the entity that this workbook maintains accounts for.
        /// </summary>
        public string BusinessName {
            get {
                if (_businessName == null) {
                    _businessName = DataProvider.FetchSetting("BusinessName");
                    if (_businessName == string.Empty) {
                        this.BusinessName = "My New Business";
                    }
                }
                return _businessName ?? string.Empty;
            }
            set {
                if (_businessName != value) {
                    _businessName = value;
                    DataProvider.PersistSetting("BusinessName", _businessName);
                    NotifyChanged(BusinessNamePropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Gets or sets the legal name of the entity that this workbook maintains accounts for.
        /// </summary>
        public string LegalName {
            get {
                if (_legalName == null) {
                    _legalName = DataProvider.FetchSetting("LegalName");
                    if (_legalName == string.Empty) {
                        this.LegalName = "My New Business";
                    }
                }
                return _legalName ?? string.Empty;
            }
            set {
                if (_legalName != value) {
                    _legalName = value;
                    DataProvider.PersistSetting("LegalName", _legalName);
                    NotifyChanged(LegalNamePropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the user who maintains this workbook.
        /// </summary>
        public string UserName {
            get {
                if (_userName == null) {
                    _userName = DataProvider.FetchSetting("UserName");
                    if (_userName == string.Empty) {
                        this.UserName = Environment.UserName;
                    }
                }
                return _userName ?? string.Empty;
            }
            set {
                if (_userName != value) {
                    _userName = value;
                    DataProvider.PersistSetting("UserName", _userName);
                    NotifyChanged(UserNamePropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Gets or sets the length of accounting periods for this workbook.
        /// </summary>
        public PeriodLength PeriodLength {
            get {
                if (_periodLength == null) {
                    string setting = DataProvider.FetchSetting("PeriodLength");
                    if (setting != string.Empty) {
                        _periodLength = (PeriodLength)Enum.Parse(typeof(PeriodLength), DataProvider.FetchSetting("PeriodLength"));
                    } else {
                        _periodLength = PeriodLength.Yearly;
                    }
                }
                return _periodLength.Value;
            }
            set {
                if (_periodLength != value) {
                    _periodLength = value;
                    DataProvider.PersistSetting("PeriodLength", _periodLength.Value.ToString());
                    NotifyChanged(PeriodLengthPropertyChangedEventArgs);
                    NotifyChanged(CurrentPeriodPropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Gets or sets the first day of a given accounting period, that is then used along with the PeriodLength 
        /// when calculating future or previous accounting periods. 
        /// </summary>
        public DateTime PeriodStartDate {
            get {
                if (_periodStartDate == DateTime.MinValue) {
                    string setting = DataProvider.FetchSetting("PeriodStartDate");
                    if (setting != string.Empty) {
                        _periodStartDate = DateTime.Parse(setting);
                    } else {
                        return new DateTime(2006, 1, 1);
                    }
                }
                return _periodStartDate.Date;
            }
            set {
                if (_periodStartDate != value) {
                    _periodStartDate = value;
                    DataProvider.PersistSetting("PeriodStartDate", _periodStartDate.ToString("yyyy-MM-dd"));
                    NotifyChanged(PeriodStartDatePropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Gets the list of in-memory Accounts.
        /// </summary>
        protected Dictionary<Guid, Account> LoadedAccounts {
            get {
                if (_loadedAccounts == null) {
                    _loadedAccounts = new Dictionary<Guid, Account>();
                }
                return _loadedAccounts;
            }
        }

        /// <summary>
        /// Gets the list of in-memory Transactions.
        /// </summary>
        protected Dictionary<Guid, Transaction> LoadedTransactions {
            get {
                if (_loadedTransactions == null) {
                    _loadedTransactions = new Dictionary<Guid, Transaction>();
                }
                return _loadedTransactions;
            }
        }

        /// <summary>
        /// Gets or sets the DataProvider used by the object workbook.
        /// </summary>
        public DataProvider DataProvider {
            get { return _dataProvider; }
            set {
                _dataProvider = value;
                Reset();
            }
        }

        /// <summary>
        /// Occurs when an account is saved.
        /// </summary>
        public event AccountEventHandler AccountSaved;

        /// <summary>
        /// Clears the workbook.
        /// </summary>
        public void Reset() {
            this.LoadedAccounts.Clear();

            // Set the current thread CultureInfo
            string culture = this.DataProvider.FetchSetting("Culture");
            if (culture == null || culture.Length == 0) {
                culture = "en-AU";
            }
            CultureInfo cultureInfo = null;
            try {
                cultureInfo = CultureInfo.CreateSpecificCulture(culture);
            } catch (Exception ex) {
                Log.Default.WriteError(ex);
            }
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private ChangeCoordinator<T> CreateAccount<T>(T newItem, bool allowAsyncLoading) where T : Account {
            Guid id = Guid.NewGuid();
            newItem.AccountID = id;
            Remember(id, newItem);
            newItem.IsDirty = false;

            return this.CreateChangeCoordinator(null, newItem);
        }

        private BindableCollection<T> FetchAccountsByIDs<T>(IList<Guid> accountIDs) where T : Account {
            BindableCollection<T> results = new BindableCollection<T>();
            foreach (Guid accountID in accountIDs) {
                Account a = FetchAccount(accountID);
                if (a is T) {
                    results.Add((T)a);
                    Remember(accountID, a);
                }
            }
            return results;
        }

        private void Remember(Guid id, AccountingDomainObject o) {
            o.Workbook = this;
            if (o is Account) {
                this.LoadedAccounts[id] = (Account)o;
            } else if (o is Transaction) {
                this.LoadedTransactions[id] = (Transaction)o;
            }
        }

        /// <summary>
        /// Loads a given account from the data provider by the ID of the account.
        /// </summary>
        /// <param name="accountID">The ID of the account to load.</param>
        /// <returns>The loaded account, or null if no matching account was found.</returns>
        public Account FetchAccount(Guid accountID) {
            Account result = null;
            if (this.LoadedAccounts.ContainsKey(accountID)) {
                result = this.LoadedAccounts[accountID];
            } else {
                result = this.DataProvider.FetchAccount(accountID);
                Remember(accountID, result);
            }
            result.IsReadOnly = true;

            return result;
        }

        /// <summary>
        /// Fetches an account from the data provider given the name of the account. At this point in time this method is not optimised.
        /// </summary>
        /// <param name="accountName">The name of the account to find.</param>
        /// <returns>The loaded account, or null if the account could not be found.</returns>
        /// <remarks>Todo: Consider optimising this method.</remarks>
        public Account FetchAccountByAccountName(string accountName) {
            Account result = null;
            accountName = (accountName ?? string.Empty).Trim();

            foreach (Account a in this.FetchAccounts()) {
                if (a.Name.ToLower() == accountName.ToLower()) {
                    result = a;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Loads an asset account from the data provider using the given ID of the asset.
        /// </summary>
        /// <param name="assetID">The ID of the asset to load.</param>
        /// <returns>The loaded asset, or null if no matching account could be found.</returns>
        public Asset FetchAsset(Guid assetID) {
            return FetchAccount(assetID) as Asset;
        }

        /// <summary>
        /// Loads a liability account from the data provider using the given ID of the liability.
        /// </summary>
        /// <param name="liabilityID">The ID of the liability to load.</param>
        /// <returns>The loaded liability, or null if no matching account could be found.</returns>
        public Liability FetchLiability(Guid liabilityID) {
            return FetchAccount(liabilityID) as Liability;
        }

        /// <summary>
        /// Loads a revenue account from the data provider using the given ID of the revenue.
        /// </summary>
        /// <param name="revenueID">The ID of the revenue to load.</param>
        /// <returns>The loaded revenue, or null if no matching account could be found.</returns>
        public Revenue FetchRevenue(Guid revenueID) {
            return FetchAccount(revenueID) as Revenue;
        }

        /// <summary>
        /// Loads an expense account from the data provider using the given ID of the exepense.
        /// </summary>
        /// <param name="expenseID">The ID of the expense to load.</param>
        /// <returns>The loaded expense, or null if no matching account could be found.</returns>
        public Expense FetchExpense(Guid expenseID) {
            return FetchAccount(expenseID) as Expense;
        }

        /// <summary>
        /// Loads an equity account from the data provider using the given ID of the equity account.
        /// </summary>
        /// <param name="equityID">The ID of the equity account to load.</param>
        /// <returns>The loaded equity, or null if the no matching account could be found.</returns>
        public Equity FetchEquity(Guid equityID) {
            return FetchAccount(equityID) as Equity;
        }

        /// <summary>
        /// Loads all known accounts from the data store.
        /// </summary>
        /// <returns>A BindableCollection of Accounts containing the loaded accounts.</returns>
        public BindableCollection<Account> FetchAccounts() {
            return FetchAccountsByIDs<Account>(this.DataProvider.FetchAccountIDs());
        }

        /// <summary>
        /// Loads all known accounts from the data store of a given account type.
        /// </summary>
        /// <param name="accountType">The type of account to look for.</param>
        /// <returns>A BindableCollection of Accounts containing the loaded accounts.</returns>
        public BindableCollection<Account> FetchAccounts(AccountType accountType) {
            IList<Guid> ids = null;

            switch (accountType) {
                case AccountType.Asset:
                    ids = this.DataProvider.FetchAssetIDs();
                    break;
                case AccountType.Liability:
                    ids = this.DataProvider.FetchLiabilityIDs();
                    break;
                case AccountType.Equity:
                    ids = this.DataProvider.FetchEquityIDs();
                    break;
                case AccountType.Revenue:
                    ids = this.DataProvider.FetchRevenueIDs();
                    break;
                case AccountType.Expense:
                    ids = this.DataProvider.FetchExpenseIDs();
                    break;
                default:
                    Debug.Fail("Unrecognised account type.");
                    break;
            }

            return FetchAccountsByIDs<Account>(ids);
        }

        /// <summary>
        /// Loads all known Assets from the data store.
        /// </summary>
        /// <returns>A BindableCollection of Assets containing the loaded accounts.</returns>
        public BindableCollection<Asset> FetchAssets() {
            return FetchAccountsByIDs<Asset>(this.DataProvider.FetchAssetIDs());
        }

        /// <summary>
        /// Loads all known Liabilities from the data store.
        /// </summary>
        /// <returns>A BindableCollection of Liabilities containing the loaded accounts.</returns>
        public BindableCollection<Liability> FetchLiabilities() {
            return FetchAccountsByIDs<Liability>(this.DataProvider.FetchLiabilityIDs());
        }

        /// <summary>
        /// Loads all known Revenue accounts from the data store.
        /// </summary>
        /// <returns>A BindableCollection of Revenue accounts containing the loaded accounts.</returns>
        public BindableCollection<Revenue> FetchRevenues() {
            return FetchAccountsByIDs<Revenue>(this.DataProvider.FetchRevenueIDs());
        }

        /// <summary>
        /// Loads all known Expenses from the data store.
        /// </summary>
        /// <returns>A BindableCollection of Expenses containing the loaded accounts.</returns>
        public BindableCollection<Expense> FetchExpenses() {
            return FetchAccountsByIDs<Expense>(this.DataProvider.FetchExpenseIDs());
        }

        /// <summary>
        /// Loads all known Equity accounts from the data store.
        /// </summary>
        /// <returns>A BindableCollection of Equity accounts containing the loaded accounts.</returns>
        public BindableCollection<Equity> FetchEquities() {
            return FetchAccountsByIDs<Equity>(this.DataProvider.FetchEquityIDs());
        }

        /// <summary>
        /// Creates a new Asset account.
        /// </summary>
        /// <returns>An ChangeCoordinator around the newly created Asset.</returns>
        public ChangeCoordinator<Asset> CreateAsset() {
            return CreateAccount<Asset>(new Asset(), false);
        }

        /// <summary>
        /// Creates a new Liability account.
        /// </summary>
        /// <returns>An ChangeCoordinator around the newly created Liability.</returns>
        public ChangeCoordinator<Liability> CreateLiability() {
            return CreateAccount<Liability>(new Liability(), false);
        }

        /// <summary>
        /// Creates a new Revenue account.
        /// </summary>
        /// <returns>An ChangeCoordinator around the newly created Revenue.</returns>
        public ChangeCoordinator<Revenue> CreateRevenue() {
            return CreateAccount<Revenue>(new Revenue(), false);
        }

        /// <summary>
        /// Creates a new Exepense account.
        /// </summary>
        /// <returns>An ChangeCoordinator around the newly created Expense.</returns>
        public ChangeCoordinator<Expense> CreateExpense() {
            return CreateAccount<Expense>(new Expense(), false);
        }

        /// <summary>
        /// Creates a new Equity account.
        /// </summary>
        /// <returns>An ChangeCoordinator around the newly created Equity.</returns>
        public ChangeCoordinator<Equity> CreateEquity() {
            return CreateAccount<Equity>(new Equity(), false);
        }

        /// <summary>
        /// Saves a given account.
        /// </summary>
        /// <param name="account">The account to save.</param>
        protected virtual void SaveAccount(Account account) {
            switch (account.AccountType) {
                case AccountType.Asset:
                    this.DataProvider.PersistAsset((Asset)account);
                    break;
                case AccountType.Liability:
                    this.DataProvider.PersistLiability((Liability)account);
                    break;
                case AccountType.Equity:
                    this.DataProvider.PersistEquity((Equity)account);
                    break;
                case AccountType.Revenue:
                    this.DataProvider.PersistRevenue((Revenue)account);
                    break;
                case AccountType.Expense:
                    this.DataProvider.PersistExpense((Expense)account);
                    break;
                default:
                    Debug.Fail("Account type was unrecognised.");
                    break;
            }
            account.IsDirty = false;

            this.OnAccountSaved(new AccountEventArgs(account));
        }

        /// <summary>
        /// Saves a given <see cref="T:Transaction"/>.
        /// </summary>
        /// <param name="transaction">The <see cref="T:Transaction"/> to save.</param>
        protected virtual void SaveTransaction(Transaction transaction) {
            if (transaction.CreditAccount != null) {
                transaction.CreditAccount.Transactions.Add(transaction);
            }
            if (transaction.DebitAccount != null) {
                transaction.DebitAccount.Transactions.Add(transaction);
            }

            this.DataProvider.PersistTransaction(transaction);
        }

        /// <summary>
        /// Creates a new Transaction.
        /// </summary>
        /// <returns>The newly created transaction.</returns>
        public ChangeCoordinator<Transaction> CreateTransaction() {
            Transaction t = new Transaction();
            t.TransactionID = Guid.NewGuid();
            Remember(t.TransactionID, t);
            return CreateChangeCoordinator<Transaction>(null, t);
        }

        /// <summary>
        /// Loads all transactions from the data store for a given Account.
        /// </summary>
        /// <param name="a">The account to fetch transactions for.</param>
        /// <returns>A TransactionCollection containing the loaded accounts.</returns>
        public TransactionCollection FetchTransactions(Account a) {
            TransactionCollection realResults = new TransactionCollection();
            TransactionCollection freshResults = DataProvider.FetchTransactions(this, a);

            foreach (Transaction t in freshResults) {
                Transaction transaction = t;
                if (this.LoadedTransactions.ContainsKey(t.TransactionID)) {
                    transaction = this.LoadedTransactions[t.TransactionID];
                } else {
                    Remember(t.TransactionID, t);
                }
                realResults.Add(transaction);
            }

            return realResults;
        }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="type">The type of account to create.</param>
        /// <returns>The newly created account.</returns>
        public virtual ChangeCoordinator<Account> CreateAccount(AccountType type) {
            switch (type) {
                case AccountType.Asset:
                    return CreateAccount<Account>(new Asset(), false);
                case AccountType.Liability:
                    return CreateAccount<Account>(new Liability(), false);
                case AccountType.Equity:
                    return CreateAccount<Account>(new Equity(), false);
                case AccountType.Revenue:
                    return CreateAccount<Account>(new Revenue(), false);
                case AccountType.Expense:
                    return CreateAccount<Account>(new Expense(), false);
                default:
                    Debug.Fail("The account type " + type.ToString() +
                               " was not expected in the Create account factory method.");
                    return null;
            }
        }

        private ChangeCoordinator<T> CreateChangeCoordinator<T>(T originalItem, T editableItem) where T : AccountingDomainObject {
            // Ensure we never forget the two items :)
            if (originalItem != null) originalItem.Workbook = this;
            if (editableItem != null) editableItem.Workbook = this;

            ChangeCoordinator<T> result = ChangeCoordinator<T>.Acquire(originalItem, editableItem);

            result.ChangesPushing += new ChangeCoordinatorChangesPushingEventHandler<T>(ChangeCoordinator_ChangesPushing<T>);
            result.ChangesPushed += new ChangeCoordinatorChangesPushedEventHandler<T>(ChangeCoordinator_ChangesPushed<T>);

            result.EditableItem.IsDirty = false;

            return result;
        }

        private void ChangeCoordinator_ChangesPushing<T>(object sender, ChangeCoordinatorChangesPushingEventArgs<T> e) where T : AccountingDomainObject {
            e.ChangeCoordinator.EditableItem.IsDirty = false;
        }

        private void ChangeCoordinator_ChangesPushed<T>(object sender, ChangeCoordinatorChangesPushedEventArgs<T> e) where T : AccountingDomainObject {

            object item = e.ChangeCoordinator.OriginalItem;
            if (item == null) {
                item = e.ChangeCoordinator.EditableItem;
            }

            if (item is Account) {
                Account a = (Account)item;
                SaveAccount(a);
                this.Remember(a.AccountID, a);
                a.IsDirty = false;
            } else if (item is Transaction) {
                Transaction t = (Transaction)item;
                SaveTransaction(t);
                this.Remember(t.TransactionID, t);
                t.IsDirty = false;
            }
        }

        /// <summary>
        /// Acquires an ChangeCoordinator on a given account. This makes the account temporarily editable while you make changes to it.
        /// </summary>
        /// <param name="account">The account to aquire a lock on.</param>
        /// <returns>The newly created ChangeCoordinator around the account.</returns>
        public ChangeCoordinator<Account> AcquireChangeCoordinator(Account account) {
            ChangeCoordinator<Account> result = null;

            switch (account.AccountType) {
                case AccountType.Asset:
                    result = CreateChangeCoordinator<Account>(account, new Asset());
                    break;
                case AccountType.Liability:
                    result = CreateChangeCoordinator<Account>(account, new Liability());
                    break;
                case AccountType.Equity:
                    result = CreateChangeCoordinator<Account>(account, new Equity());
                    break;
                case AccountType.Revenue:
                    result = CreateChangeCoordinator<Account>(account, new Revenue());
                    break;
                case AccountType.Expense:
                    result = CreateChangeCoordinator<Account>(account, new Expense());
                    break;
                default:
                    Debug.Fail("Unrecognised account type.");
                    break;
            }
            return result;
        }

        /// <summary>
        /// Acquires an ChangeCoordinator around a given asset.
        /// </summary>
        /// <param name="asset">The asset to acquire to lock on.</param>
        /// <returns>The newly created ChangeCoordinator.</returns>
        public ChangeCoordinator<Asset> AcquireChangeCoordinator(Asset asset) {
            return CreateChangeCoordinator<Asset>(asset, new Asset());
        }

        /// <summary>
        /// Acquires an ChangeCoordinator around a given liability.
        /// </summary>
        /// <param name="liability">The liability to acquire to lock on.</param>
        /// <returns>The newly created ChangeCoordinator.</returns>
        public ChangeCoordinator<Liability> AcquireChangeCoordinator(Liability liability) {
            return CreateChangeCoordinator<Liability>(liability, new Liability());
        }

        /// <summary>
        /// Acquires an ChangeCoordinator around a given equity.
        /// </summary>
        /// <param name="equity">The equity to acquire to lock on.</param>
        /// <returns>The newly created ChangeCoordinator.</returns>
        public ChangeCoordinator<Equity> AcquireChangeCoordinator(Equity equity) {
            return CreateChangeCoordinator<Equity>(equity, new Equity());
        }

        /// <summary>
        /// Acquires an ChangeCoordinator around a given expense.
        /// </summary>
        /// <param name="expense">The expense to acquire to lock on.</param>
        /// <returns>The newly created ChangeCoordinator.</returns>
        public ChangeCoordinator<Expense> AcquireChangeCoordinator(Expense expense) {
            return CreateChangeCoordinator<Expense>(expense, new Expense());
        }

        /// <summary>
        /// Acquires an ChangeCoordinator around a given revenue.
        /// </summary>
        /// <param name="revenue">The revenue to acquire to lock on.</param>
        /// <returns>The newly created ChangeCoordinator.</returns>
        public ChangeCoordinator<Revenue> AcquireChangeCoordinator(Revenue revenue) {
            return CreateChangeCoordinator<Revenue>(revenue, new Revenue());
        }

        /// <summary>
        /// Acquires an ChangeCoordinator around a given transaction.
        /// </summary>
        /// <param name="transaction">The transaction to acquire an edit lock on.</param>
        /// <returns>The newly created ChangeCoordinator.</returns>
        public ChangeCoordinator<Transaction> AcquireChangeCoordinator(Transaction transaction) {
            return CreateChangeCoordinator<Transaction>(transaction, new Transaction());
        }

        /// <summary>
        /// Raises the AccountSaved event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnAccountSaved(AccountEventArgs e) {
            if (this.AccountSaved != null) {
                this.AccountSaved(this, e);
            }
        }

        /// <summary>
        /// Raises the CurrentPeriodChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnCurrentPeriodChanged(PeriodEventArgs e) {
            if (this.CurrentPeriodChanged != null) {
                this.CurrentPeriodChanged(this, e);
            }
        }
    }
}
