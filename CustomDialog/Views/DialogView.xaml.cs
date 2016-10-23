using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace CustomDialog.Views
{
    [ContentProperty(Name = nameof(ContentElement))]
    public sealed partial class DialogView : UserControl
    {
        public static readonly DependencyProperty InBusyProperty = DependencyProperty.Register("InBusy", typeof (bool),
            typeof (DialogView), new PropertyMetadata(true));

        public readonly double DesktopMinWidth = 1024;

        public readonly double DesktopWidth = 800;
        public readonly double MobileWidth = 450;
        public readonly double TabletMinWidth = 700;
        public readonly double TabletWidth = 620;

        private bool _animate;

        private ConnectedAnimation _animation;
        private FrameworkElement _fromElement;

        public DialogView()
        {
            InitializeComponent();
            SetInitWidth();
        }

        public FrameworkElement ContentElement { get; set; }

        public bool IsBusy
        {
            get { return (bool) GetValue(InBusyProperty); }

            set
            {
                SetValue(InBusyProperty, value);
                if (value)
                {
                    ProgressRingGrid.Visibility = Visibility.Visible;
                    ContentLayout.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ProgressRingGrid.Visibility = Visibility.Collapsed;
                    ContentLayout.Visibility = Visibility.Visible;
                }
            }
        }

        private void SetInitWidth()
        {
            if (Window.Current.Bounds.Width >= DesktopMinWidth)
            {
                MainGrid.Width = DesktopWidth;
                return;
            }
            if (Window.Current.Bounds.Width >= TabletMinWidth)
            {
                MainGrid.Width = TabletWidth;
                return;
            }
            MainGrid.Width = MobileWidth;
        }

        public void Open(FrameworkElement from, bool animate)
        {
            _fromElement = from;
            _animate = animate;

            if (_animate)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ButtonRect", _fromElement);
            }
            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManagerBackRequested;
            Visibility = Visibility.Visible;

            if (_animate)
            {
                _animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ButtonRect");
                _animation?.TryStart(MainGrid);
            }
            MainGrid.Opacity = 1;
            OpenStoryBoard.Begin();
        }

        private void SystemNavigationManagerBackRequested(object sender, BackRequestedEventArgs e)
        {
            Close(_animate);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close(_animate);
        }

        private void MainLayout_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Close(_animate);
        }

        private void Close(bool animate)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= SystemNavigationManagerBackRequested;

            if (OpenStoryBoard.GetCurrentState() == ClockState.Active)
            {
                OpenStoryBoard.Stop();
            }
            _animation?.Cancel();
            _animation = null;

            if (animate)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ButtonRectClose", MainGrid);
            }
            MainGrid.Opacity = 0;
            CloseStoryBoard.Completed += CloseStoryBoard_Completed;
            CloseStoryBoard.Begin();

            if (_animate)
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("ButtonRectClose");
                animation?.TryStart(_fromElement);
            }
        }

        private void CloseStoryBoard_Completed(object sender, object e)
        {
            CloseStoryBoard.Completed -= CloseStoryBoard_Completed;
            Visibility = Visibility.Collapsed;
        }

        private void MainGrid_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}