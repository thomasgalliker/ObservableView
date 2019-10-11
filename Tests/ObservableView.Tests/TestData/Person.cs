using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ObservableView.Tests.TestData
{
    public static class Countries
    {
        public static readonly Country Switzerland = new Country { Name = "Switzerland", Continent = Continent.Europe, Iso3166Code="CH", Population = 8211700 };
        public static readonly Country Germany = new Country { Name = "Germany", Continent = Continent.NorthAmerica, Iso3166Code = "DE", Population = 81083600 };
        public static readonly Country USA = new Country { Name = "United States of America", Continent = Continent.NorthAmerica, Iso3166Code = "US", Population = 322369319 };
    }

    public static class Persons
    {
        public static readonly Person ThomasGalliker = new Person { Name = "Galliker", Surname = "Thomas", Country = Countries.Switzerland, Birthdate = new DateTime(1986, 07, 11) };
        public static readonly Person WillhelmTell = new Person { Name = "Tell", Surname = "Willhelm", Country = Countries.Switzerland, Birthdate = new DateTime(1900, 1, 1) };
        public static readonly Person LoremIpsum  = new Person { Name = "Ipsum", Surname = "Lorem", Country = Countries.USA };
        public static readonly Person AngelaMerkel  = new Person { Name = "Merkel", Surname = "Angela", Country = Countries.Germany };

        public static IEnumerable<Person> GetAll()
        {
            yield return ThomasGalliker;
            yield return WillhelmTell;
            yield return LoremIpsum;
            yield return AngelaMerkel;
        }
    }

    [DebuggerDisplay("Surname={Surname}, Name={Name}, Birthdate={Birthdate}")]
    public class Person
    {
        public string Surname { get; set; }

        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public Country Country { get; set; }
    }

    public class Country
    {
        public string Name { get; set; }

        public string Iso3166Code { get; set; }

        public Continent Continent { get; set; }

        public int Population { get; set; }
    }

    public enum Continent
    {
        Asia,
        Africa,
        Antarctica,
        Australia,
        Europe,
        NorthAmerica,
        SouthAmerica,
    }
}
