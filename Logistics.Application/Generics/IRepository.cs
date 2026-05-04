namespace Logistics.Application.Generics;

/// <summary>
/// Type-safe generic repository contract.
/// </summary>
public interface IRepository<T> where T : class
{
    void Add(T item);

    IReadOnlyCollection<T> GetAll();

    IReadOnlyCollection<T> Find(Func<T, bool> predicate);

    bool Remove(Func<T, bool> predicate);
}
