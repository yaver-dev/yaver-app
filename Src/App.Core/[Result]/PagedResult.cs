// ReSharper disable once CheckNamespace

namespace Yaver.App;

/// <summary>
/// Represents a paged result that contains the total count and the value.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class PagedResult<T>(int totalCount, List<T> items) : Result<List<T>>(items) {
  public int TotalCount { get; } = totalCount;
}
