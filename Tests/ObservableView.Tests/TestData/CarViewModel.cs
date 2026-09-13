using System.ComponentModel;

namespace ObservableView.Tests.TestData
{
    public class CarViewModel : INotifyPropertyChanged
    {
        private CarBrand brand;
        private string model;

        public CarViewModel(CarBrand brand, string model)
        {
            this.brand = brand;
            this.model = model;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool HasPropertyChangedSubscribers => this.PropertyChanged != null;

        public CarBrand Brand
        {
            get => this.brand;
            set
            {
                if (this.brand == value)
                {
                    return;
                }

                this.brand = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Brand)));
            }
        }

        public string Model
        {
            get => this.model;
            set
            {
                if (this.model == value)
                {
                    return;
                }

                this.model = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Model)));
            }
        }
    }
}
