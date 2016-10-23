using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CustomDialog
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();            
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var container = sender as Button;
            if (container != null)
            {
                FindName("DialogViewInstance");
                bool animate = AnimationToggleSwitch.IsOn;
                DialogViewInstance.Open(container,animate);
            }

        }

        private void PopUp2Button_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var container = sender as Button;
            if (container != null)
            {
                FindName("DialogViewInstance2");
                bool animate = AnimationToggleSwitch.IsOn;
                DialogViewInstance2.Open(container,animate);
            }
        }
    }
}
