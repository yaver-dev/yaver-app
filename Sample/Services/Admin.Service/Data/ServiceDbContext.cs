using Admin.Service.Features.DatabaseServers.Entities;
using Admin.Service.Features.Tenants.Entities;

using Microsoft.EntityFrameworkCore;

using Yaver.App;
using Yaver.Db;

namespace Admin.Service.Data;

public class ServiceDbContext : InMemoryDbContext {
  private const string Schema = "multitenancy";

  public ServiceDbContext(IConfiguration configuration, IAuditMetadata auditMetadata)
    : base(auditMetadata.AuditInfo.UserId) {
    var connectionString = configuration.GetSection("ConnectionString").Value;
  }


  public DbSet<DatabaseServer> DatabaseServers { get; set; }
  public DbSet<Tenant> Tenants { get; set; }


  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    optionsBuilder.UseInMemoryDatabase(Schema);
    // base.OnConfiguring(optionsBuilder);
    // var connStr = _configuration.GetSection("MainDbConnectionString").Value;

    // _logger.LogInformation($"mainDbConn: {connStr}");
    // if (!optionsBuilder.IsConfigured) {
    // 	optionsBuilder.UseNpgsql(
    // 			connStr,
    // 			b => {
    // 				b.MigrationsAssembly("Admin.Service.App");
    // 				b.MigrationsHistoryTable("__EFMigrationsHistory", Schema);
    // 			}
    // 	);
    // }
  }

  protected override void OnModelCreating(ModelBuilder builder) {
    // base.OnModelCreating(builder);

    // builder.HasDefaultSchema(Schema);
    builder.ApplyConfiguration(new DatabaseServerEntityTypeConfiguration());
  }
}
