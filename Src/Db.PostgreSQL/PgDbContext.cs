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
public class PgDbContext(DbContextOptions options) : BaseDbContext(options) {

  /// <summary>
  ///   Override this method to further configure the model that was discovered by convention from the entity types
  ///   exposed in <see cref="DbSet{TEntity}" /> properties on your derived context. The resulting model may be cached
  ///   and re-used for subsequent instances of your derived context.
  /// </summary>
  /// <param name="builder">The builder being used to construct the model for this context.</param>
  protected override void OnModelCreating(ModelBuilder builder) {
    base.OnModelCreating(builder);
    builder
      .HasPostgresExtension("citext");

    foreach (var e in builder.Model.GetEntityTypes()) {
      foreach (var prop in e.GetProperties().Where(p => p.ClrType == typeof(string))) {
        prop.SetColumnType("citext");
      }
    }
  }

  public void GenerateUuidV7Function() {
    using var command = Database.GetDbConnection().CreateCommand();

    //https://gist.githubusercontent.com/kjmph/5bd772b2c2df145aa645b837da7eca74/raw/db903fb49a01380e3791f9893e4a8f62c1dd3085/A_UUID_v7_for_Postgres.sql
    command.CommandText = @"
    create or replace function uuid_generate_v7()
    returns uuid
    as $$
    begin
      -- use random v4 uuid as starting point (which has the same variant we need)
      -- then overlay timestamp
      -- then set version 7 by flipping the 2 and 1 bit in the version 4 string
      return encode(
        set_bit(
          set_bit(
            overlay(uuid_send(gen_random_uuid())
                    placing substring(int8send(floor(extract(epoch from clock_timestamp()) * 1000)::bigint) from 3)
                    from 1 for 6
            ),
            52, 1
          ),
          53, 1
        ),
        'hex')::uuid;
    end
    $$
    language plpgsql
    volatile;
    ";

    Database.OpenConnection();

    command.ExecuteNonQuery();
  }
}
