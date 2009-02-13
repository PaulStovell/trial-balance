using System;
using System.Collections.Generic;
using System.Text;
using PaulStovell.TrialBalance.DomainModel;

namespace PaulStovell.TrialBalance.UserInterface {
    public class WorkbookDetailsDialog : WizardDialog {
        private static WorkbookDetailsDialog _instance = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        private WorkbookDetailsDialog(WizardPage wizardPage)
            : base(wizardPage) {

        }

        /// <summary>
        /// Shows the AccountDetailsDialog in a mode to create a new account.
        /// </summary>
        public static WorkbookDetailsDialog ShowNew(Workbook workbook) {
            if (_instance == null || _instance.IsVisible == false) {
                _instance = new WorkbookDetailsDialog(new WorkbookDetailsPage(workbook));
                _instance.Show();
            } else {
                _instance.Activate();
            }
            return _instance;
        }
    }
}
