using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Project.Generic;
public static class Extensions
{
    public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2>? kvp, out T1 key, out T2 value)
    {
        if (kvp is null)
        {
            key = default!;
            value = default!;
            return;
        }
        key = kvp.Value.Key;
        value = kvp.Value.Value;
    }

    public static void Clear(this IMemoryCache cache) => (cache as MemoryCache)?.Clear();

    public static string[] SplitValuesToArray(this StringValues values, string delimiter = ",")
    {
        return values
            .SelectMany(x => x?.Split(delimiter, StringSplitOptions.RemoveEmptyEntries) ?? [])
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();
    }

    public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
    {
        return source.Select(selector).Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
    }
}
