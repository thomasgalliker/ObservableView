using System.Windows.Controls;

using ObservableViewSample.WPF.ViewModels;

namespace ObservableViewSample.WPF
{
    public partial class FilterControl : UserControl
    {
        public FilterControl()
        {
            this.InitializeComponent();
            this.DataContext = new FilterViewModel();
        }
    }
}