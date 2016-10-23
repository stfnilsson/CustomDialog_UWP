using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using CustomDialog.Views;

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
            FindName("DialogViewInstance");
            OpenDialog(DialogViewInstance, sender as FrameworkElement);
        }

        private async void PopUp2Button_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            FindName("DialogViewInstance2");
            DialogViewInstance2.IsBusy = true;

            OpenDialog(DialogViewInstance2, sender as FrameworkElement);

            await Task.Delay(TimeSpan.FromSeconds(2));

            DialogViewInstance2.IsBusy = false;
        }

        private void OpenDialog(DialogView dialog, FrameworkElement control)
        {
            if (control != null)
            {
                FindName("DialogViewInstance2");
                bool animate = AnimationToggleSwitch.IsOn;
                dialog.Open(control, animate);
            }
        }
    }
}
