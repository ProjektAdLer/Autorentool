namespace Shared.Extensions;

/// <summary>
/// Contains static helper functions for generic enumerables.
/// </summary>
public static class EnumerableHelper
{
    /// <summary>
    /// Splits enumerable into even slices of requested size.
    /// </summary>
    /// <remarks>Last slice may contain less elements than requested size, in case the size of the enumerable isn't
    /// evenly divisible by requested size.</remarks>
    /// <param name="enumerable">The enumerable to be split.</param>
    /// <param name="size">The requested size of the slices.</param>
    /// <typeparam name="T">Type of the elements in the enumerable, should be inferred.</typeparam>
    /// <returns>An enumerable containing slices of requested size.</returns>
    /// <exception cref="ArgumentException">Argument <paramref name="size"/> cannot be 0.</exception>
    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> enumerable, uint size)
    {
        if (size == 0)
        {
            throw new ArgumentException("Size cannot be 0", nameof(size));
        }
        var enumerated = enumerable as T[] ?? enumerable.ToArray();
        for (var i = 0; i < (float)enumerated.Length / size; i++)
        {
            yield return enumerated.Skip((int)(i * size)).Take((int)size);
        }
    }
}