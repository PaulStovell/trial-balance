using System;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Debit accounts are accounts with a default balance type of debit. Assets and Expenses are debit accounts.
    /// </summary>
    public abstract class DebitAccount : Account {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accountType">The type of account that the concrete account represents.</param>
        protected DebitAccount(AccountType accountType) :
            base(accountType, BalanceType.Debit) {}

        /// <summary>
        /// Copies the properties of this object to those of a new object.
        /// </summary>
        /// <param name="targetObject">The object to copy the properties to.</param>
        public override void CopyTo(object targetObject) {
            base.CopyTo(targetObject);
        }
    }
}