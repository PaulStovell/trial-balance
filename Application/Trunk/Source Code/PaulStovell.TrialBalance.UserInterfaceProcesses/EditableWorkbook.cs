using System;
using System.Collections.Generic;
using System.Text;
using PaulStovell.Common.BindingFramework;
using PaulStovell.TrialBalance.DomainModel;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.UserInterfaceProcesses {
    public class EditableWorkbook : DomainObject {
        private string _businessName;
        private string _legalName;
        private string _userName;
        private Workbook _workbook;
        private PeriodLength _periodLength;
        private DateTime _periodStartDate;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="workbook">The workbook being edited.</param>
        public EditableWorkbook(Workbook workbook) {
            this.Workbook = workbook;
        }

        /// <summary>
        /// Raised by the PropertyChanged event when the BusinessName property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs BusinessNamePropertyChangedEventArgs = new PropertyChangedEventArgs("BusinessName");

        /// <summary>
        /// Raised by the PropertyChanged event when the LegalName property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs LegalNamePropertyChangedEventArgs = new PropertyChangedEventArgs("LegalName");

        /// <summary>
        /// Raised by the PropertyChanged event when the UserName property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs UserNamePropertyChangedEventArgs = new PropertyChangedEventArgs("UserName");

        /// <summary>
        /// Raised by the PropertyChanged event when the PeriodLength property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs PeriodLengthPropertyChangedEventArgs = new PropertyChangedEventArgs("PeriodLength");

        /// <summary>
        /// Raised by the PropertyChanged event when the PeriodStartDate property changes.
        /// </summary>
        public static readonly PropertyChangedEventArgs PeriodStartDatePropertyChangedEventArgs = new PropertyChangedEventArgs("PeriodStartDate");

        /// <summary>
        /// Gets the underlying Workbook being edited by this EditableWorkbook.
        /// </summary>
        public Workbook Workbook {
            get { return _workbook; }
            protected set { 
                _workbook = value;
                if (_workbook != null) {
                    this.BusinessName = _workbook.BusinessName;
                    this.LegalName = _workbook.LegalName;
                    this.UserName = _workbook.UserName;
                    this.PeriodStartDate = _workbook.PeriodStartDate;
                    this.PeriodLength = _workbook.PeriodLength;
                    this.IsDirty = false;
                }
            }
        }

        public string BusinessName {
            get { return _businessName ?? string.Empty; }
            set {
                _businessName = value;
                NotifyChanged(BusinessNamePropertyChangedEventArgs);
            }
        }

        public string LegalName {
            get { return _legalName ?? string.Empty; }
            set {
                _legalName = value;
                NotifyChanged(LegalNamePropertyChangedEventArgs);
            }
        }

        public string UserName {
            get { return _userName ?? string.Empty; }
            set {
                _userName = value;
                NotifyChanged(UserNamePropertyChangedEventArgs);
            }
        }

        public PeriodLength PeriodLength {
            get { return _periodLength; }
            set {
                _periodLength = value;
                NotifyChanged(PeriodLengthPropertyChangedEventArgs);
            }
        }

        public DateTime PeriodStartDate {
            get { return _periodStartDate; }
            set {
                _periodStartDate = value;
                NotifyChanged(PeriodStartDatePropertyChangedEventArgs);
            }
        }

        public void CommitChanges() {
            if (this.Workbook != null) {
                this.Workbook.BusinessName = this.BusinessName;
                this.Workbook.LegalName = this.LegalName;
                this.Workbook.UserName = this.UserName;
                this.Workbook.PeriodLength = this.PeriodLength;
                this.Workbook.PeriodStartDate = this.PeriodStartDate;
                this.Workbook.CurrentPeriod = Period.CalculatePeriodForDate(DateTime.Now, this.Workbook.PeriodStartDate, this.Workbook.PeriodLength);
                this.IsDirty = false;
            }
        }

        protected override List<Rule> CreateRules() {
            List<Rule> rules = base.CreateRules();
            rules.Add(new SimpleRule("BusinessName", "A business name is required.", delegate { return this.BusinessName.Length > 0; }));
            rules.Add(new SimpleRule("LegalName", "A legal name is required.", delegate { return this.LegalName.Length > 0; }));
            rules.Add(new SimpleRule("UserName", "A user name is required.", delegate { return this.UserName.Length > 0; }));
            return rules;
        }
    }
}
