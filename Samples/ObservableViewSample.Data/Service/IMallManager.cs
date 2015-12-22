using System.Collections.ObjectModel;

using ObservableViewSample.Data.Model;

namespace ObservableViewSample.Data.Service
{
    public interface IMallManager
    {
        ObservableCollection<Mall> GetMalls();
    }
}