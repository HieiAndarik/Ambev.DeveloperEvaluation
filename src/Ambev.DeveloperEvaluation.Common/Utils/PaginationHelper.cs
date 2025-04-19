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
}