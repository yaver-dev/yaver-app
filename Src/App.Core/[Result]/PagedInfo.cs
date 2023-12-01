// ReSharper disable once CheckNamespace

namespace Yaver.App;

/// <summary>
///   Represents the pagination information for a paged result set.
/// </summary>
public class PagedInfo(long pageNumber, long pageSize, long totalPages, long totalRecords) {
  /// <summary>
  ///   Gets or sets the current page number.
  /// </summary>
  public long PageNumber { get; private set; } = pageNumber;

  /// <summary>
  ///   Gets or sets the number of items per page.
  /// </summary>
  public long PageSize { get; private set; } = pageSize;

  /// <summary>
  ///   Gets or sets the total number of pages.
  /// </summary>
  public long TotalPages { get; private set; } = totalPages;

  /// <summary>
  ///   Gets or sets the total number of items.
  /// </summary>
  public long TotalRecords { get; private set; } = totalRecords;

  /// <summary>
  ///   Sets the current page number.
  /// </summary>
  /// <param name="pageNumber">The page number to set.</param>
  /// <returns>The updated PagedInfo instance.</returns>
  public PagedInfo SetPageNumber(long pageNumber) {
    PageNumber = pageNumber;

    return this;
  }

  /// <summary>
  ///   Sets the number of items per page.
  /// </summary>
  /// <param name="pageSize">The page size to set.</param>
  /// <returns>The updated PagedInfo instance.</returns>
  public PagedInfo SetPageSize(long pageSize) {
    PageSize = pageSize;

    return this;
  }

  /// <summary>
  ///   Sets the total number of pages.
  /// </summary>
  /// <param name="totalPages">The total number of pages to set.</param>
  /// <returns>The updated PagedInfo instance.</returns>
  public PagedInfo SetTotalPages(long totalPages) {
    TotalPages = totalPages;

    return this;
  }

  /// <summary>
  ///   Sets the total number of items.
  /// </summary>
  /// <param name="totalRecords">The total number of items to set.</param>
  /// <returns>The updated PagedInfo instance.</returns>
  public PagedInfo SetTotalRecords(long totalRecords) {
    TotalRecords = totalRecords;

    return this;
  }
}
