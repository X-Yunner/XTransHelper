using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Data;
using HookManager;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using UserControl = System.Windows.Controls.UserControl;

namespace XTranslation.View.Controls
{
    public partial class ColorBorder : UserControl
    {
        public MouseHook mouseHook = new MouseHook();

        private Brush storeBrush;

        public ColorBorder()
        {
            InitializeComponent();
            mouseHook.LeftDown += MouseHook_LeftDown;
        }

        //按下鼠标键触发的事件
        private void MouseHook_LeftDown(object sender, MouseEventArgs e)
        {
            Point point = default;
            if (PresentationSource.FromVisual(ColorPicker) != null)
            {
                point = ColorPicker.PointToScreen(new Point(0, 0));
                var ColorPicker_rect = new Rect(point.X, point.Y, ColorPicker.ActualWidth, ColorPicker.ActualHeight);
                point = ColorBox.PointToScreen(new Point(0, 0));
                var ColorBox_rect = new Rect(point.X, point.Y, ColorBox.ActualWidth, ColorBox.ActualHeight);
                if (!ColorPicker_rect.Contains(e.X, e.Y) && !ColorBox_rect.Contains(e.X, e.Y))
                {
                    mouseHook.Stop();
                    ColorBox.Background = storeBrush;
                    ColorPicker.SelectedBrush = (SolidColorBrush)storeBrush;
                    popColorPicker.IsOpen = false;
                    var args = new RoutedEventArgs(Canceled, e);
                    ColorPicker.RaiseEvent(args);
                }
            }
        }


        private void ColorBox_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (popColorPicker.IsOpen)
            {
                case true:
                {
                    popColorPicker.IsOpen = false;
                    ColorBox.Background = storeBrush;
                }
                    break;
                case false:
                {
                    storeBrush = ColorBox.Background;
                    popColorPicker.IsOpen = true;
                    mouseHook.Start();
                }
                    break;
            }

            var args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton,
                e.StylusDevice);
            args.RoutedEvent = MouseLeftDownEvent;
            ColorBox.RaiseEvent(args);
        }

        private void ColorPicker_OnSelectedColorChanged(object sender, FunctionEventArgs<Color> e)
        {
            var args = new FunctionEventArgs<Color>(SelectedColorChanged, e);
            args.Info = e.Info;
            ColorPicker.RaiseEvent(args);
        }

        private void ColorPicker_OnCanceled(object sender, EventArgs e)
        {
            popColorPicker.IsOpen = false;
            var args = new RoutedEventArgs(Canceled, e);
            ColorPicker.RaiseEvent(args);
        }

        private void ColorPicker_OnConfirmed(object sender, FunctionEventArgs<Color> e)
        {
            popColorPicker.IsOpen = false;
            var args = new FunctionEventArgs<Color>(Confirmed, e);
            args.Info = e.Info;
            ColorPicker.RaiseEvent(args);
        }

        #region 注册依赖属性

        public static readonly DependencyProperty SelectBrushProperty = DependencyProperty.Register(nameof(SelectBrush),
            typeof(SolidColorBrush), typeof(ColorBorder),
            new FrameworkPropertyMetadata(Brushes.Black)
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });


        public SolidColorBrush SelectBrush
        {
            get => (SolidColorBrush)GetValue(SelectBrushProperty);
            set => SetValue(SelectBrushProperty, value);
        }

        #endregion


        #region 注册路由事件

        public static readonly RoutedEvent MouseLeftDownEvent = EventManager.RegisterRoutedEvent(
            nameof(OnMouseLeftButtonDown), RoutingStrategy.Bubble, typeof(MouseButtonEventHandler),
            typeof(ColorBorder));


        public static readonly RoutedEvent SelectedColorChanged = EventManager.RegisterRoutedEvent(
            nameof(OnSelectedColorChanged), RoutingStrategy.Bubble, typeof(EventHandler<FunctionEventArgs<Color>>),
            typeof(ColorBorder));

        public static readonly RoutedEvent Canceled = EventManager.RegisterRoutedEvent(
            nameof(OnCanceled), RoutingStrategy.Bubble, typeof(EventHandler),
            typeof(ColorBorder));

        public static readonly RoutedEvent Confirmed = EventManager.RegisterRoutedEvent(
            nameof(OnConfirmed), RoutingStrategy.Bubble, typeof(EventHandler<FunctionEventArgs<Color>>),
            typeof(ColorBorder));

        public event MouseButtonEventHandler OnMouseLeftButtonDown
        {
            add => AddHandler(MouseLeftDownEvent, value);
            remove => RemoveHandler(MouseLeftDownEvent, value);
        }

        public event EventHandler<FunctionEventArgs<Color>> OnSelectedColorChanged
        {
            add => AddHandler(SelectedColorChanged, value);
            remove => RemoveHandler(SelectedColorChanged, value);
        }

        public event EventHandler OnCanceled
        {
            add => AddHandler(Canceled, value);
            remove => RemoveHandler(Canceled, value);
        }

        public event EventHandler<FunctionEventArgs<Color>> OnConfirmed
        {
            add => AddHandler(Confirmed, value);
            remove => RemoveHandler(Confirmed, value);
        }

        #endregion
    }
}