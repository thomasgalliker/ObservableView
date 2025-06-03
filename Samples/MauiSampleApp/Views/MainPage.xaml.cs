using ObservableViewSample.ViewModel;

namespace MauiSampleApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel mainViewModel)
        {
            this.InitializeComponent();
            this.BindingContext = mainViewModel;
        }
    }
}
