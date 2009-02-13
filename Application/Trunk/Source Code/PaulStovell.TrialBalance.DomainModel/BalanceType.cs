using System;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Indicates the different types of balances.
    /// </summary>
    public enum BalanceType {
        /// <summary>
        /// A debit balance.
        /// </summary>
        [Description("DR")] Debit,

        /// <summary>
        /// A credit balance.
        /// </summary>
        [Description("CR")] Credit
    }
}