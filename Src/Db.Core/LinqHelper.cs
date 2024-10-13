using System.Linq.Expressions;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace Yaver.Db;

/// <summary>
///   Provides helper methods for building LINQ expressions and dynamic filtering, 
///   including support for searching, ordering, and building property chains.
/// </summary>
/// <remarks>
///   The <see cref="LinqHelper"/> class includes methods designed to simplify complex 
///   LINQ operations, such as dynamically constructing property chains and generating 
///   filter expressions for advanced queries.
///   
///   <list type="bullet">
///     <item>
///       <description>
///         <see cref="BuildExpressionFromPropertyChain"/> - Dynamically constructs an expression tree 
///         from a string representing a property chain, enabling flexible property access in LINQ queries.
///       </description>
///     </item>
///     <item>
///       <description>
///         <see cref="BuildFilter{T}"/> - Generates a LINQ expression for filtering by a search term 
///         across specified properties, supporting different data types including enums and primitives.
///       </description>
///     </item>
///   </list>
/// </remarks>
/// <example>
///   <code>
///     var filter = LinqHelper.BuildFilter&lt;Product&gt;("widget", new[] { "Name", "Category" });
///     var results = dbContext.Products.Where(filter).ToList();
///   </code>
/// </example>
public static class LinqHelper {
  // Holds a reference to the EF Core 'Like' method used for building filter expressions.
  private static readonly MethodInfo _efLikeMethod = typeof(DbFunctionsExtensions)
    .GetMethod(
      "Like",
      BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
      null,
      [
        typeof(DbFunctions),
        typeof(string),
        typeof(string)
      ],
      null
    )!;

  /// <summary>
  ///   Builds a LINQ expression to filter entities of a given type based on a search term across specified fields.
  /// </summary>
  /// <typeparam name="T">The type of the entity to filter.</typeparam>
  /// <param name="term">The search term used to filter the entities.</param>
  /// <param name="fields">
  ///   An array of field names (as strings) representing the properties of the entity type <typeparamref name="T"/> 
  ///   that will be searched against the search term.
  /// </param>
  /// <returns>
  ///   A <see cref="Func{T, Boolean}"/> expression that filters the entities of type <typeparamref name="T"/> 
  ///   based on whether any of the specified fields match the search term.
  /// </returns>
  /// <remarks>
  ///   This method constructs an OR-based filter expression that can be used with LINQ methods 
  ///   such as <see cref="Queryable.Where{TSource}(IQueryable{TSource}, Expression{Func{TSource, Boolean}})"/>. 
  ///   Each field in the <paramref name="fields"/> array is checked using a case-insensitive LIKE pattern 
  ///   to see if it contains the search term.
  /// </remarks>
  public static Expression<Func<T, bool>> BuildFilter<T>(string term, string[] fields) {
    Expression filterBody = Expression.Constant(false);
    var parameterExpression = Expression.Parameter(typeof(T), "entity");

    foreach (var field in fields) {
      var fieldExpression = BuildExpressionFromPropertyChain(typeof(T), parameterExpression, field);

      var propertyType = fieldExpression is MemberExpression memberExpression
        ? ((PropertyInfo)memberExpression.Member).PropertyType
        : null;

      if (propertyType == null) continue;

      var fieldFilter = propertyType switch {
        not null when propertyType == typeof(string) => (Expression)Expression.Call(
          _efLikeMethod!,
          Expression.Property(null, typeof(EF), nameof(EF.Functions)),
          fieldExpression!,
          Expression.Constant($"%{term}%", typeof(string))
        ),
        not null when propertyType == typeof(int) && int.TryParse(term, out var intValue) =>
          Expression.Equal(fieldExpression!, Expression.Constant(intValue)),
        not null when propertyType == typeof(bool) && bool.TryParse(term, out var boolValue) =>
          Expression.Equal(fieldExpression!, Expression.Constant(boolValue)),
        { IsEnum: true } when Enum.TryParse(propertyType, term, true, out var enumValue) =>
          Expression.Equal(fieldExpression!, Expression.Constant(enumValue)),
        _ => null
      };

      if (fieldFilter != null) {
        filterBody = Expression.OrElse(filterBody, fieldFilter);
      }
    }

    return Expression.Lambda<Func<T, bool>>(filterBody, parameterExpression);
  }


  /// <summary>
  /// Builds an expression tree that represents accessing a specified property chain
  /// on a given type. This enables dynamically building expressions for nested properties
  /// using a dot-separated property name string.
  /// </summary>
  /// <param name="type">The type to start from, where the property chain begins.</param>
  /// <param name="boundExpression">The initial expression to which the property chain is bound.</param>
  /// <param name="propertyName">The dot-separated property chain string (e.g., "Parent.Child.Property").</param>
  /// <returns>
  /// An <see cref="Expression"/> that represents the property chain access, or <c>null</c> 
  /// if any property in the chain does not exist.
  /// </returns>
  /// <remarks>
  /// This method uses reflection to access each property in the chain, allowing for
  /// complex, nested property access in dynamically generated LINQ expressions.
  /// </remarks>
  public static Expression? BuildExpressionFromPropertyChain(
    Type type,
    Expression boundExpression,
    string propertyName
  ) {
    var propertyNameChain = propertyName.Split(".");
    var expression = boundExpression;

    foreach (var property in propertyNameChain) {
      var propInfo = type.GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
      if (propInfo == null) {
        return null; // Property chain is invalid
      }

      expression = Expression.Property(expression, propInfo);
      type = propInfo.PropertyType; // Update type for the next property in the chain
    }

    return expression;
  }
}
