using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Packaging;
using System.IO;
using System.Xml.XLinq;

namespace PaulStovell.TrialBalance.DomainModel.PackageDataProvider {
    /// <summary>
    /// A data provider for dealing with zip packages.
    /// </summary>
    public class PackageDataProvider : DataProvider {
        private string _filePath;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="filePath">The path to the file to store TrialBalance data in.</param>
        protected PackageDataProvider(string filePath) {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Gets or sets the path to the file to store TrialBalance data in.
        /// </summary>
        public string FilePath {
            get { return _filePath ?? string.Empty; }
            protected set { _filePath = value; }
        }

        /// <summary>
        /// Creates a new TrialBalance data package at a given location.
        /// </summary>
        /// <param name="filePath">The location where the data package should be stored.</param>
        /// <returns>The newly created <see cref="T:PackageDataProvider"/>.</returns>
        public static PackageDataProvider CreateFile(string filePath) {
            PackageDataProvider pp = new PackageDataProvider(filePath);
            return pp;
        }

        /// <summary>
        /// Opens a TrialBalance data package from a given location.
        /// </summary>
        /// <param name="filePath">The location where the data package is stored.</param>
        /// <returns>The opened <see cref="T:PackageDataProvider"/>.</returns>
        public static PackageDataProvider OpenFile(string filePath) {
            return new PackageDataProvider(filePath);
        }

        /// <summary>
        /// Retrieves an account from the data store. 
        /// </summary>
        /// <param name="accountID">The ID of the account.</param>
        /// <returns>The loaded account, or null if no account with the given ID exists.</returns>
        public override Account FetchAccount(Guid accountID) {
            return ReadAccount(accountID);
        }

        /// <summary>
        /// Retrieves a list of the ID's of all accounts in the data store.
        /// </summary>
        /// <returns>An array of AccountID's.</returns>
        public override IList<Guid> FetchAccountIDs() {
            List<Guid> results = new List<Guid>();
            results.AddRange(FetchAssetIDs());
            results.AddRange(FetchEquityIDs());
            results.AddRange(FetchExpenseIDs());
            results.AddRange(FetchLiabilityIDs());
            results.AddRange(FetchRevenueIDs());
            return results;
        }

        /// <summary>
        /// Retrieves an asset for the current business from the data store.
        /// </summary>
        /// <returns>
        /// The loaded Asset, or null if no account was found.
        /// </returns>
        public override Asset FetchAsset(Guid assetID) {
            return ReadAccount(assetID) as Asset;
        }

        /// <summary>
        /// Retrieves all assets for the current business from the data store.
        /// </summary>
        /// <returns>The loaded assets.</returns>
        public override IList<Guid> FetchAssetIDs() {
            return ReadIndex(AccountType.Asset);
        }

        /// <summary>
        /// Retrieves a liability for the current business from the data store given the unique identifier for the liability.
        /// </summary>
        /// <returns>
        /// The loaded Liability, or null of a liability with the given ID was not found.
        /// </returns>
        public override Liability FetchLiability(Guid liabilityID) {
            return ReadAccount(liabilityID) as Liability;
        }

        /// <summary>
        /// Retrieves all liabilities for the current business from the data store.
        /// </summary>
        /// <returns>The loaded liabilities.</returns>
        public override IList<Guid> FetchLiabilityIDs() {
            return ReadIndex(AccountType.Liability);
        }

        /// <summary>
        /// Retrieves an equity account for the current business from the data store given the unique identifier for the equity.
        /// </summary>
        /// <returns>
        /// The loaded Equity, or null if an equity with the given ID was not found.
        /// </returns>
        public override Equity FetchEquity(Guid equityID) {
            return ReadAccount(equityID) as Equity;
        }

        /// <summary>
        /// Retrieves all equity accounts for the current business from the data store.
        /// </summary>
        /// <returns>The loaded equity accounts.</returns>
        public override IList<Guid> FetchEquityIDs() {
            return ReadIndex(AccountType.Equity);
        }

        /// <summary>
        /// Retrieves an expense account for the current business from the data store given the unique identifier for the expense.
        /// </summary>
        /// <returns>
        /// The loaded Expense, or null if an expense with the given ID was not found.
        /// </returns>
        public override Expense FetchExpense(Guid expenseID) {
            return ReadAccount(expenseID) as Expense;
        }

        /// <summary>
        /// Retrieves all expense accounts for the current business from the data store.
        /// </summary>
        /// <returns>The loaded expense accounts.</returns>
        public override IList<Guid> FetchExpenseIDs() {
            return ReadIndex(AccountType.Expense);
        }

        /// <summary>
        /// Retrieves a revenue account for the current business from the data store given the unique identifier for the revenue.
        /// </summary>
        /// <returns>
        /// The loaded Revenue, or null if a revenue account with the given ID was not found.
        /// </returns>
        public override Revenue FetchRevenue(Guid revenueID) {
            return ReadAccount(revenueID) as Revenue;
        }

        /// <summary>
        /// Retrieves all revenue accounts for the current business from the data store.
        /// </summary>
        /// <returns>The loaded revenue accounts.</returns>
        public override IList<Guid> FetchRevenueIDs() {
            return ReadIndex(AccountType.Revenue);
        }

        /// <summary>
        /// Saves a given asset.
        /// </summary>
        /// <param name="asset">The asset to save.</param>
        public override void PersistAsset(Asset asset) {
            SaveAccount(asset);
        }

        /// <summary>
        /// Saves a liability account to the data store.
        /// </summary>
        /// <param name="liability">The liability to save.</param>
        public override void PersistLiability(Liability liability) {
            SaveAccount(liability);
        }

        /// <summary>
        /// Saves an expense account to the data store.
        /// </summary>
        /// <param name="expense">The expense account to save.</param>
        public override void PersistExpense(Expense expense) {
            SaveAccount(expense);
        }

        /// <summary>
        /// Saves a revenue account to the data store.
        /// </summary>
        /// <param name="revenue">The revenue account to save.</param>
        public override void PersistRevenue(Revenue revenue) {
            SaveAccount(revenue);
        }

        /// <summary>
        /// Saves an equity account to the data store.
        /// </summary>
        /// <param name="equity">The equity account to save.</param>
        public override void PersistEquity(Equity equity) {
            SaveAccount(equity);
        }

        /// <summary>
        /// Deletes a given account.
        /// </summary>
        /// <param name="account">The account to delete.</param>
        public override void DeleteAccount(Account account) {
            // Remove the account ID from the index
            List<Guid> accountIDs = ReadIndex(account.AccountType);
            if (accountIDs.Contains(account.AccountID)) {
                accountIDs.Remove(account.AccountID);
                SaveIndex(accountIDs, account.AccountType);
            }

            Uri accountUri = new Uri("/Accounts/" + account.AccountID.ToString() + ".xml", UriKind.Relative);
            using (Package package = ZipPackage.Open(this.FilePath, System.IO.FileMode.OpenOrCreate)) {
                if (package.PartExists(accountUri)) {
                    package.DeletePart(accountUri);
                }
            }
        }

        /// <summary>
        /// Loads all transactions for the current business from the data source that were applied to a given account.
        /// </summary>
        /// <param name="workbook">
        /// The workbook used to associate the loaded transactions with.
        /// </param>
        /// <param name="account">The account that will be used to search for transactions.</param>
        /// <returns>The loaded transactions.</returns>
        public override TransactionCollection FetchTransactions(Workbook workbook, Account account) {
            TransactionCollection results = new TransactionCollection();
            results.AddRange(ReadTransactions(workbook, account));
            return results;
        }

        /// <summary>
        /// Saves a given transaction to the data store.
        /// </summary>
        /// <param name="transaction">The transaction to save.</param>  
        public override void PersistTransaction(Transaction transaction) {
            List<Transaction> transactions = ReadTransactions(transaction.Workbook, null);
            bool alreadyInCollection = false;
            for (int i = 0; i < transactions.Count; i++) {
                if (transactions[i].TransactionID == transaction.TransactionID) {
                    transactions[i] = transaction;
                    alreadyInCollection = true;
                    break;
                }
            }

            if (!alreadyInCollection) {
                transactions.Add(transaction);
            }
            SaveTransactions(transactions);
        }

        /// <summary>
        /// Deletes a given transaction from the data source.
        /// </summary>
        /// <param name="transaction">The transaction to delete.</param>
        public override void DeleteTransaction(Transaction transaction) {
            List<Transaction> transactions = ReadTransactions(transaction.Workbook, null);
            for (int i = 0; i < transactions.Count; i++) {
                if (transactions[i].TransactionID == transaction.TransactionID) {
                    transactions.RemoveAt(i);
                    break;
                }
            }
            SaveTransactions(transactions);
        }

        /// <summary>
        /// Retrieves a given setting from the data source.
        /// </summary>
        /// <param name="name">The name of the setting to retrieve.</param>
        /// <returns>The setting if found, otherwise null.</returns>
        public override string FetchSetting(string name) {
            name = (name ?? string.Empty).ToLower();
            string result = string.Empty;
            Dictionary<string, string> settings = ReadSettings();
            Dictionary<string, string>.Enumerator enumerator = settings.GetEnumerator();
            while (enumerator.MoveNext()) {
                if (enumerator.Current.Key.ToLower() == name) {
                    name = enumerator.Current.Key;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Saves a given setting.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public override void PersistSetting(string name, string value) {
            Dictionary<string, string> settings = ReadSettings();
            settings[name] = value;
            SaveSettings(settings);
        }

        private List<Guid> ReadIndex(AccountType accountType) {
            List<Guid> results = new List<Guid>();
            Uri indexFileUri = new Uri("/" + accountType.ToString() + "Index.txt", UriKind.Relative);
            using (Package package = ZipPackage.Open(this.FilePath, System.IO.FileMode.OpenOrCreate)) {
                if (package.PartExists(indexFileUri)) {
                    PackagePart indexPart = package.GetPart(indexFileUri);
                    using (StreamReader reader = new StreamReader(indexPart.GetStream())) {
                        string line = null;
                        while ((line = reader.ReadLine()) != null) {
                            Guid accountId = new Guid(line);
                            results.Add(accountId);
                        }
                    }
                }
            }
            return results;
        }

        private void SaveIndex(List<Guid> indexItems, AccountType accountType) {
            Dictionary<string, string> results = new Dictionary<string, string>();
            Uri indexFileUri = new Uri("/" + accountType.ToString() + "Index.txt", UriKind.Relative);
            using (Package package = ZipPackage.Open(this.FilePath, System.IO.FileMode.OpenOrCreate)) {
                // Either get the existing PackagePart or create a new one
                PackagePart indexPart = null;
                if (package.PartExists(indexFileUri)) {
                    indexPart = package.GetPart(indexFileUri);
                } else {
                    indexPart = package.CreatePart(indexFileUri, "text/xml");
                }

                // Now write the settings to the package
                using (StreamWriter sw = new StreamWriter(indexPart.GetStream())) {
                    foreach (Guid accountId in indexItems) {
                        sw.WriteLine(accountId.ToString());
                    }
                }

            }
        }

        private Account ReadAccount(Guid accountId) {
            Account result = null;
            Uri accountUri = new Uri("/Accounts/" + accountId.ToString() + ".xml", UriKind.Relative);
            using (Package package = ZipPackage.Open(this.FilePath, System.IO.FileMode.OpenOrCreate)) {
                if (package.PartExists(accountUri)) {
                    PackagePart accountPart = package.GetPart(accountUri);
                    using (StreamReader reader = new StreamReader(accountPart.GetStream())) {
                        XElement xml = XElement.Load(reader);
                        AccountXElementAdapter adapter = new AccountXElementAdapter();
                        result = adapter.FromXElement(xml);
                    }
                }
            }
            return result;
        }

        private void SaveAccount(Account account) {
            // Add the account ID to the index
            List<Guid> accountIDs = ReadIndex(account.AccountType);
            if (!accountIDs.Contains(account.AccountID)) {
                accountIDs.Add(account.AccountID);
                SaveIndex(accountIDs, account.AccountType);
            }

            AccountXElementAdapter adapter = new AccountXElementAdapter();
            XElement xmlElement = adapter.ToXElement(account);
            Uri accountUri = new Uri("/Accounts/" + account.AccountID.ToString() + ".xml", UriKind.Relative);

            using (Package package = ZipPackage.Open(this.FilePath, System.IO.FileMode.OpenOrCreate)) {
                // Either get the existing PackagePart or create a new one
                PackagePart accountPart = null;
                if (package.PartExists(accountUri)) {
                    accountPart = package.GetPart(accountUri);
                } else {
                    accountPart = package.CreatePart(accountUri, "text/xml");
                }

                // Now write the account, overwriting whatever was there previously
                // Note: If concurrency checks were to be implemented (at this stage, they wouldn't 
                // make much sense as TrialBalance is a single-user application) they should be 
                // implemented here.
                using (StreamWriter sw = new StreamWriter(accountPart.GetStream())) {
                    sw.Write(xmlElement.ToString());
                }
            
            }
        }

        private Dictionary<string, string> ReadSettings() {
            Dictionary<string, string> results = new Dictionary<string, string>();
            Uri accountUri = new Uri("/Settings.txt", UriKind.Relative);
            using (Package package = ZipPackage.Open(this.FilePath, System.IO.FileMode.OpenOrCreate)) {
                if (package.PartExists(accountUri)) {
                    PackagePart accountPart = package.GetPart(accountUri);
                    using (StreamReader reader = new StreamReader(accountPart.GetStream())) {
                        string line = null;
                        while ((line = reader.ReadLine()) != null) {
                            string[] keyValuePair = line.Split('=');
                            string key = keyValuePair[0].Trim();
                            string value = keyValuePair[1].Trim();
                            results.Add(key, value);
                        }
                    }
                }
            }

            return results;
        }

        private void SaveSettings(Dictionary<string, string> settings) {
            Dictionary<string, string> results = new Dictionary<string, string>();
            Uri settingsFileUri = new Uri("/Settings.txt", UriKind.Relative);
            using (Package package = ZipPackage.Open(this.FilePath, System.IO.FileMode.OpenOrCreate)) {
                // Either get the existing PackagePart or create a new one
                PackagePart settingsPart = null;
                if (package.PartExists(settingsFileUri)) {
                    settingsPart = package.GetPart(settingsFileUri);
                } else {
                    settingsPart = package.CreatePart(settingsFileUri, "text/txt");
                }

                // Now write the settings to the package
                using (StreamWriter sw = new StreamWriter(settingsPart.GetStream())) {
                    Dictionary<string, string>.Enumerator settingsEnumerator = settings.GetEnumerator();
                    while (settingsEnumerator.MoveNext()) {
                        sw.WriteLine(settingsEnumerator.Current.Key + '=' + settingsEnumerator.Current.Value);
                    }
                }
                
            }
        }

        private List<Transaction> ReadTransactions(Workbook workbook, Account account) {
            List<Transaction> results = new List<Transaction>();
            Uri accountUri = new Uri("/Accounts/Transactions/Transactions.xml", UriKind.Relative);

            using (Package package = ZipPackage.Open(this.FilePath, System.IO.FileMode.OpenOrCreate)) {
                if (package.PartExists(accountUri)) {
                    PackagePart accountPart = package.GetPart(accountUri);
                    using (StreamReader reader = new StreamReader(accountPart.GetStream())) {
                        XElement transactions = XElement.Load(reader);
                        TransactionXElementAdapter adapter = new TransactionXElementAdapter();
                        foreach (XElement transaction in transactions.Nodes()) {
                            Transaction fetchedTransaction = adapter.FromXElement(transaction, workbook);
                            if (account == null || fetchedTransaction.CreditAccount == account || fetchedTransaction.DebitAccount == account) {
                                results.Add(fetchedTransaction);
                            }
                        }
                    }
                }
            }
            return results;
        }

        private void SaveTransactions(List<Transaction> transactions) {
            Uri accountUri = new Uri("/Accounts/Transactions/Transactions.xml", UriKind.Relative);

            using (Package package = ZipPackage.Open(this.FilePath, System.IO.FileMode.OpenOrCreate)) {
                // Either get the existing PackagePart or create a new one
                PackagePart accountPart = null;
                if (package.PartExists(accountUri)) {
                    accountPart = package.GetPart(accountUri);
                } else {
                    accountPart = package.CreatePart(accountUri, "text/xml");
                }
                
                
                XElement resultingXElement = new XElement("Transactions");

                TransactionXElementAdapter adapter = new TransactionXElementAdapter();
                foreach (Transaction transaction in transactions) {
                    resultingXElement.Add(adapter.ToXElement(transaction));
                }

                using (StreamWriter sw = new StreamWriter(accountPart.GetStream())) {
                    sw.Write(resultingXElement.ToString());
                }   
            }
        }
    }
}
