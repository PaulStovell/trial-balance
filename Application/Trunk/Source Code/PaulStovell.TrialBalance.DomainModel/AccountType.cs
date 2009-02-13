using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents the different types of accounts.
    /// </summary>
    public enum AccountType {
        /// <summary>
        /// Represents an asset.
        /// </summary>
        [Description("Asset")] Asset,
        /// <summary>
        /// Represents a liability.
        /// </summary>
        [Description("Liability")] Liability,
        /// <summary>
        /// Represents owners equity.
        /// </summary>
        [Description("Owners Equity")] Equity,

        /// <summary>
        /// Represents revenue.
        /// </summary>
        [Description("Revenue")] Revenue,

        /// <summary>
        /// Represents an expense.
        /// </summary>
        [Description("Expense")] Expense
    }
}