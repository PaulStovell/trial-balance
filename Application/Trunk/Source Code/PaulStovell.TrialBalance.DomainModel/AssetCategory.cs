using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Represents the different categories an Asset can belong to.
    /// </summary>
    public enum AssetCategory {
        /// <summary>
        /// A highly liquid asset, such as Cash at Bank.
        /// </summary>
        [Description("Current")] Current = 1,

        /// <summary>
        /// An asset that is much harder to liquidate, such as a factory or company vehicles.
        /// </summary>
        [Description("Non-current")] NonCurrent = 2,

        /// <summary>
        /// An asset that isn't really used to produce sales directly but in the future, such as stocks
        /// in another company.
        /// </summary>
        [Description("Investment")] Investment = 3,

        /// <summary>
        /// An asset with no tangible revenue producing capabilities but that has intrinsic values, such as 
        /// brand name, patents or IP. 
        /// </summary>
        [Description("Intangible")] Intangible = 4
    }
}