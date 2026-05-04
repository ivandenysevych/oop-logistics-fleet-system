namespace Logistics.Application.Extensions;

/// <summary>
/// Reusable generic extensions for collection processing.
/// </summary>
public static class EnumerableExtensions
{
    public static void ForEachItem<T>(this IEnumerable<T> source, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in source)
        {
            action(item);
        }
    }

    public static IReadOnlyList<TResult> Map<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        return source.Select(selector).ToList();
    }

    public static TAccumulate Reduce<TSource, TAccumulate>(
        this IEnumerable<TSource> source,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> aggregator)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(aggregator);

        return source.Aggregate(seed, aggregator);
    }
}
