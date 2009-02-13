using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents the different categories a Liability can belong to.
    /// </summary>
    public enum LiabilityCategory {
        /// <summary>
        /// A current liability. These liabilities are usually short term debts that will be paid off in the next accounting 
        /// period.
        /// </summary>
        [Description("Current")] Current = 1,

        /// <summary>
        /// A non-current liability. These liabilities are usually much longer term liabilities, such as mortgages or loans and 
        /// are not expected to be paid off in the next accounting period.
        /// </summary>
        [Description("Non-current")] NonCurrent = 2,
    }
}