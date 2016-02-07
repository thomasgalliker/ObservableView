using System.Collections.ObjectModel;

namespace ObservableView.Tests.TestData
{
    /// <summary>
    /// Predefined car pool
    /// </summary>
    static class CarPool
    {
        public static readonly Car carAudiA1 = new Car(CarBrand.Audi, "A1", "AA6D11BF", 2013, Engines.tfsi20Engine);
        public static readonly Car carAudiA4 = new Car(CarBrand.Audi, "A4 Avant", null, 2000, null);
        public static readonly Car carAudiA3 = new Car(CarBrand.Audi, "A3 Sportback", "AA8AFB35", 2015, Engines.tfsi20Engine);
        public static readonly Car carBmwM1 = new Car(CarBrand.BMW, "M1", "9E837EB8", 2014, Engines.tfsi20Engine);
        public static readonly Car carBmwM3 = new Car(CarBrand.BMW, "M3", "C1C2D592", 2012, Engines.fsi42Engine);
        public static readonly Car carBmwX5 = new Car(CarBrand.BMW, "X5", "675A2278", 2012, Engines.tdi30dEngine);
        public static readonly Car carVwPolo = new Car(CarBrand.VW, "Polo 1.4 TDI", "76B25F39", 1999, Engines.tdi14Engine);
        public static readonly Car carVwGolf = new Car(CarBrand.VW, "Golf 20th Birthdäy Ed.", "GOLF2000", 1990, Engines.electricEngine);
        public static readonly Car carVwGolfGti = new Car(CarBrand.VW, "Golf GTI 2.0", "AAA1FAF2", 2016, Engines.tfsi20Engine);

        public static ObservableCollection<Car> GetDefaultCarsList()
        {
            var carsList = new ObservableCollection<Car>
            {
                CarPool.carAudiA1,
                CarPool.carAudiA4,
                CarPool.carAudiA3,
                CarPool.carBmwM1,
                CarPool.carBmwM3,
                CarPool.carVwPolo,
                CarPool.carVwGolf
            };

            return carsList;
        }
    }

    static class Engines
    {
        public static readonly Engine electricEngine = new Engine("Electro", cylinders: 0, horsePower: 90, fuelType: FuelType.Electric);
        public static readonly Engine tfsi20Engine = new Engine("TFSI 2.0", cylinders: 4, horsePower: 200, fuelType: FuelType.Petrol);
        public static readonly Engine fsi42Engine = new Engine("FSI 4.2", cylinders: 8, horsePower: 450, fuelType: FuelType.Petrol);
        public static readonly Engine tdi14Engine = new Engine("TDI 1.4", cylinders: 4, horsePower: 140, fuelType: FuelType.Diesel);
        public static readonly Engine tdi30dEngine = new Engine("TDI 3.0d", cylinders: 6, horsePower: 235, fuelType: FuelType.Diesel);
    }
}
