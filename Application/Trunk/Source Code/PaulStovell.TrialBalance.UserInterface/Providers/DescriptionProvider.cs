using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

namespace PaulStovell.TrialBalance.UserInterface.Providers {
    public static class DescriptionProvider {
        public static string GetDescriptionText(DependencyObject obj) {
            return (string)obj.GetValue(DescriptionTextProperty);
        }

        public static void SetDescriptionText(DependencyObject obj, string value) {
            obj.SetValue(DescriptionTextProperty, value);
        }

        public static readonly DependencyProperty DescriptionTextProperty =
            DependencyProperty.RegisterAttached("DescriptionText", typeof(string), typeof(DescriptionProvider), new UIPropertyMetadata(string.Empty,
            DescriptionTextProperty_PropertyChanged));

        private static void DescriptionTextProperty_PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e) {
            TextBox textBox = dependencyObject as TextBox;
            if (textBox != null) {
                RememberHasText(textBox);
                textBox.TextChanged += new TextChangedEventHandler(TextBox_TextChanged);
            }
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            TextBox textBox = e.Source as TextBox;
            if (textBox != null) {
                RememberHasText(textBox);
            }
        }

        private static void RememberHasText(TextBox textBox) {
            bool hasText = (textBox.Text != null && textBox.Text.Length > 0);
            textBox.SetValue(HasTextProperty, hasText);
        }

        public static readonly DependencyPropertyKey HasTextProperty = DependencyProperty.RegisterAttachedReadOnly("HasText", typeof(bool), typeof(DescriptionProvider),
            new PropertyMetadata(false));

        public static bool GetHasText(DependencyObject o) {
            return (bool)o.GetValue(HasTextProperty.DependencyProperty);
        }



        public static HorizontalAlignment GetDescriptionHorizontalAlignment(DependencyObject obj) {
            return (HorizontalAlignment)obj.GetValue(DescriptionHorizontalAlignmentProperty);
        }

        public static void SetDescriptionHorizontalAlignment(DependencyObject obj, HorizontalAlignment value) {
            obj.SetValue(DescriptionHorizontalAlignmentProperty, value);
        }

        public static readonly DependencyProperty DescriptionHorizontalAlignmentProperty =
            DependencyProperty.RegisterAttached("DescriptionHorizontalAlignment", typeof(HorizontalAlignment), typeof(DescriptionProvider), new UIPropertyMetadata(HorizontalAlignment.Left));




        public static VerticalAlignment GetDescriptionVerticalAlignment(DependencyObject obj) {
            return (VerticalAlignment)obj.GetValue(DescriptionVerticalAlignmentProperty);
        }

        public static void SetDescriptionVerticalAlignment(DependencyObject obj, VerticalAlignment value) {
            obj.SetValue(DescriptionVerticalAlignmentProperty, value);
        }

        public static readonly DependencyProperty DescriptionVerticalAlignmentProperty =
            DependencyProperty.RegisterAttached("DescriptionVerticalAlignment", typeof(VerticalAlignment), typeof(DescriptionProvider), new UIPropertyMetadata(VerticalAlignment.Top));
    }
}
