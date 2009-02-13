using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// Encapsulates a history of changes made to an item over time.
    /// </summary>
    public class ChangeSetHistory : IEnumerable<ChangeSet> {
        private readonly List<ChangeSet> _innerList = new List<ChangeSet>();

        /// <summary>
        /// Appends a new set of changes to this history.
        /// </summary>
        /// <param name="changeSet">The set of changes to add.</param>
        public void Append(ChangeSet changeSet) {
            if (changeSet != null) {
                _innerList.Add(changeSet);
                _innerList.Sort();
            }
        }

        /// <summary>
        /// Appends a list of changes to this history.
        /// </summary>
        /// <param name="changeSets">The set of changes to add.</param>
        public void AppendRange(IEnumerable<ChangeSet> changeSets) {
            if (changeSets != null) {
                _innerList.AddRange(changeSets);
                _innerList.Sort();
            }
        }

        /// <summary>
        /// Gets an enumerator over this ChangeSetHistory.
        /// </summary>
        /// <returns>A new enumerator.</returns>
        public IEnumerator<ChangeSet> GetEnumerator() {
            foreach (ChangeSet c in _innerList) {
                yield return c;
            }
        }

        /// <summary>
        /// Gets an enumerator over this ChangeSetHistory.
        /// </summary>
        /// <returns>A new enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
