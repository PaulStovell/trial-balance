using System;
using System.Collections.Generic;
using System.Diagnostics;
using PaulStovell.Common;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents the class that creates, loads and persists data to the data source.
    /// </summary>
    public abstract class DataProvider {
        /// <summary>
        /// Retrieves an account from the data store. 
        /// </summary>
        /// <param name="accountID">The ID of the account.</param>
        /// <returns>The loaded account, or null if no account with the given ID exists.</returns>
        public abstract Account FetchAccount(Guid accountID);

        /// <summary>
        /// Retrieves a list of the ID's of all accounts in the data store.
        /// </summary>
        /// <returns>An array of AccountID's.</returns>
        public abstract IList<Guid> FetchAccountIDs();

        /// <summary>
        /// Retrieves an asset for the current business from the data store.
        /// </summary>
        /// <returns>
        /// The loaded Asset, or null if no account was found.
        /// </returns>
        public abstract Asset FetchAsset(Guid assetID);

        /// <summary>
        /// Retrieves all assets for the current business from the data store.
        /// </summary>
        /// <returns>The loaded assets.</returns>
        public abstract IList<Guid> FetchAssetIDs();

        /// <summary>
        /// Retrieves a liability for the current business from the data store given the unique identifier for the liability.
        /// </summary>
        /// <returns>
        /// The loaded Liability, or null of a liability with the given ID was not found.
        /// </returns>
        public abstract Liability FetchLiability(Guid liabilityID);

        /// <summary>
        /// Retrieves all liabilities for the current business from the data store.
        /// </summary>
        /// <returns>The loaded liabilities.</returns>
        public abstract IList<Guid> FetchLiabilityIDs();

        /// <summary>
        /// Retrieves an equity account for the current business from the data store given the unique identifier for the equity.
        /// </summary>
        /// <returns>
        /// The loaded Equity, or null if an equity with the given ID was not found.
        /// </returns>
        public abstract Equity FetchEquity(Guid equityID);

        /// <summary>
        /// Retrieves all equity accounts for the current business from the data store.
        /// </summary>
        /// <returns>The loaded equity accounts.</returns>
        public abstract IList<Guid> FetchEquityIDs();

        /// <summary>
        /// Retrieves an expense account for the current business from the data store given the unique identifier for the expense.
        /// </summary>
        /// <returns>
        /// The loaded Expense, or null if an expense with the given ID was not found.
        /// </returns>
        public abstract Expense FetchExpense(Guid expenseID);

        /// <summary>
        /// Retrieves all expense accounts for the current business from the data store.
        /// </summary>
        /// <returns>The loaded expense accounts.</returns>
        public abstract IList<Guid> FetchExpenseIDs();

        /// <summary>
        /// Retrieves a revenue account for the current business from the data store given the unique identifier for the revenue.
        /// </summary>
        /// <returns>
        /// The loaded Revenue, or null if a revenue account with the given ID was not found.
        /// </returns>
        public abstract Revenue FetchRevenue(Guid revenueID);

        /// <summary>
        /// Retrieves all revenue accounts for the current business from the data store.
        /// </summary>
        /// <returns>The loaded revenue accounts.</returns>
        public abstract IList<Guid> FetchRevenueIDs();

        /// <summary>
        /// Saves a given asset.
        /// </summary>
        /// <param name="asset">The asset to save.</param>
        public abstract void PersistAsset(Asset asset);

        /// <summary>
        /// Saves a liability account to the data store.
        /// </summary>
        /// <param name="liability">The liability to save.</param>
        public abstract void PersistLiability(Liability liability);

        /// <summary>
        /// Saves an expense account to the data store.
        /// </summary>
        /// <param name="expense">The expense account to save.</param>
        public abstract void PersistExpense(Expense expense);

        /// <summary>
        /// Saves a revenue account to the data store.
        /// </summary>
        /// <param name="revenue">The revenue account to save.</param>
        public abstract void PersistRevenue(Revenue revenue);

        /// <summary>
        /// Saves an equity account to the data store.
        /// </summary>
        /// <param name="equity">The equity account to save.</param>
        public abstract void PersistEquity(Equity equity);

        /// <summary>
        /// Deletes a given account.
        /// </summary>
        /// <param name="account">The account to delete.</param>
        public abstract void DeleteAccount(Account account);

        /// <summary>
        /// Creates a transaction.
        /// </summary>
        /// <returns>The newly created transaction.</returns>
        public virtual Transaction CreateTransaction() {
            return new Transaction();
        }

        /// <summary>
        /// Loads all transactions for the current business from the data source that were applied to a given account.
        /// </summary>
        /// <param name="workbook">
        /// The workbook used to associate the loaded transactions with.
        /// </param>
        /// <param name="account">The account that will be used to search for transactions.</param>
        /// <returns>The loaded transactions.</returns>
        public abstract TransactionCollection FetchTransactions(Workbook workbook, Account account);

        /// <summary>
        /// Saves a given transaction to the data store.
        /// </summary>
        /// <param name="transaction">The transaction to save.</param>
        public abstract void PersistTransaction(Transaction transaction);

        /// <summary>
        /// Saves a list of transactions to the data store.
        /// </summary>
        /// <param name="transactions">A list of transactions to save.</param>
        public virtual void PersistTransactions(IList<Transaction> transactions) {
            foreach (Transaction t in transactions) {
                PersistTransaction(t);
            }
        }

        /// <summary>
        /// Deletes a given transaction from the data source.
        /// </summary>
        /// <param name="transaction">The transaction to delete.</param>
        public abstract void DeleteTransaction(Transaction transaction);

        /// <summary>
        /// Deletes a list of transactions from the data source.
        /// </summary>
        /// <param name="transactions">The transactions to delete.</param>
        public virtual void DeleteTransaction(IList<Transaction> transactions) {
            foreach (Transaction t in transactions) {
                DeleteTransaction(t);
            }
        }

        /// <summary>
        /// Retrieves a given setting from the data source.
        /// </summary>
        /// <param name="name">The name of the setting to retrieve.</param>
        /// <returns>The setting if found, otherwise null.</returns>
        public abstract string FetchSetting(string name);

        /// <summary>
        /// Saves a given setting.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public abstract void PersistSetting(string name, string value);
    }
}