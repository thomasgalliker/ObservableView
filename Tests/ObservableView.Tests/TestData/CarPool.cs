using System.Collections.ObjectModel;

namespace ObservableView.Tests.TestData
{
    static class CarPool
    {
        // Predefined car pool
        public static readonly Car carAudiA1 = new Car(CarBrand.Audi, "A1", 2013);
        public static readonly Car carAudiA4 = new Car(CarBrand.Audi, "A4 Avant", 2000);
        public static readonly Car carAudiA3 = new Car(CarBrand.Audi, "A3 Sportback", 2015);
        public static readonly Car carBmwM1 = new Car(CarBrand.BMW, "M1", 2014);
        public static readonly Car carBmwM3 = new Car(CarBrand.BMW, "M3", 2012);
        public static readonly Car carBmwX5 = new Car(CarBrand.BMW, "X5", 2012);
        public static readonly Car carVwPolo = new Car(CarBrand.VW, "Polo 1.4 TDI", 1999);
        public static readonly Car carVwGolf = new Car(CarBrand.VW, "Golf 20th Birthday Ed.", 1990);
        public static readonly Car carVwGolfGti = new Car(CarBrand.VW, "Golf GTI 2.0", 2016);

        public static ObservableCollection<Car> GetDefaultCarsList()
        {
            var carsList = new ObservableCollection<Car>
            {
                CarPool.carAudiA1,
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
