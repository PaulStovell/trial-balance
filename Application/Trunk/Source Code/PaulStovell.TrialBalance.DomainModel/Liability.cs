using PaulStovell.Common;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// This class represents an accounting liability. A liability is defined as the future sacrifices 
    /// an entity is obliged to make to other entities as a result of past transactions. Loans and mortgages the 
    /// business has taken out are a great example of a liability.
    /// </summary>
    public class Liability : CreditAccount {
        private LiabilityCategory _category;

        /// <summary>
        /// Constructor.
        /// </summary>
        internal Liability()
            : base(AccountType.Liability) {
            this.Category = LiabilityCategory.Current;
        }

        /// <summary>
        /// Raised by the PropertyChanged event when the Category property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs CategoryPropertyChangedEventArgs = new PropertyChangedEventArgs("Category");

        /// <summary>
        /// Gets or sets the category of liabilities that this liability belongs to. By default, for a 
        /// new account this will be a Current liability.
        /// </summary>
        [Auditable("Category")]
        public LiabilityCategory Category {
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

            Liability liability = targetObject as Liability;
            if (liability != null) {
                liability.Category = this.Category;
            }
        }
    }
}