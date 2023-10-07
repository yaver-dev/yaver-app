using System.Linq.Expressions;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Yaver.Db;

/// <summary>
/// Provides extension methods for <see cref="IQueryable{T}"/> to enable pagination and dynamic sorting.
/// </summary>
public static class QueryableExtensions {
  private static readonly MethodInfo _orderByMethod =
      typeof(Queryable).GetMethods().Single(method =>
          method.Name == "OrderBy" && method.GetParameters().Length == 2);

  private static readonly MethodInfo _orderByDescendingMethod =
      typeof(Queryable).GetMethods().Single(method =>
          method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

  private static readonly MethodInfo _thenByMethod =
      typeof(Queryable).GetMethods().Single(method =>
          method.Name == "ThenBy" && method.GetParameters().Length == 2);

  private static readonly MethodInfo _thenByDescendingMethod =
      typeof(Queryable).GetMethods().Single(method =>
          method.Name == "ThenByDescending" && method.GetParameters().Length == 2);

  /// <summary>
  /// Paginates the given queryable source with optional filter, sort, offset and limit parameters.
  /// </summary>
  /// <typeparam name="T">The type of the elements of source.</typeparam>
  /// <param name="source">The source to paginate.</param>
  /// <param name="filter">The filter expression to apply to the source.</param>
  /// <param name="sort">The sort expression to apply to the source.</param>
  /// <param name="offset">The number of items to skip from the beginning of the source.</param>
  /// <param name="limit">The maximum number of items to return from the source.</param>
  /// <returns>A pagination result containing the count of items and the paginated items.</returns>
  public static async Task<PaginationResult<T>> PaginateAsync<T>(
      this IQueryable<T> source,
      Expression<Func<T, bool>> filter = null,
      string? sort = null,
      int? offset = null,
      int? limit = null
  ) where T : class {
    var itemIndex = offset ?? 0;
    var pageSize = limit ?? 25;

    if (filter != null)
      source = source.Where(filter);

    var count = await source.CountAsync();

    if (!string.IsNullOrEmpty(sort)) {
      var then = false;
      foreach (var item in sort.Split(",")) {
        var desc = false;
        var sortItem = item;
        if (item.StartsWith("-")) {
          sortItem = item[1..];
          desc = true;
        }

        source = desc
            ? source.OrderByPropertyDescending(sortItem, then)
            : source.OrderByProperty(sortItem, then);

        then = true;
      }
    }

    source = source.Skip(itemIndex);

    source = source.Take(pageSize);

    var items = await source.ToListAsync();

    return new PaginationResult<T>(default, default) {
      Count = count,
      Items = items
    };
  }

  /// <summary>
  /// Determines whether the specified property exists in the given IQueryable source.
  /// </summary>
  /// <typeparam name="T">The type of the IQueryable source.</typeparam>
  /// <param name="source">The IQueryable source to check for the property.</param>
  /// <param name="propertyName">The name of the property to check for.</param>
  /// <returns>true if the property exists in the IQueryable source; otherwise, false.</returns>
  public static bool PropertyExists<T>(this IQueryable<T> source, string propertyName) {
    return typeof(T).GetProperty(propertyName,
        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null;
  }

  private static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName, bool then = false) {
    var parameterExpression = Expression.Parameter(typeof(T));
    var orderByProperty = LinqHelper.BuildExpressionFromPropertyChain(typeof(T), parameterExpression, propertyName);
    if (orderByProperty == null)
      throw new ArgumentException($"Property '{propertyName}' is not a member of {typeof(T)}");

    var lambda = Expression.Lambda(orderByProperty, parameterExpression);
    var genericMethod =
        then
            ? _thenByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type)
            : _orderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
    var ret = genericMethod.Invoke(null, new object[] { source, lambda });
    return (IQueryable<T>)ret;
  }

  private static IQueryable<T> OrderByPropertyDescending<T>(this IQueryable<T> source, string propertyName,
      bool then = false) {
    var parameterExpression = Expression.Parameter(typeof(T));
    var orderByProperty = LinqHelper.BuildExpressionFromPropertyChain(typeof(T), parameterExpression, propertyName);
    if (orderByProperty == null)
      throw new ArgumentException($"Property '{propertyName}' is not a member of {typeof(T)}");

    var lambda = Expression.Lambda(orderByProperty, parameterExpression);
    var genericMethod =
        then
            ? _thenByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type)
            : _orderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
    var ret = genericMethod.Invoke(null, new object[] { source, lambda });
    return (IQueryable<T>)ret;
  }
}
