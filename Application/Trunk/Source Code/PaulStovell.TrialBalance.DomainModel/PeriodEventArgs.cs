using System;
using System.Collections.Generic;
using System.Text;

namespace PaulStovell.TrialBalance.DomainModel {
    public delegate void PeriodEventHandler(object sender, PeriodEventArgs e);

    public class PeriodEventArgs : EventArgs {
        private Period _period;

        public PeriodEventArgs(Period period) {
            _period = period;
        }

        public Period Period {
            get { return _period; }
        }
    }
}
