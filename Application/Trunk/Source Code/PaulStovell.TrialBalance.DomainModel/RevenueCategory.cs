using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents the different categories of revenue.
    /// </summary>
    public enum RevenueCategory {
        /// <summary>
        /// Revenue created by sales. Accounts in this category are usually the major sources of income for a 
        /// business and are what the business relies on for the bulk of its earnings. 
        /// </summary>
        [Description("Sales")] Sales = 1,

        /// <summary>
        /// Revenue accounts created by other means. These are usually the minor sources of income for a business and 
        /// aren't usually related to the businesses direct line of work.
        /// </summary>
        [Description("Other")] Other = 2
    }
}