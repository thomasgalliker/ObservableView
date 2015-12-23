using System;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using ObservableView;
using ObservableView.Sorting;

using ObservableViewSample.Data.Model;
using ObservableViewSample.Data.Service;

namespace ObservableViewSample.Data.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private RelayCommand addMallCommand;
        private RelayCommand<Mall> deleteMallCommand;
        private RelayCommand refreshCommand;
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
            this.MallsList.AddSearchSpecification(x => x.Title);
            this.MallsList.AddSearchSpecification(x => x.Subtitle);
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

        public RelayCommand RefreshCommand
        {
            get
            {
                return this.refreshCommand ?? (this.refreshCommand = new RelayCommand(
                         () =>
                         {
                             this.MallsList.Refresh();
                         }));
            }
        }

        public string NewMallTitle
        {
            get
            {
                return this.newMallTitle;
            }
            set
            {
                this.newMallTitle = value;
                this.RaisePropertyChanged(() => this.NewMallTitle);
                this.RaisePropertyChanged(() => this.IsAddMallButtonEnabled);
            }
        }

        public string NewMallSubtitle
        {
            get
            {
                return this.newMallSubtitle;
            }
            set
            {
                this.newMallSubtitle = value;
                this.RaisePropertyChanged(() => this.NewMallSubtitle);
                this.RaisePropertyChanged(() => this.IsAddMallButtonEnabled);
            }
        }

        public int NewMallNumberOf
        {
            get
            {
                return this.newMallNumberOf;
            }
            set
            {
                this.newMallNumberOf = Math.Abs(value);
                this.RaisePropertyChanged(() => this.NewMallNumberOf);
            }
        }

        public bool IsAddMallButtonEnabled
        {
            get
            {
                return !string.IsNullOrEmpty(this.NewMallTitle) && !string.IsNullOrEmpty(this.NewMallSubtitle);
            }
        }
    }
}