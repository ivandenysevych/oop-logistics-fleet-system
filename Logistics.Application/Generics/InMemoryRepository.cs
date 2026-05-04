namespace Logistics.Application.Generics;

/// <summary>
/// In-memory repository for domain objects.
/// </summary>
public sealed class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    public IReadOnlyCollection<T> GetAll()
    {
        return _items.AsReadOnly();
    }

    public IReadOnlyCollection<T> Find(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return _items.Where(predicate).ToList();
    }

    public bool Remove(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var index = _items.FindIndex(item => predicate(item));
        if (index < 0)
        {
            return false;
        }

        _items.RemoveAt(index);
        return true;
    }
}
