using System;
using System.Data;
using System.Configuration;
using System.ComponentModel;
using PaulStovell.Common.BindingFramework;
using System.Reflection;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// An ChangeCoordinator applies to a domain object and makes it editable until the changes are pushed back. 
    /// </summary>
    /// <typeparam name="T">The type of object this lock applies to.</typeparam>
    public sealed class ChangeCoordinator<T> where T : AccountingDomainObject {
        private T _editableItem;
        private T _originalItem;
        private bool _hasPushedChanges;

        /// <summary>
        /// Occurs just before changes have been copied from the editable item to the original item.
        /// </summary>
        internal event ChangeCoordinatorChangesPushingEventHandler<T> ChangesPushing;

        /// <summary>
        /// Occurs just after changes have been copied from the editable item to the original item.
        /// </summary>
        internal event ChangeCoordinatorChangesPushedEventHandler<T> ChangesPushed;

        /// <summary>
        /// Constructor.
        /// </summary>
        private ChangeCoordinator() {

        }

        /// <summary>
        /// Gets the editable version of the item. This item is writable, and any changes will be copied back to the 
        /// original object when "PushChanges" is called. 
        /// </summary>
        public T EditableItem {
            get {
                EnsureNotYetPushed();
                return _editableItem; 
            }
            private set { _editableItem = value; }
        }

        /// <summary>
        /// Gets the original object that was locked. This object is read-only and any attempts to change 
        /// it will result in an exception.
        /// </summary>
        public T OriginalItem {
            get {
                EnsureNotYetPushed();
                return _originalItem;
            }
            private set { _originalItem = value; }
        }

        /// <summary>
        /// Saves any changes to the EditableItem back to the OriginalItem.
        /// </summary>
        public void PushChanges() {
            EnsureNotYetPushed();

            ChangeCoordinatorChangesPushingEventArgs<T> arguments = new ChangeCoordinatorChangesPushingEventArgs<T>(this);
            OnChangesPushing(arguments);
            if (!arguments.Cancel) {
                // Generate a changeset for auditing based on the snapshots of the original and editable object. This is done by 
                // looking for properties marked with the [Auditable] attribute.
                ChangeSet auditHistory = new ChangeSet();
                auditHistory.Applied = DateTime.Now;
                auditHistory.Username = Environment.UserName;
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties()) {
                    object[] auditableAttributes = propertyInfo.GetCustomAttributes(typeof(AuditableAttribute), true);
                    if (auditableAttributes.Length > 0) {
                        Change historyItem = new Change();
                        historyItem.PropertyName = ((AuditableAttribute)auditableAttributes[0]).Description;
                        historyItem.OldValue = string.Empty;
                        if (this.OriginalItem != null) {
                            historyItem.OldValue = (propertyInfo.GetValue(this.OriginalItem, null) ?? string.Empty).ToString();
                        }
                        historyItem.NewValue = (propertyInfo.GetValue(this.EditableItem, null) ?? string.Empty).ToString();
                        if (historyItem.OldValue != historyItem.NewValue) {
                            auditHistory.Changes.Add(historyItem);
                        }
                    }
                }

                // Now apply the changes
                if (this.OriginalItem != null) {
                    bool wasReadOnly = this.OriginalItem.IsReadOnly;
                    this.OriginalItem.IsReadOnly = false;
                    this.EditableItem.CopyTo(this.OriginalItem);
                    this.OriginalItem.IsReadOnly = wasReadOnly;
                    this.OriginalItem.ChangeSetHistory.Append(auditHistory);
                } else {
                    this.EditableItem.ChangeSetHistory.Append(auditHistory);
                }
                this.EditableItem.IsReadOnly = true;

                this.OnChangesPushed(new ChangeCoordinatorChangesPushedEventArgs<T>(this));
                _hasPushedChanges = true;
            }
        }


        /// <summary>
        /// Raises the ChangesPushing event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        internal void OnChangesPushing(ChangeCoordinatorChangesPushingEventArgs<T> e) {
            if (this.ChangesPushing != null) {
                this.ChangesPushing(this, e);
            }
        }

        /// <summary>
        /// Raises the ChangesPushed event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        internal void OnChangesPushed(ChangeCoordinatorChangesPushedEventArgs<T> e) {
            if (this.ChangesPushed != null) {
                this.ChangesPushed(this, e);
            }
        }

        /// <summary>
        /// Ensures PushChanges can never be called twice, and that the ChangeCoordinator cannot be used after PushChanges has been called.
        /// </summary>
        private void EnsureNotYetPushed() {
            if (_hasPushedChanges) {
                throw new InvalidOperationException("The changes have already been pushed once and cannot be pushed again.");
            }
        }


        /// <summary>
        /// Acquires an ChangeCoordinator on a given object.
        /// </summary>
        /// <param name="newObject">The new object that will be editable.</param>
        /// <returns>A new ChangeCoordinator containing the given items.</returns>
        internal static ChangeCoordinator<T> Acquire(T newObject) {
            return Acquire(null, newObject);
        }

        /// <summary>
        /// Acquires an ChangeCoordinator on a given object.
        /// </summary>
        /// <param name="targetObject">The original object being locked.</param>
        /// <param name="newObject">The new object that will be editable.</param>
        /// <returns>A new ChangeCoordinator containing the given items.</returns>
        internal static ChangeCoordinator<T> Acquire(T targetObject, T newObject) {
            if (newObject == null) {
                throw new NullReferenceException("newObject cannot be null when attempting to acquire a lock.");
            }

            if (newObject == targetObject) {
                throw new NullReferenceException("newObject and targetObject cannot reference the same object when attempting to acquire a lock.");
            }

            ChangeCoordinator<T> result = new ChangeCoordinator<T>();

            newObject.IsReadOnly = false;
            
            if (targetObject != null) {
                targetObject.CopyTo(newObject);
            }

            result.EditableItem = newObject;
            result.OriginalItem = targetObject;

            return result;
        }
    }
}
