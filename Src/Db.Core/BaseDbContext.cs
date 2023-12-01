using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Yaver.Db;

/// <summary>
///   Base class for database contexts that provides common functionality such as auditing and naming conventions.
/// </summary>
/// <remarks>
///   This class should be inherited by specific database contexts and configured accordingly.
/// </remarks>
public class BaseDbContext(Guid currentUserId) : DbContext {
  /// <summary>
  ///   Configures the context with options such as the connection string, database provider, and other settings.
  /// </summary>
  /// <param name="optionsBuilder">The builder used to configure the context options.</param>
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    base.OnConfiguring(optionsBuilder);
    // optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
    optionsBuilder.AddInterceptors(new AuditableEntitiesInterceptor(currentUserId));

    optionsBuilder.UseSnakeCaseNamingConvention();
  }
}
