using System;
using System.Collections.Generic;
using System.Text;

namespace PaulStovell.TrialBalance.DomainModel {
    public interface ICurrentPeriodProvider {
        Period CurrentPeriod { get;}
        event PeriodEventHandler CurrentPeriodChanged;
    }
}
