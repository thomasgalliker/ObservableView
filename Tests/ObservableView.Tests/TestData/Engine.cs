namespace ObservableView.Tests.TestData
{
    public class Engine
    {
        public Engine(string name, int cylinders, int horsePower, FuelType fuelType)
        {
            this.Name = name;
            this.Cylinders = cylinders;
            this.HorsePower = horsePower;
            this.FuelType = fuelType;
        }

        public string Name { get; private set; }

        public int Cylinders { get; private set; }

        public int HorsePower { get; private set; }

        public FuelType FuelType { get; set; }
    }
}