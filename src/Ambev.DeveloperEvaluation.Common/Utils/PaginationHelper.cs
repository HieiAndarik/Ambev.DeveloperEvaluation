using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Common.Utils;

public static class PaginationHelper
{
    public static IEnumerable<T> ApplyPagination<T>(this IEnumerable<T> source, int page, int size)
    {
        return source.Skip((page - 1) * size).Take(size);
    }

    public static IEnumerable<T> ApplyOrdering<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, bool ascending = true)
    {
        return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }

    public static IEnumerable<T> ApplyOrdering<T>(this IEnumerable<T> source, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return source;

        var props = orderBy
            .Split(',')
            .Select(p => p.Trim().Split(' '))
            .Select(parts => new
            {
                Property = parts[0],
                Desc = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase)
            });

        IOrderedEnumerable<T>? ordered = null;

        foreach (var prop in props)
        {
            var param = Expression.Parameter(typeof(T));
            var property = Expression.Property(param, prop.Property);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

            ordered = ordered == null
                ? (prop.Desc ? source.OrderByDescending(lambda.Compile()) : source.OrderBy(lambda.Compile()))
                : (prop.Desc ? ordered.ThenByDescending(lambda.Compile()) : ordered.ThenBy(lambda.Compile()));
        }

        return ordered ?? source;
    }

}