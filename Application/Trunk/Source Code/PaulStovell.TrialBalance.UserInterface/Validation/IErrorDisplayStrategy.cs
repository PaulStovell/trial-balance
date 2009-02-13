using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PaulStovell.TrialBalance.UserInterface.Validation {
    /// <summary>
    /// Implemented by any classes that are used to display errors from an ErrorProvider.
    /// </summary>
    public interface IErrorDisplayStrategy {
        /// <summary>
        /// Indicates whether we can display error messages for a given element.
        /// </summary>
        /// <param name="element">The element that needs to be validated.</param>
        /// <returns>True if this class can display error messages for the given element, otherwise false.</returns>
        bool CanDisplayForElement(FrameworkElement element);

        /// <summary>
        /// Displays the error message on a given element.
        /// </summary>
        /// <param name="element">The element to display the error on.</param>
        /// <param name="errorMessage">The error message to display.</param>
        void DisplayError(FrameworkElement element, string errorMessage);

        /// <summary>
        /// Clears any error messages on an element.
        /// </summary>
        /// <param name="element">The element to clear any error messages for.</param>
        void ClearError(FrameworkElement element);
    }

}
