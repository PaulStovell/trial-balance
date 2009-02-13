using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Encapsulates a group of changes to an object committed at the same time.
    /// </summary>
    public class ChangeSet : IComparable<ChangeSet> {
        private string _username;
        private DateTime _applied;
        private ObservableCollection<Change> _changes;

        /// <summary>
        /// The name of the user who made the change.
        /// </summary>
        public string Username {
            get { return _username ?? string.Empty; }
            set { _username = value; }
        }

        /// <summary>
        /// The date the change was applied.
        /// </summary>
        public DateTime Applied {
            get { return _applied; }
            set { _applied = value; }
        }

        /// <summary>
        /// The property changes that were made.
        /// </summary>
        public ObservableCollection<Change> Changes {
            get {
                if (_changes == null) {
                    _changes = new ObservableCollection<Change>();
                }
                return _changes;
            }
        }

        /// <summary>
        /// Compares this <see cref="T:ChangeSet"/> to a given <see cref="T:ChangeSet"/> by comparing the dates they were applied. 
        /// </summary>
        /// <param name="other">The <see cref="T:ChangeSet"/> to compare with.</param>
        /// <returns>A value indicating whether one <see cref="T:ChangeSet"/> was applied before or after another.</returns>
        public int CompareTo(ChangeSet other) {
            return this.Applied.CompareTo(other.Applied);
        }
    }
}
