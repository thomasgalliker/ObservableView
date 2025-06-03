using MauiSampleApp.Views;

namespace MauiSampleApp
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();

            var mainPage = IPlatformApplication.Current.Services.GetRequiredService<MainPage>();
            this.MainPage = new NavigationPage(mainPage);
        }
    }
}
