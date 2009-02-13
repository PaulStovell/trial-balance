using System.ComponentModel;
namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents the available period lengths.
    /// </summary>
    /// <remarks>
    /// The values given correspond to the number of months in the period. This is done because most period calculations 
    /// are done by going something like: DateTime.Now.AddMonths(periodLength).
    /// </remarks>
    public enum PeriodLength : int {
        /// <summary>
        /// Indicates monthly accounting periods.
        /// </summary>
        [Description("Monthly")]
        Monthly = 1,
        /// <summary>
        /// Indicates quarterly accounting periods.
        /// </summary>
        [Description("Quarterly")]
        Quarterly = 3,
        /// <summary>
        /// Indicates half-yearly (semi-annually) accounting periods.
        /// </summary>
        [Description("Semi-annually")]
        HalfYearly = 6,
        /// <summary>
        /// Represents yearly accounting periods.
        /// </summary>
        [Description("Yearly")]
        Yearly = 12,
    }
}