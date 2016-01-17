using System.Collections.ObjectModel;

using ObservableViewSample.Model;

namespace ObservableViewSample.Service
{
    public interface IMallManager
    {
        ObservableCollection<Mall> GetMalls();
    }
}