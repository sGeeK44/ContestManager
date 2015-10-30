namespace Contest.Core.Windows.Mvvm
{
    public interface IViewModelItem<T>
    {
        void FromT(T obj);
        T ToT();
        void UpdateT(T obj);
    }
}
