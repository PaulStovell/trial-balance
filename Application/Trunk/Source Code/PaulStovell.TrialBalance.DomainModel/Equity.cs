using System;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents an equity account. Equity accounts are accounts that indicate the investments made by the owners of the
    /// business. Owners Equity, Profit and Loss are all kinds of Equity accounts.
    /// </summary>
    public class Equity : CreditAccount {
        /// <summary>
        /// Constructor.
        /// </summary>
        internal Equity()
            : base(AccountType.Equity) {
        }

        /// <summary>
        /// Copies the properties of this object to those of a new object.
        /// </summary>
        /// <param name="targetObject">The object to copy the properties to.</param>
        public override void CopyTo(object targetObject) {
            base.CopyTo(targetObject);
        }
    }
}