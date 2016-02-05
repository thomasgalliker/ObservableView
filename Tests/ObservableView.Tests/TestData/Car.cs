using System.Diagnostics;

using ObservableView.Searching;

namespace ObservableView.Tests.TestData
{
    [DebuggerDisplay("Brand={Brand}, Model={Model}, ChasisNumber={ChasisNumber}, Year={Year}")]
    public class Car
    {
        public Car(CarBrand brand, string model, string chasisNumber, int year)
        {
            this.Brand = brand;
            this.Model = model;
            this.ChasisNumber = chasisNumber;
            this.Year = year;
        }

        public CarBrand Brand { get; private set; }

        [Searchable]
        public string Model { get; private set; }

        public string ChasisNumber { get; private set; }

        public int Year { get; private set; }
    }
}