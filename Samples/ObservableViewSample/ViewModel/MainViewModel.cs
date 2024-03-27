using System;
using CommunityToolkit.Mvvm.Input;
using ObservableView;
using ObservableView.Grouping;
using ObservableView.Searching.Operators;
using ObservableView.Sorting;

using ObservableViewSample.Model;
using ObservableViewSample.Service;
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;

namespace ObservableViewSample.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private RelayCommand addMallCommand;
        private RelayCommand<Mall> deleteMallCommand;
        private RelayCommand refreshCommand;
        private RelayCommand searchBoxClearCommand;
        private string newMallTitle;
        private string newMallSubtitle;
        private int newMallNumberOf = 1;

        public MainViewModel(IMallManager mallManager)
        {
            var listItems = mallManager.GetMalls();

            this.MallsList = new ObservableView<Mall>(listItems);

            // Add sort specifications
            this.MallsList.AddOrderSpecification(x => x.Title, OrderDirection.Ascending);
            this.MallsList.AddOrderSpecification(x => x.Subtitle, OrderDirection.Descending);

            // Add search specifications
            this.MallsList.SearchSpecification.Add(x => x.Title, BinaryOperator.Contains);
            this.MallsList.SearchSpecification.Add(x => x.Subtitle, BinaryOperator.Contains);

            // Add grouping specifications
            this.MallsList.GroupKeyAlgorithm = new AlphaGroupKeyAlgorithm();
            this.MallsList.GroupKey = mall => mall.Title;

            this.MallsList.FilterHandler += this.MallsList_FilterHandler;
        }

        private void MallsList_FilterHandler(object sender, ObservableView.Filtering.FilterEventArgs<Mall> e)
        {
            if (e.Item.Title.Contains("Aber"))
            {
                e.IsAllowed = false;
            }
        }

        public ObservableView<Mall> MallsList { get; private set; }

        public RelayCommand AddMallCommand
        {
            get
            {
                return this.addMallCommand ?? (this.addMallCommand = new RelayCommand(
                        () =>
                        {
                            // Add new item to ObservableView
                            for (int i = 0; i < this.NewMallNumberOf; i++)
                            {
                                this.MallsList.Source.Add(new Mall(this.NewMallTitle + (i > 0 ? "_" + i : ""), this.NewMallSubtitle));
                            }

                            // Reset the text input
                            this.NewMallTitle = string.Empty;
                            this.NewMallSubtitle = string.Empty;
                        }));
            }
        }

        public RelayCommand<Mall> DeleteMallCommand
        {
            get
            {
                return this.deleteMallCommand ?? (this.deleteMallCommand = new RelayCommand<Mall>(
                         (mall) =>
                         {
                             // Remove new item to ObservableView
                             this.MallsList.Source.Remove(mall);

                             // Reset the text input
                             this.NewMallTitle = string.Empty;
                             this.NewMallSubtitle = string.Empty;
                         }));
            }
        }

        public RelayCommand RefreshCommand => this.refreshCommand ??= new RelayCommand(this.MallsList.Refresh);


        public RelayCommand SearchBoxClearCommand => this.searchBoxClearCommand ??= new RelayCommand(this.MallsList.ClearSearch);

        public string NewMallTitle
        {
            get => this.newMallTitle;
            set
            {
                if (this.SetProperty(ref this.newMallTitle, value))
                {
                    this.OnPropertyChanged(nameof(this.NewMallTitle));
                    this.OnPropertyChanged(nameof(this.IsAddMallButtonEnabled));
                }
            }
        }

        public string NewMallSubtitle
        {
            get => this.newMallSubtitle;
            set
            {
                this.newMallSubtitle = value;
                this.OnPropertyChanged(nameof(this.NewMallSubtitle));
                this.OnPropertyChanged(nameof(this.IsAddMallButtonEnabled));
            }
        }

        public int NewMallNumberOf
        {
            get => this.newMallNumberOf;
            set
            {
                this.newMallNumberOf = Math.Abs(value);
                this.OnPropertyChanged(nameof(this.NewMallNumberOf));
            }
        }

        public bool IsAddMallButtonEnabled
        {
            get => !string.IsNullOrEmpty(this.NewMallTitle) && !string.IsNullOrEmpty(this.NewMallSubtitle);
        }
    }
}