using Blazorise.DataGrid;

namespace MTGView.Blazor.Server.Extensions;
public static class IEnumerableExtensions
{
    #region IQueryable Methods
    /// <summary>
    /// Orders the provided <see cref="IQueryable{T}"/> <paramref name="source"/> by the columns in <paramref name="columnStates"/>
    /// </summary>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <param name="source">Provided list of entities of <typeparamref name="T"/></param>
    /// <param name="columnStates">The provided list of columns and their sort directions as a <see cref="ColumnState"/></param>
    /// <returns>An <see cref="IQueryable{T}"/> that is ordered by the provided <paramref name="columnStates"/></returns>
    public static IQueryable<T> DynamicSort<T>(this IQueryable<T> source, DataGridReadDataEventArgs<T> columnStates)
    {
        source = columnStates
            .Columns
            .Where(cs => cs.SortDirection is not SortDirection.Default)
            .Aggregate(source, (current, columnState) => SortBy(current.AsQueryable(), columnState));

        return source;
    }

    /// <summary>
    /// Filters results by the specified filterable parameters in the <paramref name="columnStates"/>
    /// </summary>
    /// <typeparam name="T">The type we're filtering results down</typeparam>
    /// <param name="source">The source queryable</param>
    /// <param name="columnStates">Contains the filtering context</param>
    /// <returns></returns>
    public static IQueryable<T> DynamicFilter<T>(this IQueryable<T> source, DataGridReadDataEventArgs<T> columnStates)
    {
        source = columnStates
            .Columns
            .Where(cs => cs.SearchValue is not null)
            .Aggregate(source, (current, columnState) => FilterBy(current.AsQueryable(), columnState));

        return source;
    }


    /// <summary>
    /// Splits the <paramref name="source"/> into chunks of a specified size, and returns them as an array to iterate through
    /// </summary>
    /// <typeparam name="T">The underlying chunk type</typeparam>
    /// <param name="source">The provided queryable</param>
    /// <param name="columnInfo">Information regarding the chunk size</param>
    /// <returns></returns>
    public static IQueryable<T> Paging<T>(this IQueryable<T> source, DataGridReadDataEventArgs<T> columnInfo)
    {
        return source
            .Skip(columnInfo.PageSize * (columnInfo.Page -1))
            .Take(columnInfo.PageSize);
    }
    #endregion
    #region IEnumerable Methods
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

        return source;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="columnStates"></param>
    /// <returns></returns>
    public static IEnumerable<T> DynamicFilter<T>(this IEnumerable<T> source, DataGridReadDataEventArgs<T> columnStates)
    {
        source = columnStates
            .Columns
            .Where(cs => cs.SearchValue is not null)
            .Aggregate(source, (current, columnState) => FilterBy(current.AsQueryable(), columnState));

        return source;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="columnInfo"></param>
    /// <returns></returns>
    public static IEnumerable<T> Paging<T>(this IEnumerable<T> source, DataGridReadDataEventArgs<T> columnInfo)
    {
        return source
            .Chunk(columnInfo.PageSize)
            .ToArray()
            [columnInfo.Page - 1]
            .ToList();
    }
    #endregion
    #region Private Methods
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
    private static bool IsOrdered<T>(this IQueryable<T> source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return source.Expression.Type == typeof(IOrderedQueryable);
    }
    #endregion
}