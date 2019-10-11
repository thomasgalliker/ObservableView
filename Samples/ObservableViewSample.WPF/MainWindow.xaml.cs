using System.Windows;
using ObservableViewSample.Service;
using ObservableViewSample.ViewModel;

namespace ObservableViewSample.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel(new MallManager()); // TODO Use IoC here
        }
    }
}