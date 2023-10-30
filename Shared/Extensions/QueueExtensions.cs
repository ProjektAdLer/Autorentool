namespace Shared.Extensions;

public static class QueueExtensions
{
    public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> enumerable)
    {
        foreach (var item in enumerable)
        {
            queue.Enqueue(item);
        }
    }
}