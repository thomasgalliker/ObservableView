using System.Windows.Controls;
using WpfSampleApp.ViewModels;

namespace WpfSampleApp.Views
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