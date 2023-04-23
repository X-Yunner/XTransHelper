using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Data;
using XTranslation.Model;
using XTranslation.Model.Message;
using XTranslation.Properties;
using XTranslation.Utils;
using XTranslation.ViewModel;

namespace XTranslation.View
{
    /// <summary>
    ///     UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class Index : UserControl
    {
        private void Set_IdAndKey(TranslationPlatformEnum flag, bool reverse = false)
        {
            if (!reverse)
            {
                switch (flag)
                {
                    case TranslationPlatformEnum.BaiDu:
                    {
                        IdPasswordBox.Password = Settings.Default.BaiDu_Id;
                        KeyPasswordBox.Password = Settings.Default.BaiDu_Key;
                        break;
                    }
                    case TranslationPlatformEnum.HuoShan:
                    {
                        IdPasswordBox.Password = Settings.Default.HuoShan_Id;
                        KeyPasswordBox.Password = Settings.Default.HuoShan_Key;
                        break;
                    }
                    case TranslationPlatformEnum.YouDao:
                    {
                        IdPasswordBox.Password = Settings.Default.YouDao_Id;
                        KeyPasswordBox.Password = Settings.Default.YouDao_Key;
                        break;
                    }
                    case TranslationPlatformEnum.Tencent:
                    {
                        IdPasswordBox.Password = Settings.Default.Tencent_Id;
                        KeyPasswordBox.Password = Settings.Default.Tencend_Key;
                        break;
                    }
                    case TranslationPlatformEnum.CaiYun:
                    {
                        IdPasswordBox.Password = Settings.Default.CaiYun_Id;
                        KeyPasswordBox.Password = Settings.Default.CaiYun_Key;
                        break;
                    }
                }
            }
            else
            {
                switch (flag)
                {
                    case TranslationPlatformEnum.BaiDu:
                    {
                        Settings.Default.BaiDu_Id = IdPasswordBox.Password;
                        Settings.Default.BaiDu_Key = KeyPasswordBox.Password;
                        break;
                    }
                    case TranslationPlatformEnum.YouDao:
                    {
                        Settings.Default.YouDao_Id = IdPasswordBox.Password;
                        Settings.Default.YouDao_Key = KeyPasswordBox.Password;
                        break;
                    }
                    case TranslationPlatformEnum.HuoShan:
                    {
                        Settings.Default.HuoShan_Id = IdPasswordBox.Password;
                        Settings.Default.HuoShan_Key = KeyPasswordBox.Password;
                        break;
                    }
                    case TranslationPlatformEnum.Tencent:
                    {
                        Settings.Default.Tencent_Id = IdPasswordBox.Password;
                        Settings.Default.Tencend_Key = KeyPasswordBox.Password;
                        break;
                    }
                    case TranslationPlatformEnum.CaiYun:
                    {
                        Settings.Default.CaiYun_Id = IdPasswordBox.Password;
                        Settings.Default.CaiYun_Key = KeyPasswordBox.Password;
                        break;
                    }
                }

                Settings.Default.Save();
            }
        }


        private readonly TranslationWnd translationWnd = new TranslationWnd();
        public IndexViewModel viewModel = new IndexViewModel();

        public Index()
        {
            InitializeComponent();
            DataContext = viewModel;
            WeakReferenceMessenger.Default.Register<MyMessage, int>(this, (int)MyAction.Exit_TranslationWnd,
                MsgHandler);
            WeakReferenceMessenger.Default.Register<MyMessage, int>(translationWnd, (int)MyAction.Trans_TranslationWnd,
                translationWnd.MsgHandler); 
            
            WeakReferenceMessenger.Default.Register<MyMessage, int>(this, (int)MyAction.HotKey_TranslationBtn,
               MsgHandler); 
            WeakReferenceMessenger.Default.Register<MyMessage, int>(this, (int)MyAction.HotKey_TopTransWndBtn,
                MsgHandler); 
            WeakReferenceMessenger.Default.Register<MyMessage, int>(this, (int)MyAction.HotKey_WndPenetrateBtn,
                MsgHandler);
            InititalSettings();
        }
        
        private void F1dw()
        {
            TranslationBtn.IsChecked = !TranslationBtn.IsChecked;
        }

        private void InititalSettings()
        {
            bgColorSelecter.SelectBrush =
                new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Default.BgColor));
            fontColorSelecter.SelectBrush =
                new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Default.FontColor));
            fontsizeSelecter.SelectedIndex = Settings.Default.Fontsize - 1;

            TopTransWndBtn.IsChecked = Settings.Default.TopTransWnd;
            WndPenetrateBtn.IsChecked = Settings.Default.WndPenetrate;
            TranslationPlatformComboBox.SelectedIndex = Settings.Default.Translation_Platform;
            TranslationFromComboBox.SelectedIndex = Settings.Default.Translation_From;
            TranslationToComboBox.SelectedIndex = Settings.Default.Translation_To;

            translationWnd.SourceInitialized += translationWnd_Initialized;
            translationWnd.resizebBar.Background =
                new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Default.ReszieBarColor));
            translationWnd.viewModel.fontsize = Settings.Default.Fontsize;
            Set_IdAndKey((TranslationPlatformEnum)Settings.Default.Translation_Platform);
            translationWnd.Width = Settings.Default.TransWndWidth;
            translationWnd.Height = Settings.Default.TransWndHeight;
            translationWnd.transGridcol1.Width = new GridLength(Settings.Default.TransWndTB_Width);
            translationWnd.transGridcol3.Width = new GridLength(translationWnd.Width -
                                                                translationWnd.resizebBar.Width
                                                                - Settings.Default.TransWndTB_Width);
            if (Settings.Default.TransWnd_Left != -1)
            {
                translationWnd.Left = Settings.Default.TransWnd_Left;
                translationWnd.Top = Settings.Default.TransWnd_Top;
            }
        }

        private void MsgHandler(object receive, MyMessage message)
        {
            switch (message.movement)
            {
                case MyAction.Exit_TranslationWnd:
                {
                    TranslationBtn.IsChecked = false;
                    break;
                }
                case MyAction.HotKey_TranslationBtn:
                {
                    TranslationBtn.IsChecked = !TranslationBtn.IsChecked;
                    break;
                }
                case MyAction.HotKey_TopTransWndBtn:
                {
                    TopTransWndBtn.IsChecked = !TopTransWndBtn.IsChecked;
                    break;
                }
                case MyAction.HotKey_WndPenetrateBtn:
                {
                    WndPenetrateBtn.IsChecked = !WndPenetrateBtn.IsChecked;
                    break;
                }
            }
        }

        private void translationWnd_Initialized(object sender, EventArgs e)
        {
            translationWnd.Topmost = (bool)TopTransWndBtn.IsChecked;
            var hwnd = new WindowInteropHelper(translationWnd).Handle;
            WinIterop.SetWindowPenetrate(hwnd, (bool)WndPenetrateBtn.IsChecked);
            translationWnd.MoveBtn.Visibility =
                (bool)WndPenetrateBtn.IsChecked ? Visibility.Collapsed : Visibility.Visible;
        }

        private void TranslationBtn_OnChecked(object sender, RoutedEventArgs e)
        {
            AuthenticationStackPanel.IsEnabled = false;
            translationWnd.ID = IdPasswordBox.Password;
            translationWnd.KEY = KeyPasswordBox.Password;
            translationWnd.translationPlatformEnum = (TranslationPlatformEnum)TranslationPlatformComboBox.SelectedIndex;
            translationWnd.From =
                TranslationOptions.ChineseToStr(TranslationFromComboBox.Text, translationWnd.translationPlatformEnum);
            translationWnd.To =
                TranslationOptions.ChineseToStr(TranslationToComboBox.Text, translationWnd.translationPlatformEnum);
            translationWnd.ShowEx();
        }

        private void TranslationBtn_OnUnchecked(object sender, RoutedEventArgs e)
        {
            AuthenticationStackPanel.IsEnabled = true;
            translationWnd.HideEx();
        }

        private void TopTransWndBtn_OnChecked(object sender, RoutedEventArgs e)
        {
            translationWnd.viewModel.TopWindow = true;
            Settings.Default.TopTransWnd = true;
            Settings.Default.Save();
        }

        private void TopTransWndBtn_OnUnchecked(object sender, RoutedEventArgs e)
        {
            translationWnd.viewModel.TopWindow = false;
            Settings.Default.TopTransWnd = false;
            Settings.Default.Save();
        }

        private void WndPenetrateBtn_OnChecked(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(translationWnd).Handle;
            WinIterop.SetWindowPenetrate(hwnd, true);
            translationWnd.MoveBtn.Visibility = Visibility.Collapsed;
            Settings.Default.WndPenetrate = true;
            Settings.Default.Save();
        }

        private void WndPenetrateBtn_OnUnchecked(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(translationWnd).Handle;
            WinIterop.SetWindowPenetrate(hwnd, false);
            translationWnd.MoveBtn.Visibility = Visibility.Visible;
            Settings.Default.WndPenetrate = false;
            Settings.Default.Save();
        }

        private void BgColorSelecter_OnOnSelectedColorChanged(object sender, FunctionEventArgs<Color> e)
        {
            translationWnd.transBorder.Background = new SolidColorBrush(e.Info);
            var color = new SolidColorBrush(e.Info).Color;
            var gap = 50;
            color.A = (byte)(255 * 0.2);
            color.R = color.R < gap ? (byte)(color.R + gap) : (byte)(color.R - gap);
            color.G = color.G < gap ? (byte)(color.G + gap) : (byte)(color.G - gap);
            color.B = color.B < gap ? (byte)(color.B + gap) : (byte)(color.B - gap);
            translationWnd.resizebBar.Background = new SolidColorBrush(color);
        }

        private void BgColorSelecter_OnOnConfirmed(object sender, FunctionEventArgs<Color> e)
        {
            translationWnd.transBorder.Background = new SolidColorBrush(e.Info);
            var color = new SolidColorBrush(e.Info).Color;
            var gap = 50;
            color.A = (byte)(255 * 0.2);
            color.R = color.R < gap ? (byte)(color.R + gap) : (byte)(color.R - gap);
            color.G = color.G < gap ? (byte)(color.G + gap) : (byte)(color.G - gap);
            color.B = color.B < gap ? (byte)(color.B + gap) : (byte)(color.B - gap);
            translationWnd.resizebBar.Background = new SolidColorBrush(color);

            Settings.Default.BgColor = e.Info.ToString();
            Settings.Default.ReszieBarColor = color.ToString();
            Settings.Default.Save();
        }

        private void FontColorSelecter_OnOnSelectedColorChanged(object sender, FunctionEventArgs<Color> e)
        {
            translationWnd.srcTextBox.Foreground = new SolidColorBrush(e.Info);
            translationWnd.dstTextBox.Foreground = new SolidColorBrush(e.Info);
        }

        private void FontColorSelecter_OnOnConfirmed(object sender, FunctionEventArgs<Color> e)
        {
            translationWnd.srcTextBox.Foreground = new SolidColorBrush(e.Info);
            translationWnd.dstTextBox.Foreground = new SolidColorBrush(e.Info);
            Settings.Default.FontColor = e.Info.ToString();
            Settings.Default.Save();
        }


        private void TranslationPlatformComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.Translation_Platform = TranslationPlatformComboBox.SelectedIndex;
            Settings.Default.Save();
            IdPasswordBox.ShowPassword = false;
            KeyPasswordBox.ShowPassword = false;
            Set_IdAndKey((TranslationPlatformEnum)TranslationPlatformComboBox.SelectedIndex);
        }

        private void IdPasswordBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            Set_IdAndKey((TranslationPlatformEnum)TranslationPlatformComboBox.SelectedIndex, true);
        }

        private void KeyPasswordBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            Set_IdAndKey((TranslationPlatformEnum)TranslationPlatformComboBox.SelectedIndex, true);
        }

        private void TranslationFromComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            translationWnd.From = TranslationOptions.ChineseToStr(e.AddedItems[0].ToString(),
                (TranslationPlatformEnum)TranslationPlatformComboBox.SelectedIndex);
            if ((bool)TranslationBtn.IsChecked)
                WeakReferenceMessenger.Default.Send(new MyMessage(MyAction.Trans_TranslationWnd),
                    (int)MyAction.Trans_TranslationWnd);
            Settings.Default.Translation_From = TranslationFromComboBox.SelectedIndex;
            Settings.Default.Save();
        }

        private void TranslationToComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            translationWnd.To = TranslationOptions.ChineseToStr(e.AddedItems[0].ToString(),
                (TranslationPlatformEnum)TranslationPlatformComboBox.SelectedIndex);
            if ((bool)TranslationBtn.IsChecked)
                WeakReferenceMessenger.Default.Send(new MyMessage(MyAction.Trans_TranslationWnd),
                    (int)MyAction.Trans_TranslationWnd);
            Settings.Default.Translation_To = TranslationToComboBox.SelectedIndex;
            Settings.Default.Save();
        }

        private void FontsizeSelecter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int temp;
            int.TryParse(e.AddedItems[0].ToString(), out temp);
            Settings.Default.Fontsize = temp;
            translationWnd.viewModel.fontsize = temp;
            Settings.Default.Save();
        }

        private void Test_OnClick(object sender, RoutedEventArgs e)
        {
          //WinIterop.UnregisterHotKey(new WindowInteropHelper(App.Current.MainWindow).Handle,Key.F3);
        }
    }
}