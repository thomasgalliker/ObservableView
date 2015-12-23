using ObservableView.Netfx.Extensions;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ObservableView.Netfx
{
    public static class ObservableViewExtensions
    {
        private static DataGrid dataGrid;

        public static readonly DependencyProperty ObservableViewProperty = DependencyProperty.RegisterAttached(
            "ObservableView",
            typeof(object),
            typeof(ObservableViewExtensions),
            new UIPropertyMetadata(null, ObservableViewPropertyChanged));

        public static object GetObservableView(DependencyObject obj)
        {
            return obj.GetValue(ObservableViewProperty);
        }

        public static void SetObservableView(DependencyObject obj, object value)
        {
            obj.SetValue(ObservableViewProperty, value);
        }

        private static void ObservableViewPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            dataGrid = d as DataGrid;
            if (dataGrid == null)
            {
                return;
            }

            var oldObservableView = e.OldValue as IObservableView;
            if (oldObservableView != null)
            {
                dataGrid.Loaded -= DataGridLoaded;
                dataGrid.Unloaded -= DataGridUnloaded;
                dataGrid.Sorting -= OnDataGridSortingChanged;
            }

            var newObservableView = e.NewValue as IObservableView;
            if (newObservableView != null)
            {
                dataGrid.Loaded += DataGridLoaded;
                dataGrid.Unloaded += DataGridUnloaded;
                dataGrid.Sorting += OnDataGridSortingChanged;

                CheckIfItemsSourcePropertyIsNotBound();
                BindObservableViewToItemsSource();
            }
        }

        /// <summary>
        /// Check if there is a binding to ItemsSource.
        /// If you use the ObservableView dependency property, you must use ItemsSource at the same time.
        /// </summary>
        private static void CheckIfItemsSourcePropertyIsNotBound()
        {
            var itemsSourceBindingExpression = dataGrid.GetBindingExpression(ItemsControl.ItemsSourceProperty);
            if (itemsSourceBindingExpression != null)
            {
                throw new InvalidOperationException("Dependency property 'ItemsSource' must not have a binding for ObservableView to work properly. " +
                    "Bind to ObservableView instead.");
            }
        }

        /// <summary>
        /// Since we want to bind the ObservableView directly to the ObservableViewProperty,
        /// we have to make sure that the ItemsSourceProperty of the DataGrid is bound programmatically to the ObservableView.View property.
        /// </summary>
        private static void BindObservableViewToItemsSource()
        {
            var observableViewBindingExpression = dataGrid.GetBindingExpression(ObservableViewProperty);
            if (observableViewBindingExpression == null)
            {
                throw new InvalidOperationException("Dependency property 'ObservableView' does not have a valid binding.");
            }

            string viewPropertyBindingName = observableViewBindingExpression.ResolvedSourcePropertyName + ".View";
            var viewPropertyBinding = new Binding(viewPropertyBindingName);
            dataGrid.SetBinding(ItemsControl.ItemsSourceProperty, viewPropertyBinding);
        }

        private static void DataGridUnloaded(object sender, RoutedEventArgs e)
        {
            if (dataGrid == null)
            {
                return;
            }

            var observableView = GetObservableView(dataGrid) as IObservableView;
            if (observableView == null)
            {
                return;
            }

            observableView.PropertyChanged -= ObservableViewOnPropertyChanged;
        }

        private static void DataGridLoaded(object sender, RoutedEventArgs e)
        {
            if (dataGrid == null)
            {
                return;
            }

            var observableView = GetObservableView(dataGrid) as IObservableView;
            if (observableView == null)
            {
                return;
            }

            observableView.PropertyChanged += ObservableViewOnPropertyChanged;

            SyncObservableViewSortWithDataGridSort(observableView);
        }

        private static void ObservableViewOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "View")
            {
                var observableView = sender as IObservableView;
                if (observableView == null)
                {
                    return;
                }

                SyncObservableViewSortWithDataGridSort(observableView);
            }
        }

        private static void OnDataGridSortingChanged(object sender, DataGridSortingEventArgs e)
        {
            if (dataGrid == null)
            {
                return;
            }

            var observableView = GetObservableView(dataGrid) as IObservableView;
            if (observableView == null)
            {
                return;
            }

            observableView.ClearOrderSpecifications();

            SyncDataGridSortWithObservableViewSort(observableView);
        }

        /// <summary>
        ///     This method is used to synchronize the ObservableView's sort specification
        ///     with the DataGrid's sort specification.
        ///     This is the case if the ObservableView refreshes its data.
        ///     (For some mysterious reasons, the DataGrid loses its sort specification in this case)
        /// </summary>
        private static void SyncObservableViewSortWithDataGridSort(IObservableView observableView)
        {
            foreach (var dataGridColumn in dataGrid.Columns)
            {
                var listSortDirection = observableView.GetSortSpecification(dataGridColumn.SortMemberPath).ToSortDirection();
                if (listSortDirection != null)
                {
                    dataGridColumn.SortDirection = listSortDirection;
                }
            }
        }

        /// <summary>
        ///     This method is used to synchronize the DataGrid's sort specification
        ///     with the ObservableView's sort specification.
        ///     This is the case if the user clicks the sort headers in the DataGrid.
        /// </summary>
        private static void SyncDataGridSortWithObservableViewSort(IObservableView observableView)
        {
            foreach (var dataGridColumn in dataGrid.Columns)
            {
                var orderDirection = dataGridColumn.SortDirection.ToSortDirection();
                if (orderDirection != null)
                {
                    observableView.AddOrderSpecification(dataGridColumn.SortMemberPath, orderDirection.Value);
                }
            }
        }
    }
}