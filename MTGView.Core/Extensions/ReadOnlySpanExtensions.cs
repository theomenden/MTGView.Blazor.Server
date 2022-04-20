namespace MTGView.Core.Extensions;
public static class ReadOnlySpanExtensions
{
    public static bool Any<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
    {
        for (var i = 0; i < source.Length - 1; i++)
        {
            if (predicate(source[i]))
            {
                return true;
            }
        }

        return false;
    }
}
