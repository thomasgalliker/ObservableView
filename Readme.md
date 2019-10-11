# ObservableView
<img src="https://raw.githubusercontent.com/thomasgalliker/ObservableView/master/logo_gradient.png" alt="ObservableView" align="right" height="100">
ObservableView is a simple wrapper for collections which provides an easy-to-use API for searching, filtering, sorting and grouping of collections. This project enhances the well-known ObservableCollection of the .Net Framework with addition, commonly-used features. The goal is to have a Swiss army knife of a collection utility which provides an easy-to-use but very powerful API while preserving maximum platform compatibility.

### Download and Install ObservableView
This library is available on NuGet: https://www.nuget.org/packages/ObservableView/
Use the following command to install ObservableView using NuGet package manager console:

    PM> Install-Package ObservableView

You can use this library in any .Net project which is compatible to .Net Framework 4.5+ and .Net Standard 1.3+ (e.g. Xamarin, WPF)

### Platform Support

|Platform|Version|
| ------------------- | :-----------: |
|Xamarin.iOS|iOS 8+|
|Xamarin.Android|API 14+|
|WPF|.NET 4.5+|

**Xamarin.iOS  Setup**
You must set the line `ObservableView.Platform.Init();` in your projects AppDelegate:
```C#
public override bool FinishedLaunching(UIApplication app, NSDictionary options)
{
    Xamarin.Forms.Forms.Init();
    ObservableView.Platform.Init(); // <--
    this.LoadApplication(new App());

    return base.FinishedLaunching(app, options);
}
```

### API Usage
#### Basic MVVM data binding with List Views
The usage of `ObservableView<T>` is not much different from `ObservableCollection<T>`:
1) Create a public `ObservableView<T>` property in your ViewModel.
```C#
public ObservableView<Mall> MallList { get; }
```

2) Fill the `ObservableView<T>.Source` with item view models.
```C#
public MallListViewModel(IMallService mallService)
{
	var allMalls = mallService.GetAllMalls();
	this.MallList = new ObservableView<Mall>(allMalls);
}
```

3) Create a View with a ListView (or any other collection control) and bind the items source to `ObservableView<T>.View`.
```C#
<ListView ItemsSource="{Binding MallList.View}">
	<ListView.View>
		<GridView>
			<!--Title-->
			<GridViewColumn Header="Title" Width="Auto">
				<GridViewColumn.CellTemplate>
					<DataTemplate>
						<TextBlock Text="{Binding Title}" />
					</DataTemplate>
				</GridViewColumn.CellTemplate>
			</GridViewColumn>

			<!--Subtitle-->
			<GridViewColumn Header="Subtitle" Width="Auto">
				<GridViewColumn.CellTemplate>
					<DataTemplate>
						<TextBlock Text="{Binding Subtitle}" />
					</DataTemplate>
				</GridViewColumn.CellTemplate>
			</GridViewColumn>
		</GridView>
	</ListView.View>
</ListView>
```

As you can observe in the example above, the XAML view binds to `MallList.View`. This is important in order to reflect operation (search, filter, group...) performed on the source collection.

#### Add, remove, update source collection
If you need to add or remove items of the source collection, you can simply do so by manipulating the MallList.Source property. By doing so, it automatically refreshes all dependent properties (e.g. View).

#### Search
Two steps are necessary in order to enable the search functionality:
1) Define search specification(s) for properties of your collection item type ```T```:
- Call ```SearchSpecification.Add``` for searchable properties:
```C#
this.MallsList.SearchSpecification.Add(x => x.Title, BinaryOperator.Contains);
this.MallsList.SearchSpecification.Add(x => x.Subtitle, BinaryOperator.Contains);
```
- Alternative: Annotate searchable properties with ```[Searchable]``` 
2) The search operation can be triggered either from within the ViewModel using ```ObservableView.Search(...)``` method or by binding ```ObservableView.SearchText``` in XAML to an input textbox.

#### Filtering
1) Subscribe FilterHandler event:
```C#
this.MallsList.FilterHandler += this.MallsList_FilterHandler;
```

2) Specify with each collection item if it is filtered or not:
```C#
private void MallsList_FilterHandler(object sender, ObservableView.Filtering.FilterEventArgs<Mall> e)
{
	if (e.Item.Title.Contains("Aber"))
	{
		e.IsAllowed = false;
	}
}
```

#### Sorting
There are many ways of how collections can be presented with defined sort orders. Method AddOrderSpecification can be used to set-up sort specifications for properties of type T.
```C#
this.MallsList.AddOrderSpecification(x => x.Title, OrderDirection.Ascending);
this.MallsList.AddOrderSpecification(x => x.Subtitle, OrderDirection.Descending);
```

In the XAML, we could either bind the ItemsSource property to MallsList.View or we can use the attached dependency property ```ObservableViewExtensions.ObservableView``` to bind MallsList directly to the DataGrid. The latter approach enables you to make use of multi-column sorting using the DataGrid headers.
```C#
<DataGrid netfx:ObservableViewExtensions.ObservableView="{Binding MallsList}"
		  AutoGenerateColumns="False">
	<DataGrid.Columns>
		<DataGridTextColumn Binding="{Binding Title}" Header="Title" CanUserSort="True" SortMemberPath="Title"/>
		<DataGridTextColumn Binding="{Binding Subtitle}" Header="Subtitle" CanUserSort="True" SortMemberPath="Subtitle"/>
	</DataGrid.Columns>
</DataGrid>
```

TODO: Describe how to use IComparer with custom column sort algorithms.

#### Grouping
ObservableView allows to specify a grouping algorithm as well as the key by which the collection is grouped:
```C#
this.MallsList.GroupKeyAlogrithm = new AlphaGroupKeyAlgorithm();
this.MallsList.GroupKey = mall => mall.Title;
```

### Performance considerations
Performance is a critical success factor for ObservableView. ObservableView has been tested with ten thousands of data records with good results. If you run into performance bottlenecks caused by ObservableView, do not hesitate to open a new issue.

### License
This project is Copyright &copy; 2019 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
