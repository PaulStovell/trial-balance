using System;
using System.Collections.Generic;
using System.Text;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// An attribute applied to <see cref="T:AccountingDomainObject"/> properties to mark them as properties 
    /// that should be audited; that is, they should be included in the object's <see cref="T:ChangeSetHistory"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AuditableAttribute : Attribute {
        private string _description;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="description">A description of the property.</param>
        public AuditableAttribute(string description) {
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets a description of the property.
        /// </summary>
        public string Description {
            get { return _description; }
            set { _description = value; }
        }
    }
}
