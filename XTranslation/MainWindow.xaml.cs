using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using CommunityToolkit.Mvvm.Messaging;
using HookManager;
using XTranslation.Model;
using XTranslation.Model.Message;
using XTranslation.Properties;
using XTranslation.Utils;
using XTranslation.View;
using XTranslation.ViewModel;

namespace XTranslation
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr hwnd = new WindowInteropHelper(App.Current.MainWindow).Handle;
            WinIterop.RegisterHotKey(hwnd, ModifierKeys.Alt, Key.Q, HotKeyHandler);
            WinIterop.RegisterHotKey(hwnd, ModifierKeys.Alt, Key.W, HotKeyHandler);
            WinIterop.RegisterHotKey(hwnd, ModifierKeys.Alt, Key.E, HotKeyHandler);
        }

        void HotKeyHandler(int flag)
        {
            switch (flag)
            {
                case (int)ModifierKeys.Alt+(int)Key.Q:
                {
                    WeakReferenceMessenger.Default.Send(new MyMessage(MyAction.HotKey_TranslationBtn), (int)MyAction.HotKey_TranslationBtn);
                    break;
                }
                case (int)ModifierKeys.Alt+(int)Key.W:
                {
                    WeakReferenceMessenger.Default.Send(new MyMessage(MyAction.HotKey_TopTransWndBtn), (int)MyAction.HotKey_TopTransWndBtn);
                    break;
                }
                case(int)ModifierKeys.Alt+(int)Key.E:
                {
                    WeakReferenceMessenger.Default.Send(new MyMessage(MyAction.HotKey_WndPenetrateBtn), (int)MyAction.HotKey_WndPenetrateBtn);
                    break;
                }
            }
        }
        

        private void NotifyIcon_OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = true;
            Show();
        }

        private void Open_Notify_OnClick(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = true;
            Show();
        }

        private void Exit_Notify_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InfoBtn_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var infoWnd = new InfoWnd();
            infoWnd.Owner = this;
            infoWnd.Show();
        }

        #region 标题栏行为

        private void MainWindow_OnStateChanged(object sender, EventArgs e)
        {
            var window = sender as Window;
            switch (window.WindowState)
            {
                case WindowState.Maximized:
                {
                    mainGrid.Margin = new Thickness(8, 8, 8, 8);
                    title_bar.Height = 26;
                    viewModel.captionHeight = 30;
                }
                    break;
                case WindowState.Normal:
                {
                    mainGrid.Margin = new Thickness(0, 0, 0, 0);
                    title_bar.Height = 30;
                    viewModel.captionHeight = 26;
                }
                    break;
            }
        }

        private void MiniBtn_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.ClosePage)
            {
                var exitWnd = new ExitWnd();
                exitWnd.Owner = this;
                exitWnd.mainWindow = mainWinow;
                exitWnd.ShowDialog();
            }
            else
            {
                if (Settings.Default.CloseType == 1)
                {
                    Close();
                }
                else if (Settings.Default.CloseType == 2)
                {
                    Hide();
                    ShowInTaskbar = false;
                }
            }
        }

        #endregion
    }
}