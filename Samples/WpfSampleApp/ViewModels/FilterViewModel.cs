using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfSampleApp.ViewModels
{
    public class FilterViewModel : ObservableObject
    {
        public FilterViewModel()
        {
            this.FilterItems = new List<FilterItemViewModel>
            {
                new FilterItemViewModel(),
                new FilterItemViewModel()
            };
        }

        public IEnumerable<FilterItemViewModel> FilterItems { get; set; }
    }
}
