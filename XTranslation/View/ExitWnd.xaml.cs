using System.Windows;
using XTranslation.Properties;

namespace XTranslation.View
{
    public partial class ExitWnd : Window
    {
        public MainWindow mainWindow;

        public ExitWnd()
        {
            InitializeComponent();
        }

        private void Closebtn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (exitFlag.IsChecked == true)
            {
                Settings.Default.ClosePage = false;
                Settings.Default.CloseType = 1;
                Settings.Default.Save();
            }

            Close();
            mainWindow.Close();
        }

        private void HiddenNotify_OnClick(object sender, RoutedEventArgs e)
        {
            if (exitFlag.IsChecked == true)
            {
                Settings.Default.ClosePage = false;
                Settings.Default.CloseType = 2;
                Settings.Default.Save();
            }

            mainWindow.Hide();
            mainWindow.ShowInTaskbar = false;
            Close();
        }
    }
}