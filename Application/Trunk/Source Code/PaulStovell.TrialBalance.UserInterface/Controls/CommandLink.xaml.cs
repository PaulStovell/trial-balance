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

namespace PaulStovell.TrialBalance.UserInterface.Controls {
    /// <summary>
    /// Interaction logic for CommandLink.xaml
    /// </summary>
    public partial class CommandLink : System.Windows.Controls.UserControl {
        public CommandLink() {
            InitializeComponent();
        }

        public event RoutedEventHandler Click;

        public static readonly DependencyProperty CommandTextProperty =
            DependencyProperty.Register("CommandText", typeof(string), typeof(CommandLink), new UIPropertyMetadata(string.Empty));
        
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(CommandLink), new UIPropertyMetadata(string.Empty));

        public string CommandText {
            get { return (string)GetValue(CommandTextProperty); }
            set { SetValue(CommandTextProperty, value); }
        }

        public string Description {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public void Button_Clicked(object sender, RoutedEventArgs e) {
            OnClick(e);
        }

        protected virtual void OnClick(RoutedEventArgs e) {
            if (this.Click != null) {
                this.Click(this, e);
            }
        }
    }
}