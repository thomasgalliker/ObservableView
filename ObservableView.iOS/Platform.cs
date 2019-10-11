using System;

//[assembly: Foundation.Preserve(typeof(System.Linq.Queryable), AllMembers = true)]
//[assembly: Foundation.Preserve(typeof(System.Linq.Enumerable), AllMembers = true)]

[assembly: Preserve]

namespace ObservableView
{
    [Preserve(AllMembers = true)]
    public static class Platform
    {
        public static void Init()
        {
            var temp = new DateTime();
        }
    }
}