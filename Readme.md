# ObservableView
<img src="https://raw.githubusercontent.com/thomasgalliker/ObservableView/master/logo_gradient.png" alt="ObservableView" align="right" height="100">
ObservableView is a simple wrapper for collections which provides an easy-to-use API for searching, filtering, sorting and grouping of collections. This project enhances the well-known ObservableCollection of the .Net Framework with addition, commonly-used features. The goal is to have a Swiss army knife of a collection utility which provides an easy-to-use but very powerful API while preserving maximum platform compatibility.

### Download and Install ObservableView
This library is available on NuGet: https://www.nuget.org/packages/ObservableView/
Use the following command to install ObservableView using NuGet package manager console:

    PM> Install-Package ObservableView

You can use this library in any .Net project which is compatible to PCL (e.g. Xamarin Android, iOS, Windows Phone, Windows Store, Universal Apps, etc.)

### API Usage
#### Basic data binding in XAML with MVVM
The usage of ObservableView is not much different from ObservableCollection: Declare and instantiate ObservableView in a ViewModel, bind it to a View and finally fill it with data.
```
// Excerpt from a basic ViewModel which loads data into MallList:
public ObservableView<Mall> MallList { get; }

public MallListViewModel(IMallService mallService)
{
	var allMalls = mallService.GetAllMalls();
	this.MallList = new ObservableView<Mall>(allMalls);
}
```
```
// Excerpt from a View which binds MallList.View to a WPF ListView:
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

As you can observe in the example above, the XAML view binds to MallList.View. This is important in order to reflect operation (search, filter,...) performed on the source collection.

#### Add, remove, update source collection
If you need to add or remove items of the source collection, you can simply do so by manipulating the MallList.Source property. By doing so, it automatically refreshes all dependent properties (e.g. View).



#### Search
```
this.MallList.AddSearchSpecification(x => x.Title);

[Searchable]
```

#### Filter
TODO: Document

#### Sort
TODO: Document

#### Group
TODO: Document

### License
This project is Copyright &copy; 2015 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
