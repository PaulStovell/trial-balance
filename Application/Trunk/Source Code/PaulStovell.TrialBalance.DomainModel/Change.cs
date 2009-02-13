using System;
using System.Collections.Generic;
using System.Text;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Encapsulates a record of a change made to a given property.
    /// </summary>
    public class Change : IComparable<Change> {
        private string _propertyName;
        private string _oldValue;
        private string _newValue;

        /// <summary>
        /// The name of the property that was changed.
        /// </summary>
        public string PropertyName {
            get { return _propertyName ?? string.Empty; }
            set { _propertyName = value; }
        }

        /// <summary>
        /// The value of the property before the change was applied.
        /// </summary>
        public string OldValue {
            get { return _oldValue ?? string.Empty; }
            set { _oldValue = value; }
        }

        /// <summary>
        /// The value of the property after the change was applied.
        /// </summary>
        public string NewValue {
            get { return _newValue ?? string.Empty; }
            set { _newValue = value; }
        }

        /// <summary>
        /// Compares one <see cref="T:Change"/> to another.
        /// </summary>
        /// <param name="other">The <see cref="T:Change"/> to compare with.</param>
        /// <returns></returns>
        public int CompareTo(Change other) {
            return this.PropertyName.CompareTo(other.PropertyName);
        }
    }
}
