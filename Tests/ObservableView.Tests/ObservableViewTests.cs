namespace ObservableView.Tests
{
    public class ObservableViewTests
    {
        private const string FilteredStringItemA = "A";
        private const string FilteredStringItemB = "B";
        private const string FilteredStringItemC = "C";

        #region Filtering

        [Fact]
        public void ShouldRetrieveAnUnfilteredView()
        {
            // Arrange
            var stringList = new List<string> { FilteredStringItemA, FilteredStringItemB, FilteredStringItemC };
            var observableStringView = new ObservableView<string>(stringList);

            // Act
            var unfilteredView = observableStringView.View;

            // Assert
            unfilteredView.Should().NotBeNull();
            unfilteredView.Should().HaveCount(3);
            unfilteredView[0].Should().Be(FilteredStringItemA);
            unfilteredView[1].Should().Be(FilteredStringItemB);
            unfilteredView[2].Should().Be(FilteredStringItemC);

            foreach (var sourceItem in observableStringView.Source)
            {
                var sourceitemIsContained = unfilteredView.Contains(sourceItem);
                sourceitemIsContained.Should().BeTrue();
            }

            foreach (var item in unfilteredView)
            {
                var sourceitemIsContained = observableStringView.Source.Contains(item);
                sourceitemIsContained.Should().BeTrue();
            }
        }

        [Fact]
        public void ShouldTestFilterHandlerWithNoFilterApplied()
        {
            // Arrange
            var stringList = new List<string> { FilteredStringItemA, FilteredStringItemB, FilteredStringItemC };
            var observableStringView = new ObservableView<string>(stringList);
            observableStringView.FilterHandler += (sender, e) => { };

            // Act
            ObservableCollection<string> filteredView = observableStringView.View;

            // Assert
            filteredView.Should().NotBeNull();
            filteredView.Should().HaveCount(3);
            filteredView[0].Should().Be(FilteredStringItemA);
            filteredView[1].Should().Be(FilteredStringItemB);
            filteredView[2].Should().Be(FilteredStringItemC);
        }

        [Fact]
        public void ShouldTestFilterHandlerWithDefinedFilterCriteria()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.FilterHandler += (sender, e) => e.IsAllowed = e.Item.Brand == CarBrand.BMW;

            // Act
            ObservableCollection<Car> filteredView = observableCarsView.View;

            // Assert
            filteredView.Should().NotBeNull();
            filteredView.Should().HaveCount(2);
            filteredView.Single(x => x.Model == CarPool.carBmwM1.Model).Should().NotBeNull();
            filteredView.Single(x => x.Model == CarPool.carBmwM3.Model).Should().NotBeNull();
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventIfNewItemsAreAdded()
        {
            // Arrange
            var receivedEvents = new List<string>();
            var carsList = CarPool.GetDefaultCarsList();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.FilterHandler += (sender, e) => e.IsAllowed = e.Item.Brand == CarBrand.BMW;
            observableCarsView.PropertyChanged += (sender, e) => receivedEvents.Add(e.PropertyName);

            // Act
            // Let's add 3 new cars. One of them is a BMW which is 'allowed' by the filter criteria.
            // So, all 3 cards should raise a Source PropertyChanged event - but only the BMW should raise a View PropertyChanged event.
            carsList.Add(CarPool.carAudiA4);
            carsList.Add(CarPool.carBmwX5);
            carsList.Add(CarPool.carVwGolfGti);

            ObservableCollection<Car> filteredView = observableCarsView.View;

            // Assert
            filteredView.Should().NotBeNull();
            filteredView.Should().HaveCount(3);
            filteredView.Single(x => x.Model == CarPool.carBmwM1.Model).Should().NotBeNull();
            filteredView.Single(x => x.Model == CarPool.carBmwM3.Model).Should().NotBeNull();
            filteredView.Single(x => x.Model == CarPool.carBmwX5.Model).Should().NotBeNull();

            receivedEvents.Should().HaveCount(9);
            receivedEvents.Count(x => x == "Source").Should().Be(3);
            receivedEvents.Count(x => x == "View").Should().Be(3);
            receivedEvents.Count(x => x == "Groups").Should().Be(3);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventIfItemsAreRemoved()
        {
            // Arrange
            var receivedEvents = new List<string>();
            var carsList = CarPool.GetDefaultCarsList();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.FilterHandler += (sender, e) => e.IsAllowed = e.Item.Brand == CarBrand.BMW;
            ;
            observableCarsView.PropertyChanged += (sender, e) => receivedEvents.Add(e.PropertyName);

            // Act
            // Let's remove 3 existing cars. The View should then only contain the remaining BMW (M3).
            carsList.Remove(CarPool.carBmwM1);
            carsList.Remove(CarPool.carVwPolo);
            carsList.Remove(CarPool.carAudiA1);

            ObservableCollection<Car> filteredView = observableCarsView.View;

            // Assert
            filteredView.Should().NotBeNull();
            filteredView.Should().HaveCount(1);
            filteredView.Single(x => x.Model == CarPool.carBmwM3.Model).Should().NotBeNull();

            receivedEvents.Should().HaveCount(9);
            receivedEvents.Count(x => x == "Source").Should().Be(3); // 3 Source changes, because we removed 3 new elements
            receivedEvents.Count(x => x == "View").Should().Be(3);
            receivedEvents.Count(x => x == "Groups").Should().Be(3);
        }

        #endregion

        #region Searching

        [Fact]
        public void ShouldFindItemsOnSearchUsingSearchableAnnotation()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();
            var observableCarsView = new ObservableView<Car>(carsList);

            // Act
            observableCarsView.Search("Polo");

            // Assert
            var searchView = observableCarsView.View;
            searchView.Should().NotBeNull();
            searchView.Should().HaveCount(1);

            searchView.Single(x => x.Model == CarPool.carVwPolo.Model).Should().NotBeNull();
        }

        [Fact]
        public void ShouldFindItemsOnSearchUsingAddSearchSpecification()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();
            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.SearchSpecification
                .Add(c => c.Model, BinaryOperator.Contains)
                .Or(c => c.Year, BinaryOperator.Contains);

            // Act
            observableCarsView.Search("20");

            // Assert
            var searchView = observableCarsView.View;
            searchView.Should().NotBeNull();
            searchView.Should().HaveCount(6); // This not only includes 'Year' filtered items, but also the [Searchable] annotated ones

            searchView.Should().Contain(CarPool.carAudiA1);
            searchView.Should().Contain(CarPool.carAudiA4);
            searchView.Should().Contain(CarPool.carAudiA3);
            searchView.Should().Contain(CarPool.carBmwM1);
            searchView.Should().Contain(CarPool.carBmwM3);
            searchView.Should().Contain(CarPool.carVwGolf);
        }

        [Fact]
        public void ShouldSplitMultipleWordsWithLogicOrOperator()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();
            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.SearchTextDelimiters = new[] { ' ' };
            observableCarsView.SearchTextLogic = SearchLogic.Or;

            // Act
            observableCarsView.Search("M 3");

            // Assert
            var searchView = observableCarsView.View;
            searchView.Should().NotBeNull();
            searchView.Should().HaveCount(3); // The whitespace delimiter should act as logic OR operator

            searchView.Should().Contain(CarPool.carAudiA3);
            searchView.Should().Contain(CarPool.carBmwM1);
            searchView.Should().Contain(CarPool.carBmwM3);
        }

        [Fact]
        public void ShouldSplitMultipleWordsWithLogicAndOperator()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();
            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.SearchTextDelimiters = new[] { ' ' };
            observableCarsView.SearchTextLogic = SearchLogic.And;

            // Act
            observableCarsView.Search("M 3"); // The whitespace delimiter should act as logic AND operator

            // Assert
            var searchView = observableCarsView.View;
            searchView.Should().NotBeNull();
            searchView.Should().HaveCount(1);

            searchView.Should().Contain(CarPool.carBmwM3);
        }

        [Fact]
        public void ShouldUseCustomSearchTextPreprocessor()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();
            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.SearchTextDelimiters = new[] { ' ' };
            observableCarsView.SearchTextPreprocessor = searchText => { return searchText.Replace("AND", ""); };

            // Act
            observableCarsView.Search("Birthday AND Golf");

            // Assert
            var searchView = observableCarsView.View;
            searchView.Should().NotBeNull();
            searchView.Should().HaveCount(1);

            searchView.Should().Contain(CarPool.carVwGolf);
        }

        [Fact]
        public void ShouldThrowExceptionWhenThereAreNoSearchSpecificationsDefined()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.SearchSpecification.Clear();

            // Act
            observableCarsView.Search("Polo");
            Action action = () => { var searchView = observableCarsView.View; };

            // Assert
            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void ShouldClearSearch()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.Search("Polo");

            // Act
            observableCarsView.ClearSearch();

            // Assert
            var searchView = observableCarsView.View;
            searchView.Should().NotBeNull();
            searchView.Should().HaveCount(carsList.Count);
        }

        [Fact]
        public void ShouldClearSearchSpecifications()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.SearchSpecification.Add(c => c.Year, BinaryOperator.Equal); //TODO: Should work with default operator now.
            observableCarsView.Search("2000");

            // Act
            observableCarsView.SearchSpecification.Clear();

            // Assert
            observableCarsView.SearchText.Should().Be(string.Empty);

            var searchView = observableCarsView.View;
            searchView.Should().NotBeNull();
            searchView.Should().HaveCount(carsList.Count);
        }

        #endregion

        #region Grouping

        [Fact]
        public void ShouldGroup_UsingAlphaGroupKeyAlgorithm_ByDefaultToGenerateGroupKeys()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.GroupKey = car => car.Brand.ToString();

            // Act
            var groups = observableCarsView.Groups.ToList();

            // Assert
            observableCarsView.GroupKeyAlgorithm.Should().NotBeNull();
            observableCarsView.GroupKeyAlgorithm.Should().BeOfType<AlphaGroupKeyAlgorithm>();

            groups.Should().NotBeNull();
            groups.Should().HaveCount(3);

            var groupAudi = groups.Single(g => g.Key == "A");
            groupAudi.Should().NotBeNull("AlphaGroupKeyAlgorithm should generate 'a' with the CarBrand.Audi.ToString()");
            groupAudi.Should().HaveCount(3);

            var groupBMW = groups.SingleOrDefault(g => g.Key == "B");
            groupBMW.Should().NotBeNull("AlphaGroupKeyAlgorithm should generate 'b' with the CarBrand.BMW.ToString()");
            groupBMW.Should().HaveCount(2);

            var groupVW = groups.Single(g => g.Key == "V");
            groupVW.Should().NotBeNull("AlphaGroupKeyAlgorithm should generate 'v' with the CarBrand.VW.ToString()");
            groupVW.Should().HaveCount(2);
        }

        [Fact]
        [UseCulture("en-US")]
        public void ShouldGroupAndSort_UsingAlphaGroupKeyAlgorithm()
        {
            // Arrange
            var carsList = CarPool.GetAllCars();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.GroupKey = t => t.CreatedDate;
            observableCarsView.GroupKeyAlgorithm = new MonthGroupAlgorithm();
            observableCarsView.AddOrderSpecification(t => t.CreatedDate, OrderDirection.Descending);

            // Act
            var groups = observableCarsView.Groups.ToList();

            // Assert
            groups.Should().NotBeNull();
            groups.Should().HaveCount(8);

            var groupDecember2016 = groups.ElementAt(0);
            groupDecember2016.Key.EndsWith("December 2016").Should().BeTrue("MonthGroupAlgorithm should generate group 'December 2016' at position 0 (first)");
            groupDecember2016.Should().HaveCount(1);

            var groupXy = groups.ElementAt(7);
            groupXy.Key.EndsWith("January 1990").Should().BeTrue("MonthGroupAlgorithm should generate group 'January 1990' at position 0 (first)");
            groupXy.Should().HaveCount(1);
        }

        #endregion

        #region Sorting

        [Fact]
        public void ShouldOrderSingleOrderSpecificationDescending()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.AddOrderSpecification(x => x.Brand, OrderDirection.Descending);

            // Act
            var orderedView = observableCarsView.View;

            // Assert
            orderedView.Should().NotBeNull();
            orderedView.Should().HaveCount(carsList.Count);

            orderedView[0].Brand.Should().Be(CarPool.carVwPolo.Brand); // Frist item in the View should be a VW
            orderedView[5].Brand.Should().Be(CarPool.carAudiA1.Brand); // Last item in the View should be an Audi
        }

        [Fact]
        public void ShouldOrderMultipleOrderSpecificationsUsingPropertyExpression()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.AddOrderSpecification(x => x.Brand);
            observableCarsView.AddOrderSpecification(x => x.Model, OrderDirection.Descending);

            // Act
            var orderedView = observableCarsView.View;

            // Assert
            orderedView.Should().NotBeNull();
            orderedView.Should().HaveCount(carsList.Count);

            orderedView.Should().ContainInOrder(
                CarPool.carAudiA4,
                // The first one should be the Audi A4
                CarPool.carAudiA3,
                CarPool.carAudiA1,
                CarPool.carBmwM3,
                CarPool.carBmwM1,
                CarPool.carVwPolo,
                CarPool.carVwGolf); // The last one should be the VW Golf
        }

        ////[Fact]
        ////public void ShouldOrderMultipleOrderSpecificationsUsingStringPropertyName()
        ////{
        ////    // Arrange
        ////    var carsList = new ObservableCollection<Car>
        ////    {
        ////        Cars.carAudiA1, 
        ////        Cars.carAudiA3, 
        ////        Cars.carBmwM1, 
        ////        Cars.carBmwM3, 
        ////        Cars.carVwPolo,
        ////        Cars.carVwGolf
        ////    };

        ////    var observableCarsView = new ObservableView<Car>(carsList);
        ////    observableCarsView.AddOrderSpecification("Brand"); // BUG: Test fails here! FIX!
        ////    observableCarsView.AddOrderSpecification("Model", OrderDirection.Descending);

        ////    // Act
        ////    var orderedView = observableCarsView.View;

        ////    // Assert
        ////    orderedView.Should().NotBeNull();
        ////    orderedView.Should().HaveCount(carsList.Count);

        ////    orderedView[0].Model.Should().Be(Cars.carAudiA3.Model); // The first one should be the Audi A3
        ////    orderedView[1].Model.Should().Be(Cars.carAudiA1.Model);
        ////    orderedView[2].Model.Should().Be(Cars.carBmwM3.Model);
        ////    orderedView[3].Model.Should().Be(Cars.carBmwM1.Model);
        ////    orderedView[4].Model.Should().Be(Cars.carVwPolo.Model);
        ////    orderedView[5].Model.Should().Be(Cars.carVwGolf.Model); // The last one should be the VW Golf
        ////}

        [Fact]
        public void ShouldRemoveOrderSpecifications()
        {
            // Arrange
            var carsList = CarPool.GetDefaultCarsList();

            var observableCarsView = new ObservableView<Car>(carsList);
            observableCarsView.AddOrderSpecification(x => x.Brand, OrderDirection.Descending);

            // Act
            observableCarsView.ClearOrderSpecifications();

            // Assert
            var orderedView = observableCarsView.View;

            orderedView.Should().NotBeNull();
            orderedView.Should().HaveCount(carsList.Count);

            orderedView.SequenceEqual(carsList).Should().BeTrue();
        }

        #endregion

    }
}