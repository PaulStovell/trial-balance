using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace PaulStovell.TrialBalance.UserInterface.Validation {
    /// <summary>
    /// An error displayer designed to display error messages on text boxes.
    /// </summary>
    public class TextBoxErrorDisplayStrategy : IErrorDisplayStrategy {
        private Dictionary<FrameworkElement, Brush> _savedBrushes;
        private Dictionary<FrameworkElement, string> _savedToolTips;
        private Color _errorBorderColor;
        private static readonly Color _errorBorderColorDefault = Color.FromRgb(0xFF, 0x42, 0x2F);

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextBoxErrorDisplayStrategy() {
            _savedBrushes = new Dictionary<FrameworkElement, Brush>();
            _savedToolTips = new Dictionary<FrameworkElement, string>();
            _errorBorderColor = _errorBorderColorDefault;
        }

        /// <summary>
        /// Gets or sets the colour that borders will be set to when an error is found on a text box field.
        /// </summary>
        [Description("The colour that borders will be set to when an error is found on a text box field.")]
        public Color ErrorBorderColor {
            get { return _errorBorderColor; }
            set { _errorBorderColor = value; }
        }

        /// <summary>
        /// Indicates whether we can display error messages for a given element.
        /// </summary>
        /// <param name="element">The element that needs to be validated.</param>
        /// <returns>True if this class can display error messages for the given element, otherwise false.</returns>
        public bool CanDisplayForElement(FrameworkElement element) {
            return element is TextBox;
        }

        /// <summary>
        /// Displays the error message on a given element.
        /// </summary>
        /// <param name="element">The element to display the error on.</param>
        /// <param name="errorMessage">The error message to display.</param>
        public void DisplayError(FrameworkElement element, string errorMessage) {
            TextBox textBox = (TextBox)element;

            if (!_savedBrushes.ContainsKey(element)) {
                _savedBrushes.Add(element, (Brush)textBox.GetValue(TextBox.BorderBrushProperty));
            }
            if (!_savedToolTips.ContainsKey(element)) {
                _savedToolTips.Add(element, (string)textBox.GetValue(TextBox.ToolTipProperty));
            }

            textBox.SetValue(TextBox.BorderBrushProperty, new SolidColorBrush(_errorBorderColor));
            textBox.SetValue(TextBox.ToolTipProperty, errorMessage);
        }

        /// <summary>
        /// Clears any error messages on an element.
        /// </summary>
        /// <param name="element">The element to clear any error messages for.</param>
        public void ClearError(FrameworkElement element) {
            TextBox textBox = (TextBox)element;

            if (_savedBrushes.ContainsKey(element)) {
                textBox.SetValue(TextBox.BorderBrushProperty, _savedBrushes[element]);
                _savedBrushes.Remove(element);
            }
            if (_savedToolTips.ContainsKey(element)) {
                textBox.SetValue(TextBox.ToolTipProperty, _savedToolTips[element]);
                _savedToolTips.Remove(element);
            }
        }
    }
}
