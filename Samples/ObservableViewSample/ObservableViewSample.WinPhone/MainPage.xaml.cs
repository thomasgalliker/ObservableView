using Microsoft.Phone.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

namespace ObservableViewSample.WindowsPhone8
{
    public partial class MainPage : FormsApplicationPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            Forms.Init();
            this.LoadApplication(new ObservableViewSample.App());
        }
    }
}