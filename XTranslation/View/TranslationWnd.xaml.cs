using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using CommunityToolkit.Mvvm.Messaging;
using XTranslation.Model;
using XTranslation.Model.Message;
using XTranslation.Properties;
using XTranslation.Utils;
using XTranslation.ViewModel;

namespace XTranslation.View
{
    /// <summary>
    ///     Window1.xaml 的交互逻辑
    /// </summary>
    public partial class TranslationWnd : Window
    {
        private const int WM_CLIPBOARDUPDATE = 0x031D; //粘贴板发生改动时发出的消息

        private readonly ManualResetEvent TransTextThreadMR = new ManualResetEvent(false); //控制线程运行

        private readonly bool TransThreadControl = true; //翻译线程总控制

        public string From;

        private IntPtr hwnd; //窗口句柄

        public string ID;

        public string KEY;

        public string To;

        private ITranslation tr; //翻译实例

        public TranslationPlatformEnum translationPlatformEnum;

        private Thread TransThread; //翻译线程

        public TranslationWndViewModel viewModel = new TranslationWndViewModel();

        public TranslationWnd()
        {
            InitializeComponent();
            DataContext = viewModel;
            Left = SystemParameters.FullPrimaryScreenWidth - Width;
            Top = SystemParameters.FullPrimaryScreenHeight * 0.2;
            WeakReferenceMessenger.Default.Register<MyMessage,int>(this,(int)MyAction.Restore_TransWndPos,MsgHandler);
        }

        public void MsgHandler(object receive, MyMessage message)
        {
            switch (message.movement)
            {
                case MyAction.Trans_TranslationWnd:
                {
                    if (viewModel.srcText != "") TransTextThreadMR.Set();
                }
                    break;
                case MyAction.Restore_TransWndPos:
                {
                    Left = SystemParameters.FullPrimaryScreenWidth - 500;
                    Top = SystemParameters.FullPrimaryScreenHeight * 0.2;
                    Height = 300;
                    Width = 500;

                }
                    break;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            HideEx();
            WeakReferenceMessenger.Default.Send(new MyMessage(MyAction.Exit_TranslationWnd),
                (int)MyAction.Exit_TranslationWnd);
        }


        public void ShowEx()
        {
            Show();
            if (hwnd != IntPtr.Zero)
            {
                WinIterop.SetClipboardFormatListener(hwnd, true);
                switch (translationPlatformEnum)
                {
                    case TranslationPlatformEnum.BaiDu:
                        tr = new BaiDuTranslation();
                        break;
                    case TranslationPlatformEnum.HuoShan:
                        tr = new HuoShanTranslation();
                        break;
                    case TranslationPlatformEnum.YouDao:
                        tr = new YouDaoTranslation();
                        break;
                    case TranslationPlatformEnum.Tencent:
                        tr = new TencentTranslation();
                        break;
                    case TranslationPlatformEnum.CaiYun:
                        tr = new CaiYunTranslation();
                        break;
                }
                tr.SetCred(ID, KEY);
            }
        }

        public void HideEx()
        {
            if (hwnd != IntPtr.Zero) WinIterop.SetClipboardFormatListener(hwnd, false);
            Hide();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            hwnd = new WindowInteropHelper(this).EnsureHandle();
            var hs = HwndSource.FromHwnd(hwnd);
            hs.AddHook(WndProc);
            TransThread = new Thread(TranslationText);
            TransThread.Start();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_CLIPBOARDUPDATE:
                {
                    try
                    {
                        string temp=Clipboard.GetText();
                        if (temp != "")
                        {
                            viewModel.srcText = temp;
                            TransTextThreadMR.Set();
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
                    break;
            }

            return IntPtr.Zero;
        }


        private void TranslationText(object obj)
        {
            while (TransThreadControl)
            {
                TransTextThreadMR.WaitOne();
                tr.Translation(viewModel.srcText, From, To);
                viewModel.dstText = tr.GetResult();
                TransTextThreadMR.Reset();
            }
        }


        private void MoveBtn_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TranslationWnd_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var temp = transGridcol1.ActualWidth / (e.PreviousSize.Width == 0 ? Width : e.PreviousSize.Width);
            var cWidth1 = temp * e.NewSize.Width > 30 ? temp * e.NewSize.Width : 30;
            var cWidth3 = e.NewSize.Width - resizebBar.Width - transGridcol1.Width.Value;
            if (cWidth3 < 30)
            {
                cWidth1 = e.NewSize.Width - 35;
                cWidth3 = 30;
            }

            transGridcol1.Width = new GridLength(cWidth1);
            transGridcol3.Width = new GridLength(cWidth3);

            Settings.Default.TransWndWidth = e.PreviousSize.Width == 0 ? Width : e.PreviousSize.Width;
            Settings.Default.TransWndHeight = e.PreviousSize.Height == 0 ? Height : e.PreviousSize.Height;
            Settings.Default.TransWndTB_Width = transGridcol1.Width.Value;
            Settings.Default.Save();
        }

        private void TranslationWnd_OnStateChanged(object sender, EventArgs e)
        {
            if (TranslationWindow.WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
        }

        private void TranslationWnd_OnLocationChanged(object sender, EventArgs e)
        {
            Settings.Default.TransWnd_Left = Left;
            Settings.Default.TransWnd_Top = Top;
            Settings.Default.Save();
        }


        #region 控制控件大小

        private bool isResizing;

        private void TranslationWnd_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                var point = e.GetPosition(this);
                double width = 0;
                if (point.X > Width - 30)
                    width = Width - 30 - resizebBar.Width;
                else if (point.X < 30)
                    width = 30;
                else
                    width = point.X;

                transGridcol1.Width = new GridLength(width);
                transGridcol3.Width = new GridLength(Width - resizebBar.Width - width);

                Settings.Default.TransWndTB_Width = transGridcol1.Width.Value;
                Settings.Default.Save();
            }
        }

        private void TranslationWnd_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (resizebBar.IsMouseOver)
            {
                isResizing = true;
                CaptureMouse();
            }
        }

        private void TranslationWnd_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isResizing = false;
            ReleaseMouseCapture();
        }

        #endregion
    }
}