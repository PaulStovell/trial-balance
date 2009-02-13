using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using PaulStovell.TrialBalance.DomainModel;
using PaulStovell.TrialBalance.DomainModel.PackageDataProvider;
using System.Windows.Input;

namespace PaulStovell.TrialBalance.UserInterface {
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SplashWindow() {
            InitializeComponent();
            this.MouseDown += new MouseButtonEventHandler(
                delegate(object sender, System.Windows.Input.MouseButtonEventArgs e) {
                    if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
                        this.DragMove();
                    }
                });
        }

        /// <summary>
        /// Called when the "Create new file" command button is clicked.
        /// </summary>
        private void NewFileCommand_Clicked(object sender, RoutedEventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.Filter = "TrialBalance data files (*.tbdx)|*.tbdx";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd.OverwritePrompt = true;
            sfd.Title = "New TrialBalance File";

            bool? dr = sfd.ShowDialog(this);
            if (dr ?? true) {
                PackageDataProvider dataProvider = PackageDataProvider.CreateFile(sfd.FileName);
                Workbook workbook = new Workbook(dataProvider);

                MainWindow mainWindow = new MainWindow(workbook);
                Application.Current.MainWindow = mainWindow;

                this.Close();
                mainWindow.Show();
            }
        }

        /// <summary>
        /// Called when the "Open a previous file" command button is clicked.
        /// </summary>
        private void OpenFileCommand_Clicked(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TrialBalance Data Files (*.tbdx)|*.tbdx|All Files (*.*)|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Title = "Open TrialBalance File";

            bool? dr = ofd.ShowDialog(this);
            if (dr ?? true) {
                PackageDataProvider dataProvider = PackageDataProvider.OpenFile(ofd.FileName);
                Workbook workbook = new Workbook(dataProvider);

                MainWindow mainWindow = new MainWindow(workbook);
                Application.Current.MainWindow = mainWindow;

                this.Close();
                mainWindow.Show();
            }
        }

        /// <summary>
        /// Called when the "Exit application" command button is clicked.
        /// </summary>
        private void CloseApplicationCommand_Clicked(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}