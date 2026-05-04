namespace Logistics.Application.Generics;

/// <summary>
/// Generic key-value cache with explicit constraints.
/// </summary>
public sealed class Cache<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _storage = new();

    public void Set(TKey key, TValue value)
    {
        _storage[key] = value;
    }

    public bool TryGet(TKey key, out TValue value)
    {
        return _storage.TryGetValue(key, out value!);
    }

    public bool Remove(TKey key)
    {
        return _storage.Remove(key);
    }
}
