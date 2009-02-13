using System;
using System.Collections.Generic;
using System.Text;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Limits the range of results included in a list of transactions.
    /// </summary>
    public class TransactionScope {
        private static TransactionScope _all;
        private DateTime? _startDate;
        private DateTime? _endDate;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="period">The accounting period for this transaction scope.</param>
        public TransactionScope(Period period) : this(period.StartDate, period.EndDate) {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="startDate">The starting date to be included in this transaction scope.</param>
        /// <param name="endDate">The ending date to be included in this transaction scope.</param>
        public TransactionScope(DateTime? startDate, DateTime? endDate) {

        }

        /// <summary>
        /// Gets the starting date included in this transaction scope.
        /// </summary>
        public DateTime? StartDate {
            get { return _startDate; }
            protected set { _startDate = value; }
        }

        /// <summary>
        /// Gets the ending date included in this transaction scope.
        /// </summary>
        public DateTime? EndDate {
            get { return _endDate; }
            protected set { _endDate = value; }
        }

        /// <summary>
        /// Gets a transaction scope that will include transactions only for the current period.
        /// </summary>
        public static TransactionScope CurrentPeriod {
            get {
                return new TransactionScope(Period.Current);; 
            }
        }

        /// <summary>
        /// Gets a transaction scope that will include all known transactions.
        /// </summary>
        public static TransactionScope All {
            get {
                if (_all == null) {
                    _all = new TransactionScope(null, null);
                }
                return _all;
            }
        }
    }
}
