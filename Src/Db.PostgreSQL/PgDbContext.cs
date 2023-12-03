using Microsoft.EntityFrameworkCore;

namespace Yaver.Db;

/// <summary>
///   Represents a PostgreSQL database context with additional configuration for case-insensitive text search.
/// </summary>
/// <remarks>
///   This context inherits from <see cref="BaseDbContext" /> and adds configuration for case-insensitive text search
///   by setting the column type of all string properties to "citext" and enabling the "citext" extension.
/// </remarks>
/// <seealso cref="BaseDbContext" />
public class PgDbContext(Guid currentUserId) : BaseDbContext(currentUserId) {
  /// <summary>
  ///   Override this method to further configure the model that was discovered by convention from the entity types
  ///   exposed in <see cref="DbSet{TEntity}" /> properties on your derived context. The resulting model may be cached
  ///   and re-used for subsequent instances of your derived context.
  /// </summary>
  /// <param name="builder">The builder being used to construct the model for this context.</param>
  protected override void OnModelCreating(ModelBuilder builder) {
    builder
      .HasPostgresExtension("citext");

    foreach (var e in builder.Model.GetEntityTypes()) {
      foreach (var prop in e.GetProperties().Where(p => p.ClrType == typeof(string))) {
        prop.SetColumnType("citext");
      }
    }

    base.OnModelCreating(builder);
  }
}
