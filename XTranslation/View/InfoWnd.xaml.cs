using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using HandyControl.Data;
using XTranslation.Model;
using XTranslation.Model.Message;
using XTranslation.Properties;
using Window = System.Windows.Window;

namespace XTranslation.View
{
    public partial class InfoWnd : Window
    {
        public InfoWnd()
        {
            InitializeComponent();
            
        }

        private void Closebtn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void RestoreCloseBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.ClosePage = true;
            Settings.Default.Save();
        }


        private void Link1_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            try
            {
                Clipboard.SetText("https://github.com/X-Yunner/XTransHelper");
            }
            catch (Exception exception)
            {
            }

            var growlInfo = new GrowlInfo();
            growlInfo.WaitTime = 1;
            growlInfo.Message = "链接已复制";
            Growl.SuccessGlobal(growlInfo);
        }

        private void Link2_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            try
            {
                Clipboard.SetText("https://space.bilibili.com/23268144");
            }
            catch (Exception exception)
            {
            }

            var growlInfo = new GrowlInfo();
            growlInfo.WaitTime = 1;
            growlInfo.Message = "链接已复制";
            Growl.SuccessGlobal(growlInfo);
        }
        
        private void RestoreTransWnd_OnClick(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage(MyAction.Restore_TransWndPos),
                (int)MyAction.Restore_TransWndPos);
        }
    }
}