using PaulStovell.Common;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents an accounting expense. An expense is a cost that the business incurs whilst trying to create revenue, 
    /// and reduces the profit of the business. An example of expenses are Purchases, Bills, and employee salaries. 
    /// </summary>
    public class Expense : DebitAccount {
        private ExpenseCategory _category;

        /// <summary>
        /// Constructor.
        /// </summary>
        internal Expense()
            : base(AccountType.Expense) {
            this.Category = ExpenseCategory.Sales;
        }

        /// <summary>
        /// Raised by the PropertyChanged event when the Category property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs CategoryPropertyChangedEventArgs = new PropertyChangedEventArgs("Category");

        /// <summary>
        /// Gets or sets the category of expenses that this account belongs to.
        /// </summary>
        [Auditable("Category")]
        public ExpenseCategory Category {
            get { return _category; }
            set {
                this.AssertCanEdit();
                _category = value;
                NotifyChanged(CategoryPropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Copies the properties of this object to those of a new object.
        /// </summary>
        /// <param name="targetObject">The object to copy the properties to.</param>
        public override void CopyTo(object targetObject) {
            base.CopyTo(targetObject);

            Expense expense = targetObject as Expense;
            if (expense != null) {
                expense.Category = this.Category;
            }
        }
    }
}