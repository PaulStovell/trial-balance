using PaulStovell.Common;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents a Revenue account. Revenue is the income for a business, and, along with expenses, results 
    /// in the net profit or loss for a business. Examples of revenue accounts are Sales, Interest Recieved and 
    /// rental income.
    /// </summary>
    public class Revenue : CreditAccount {
        private RevenueCategory _category;

        /// <summary>
        /// Constructor.
        /// </summary>
        internal Revenue()
            : base(AccountType.Revenue) {
            this.Category = RevenueCategory.Sales;
        }

        /// <summary>
        /// Raised by the PropertyChanged event when the Category property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs CategoryPropertyChangedEventArgs = new PropertyChangedEventArgs("Category");

        /// <summary>
        /// Gets or sets the category of revenues that this revenue account belongs to.
        /// </summary>
        [Auditable("Category")]
        public RevenueCategory Category {
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

            Revenue revenue = targetObject as Revenue;
            if (revenue != null) {
                revenue.Category = this.Category;
            }
        }
    }
}