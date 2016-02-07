using System.Collections.ObjectModel;

namespace ObservableView.Tests.TestData
{
    /// <summary>
    /// Predefined car pool
    /// </summary>
    static class CarPool
    {
        public static readonly Car carAudiA1 = new Car(CarBrand.Audi, "A1", "AA6D11BF", 2013);
        public static readonly Car carAudiA4 = new Car(CarBrand.Audi, "A4 Avant", null, 2000);
        public static readonly Car carAudiA3 = new Car(CarBrand.Audi, "A3 Sportback", "AA8AFB35", 2015);
        public static readonly Car carBmwM1 = new Car(CarBrand.BMW, "M1", "9E837EB8", 2014);
        public static readonly Car carBmwM3 = new Car(CarBrand.BMW, "M3", "C1C2D592", 2012);
        public static readonly Car carBmwX5 = new Car(CarBrand.BMW, "X5", "675A2278", 2012);
        public static readonly Car carVwPolo = new Car(CarBrand.VW, "Polo 1.4 TDI", "76B25F39", 1999);
        public static readonly Car carVwGolf = new Car(CarBrand.VW, "Golf 20th Birthdäy Ed.", "GOLF2000", 1990);
        public static readonly Car carVwGolfGti = new Car(CarBrand.VW, "Golf GTI 2.0", "AAA1FAF2", 2016);

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
}
