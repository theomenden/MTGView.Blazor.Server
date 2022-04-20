using System.Buffers;

namespace MTGView.Core.Extensions;

public static class ArrayExtensions
{
    public static T[] SubArray<T>(this T[] source, Int32 index)
    {
        return SubArray<T>(source, index, source.Length - index);
    }

    public static T[] SubArray<T>(this T[] source, Int32 index, Int32 length)
    {
        if (index == 0 && length == source.Length)
        {
            return source;
        }

        if (length == 0)
        {
            return Array.Empty<T>();
        }

        var subarray = new T[length];

        Array.Copy(source, index, subarray, 0, length);

        return subarray;
    }

    public static T[] Append<T>(this T[] source, T[] appendArray, Int32 index, Int32 length)
    {
        if (length == 0)
        {
            return source;
        }

        var newLength = source.Length + length - index;

        if (newLength <= 0)
        {
            return Array.Empty<T>();
        }

        var newArray = new T[newLength];

        Array.Copy(source, 0, newArray, 0, source.Length);

        Array.Copy(appendArray, index, newArray, source.Length, length - index);

        return newArray;
    }

    public static T[] CopyPooled<T>(this T[] source)
    {
        return SubArrayPooled(source, 0, source.Length);
    }

    public static T[] SubArrayPooled<T>(this T[] source, Int32 index, Int32 length)
    {
        var subarray = ArrayPool<T>.Shared.Rent(length);

        Array.Copy(source, index, subarray, 0, length);

        return subarray;
    }

    public static void ReturnArrayToPool<T>(this T[] source)
    {
        if (source is null)
        {
            return;
        }

        ArrayPool<T>.Shared.Return(source);
    }
}

