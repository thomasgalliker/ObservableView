using System.Diagnostics;

using ObservableView.Searching;

namespace ObservableView.Tests.TestData
{
    [DebuggerDisplay("Brand={Brand}, Model={Model}, Year={Year}")]
    public class Car
    {
        public Car(CarBrand brand, string model, int year)
        {
            this.Brand = brand;
            this.Model = model;
            this.Year = year;
        }

        public CarBrand Brand { get; private set; }

        [Searchable]
        public string Model { get; private set; }

        public int Year { get; private set; }
    }
}