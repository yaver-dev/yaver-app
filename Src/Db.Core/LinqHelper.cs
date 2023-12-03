using System.Linq.Expressions;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Yaver.Db;

/// <summary>
///   Provides helper methods for LINQ expressions and filters.
/// </summary>
public static class LinqHelper {
  /// <summary>
  ///   Builds an expression tree from a property chain string for a given type.
  /// </summary>
  /// <param name="T">The type to build the expression for.</param>
  /// <param name="boundExpression">The expression to bind the property chain to.</param>
  /// <param name="propertyName">The property chain string.</param>
  /// <returns>The expression tree representing the property chain.</returns>
  public static Expression? BuildExpressionFromPropertyChain(
    Type T,
    Expression boundExpression,
    string propertyName) {
    var propertyNameChain = propertyName.Split(".");
    var remainingChain = propertyNameChain.Skip(1).ToArray();

    var property = T.GetProperty(propertyNameChain[0],
      BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
    if (property == null) {
      return null;
    }

    var expression = Expression.Property(boundExpression, property);

    return remainingChain.Length > 0
      ? BuildExpressionFromPropertyChain(property.PropertyType, expression, string.Join(".", remainingChain))
      : expression;
  }

  // Generic filter builder. TODO: Could be moved to more appropriate extension class.
  /// <summary>
  ///   Builds a filter expression for the specified search term and fields.
  /// </summary>
  /// <typeparam name="T">The type of entity to filter.</typeparam>
  /// <param name="term">The search term to filter by.</param>
  /// <param name="fields">The fields to search for the search term.</param>
  /// <returns>An expression that filters entities based on the search term and fields.</returns>
  public static Expression<Func<T, bool>> BuildFilter<T>(string term, string[] fields) {
    Expression filterBody = Expression.Constant(false);

    var efLikeMethod = typeof(DbFunctionsExtensions).GetMethod("Like",
      BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
      null,
      [typeof(DbFunctions), typeof(string), typeof(string)],
      null);
    var pattern = Expression.Constant($"%{term}%", typeof(string));

    var parameterExpression = Expression.Parameter(typeof(T), "entity");

    foreach (var field in fields) {
      var fieldExpression = BuildExpressionFromPropertyChain(typeof(T), parameterExpression, field);

      if (efLikeMethod != null) {
        var fieldFilter = Expression.Call(
          efLikeMethod,
          Expression.Property(null, typeof(EF), nameof(EF.Functions)),
          fieldExpression,
          pattern
        );

        filterBody = Expression.OrElse(filterBody, fieldFilter);
      }
    }

    return Expression.Lambda<Func<T, bool>>(filterBody, parameterExpression);
  }
}
