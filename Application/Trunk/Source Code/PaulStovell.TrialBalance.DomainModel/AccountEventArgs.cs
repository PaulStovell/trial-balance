using System;
using System.Collections.Generic;
using System.Text;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// A delegate for events raised with information about an account.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void AccountEventHandler(object sender, AccountEventArgs e);

    /// <summary>
    /// Event arguments for events that involve an <see cref="Account" />.
    /// </summary>
    public class AccountEventArgs : EventArgs {
        private Account _account;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="account">The account to create.</param>
        public AccountEventArgs(Account account) {
            _account = account;
        }

        /// <summary>
        /// Gets the account involved in this event.
        /// </summary>
        public Account Account {
            get { return _account; }
        }
    }
}
