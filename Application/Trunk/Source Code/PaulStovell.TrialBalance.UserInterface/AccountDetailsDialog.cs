using System;
using System.Collections.Generic;
using System.Text;
using PaulStovell.TrialBalance.DomainModel;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// A dialog that displays the properties of an account. Designed to allow 
    /// creation or updating of account details. 
    /// </summary>
    public class AccountDetailsDialog : WizardDialog {
        private static Dictionary<Account, AccountDetailsDialog> _openWindows = new Dictionary<Account, AccountDetailsDialog>();

        /// <summary>
        /// Constructor.
        /// </summary>
        private AccountDetailsDialog(WizardPage wizardPage)
            : base(wizardPage) {

        }

        /// <summary>
        /// Shows the AccountDetailsDialog in a mode to create a new account.
        /// </summary>
        public static AccountDetailsDialog ShowNew(Workbook workbook) {
            return Show(workbook, null, null);
        }

        /// <summary>
        /// Shows this dialog for a given account. 
        /// </summary>
        /// <param name="editableAccount">An <see cref="T:ChangeCoordinator[T]"/> around a given account to show.</param>
        public static AccountDetailsDialog Show(ChangeCoordinator<Account> editableAccount) {
            return Show(null, editableAccount, null);
        }

        /// <summary>
        /// Shows this dialog for a given account. 
        /// </summary>
        /// <param name="editableAccount">An <see cref="T:ChangeCoordinator[T]"/> around a given account to show.</param>
        public static AccountDetailsDialog Show(ChangeCoordinator<Account> editableAccount, WizardPageReturnEventHandler returnEventHandler) {
            return Show(null, editableAccount, returnEventHandler);
        }

        /// <summary>
        /// Shows this dialog for a given account. 
        /// </summary>
        /// <param name="editableAccount">An <see cref="T:ChangeCoordinator[T]"/> around a given account to show.</param>
        private static AccountDetailsDialog Show(Workbook workbook, ChangeCoordinator<Account> editableAccount, WizardPageReturnEventHandler returnEventHandler) {
            AccountDetailsDialog result = null;

            Account account = null;
            if (editableAccount != null) {
                account = editableAccount.OriginalItem;
                if (account == null) {
                    account = editableAccount.EditableItem;
                }
            }

            if (editableAccount != null && _openWindows.ContainsKey(account)) {
                result = _openWindows[account];
                result.Activate();
            } else {
                WizardPage page = null;
                if (editableAccount != null) {
                    AccountDetailsPage detailsPage = new AccountDetailsPage();
                    detailsPage.AccountChangeCoordinator = editableAccount;
                    page = detailsPage;
                } else {
                    page = new AccountTypePage(workbook);
                }

                result = new AccountDetailsDialog(page);
                if (returnEventHandler != null) {
                    result.Return += returnEventHandler;
                }
                result.Show();
                if (account != null) {
                    _openWindows[account] = result;
                    result.Closed += new EventHandler(
                        delegate(object sender, EventArgs e) {
                            _openWindows.Remove(account);
                        });
                }
            }

            return result;
        }
    }
}
