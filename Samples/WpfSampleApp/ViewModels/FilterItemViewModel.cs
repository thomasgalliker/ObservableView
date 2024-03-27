using CommunityToolkit.Mvvm.ComponentModel;
using ObservableView.Searching.Operators;

namespace WpfSampleApp.ViewModels
{
    public class FilterItemViewModel : ObservableObject
    {
        private BinaryOperator selectedOperator;

        public FilterItemViewModel()
        {

        }

        public BinaryOperator SelectedOperator
        {
            get
            {
                return this.selectedOperator;
            }

            set
            {
                if (value != this.selectedOperator)
                {
                    this.selectedOperator = value;
                    this.OnPropertyChanged(nameof(this.SelectedOperator));
                    //this.OnFilterCriteriaChanged();
                }
            }
        }
    }
}
