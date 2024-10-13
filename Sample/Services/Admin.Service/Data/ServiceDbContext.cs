using Admin.Service.Features.DatabaseServers.Entities;
using Admin.Service.Features.Tenants.Entities;

using Microsoft.EntityFrameworkCore;

using Yaver.Db;

namespace Admin.Service.Data;

public class ServiceDbContext : InMemoryDbContext {
  private const string Schema = "multitenancy";


  public DbSet<DatabaseServer> DatabaseServers { get; init; }
  public DbSet<Tenant> Tenants { get; init; }


  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    optionsBuilder.UseInMemoryDatabase(Schema);
  }

  protected override void OnModelCreating(ModelBuilder builder) {
    builder.ApplyConfiguration(new DatabaseServerEntityTypeConfiguration());
  }
}
