using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace PaulStovell.TrialBalance.UserInterface {
    public class BaseWindow : Window, INotifyPropertyChanged {
        private bool _hasSourceInitialized;

        public void EnableWindowDragging() {
            this.MouseDown += new MouseButtonEventHandler(
                delegate(object sender, System.Windows.Input.MouseButtonEventArgs e) {
                    if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
                        this.DragMove();
                        e.Handled = true;
                    }
                });
            this.MouseUp += new MouseButtonEventHandler(
                delegate(object sender, System.Windows.Input.MouseButtonEventArgs e) {
                    e.Handled = true;
                });
        }

        public void EnableGlass(Thickness glassThickness) {
            if (!_hasSourceInitialized) {
                this.SourceInitialized += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        GlassHelper.ExtendGlassFrame(this, glassThickness);
                    });
            } else {
                GlassHelper.ExtendGlassFrame(this, glassThickness);
            }
        }

        protected override void OnSourceInitialized(EventArgs e) {
            base.OnSourceInitialized(e);
            this._hasSourceInitialized = true;
        }

        protected void SuppressMouseDown(object sender, MouseEventArgs e) {
            e.Handled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            if (this.PropertyChanged != null) {
                this.PropertyChanged(this, e);
            }
        }
    }
}
