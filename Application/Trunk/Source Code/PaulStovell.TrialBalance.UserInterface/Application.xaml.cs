using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Windows.Navigation;
using System.Windows.Controls;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// Interaction logic for MyApp.xaml
    /// </summary>
    public partial class Application : System.Windows.Application {
        /// <summary>
        /// Called when the application starts.
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e) {
            SplashWindow splash = new SplashWindow();
            splash.Show();
        }
    }
}