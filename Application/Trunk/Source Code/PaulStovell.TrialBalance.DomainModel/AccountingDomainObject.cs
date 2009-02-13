using System;
using System.Collections.Generic;
using PaulStovell.Common;
using PaulStovell.Common.BindingFramework;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.DomainModel {
    /// <summary>
    /// A base class for all accounting domain-related objects. 
    /// </summary>
    public abstract class AccountingDomainObject : PaulStovell.Common.BindingFramework.DomainObject {
        private DateTime _updatedDate = DateTime.MinValue;
        private DateTime _createdDate;
        private string _updatedByUsername;
        private string _createdByUsername;
        private Workbook _workbook;
        private ChangeSetHistory _changeSetHistory;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AccountingDomainObject() {
            _createdDate = DateTime.Now;
            _createdByUsername = Environment.UserName;
        }

        /// <summary>
        /// Raised by the PropertyChanged event when the CreatedDate property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs CreatedDatePropertyChangedEventArgs = new PropertyChangedEventArgs("CreatedDate");

        /// <summary>
        /// Raised by the PropertyChanged event when the CreatedByUsername property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs CreatedByUsernamePropertyChangedEventArgs = new PropertyChangedEventArgs("CreatedByUsername");

        /// <summary>
        /// Raised by the PropertyChanged event when the UpdatedDate property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs UpdatedDatePropertyChangedEventArgs = new PropertyChangedEventArgs("UpdatedDate");

        /// <summary>
        /// Raised by the PropertyChanged event when the UpdatedByUsername property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs UpdatedByUsernamePropertyChangedEventArgs = new PropertyChangedEventArgs("UpdatedByUsername");
	
        /// <summary>
        /// Gets or sets whether or not this domain object is read-only. The default is true. When the domain object is read-only, 
        /// any attempt to change properties on the object will raise an AccessViolationException.
        /// </summary>
        internal new bool IsReadOnly {
            get { return base.IsReadOnly; }
            set { base.IsReadOnly = value; }
        }

        /// <summary>
        /// Gets or sets the workbook that was used to create this object.
        /// </summary>
        public Workbook Workbook
        {
            get { return _workbook; }
            internal set { _workbook = value; }
        }

        /// <summary>
        /// Gets or sets the time this item was last updated in the data store.
        /// </summary>
        public virtual DateTime UpdatedDate {
            get { return (_updatedDate == DateTime.MinValue) ? CreatedDate : _updatedDate; }
            set {
                AssertCanEdit();
                _updatedDate = value;
                NotifyChanged(UpdatedByUsernamePropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets or sets the time this item was created in the data store.
        /// </summary>
        public virtual DateTime CreatedDate {
            get { return _createdDate; }
            internal set {
                _createdDate = value;
                NotifyChanged(CreatedDatePropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets or sets the name of the user to last update this item in the data store.
        /// </summary>
        public virtual string UpdatedByUsername {
            get {
                if (CleanString(_updatedByUsername).Length == 0) {
                    return this.CreatedByUsername;
                }
                else {
                    return CleanString(_updatedByUsername);
                }
            }
            set {
                AssertCanEdit();
                _updatedByUsername = CleanString(value);
                NotifyChanged(UpdatedByUsernamePropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets the name of the user who created this item in the data store.
        /// </summary>
        public virtual string CreatedByUsername {
            get { return _createdByUsername; }
            internal set {
                _createdByUsername = CleanString(value);
                NotifyChanged(CreatedByUsernamePropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets the history of changes to this object.
        /// </summary>
        public ChangeSetHistory ChangeSetHistory {
            get {
                if (_changeSetHistory == null) {
                    _changeSetHistory = new ChangeSetHistory();
                }
                return _changeSetHistory;
            }
            internal set {
                _changeSetHistory = value;
            }
        }

        /// <summary>
        /// Override this method to add rules to this domain object.
        /// </summary>
        protected override List<Rule> CreateRules() {
            List<Rule> rules = base.CreateRules();
            rules.Add(new SimpleRule(CreatedDatePropertyChangedEventArgs.PropertyName, "The creation date can never be blank or set that low.", delegate { return this.CreatedDate != DateTime.MinValue; }));
            rules.Add(new SimpleRule(CreatedByUsernamePropertyChangedEventArgs.PropertyName, "The creator user can never be blank.", delegate { return this.CreatedByUsername.Length > 0; }));
            rules.Add(new SimpleRule(UpdatedDatePropertyChangedEventArgs.PropertyName, "The last updated date can never be blank or set that low.", delegate { return this.UpdatedDate != DateTime.MinValue; }));
            rules.Add(new SimpleRule(UpdatedByUsernamePropertyChangedEventArgs.PropertyName, "The last updating user can never be blank.", delegate { return this.UpdatedByUsername.Length > 0; }));
            return rules;
        }

        /// <summary>
        /// Copies the properties of this object to those of a new object.
        /// </summary>
        /// <param name="targetObject">The object to copy the properties to.</param>
        public override void CopyTo(object targetObject) {
            base.CopyTo(targetObject);

            AccountingDomainObject accountingObject = targetObject as AccountingDomainObject;
            if (accountingObject != null) {
                accountingObject.CreatedByUsername = this.CreatedByUsername;
                accountingObject.CreatedDate = this.CreatedDate;
                accountingObject.UpdatedByUsername = this.UpdatedByUsername;
                accountingObject.UpdatedDate = this.UpdatedDate;
                if (this.Workbook != null) {
                    accountingObject.Workbook = this.Workbook;
                }
            }
        }
    }
}