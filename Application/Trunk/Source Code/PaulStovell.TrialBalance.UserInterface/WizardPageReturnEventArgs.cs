using System;
using System.Collections.Generic;
using System.Text;

namespace PaulStovell.TrialBalance.UserInterface {
    public delegate void WizardPageReturnEventHandler(object sender, WizardPageReturnEventArgs e);

    public class WizardPageReturnEventArgs : EventArgs {
        private object _currentItem;
        private bool _cancelled;

        public WizardPageReturnEventArgs(object currentItem, bool cancelled) {
            this.CurrentItem = currentItem;
            this.Cancelled = cancelled;
        }

        public object CurrentItem {
            get { return _currentItem; }
            set { _currentItem = value; } 
        }

        public bool Cancelled {
            get { return _cancelled; }
            set { _cancelled = value; }
        }
    }
}
