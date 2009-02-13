using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PaulStovell.TrialBalance.DomainModel;
using System.ComponentModel;
using PaulStovell.Common;
using System.Collections.Specialized;
using PaulStovell.TrialBalance.UserInterfaceProcesses;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// Interaction logic for WorkbookDetailsPage.xaml
    /// </summary>
    public partial class WorkbookDetailsPage : WizardPage {
        private BindableCollection<BindableEnumerationValue> _availablePeriodLengths;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="workbook">The workbook being modified</param>
        public WorkbookDetailsPage(Workbook workbook) {
            InitializeComponent();
            this.Cancelling += new CancelEventHandler(Page_Cancelling);
            this.EditableWorkbook = new EditableWorkbook(workbook);
        }

        /// <summary>
        /// Gets or sets the workbook that is used for this page.
        /// </summary>
        public EditableWorkbook EditableWorkbook {
            get { return this.DataContext as EditableWorkbook; }
            set {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// Gets the list of available cate
        /// </summary>
        public BindableCollection<BindableEnumerationValue> AvailablePeriodLengths {
            get {
                if (_availablePeriodLengths == null) {
                    _availablePeriodLengths = new BindableCollection<BindableEnumerationValue>();
                    _availablePeriodLengths.AddRange(BindableEnumerationValue.BuildCollection(new PeriodLength()));
                    _availablePeriodLengths.CollectionChanged += new NotifyCollectionChangedEventHandler(
                        delegate(object sender, NotifyCollectionChangedEventArgs e) {
                            this.OnPropertyChanged(new PropertyChangedEventArgs("AvailablePeriodLengths"));
                        });
                }
                return _availablePeriodLengths;
            }
        }

        /// <summary>
        /// Called when the "Cancel" button is clicked.
        /// </summary>
        private void Page_Cancelling(object sender, CancelEventArgs e) {
            if (this.EditableWorkbook.IsDirty) {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Are you sure you wish to cancel?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No) {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Called when the Save button is clicked.
        /// </summary>
        public override void Next() {
            if (_errorProvider.Validate() == true) {
                this.EditableWorkbook.CommitChanges();
                this.OnReturn(new WizardPageReturnEventArgs(null, false));
            }
        }
    }
}