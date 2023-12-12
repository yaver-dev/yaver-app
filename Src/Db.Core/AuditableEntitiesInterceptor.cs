using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Yaver.Db;

/// <summary>
///   Initializes a new instance of the <see cref="AuditableEntitiesInterceptor" /> class with the specified current user
///   ID.
/// </summary>
/// <param name="currentUserId">The ID of the current user.</param>
public class AuditableEntitiesInterceptor(Guid currentUserId
  // IHttpContextAccessor contextAccessor,
  // ILogger<AuditableEntitiesInterceptor> logger
) : SaveChangesInterceptor {
  // private readonly IHttpContextAccessor _httpContextAccessor;
  // private readonly Guid _currentUserId = currentUserId;

  /// <summary>
  ///   Overrides the SavingChanges method of the <see cref="Microsoft.EntityFrameworkCore.Diagnostics.Interceptor" /> class
  ///   to execute the BeforeSaveTriggers method before saving changes to the database.
  /// </summary>
  /// <param name="eventData">
  ///   The <see cref="Microsoft.EntityFrameworkCore.Diagnostics.DbContextEventData" /> instance
  ///   containing the event data.
  /// </param>
  /// <param name="result">
  ///   The <see cref="Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult{TResult}" /> instance
  ///   containing the interception result.
  /// </param>
  /// <returns>
  ///   The <see cref="Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult{TResult}" /> instance containing
  ///   the interception result.
  /// </returns>
  public override InterceptionResult<int> SavingChanges(
    DbContextEventData eventData,
    InterceptionResult<int> result
  ) {
    ArgumentNullException.ThrowIfNull(eventData);

    BeforeSaveTriggers(eventData.Context);
    return result;
  }

  /// <summary>
  ///   Asynchronously intercepts DbContext events for saving changes and executes the BeforeSaveTriggers method before
  ///   saving changes.
  /// </summary>
  /// <param name="eventData">The event data.</param>
  /// <param name="result">The interception result.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>A task that represents the asynchronous operation and contains the interception result.</returns>
  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default
  ) {
    ArgumentNullException.ThrowIfNull(eventData);

    BeforeSaveTriggers(eventData.Context);
    return ValueTask.FromResult(result);
  }

  private void BeforeSaveTriggers(DbContext? context) {
    var entries = context?.ChangeTracker
      .Entries()
      .Where(e => e is { Entity: AuditableEntity, State: EntityState.Added or EntityState.Modified });

    // var principal = contextAccessor.HttpContext.User;
    //
    // var currentUserId = Guid.Empty;
    //
    // if (!principal.Identity!.IsAuthenticated) {
    //   logger.LogCritical("Claims principal is not authenticated and could not generate audit logs");
    // } else {
    //   currentUserId = Guid.Parse(principal.FindFirst("sub")!.Value);
    // }

    if (entries == null) return;

    foreach (var entityEntry in entries) {
      if (entityEntry.Entity is not AuditableEntity auditableEntity) continue;

      auditableEntity.UpdatedAt = DateTime.UtcNow;
      auditableEntity.UpdatedBy = currentUserId;

      if (entityEntry.State != EntityState.Added) continue;

      auditableEntity.CreatedAt = DateTime.UtcNow;
      auditableEntity.CreatedBy = currentUserId;
    }
  }
}
