using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {

    /// <summary>
    /// A delegate used to raise events when Edit Locks have finished pushing changes.
    /// </summary>
    /// <typeparam name="T">The type of item that has been locked.</typeparam>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    public delegate void ChangeCoordinatorChangesPushedEventHandler<T>(object sender, ChangeCoordinatorChangesPushedEventArgs<T> e) where T : AccountingDomainObject;

    /// <summary>
    /// Event arguments for the ChangeCoordinatorChangesPushedEventHandler.
    /// </summary>
    /// <typeparam name="T">The type of item that has been locked.</typeparam>
    public class ChangeCoordinatorChangesPushedEventArgs<T> : EventArgs where T : AccountingDomainObject {
        private ChangeCoordinator<T> _ChangeCoordinator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ChangeCoordinator">The edit lock involved in the event.</param>
        public ChangeCoordinatorChangesPushedEventArgs(ChangeCoordinator<T> ChangeCoordinator) {
            _ChangeCoordinator = ChangeCoordinator;
        }

        /// <summary>
        /// Gets the ChangeCoordinator involved in the event.
        /// </summary>
        public ChangeCoordinator<T> ChangeCoordinator {
            get { return _ChangeCoordinator; }
        }
    }
}
