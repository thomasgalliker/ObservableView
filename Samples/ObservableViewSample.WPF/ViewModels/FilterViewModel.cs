using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

using GalaSoft.MvvmLight;

using ObservableView.Filtering;
using ObservableView.Searching.Operators;

namespace ObservableViewSample.WPF.ViewModels
{
    public class FilterViewModel : ViewModelBase
    {
        public FilterViewModel()
        {
            this.FilterItems = new List<FilterItemViewModel> { new FilterItemViewModel(), new FilterItemViewModel() };
        }

        public IEnumerable<FilterItemViewModel> FilterItems { get; set; }
    }

    public class FilterItemViewModel : ViewModelBase
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
                    this.RaisePropertyChanged(() => this.SelectedOperator);
                    //this.OnFilterCriteriaChanged();
                }
            }
        }
    }
}
