using Blazorise.DataGrid;

namespace MTGView.Blazor.Server.Extensions;
public static class IEnumerableExtensions
{
    public static bool IsOrdered<T>(this IQueryable<T> source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return source.Expression.Type == typeof(IOrderedQueryable);
    }

    /// <summary>
    /// Orders the provided <see cref="IQueryable{T}"/> <paramref name="source"/> by the columns in <paramref name="columnStates"/>
    /// </summary>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <param name="source">Provided list of entities of <typeparamref name="T"/></param>
    /// <param name="columnStates">The provided list of columns and their sort directions as a <see cref="ColumnState"/></param>
    /// <returns>An <see cref="IEnumerable{T}"/> that is ordered by the provided <paramref name="columnStates"/></returns>
    public static IEnumerable<T> DynamicSort<T>(this IEnumerable<T> source, DataGridReadDataEventArgs<T> columnStates)
    {
        source = columnStates
            .Columns
            .Where(cs => cs.SortDirection is not SortDirection.Default)
            .Aggregate(source, (current, columnState) => SortBy(current.AsQueryable(), columnState));

        return source.ToList();
    }

    public static IEnumerable<T> DynamicFilter<T>(this IEnumerable<T> source, DataGridReadDataEventArgs<T> columnStates)
    {
        source = columnStates
            .Columns
            .Where(cs => cs.SearchValue is not null)
            .Aggregate(source, (current, columnState) => FilterBy(current.AsQueryable(), columnState));

        return source.ToList();
    }

    public static IEnumerable<T> Paging<T>(this IEnumerable<T> source, DataGridReadDataEventArgs<T> columnInfo)
    {
        return source
            .Chunk(columnInfo.PageSize)
            .ToArray()
            [columnInfo.Page - 1]
            .ToList();
    }

    private static IQueryable<T> SortBy<T>(this IQueryable<T> source, DataGridColumnInfo columnState)
    {
        if (!source.IsOrdered())
        {
            return source.OrderBy($"{columnState.Field} {GetSortDirection(columnState.SortDirection)}");
        }

        var orderedQuery = source as IOrderedQueryable<T>;

        return orderedQuery.ThenBy($"{columnState.Field} {GetSortDirection(columnState.SortDirection)}");
    }

    private static String GetSortDirection(SortDirection sortDirection) =>
        sortDirection switch
        {
            SortDirection.Ascending => "asc",
            SortDirection.Descending => "desc",
            _ => String.Empty
        };

    private static IQueryable<T> FilterBy<T>(this IQueryable<T> source, DataGridColumnInfo columnState)
    {
        source = source.WhereInterpolated($"{columnState.Field} == {columnState.SearchValue}");

        return source;
    }
}