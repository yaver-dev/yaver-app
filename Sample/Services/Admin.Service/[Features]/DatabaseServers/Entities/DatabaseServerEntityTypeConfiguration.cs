using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Admin.ServiceBase.DatabaseServers.Entities;

public class DatabaseServerEntityTypeConfiguration : IEntityTypeConfiguration<DatabaseServer>
{
  public void Configure(EntityTypeBuilder<DatabaseServer> entity)
  {
    entity.HasIndex(b => new { b.Host }).IsUnique();

    entity
            .Property(e => e.Status)
            .HasConversion(
                    v => v.ToString(),
                    v => (DatabaseServerStatus)Enum.Parse(typeof(DatabaseServerStatus), v));
  }
}
