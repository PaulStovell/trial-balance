using PaulStovell.Common;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents an asset. Assets are one of the five types of accounts, and are used to represent the things that a 
    /// business owns that are used to generate future revenue. 
    /// </summary>
    public class Asset :
        DebitAccount {
        private AssetCategory _category;

        /// <summary>
        /// Constructor.
        /// </summary>
        internal Asset()
            : base(AccountType.Asset) {
            this.Category = AssetCategory.Current;
        }

        /// <summary>
        /// Raised by the PropertyChanged event when the Category property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs CategoryPropertyChangedEventArgs = new PropertyChangedEventArgs("Category");

        /// <summary>
        /// Gets or sets the category that this account belongs to. By default, this will be a Current asset.
        /// </summary>
        [Auditable("Category")]
        public AssetCategory Category {
            get { return _category; }
            set {
                this.AssertCanEdit();
                if (_category != value) {
                    _category = value;
                    NotifyChanged(CategoryPropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Copies the properties of this object to those of a new object.
        /// </summary>
        /// <param name="targetObject">The object to copy the properties to.</param>
        public override void CopyTo(object targetObject) {
            base.CopyTo(targetObject);

            Asset asset = targetObject as Asset;
            if (asset != null) {
                asset.Category = this.Category;
            }
        }
    }
}