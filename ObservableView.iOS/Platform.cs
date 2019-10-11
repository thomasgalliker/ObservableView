[assembly: Foundation.Preserve(typeof(System.Linq.Queryable), AllMembers = true)]
[assembly: Foundation.Preserve(typeof(System.Linq.Enumerable), AllMembers = true)]

[assembly: Preserve(AllMembers = true)]

namespace ObservableView
{
    [Preserve(AllMembers = true)]
    public static class Platform
    {
        public static void Init()
        {
            var observableView = new ObservableView<object>();
            observableView.PropertyChanged += (s, e) => observableView.SearchText = "";
        }
    }
}