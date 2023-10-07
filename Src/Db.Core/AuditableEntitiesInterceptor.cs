using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Yaver.Db;

/// <summary>
/// Initializes a new instance of the <see cref="AuditableEntitiesInterceptor"/> class with the specified current user ID.
/// </summary>
/// <param name="currentUserId">The ID of the current user.</param>
public class AuditableEntitiesInterceptor(Guid currentUserId) : SaveChangesInterceptor {

  // private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly Guid _currentUserId = currentUserId;


  /// <summary>
  /// Overrides the SavingChanges method of the <see cref="Microsoft.EntityFrameworkCore.Diagnostics.Interceptor"/> class
  /// to execute the BeforeSaveTriggers method before saving changes to the database.
  /// </summary>
  /// <param name="eventData">The <see cref="Microsoft.EntityFrameworkCore.Diagnostics.DbContextEventData"/> instance containing the event data.</param>
  /// <param name="result">The <see cref="Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult{TResult}"/> instance containing the interception result.</param>
  /// <returns>The <see cref="Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult{TResult}"/> instance containing the interception result.</returns>
  public override InterceptionResult<int> SavingChanges(
      DbContextEventData eventData,
      InterceptionResult<int> result) {
    if (eventData != null) {
      BeforeSaveTriggers(eventData.Context);
      return result;
    }

    throw new ArgumentNullException(nameof(eventData));
  }

  /// <summary>
  /// Asynchronously intercepts DbContext events for saving changes and executes the BeforeSaveTriggers method before saving changes.
  /// </summary>
  /// <param name="eventData">The event data.</param>
  /// <param name="result">The interception result.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>A task that represents the asynchronous operation and contains the interception result.</returns>
  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
      DbContextEventData eventData,
      InterceptionResult<int> result,
      CancellationToken cancellationToken = default) {
    if (eventData != null) {
      BeforeSaveTriggers(eventData.Context);
      return ValueTask.FromResult(result);
    }

    throw new ArgumentNullException(nameof(eventData));
  }

  private void BeforeSaveTriggers(DbContext? context) {
    var entries = context?.ChangeTracker
        .Entries()
        .Where(e => e.Entity is AuditableEntity && (
            e.State == EntityState.Added
            || e.State == EntityState.Modified));

    if (entries != null) {
      foreach (var entityEntry in entries) {
        if (entityEntry.Entity is AuditableEntity auditableEntity) {
          auditableEntity.UpdatedAt = DateTime.UtcNow;
          auditableEntity.UpdatedBy = _currentUserId;

          if (entityEntry.State == EntityState.Added) {
            auditableEntity.CreatedAt = DateTime.UtcNow;
            auditableEntity.CreatedBy = _currentUserId;
          }
        }
      }
    }
  }
}
