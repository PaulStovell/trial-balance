using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.UserInterfaceProcesses {
    public delegate void LedgerTransactionSavingEventHandler(object sender, LedgerTransactionSavingEventArgs e);
    
    public class LedgerTransactionSavingEventArgs : CancelEventArgs {
        private LedgerTransaction _ledgerTransaction;

        public LedgerTransactionSavingEventArgs(LedgerTransaction transaction)
            : base() {
            _ledgerTransaction = transaction;
        }

        public LedgerTransaction LedgerTransaction {
            get { return _ledgerTransaction; }
        }
    }
}
