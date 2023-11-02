using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Admin.Service.Features.Tenants.Entities;

public class TenantEntityTypeConfiguration : IEntityTypeConfiguration<Tenant> {
  public void Configure(EntityTypeBuilder<Tenant> builder) {
    builder.HasIndex(b => new { TenantIdentifier = b.Identifier }).IsUnique();
  }
}
