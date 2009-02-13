using System;
using System.Collections.Generic;
using System.Text;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// A factory for creating <see cref="T:Account">Accounts</see>.
    /// </summary>
    public static class AccountFactory {
        /// <summary>
        /// Creates an account given the <see cref="T:AccountType"/> to create.
        /// </summary>
        /// <param name="accountType">The <see cref="T:AccountType"/> of the account to create.</param>
        /// <returns>The newly created account.</returns>
        public static Account Create(AccountType accountType) {
            return Create(accountType.ToString());
        }

        /// <summary>
        /// Creates an account given the name of an account type.
        /// </summary>
        /// <param name="accountType">The account type to create.</param>
        /// <returns>The newly created account.</returns>
        public static Account Create(string accountType) {
            switch ((accountType ?? string.Empty).Trim().ToLower()) {
                case "asset":
                    return new Asset();
                case "liability":
                    return new Liability();
                case "expense":
                    return new Expense();
                case "revenue":
                    return new Revenue();
                case "equity":
                    return new Equity();
                default:
                    throw new NotSupportedException("Account type '" + accountType + "' was not expected.");
            }
        }
    }
}
