using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Admin.Service.Tenants.Entities;

public class TenantEntityTypeConfiguration : IEntityTypeConfiguration<Tenant>
{
  public void Configure(EntityTypeBuilder<Tenant> builder)
  {
    builder.HasIndex(b => new { TenantIdentifier = b.Identifier }).IsUnique();
  }
}
