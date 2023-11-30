using System.Linq.Expressions;

public static class LinqExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool IsInQuery, Expression<Func<T, bool>> predicate)
    {
        if (IsInQuery)
            query = query.Where(predicate);

        return query;
    }

    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool IsInQuery, Func<T, bool> predicate)
    {
        if (IsInQuery)
            query = query.Where(predicate);

        return query;
    }
}