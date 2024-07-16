using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Yaver.Db;

/// <summary>
///   Base class for database contexts that provides common functionality such as auditing and naming conventions.
/// </summary>
/// <remarks>
///   This class should be inherited by specific database contexts and configured accordingly.
/// </remarks>
public class BaseDbContext(DbContextOptions options) : DbContext(options) {
  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) {
    base.ConfigureConventions(configurationBuilder);

    configurationBuilder
      .Properties<DbId>()
      .HaveConversion<DbIdToGuidValueConverter>();
  }
}
