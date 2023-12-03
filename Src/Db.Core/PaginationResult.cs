// ReSharper disable once CheckNamespace

namespace Yaver.Db;

/// <summary>
///   Represents a paginated result containing a count and a list of items.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
/// <param name="Count">The total count of items.</param>
/// <param name="Items">The list of items.</param>
public record PaginationResult<T>(
  int Count,
  List<T> Items);
