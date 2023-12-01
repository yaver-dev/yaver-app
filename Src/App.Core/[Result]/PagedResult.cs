// ReSharper disable once CheckNamespace

namespace Yaver.App;

/// <summary>
/// Represents a paged result containing a list of items and the total count.
/// </summary>
/// <typeparam name="T">The type of items in the paged result.</typeparam>
public class PagedResult<T>(int totalCount, List<T> items) : Result<List<T>>(items) {
  /// <summary>
  /// Gets the total count of items.
  /// </summary>
  public int TotalCount { get; } = totalCount;
}
