using System.Runtime.CompilerServices;

#if NET
[assembly: XmlnsDefinition("http://observableview", "ObservableView")]
[assembly: XmlnsDefinition("http://observableview", "ObservableView.Extensions")]
#endif

[assembly: InternalsVisibleTo("ObservableView.Tests")]