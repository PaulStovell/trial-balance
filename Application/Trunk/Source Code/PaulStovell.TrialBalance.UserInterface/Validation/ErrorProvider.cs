using System;
using System.Data;
using System.Configuration;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;
using System.Collections.Generic;
using PaulStovell.Common.BindingFramework;
using PaulStovell.Common;
using System.Collections.Specialized;

namespace PaulStovell.TrialBalance.UserInterface.Validation {
    /// <summary>
    /// A Windows Presentaion Foundation error provider.
    /// </summary>
    public class ErrorProvider : Decorator, INotifyPropertyChanged {
        private BindableCollection<PaulStovell.Common.BindingFramework.ValidationError> _errors;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ErrorProvider() {
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(ErrorProvider_DataContextChanged);
            BindToDataContext(null, this.DataContext);
        }

        /// <summary>
        /// Gets the collection of error messages that the ErrorProvider provides.
        /// </summary>
        public BindableCollection<PaulStovell.Common.BindingFramework.ValidationError> Errors {
            get {
                if (_errors == null) {
                    _errors = new BindableCollection<PaulStovell.Common.BindingFramework.ValidationError>();
                    _errors.CollectionChanged +=
                        delegate(object sender, NotifyCollectionChangedEventArgs e) {
                            this.OnPropertyChanged(new PropertyChangedEventArgs("HasErrors"));
                            this.OnPropertyChanged(new PropertyChangedEventArgs("IsValid"));
                        };
                }
                return _errors;
            }
            protected set {
                _errors = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Errors"));
            }
        }

        public bool HasErrors {
            get { return this.Errors.Count > 0; }
        }

        public bool IsValid {
            get { return this.Errors.Count == 0; }
        }

        /// <summary>
        /// Occurs when a property on this ErrorProvider changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when this ErrorProvider's DataContext changes.
        /// </summary>
        private void ErrorProvider_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            BindToDataContext(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// Subscribes to the DataContext's PropertyChanged event and uses it to keep in sync with the error list.
        /// </summary>
        private void BindToDataContext(object oldDataContext, object newDataContext) {
            if (oldDataContext != null) {
                if (oldDataContext is INotifyPropertyChanged) {
                    ((INotifyPropertyChanged)oldDataContext).PropertyChanged -= new PropertyChangedEventHandler(DataContext_PropertyChanged);
                }
            }
            if (newDataContext != null) {
                if (newDataContext is INotifyPropertyChanged) {
                    ((INotifyPropertyChanged)newDataContext).PropertyChanged += new PropertyChangedEventHandler(DataContext_PropertyChanged);
                }
            }

            this.Validate();
        }

        /// <summary>
        /// Called when a property on the current DataContext changes.
        /// </summary>
        private void DataContext_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            Validate();
        }

        /// <summary>
        /// Validates the current DataContext and updates any displayed errors.
        /// </summary>
        public bool Validate() {
            bool result = true;
            IValidateable dataContext = this.DataContext as IValidateable;
            if (dataContext != null) {
                PaulStovell.Common.BindingFramework.ValidationError[] errors = dataContext.GetValidationErrors();
                result = errors.Length == 0;
                this.Errors.Clear();
                this.Errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, e);
            }
        }
    }
}
