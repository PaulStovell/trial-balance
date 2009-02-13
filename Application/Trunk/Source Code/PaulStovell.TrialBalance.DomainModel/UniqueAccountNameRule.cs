using System;
using System.Collections.Generic;
using System.Text;
using PaulStovell.Common.BindingFramework;
using PaulStovell.Common;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// A rule that is applied to an account to ensure that account names are always unique.
    /// </summary>
    public class UniqueAccountNameRule : Rule {
        private Workbook _workbook;
        private Account _account;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="workbook">The workbook to search for the name.</param>
        /// <param name="account">The account to validate.</param>
        public UniqueAccountNameRule(Workbook workbook, Account account)
            : base("Name", "Account names must be unique.") {
            _workbook = workbook;
            _account = account;
        }

        /// <summary>
        /// Validates that the rule has been followed.
        /// </summary>
        /// <param name="domainObject">The domain object to validate.</param>
        /// <returns>True if the rule has been followed, otherwise false.</returns>
        public override bool ValidateRule(DomainObject domainObject) {
            bool isValid = true;

            BindableCollection<Account> accounts = _workbook.FetchAccounts();
            accounts.Sort();

            string name = _account.Name.Trim().ToLower();
            foreach (Account account in accounts) {
                if (account != _account && account.Name.Trim().ToLower() == name) {
                    isValid = false;
                    this.Description = string.Format("Account names must be unique. The name '{0}' is already in use.", account.Name);
                    break;
                }
            }

            return isValid;
        }
    }
}
