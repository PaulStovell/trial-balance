using System;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// A type of account which has a credit balance by default.
    /// </summary>
    public abstract class CreditAccount : Account {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accountType">The type of account that the conrete class represents.</param>
        protected CreditAccount(AccountType accountType) :
            base(accountType, BalanceType.Credit) { }

        /// <summary>
        /// Copies the properties of this object to those of a new object.
        /// </summary>
        /// <param name="targetObject">The object to copy the properties to.</param>
        public override void CopyTo(object targetObject) {
            base.CopyTo(targetObject);
        }
    }
}