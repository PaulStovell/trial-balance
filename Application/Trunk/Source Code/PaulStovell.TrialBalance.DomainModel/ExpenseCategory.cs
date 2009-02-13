using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents the different categories of expenses.
    /// </summary>
    public enum ExpenseCategory {
        /// <summary>
        /// Indicates a selling expense.
        /// </summary>
        [Description("Sales")] Sales = 1,
        /// <summary>
        /// Represents an administrative expense.
        /// </summary>
        [Description("Administrtive")] Administrative = 2,
        /// <summary>
        /// Represents a financial expense.
        /// </summary>
        [Description("Financial")] Financial = 3,
    }
}