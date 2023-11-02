namespace Yaver.App.Result;

/// <summary>
/// Represents a paged result that contains the paged information and the result value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public class PagedResult<T>(PagedInfo pagedInfo, T value) : Result<T>(value) {
  /// <summary>
  /// Gets the information about the pagination of the result set.
  /// </summary>
  public PagedInfo PagedInfo { get; } = pagedInfo;
}
