using System;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// This class represents a single period in the accounting cycle.
    /// </summary>
    public class Period {
        private DateTime _startDate;
        private PeriodLength _length;

        /// <summary>
        /// Constructor.
        /// </summary>
        private Period() {}

        /// <summary>
        /// Gets the starting day of the accounting period.
        /// </summary>
        public DateTime StartDate {
            get { return _startDate.Date; }
        }

        /// <summary>
        /// Gets the length of the accounting period.
        /// </summary>
        public PeriodLength Length {
            get { return _length; }
        }

        /// <summary>
        /// Gets the last day of the accounting period.
        /// </summary>
        public DateTime EndDate {
            get { return _startDate.AddMonths((int) _length).AddDays(-1).Date; }
        }

        /// <summary>
        /// Gets the next accounting period after this one.
        /// </summary>
        public Period GetNext() {
            return CalculatePeriodForDate(this.StartDate.AddMonths((int) _length), this.StartDate, this.Length);
        }

        /// <summary>
        /// Gets the accounting period that came before this one.
        /// </summary>
        /// <returns></returns>
        public Period GetPrevious() {
            return CalculatePeriodForDate(this.StartDate.AddMonths(-1 * (int)_length), this.StartDate, this.Length);
        }

        /// <summary>
        /// Gets the accounting period for a given date.
        /// </summary>
        /// <param name="date">The date to find the accounting period for.</param>
        /// <param name="periodStartDate">A date that an account period starts on.</param>
        /// <param name="periodLength">The length of accounting periods.</param>
        /// <returns>The period.</returns>
        public static Period CalculatePeriodForDate(DateTime date, DateTime periodStartDate, PeriodLength periodLength) {
            Period result = new Period();

            DateTime lower =
                new DateTime(date.Year - 1, periodStartDate.Month, periodStartDate.Day).
                    Date.AddMonths(-1 * ((int)periodLength));
            DateTime upper = lower.AddMonths((int)periodLength).AddDays(-1).Date;

            while (date < lower || date > upper) {
                lower = upper.AddDays(1).Date;
                upper = lower.AddMonths((int)periodLength).AddDays(-1).Date;
            }

            result._startDate = lower.Date;
            result._length = periodLength;
            return result;
        }

        /// <summary>
        /// Gets a description of this period.
        /// </summary>
        public string Description {
            get {
                return string.Format("{0} - {1}",
                                     this.StartDate.ToString("dd MMM yyyy"),
                                     this.EndDate.ToString("dd MMM yyyy"));
            }
        }

        public bool IncludesDate(DateTime date) {
            return this.StartDate <= date.Date && this.EndDate >= date.Date;
        }
    }
}