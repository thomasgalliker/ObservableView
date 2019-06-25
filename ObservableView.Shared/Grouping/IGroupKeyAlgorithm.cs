
namespace ObservableView.Grouping
{
    public interface IGroupKeyAlgorithm<in T> : IGroupKeyAlgorithm
    {
        string GetGroupKey(T date);
    }

    public interface IGroupKeyAlgorithm
    {
        string GetGroupKey(object item);
    }
}
