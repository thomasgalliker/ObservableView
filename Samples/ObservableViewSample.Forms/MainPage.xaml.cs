using ObservableViewSample.Service;
using ObservableViewSample.ViewModel;
using Xamarin.Forms;

namespace ObservableViewSample.Forms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.BindingContext = new MainViewModel(new MallManager()); // TODO Use IoC here
        }
    }
}
