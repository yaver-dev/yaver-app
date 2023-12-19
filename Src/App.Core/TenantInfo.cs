using Microsoft.AspNetCore.Http;

namespace Yaver.App;

public record TenantInfo(
  // Guid Id,
  string Name
);

public interface ITenantMetadata {
  TenantInfo? TenantInfo { get; }
}

public class TenantMetadata : ITenantMetadata {
  private readonly TenantInfo? _tenantInfo;

  public TenantMetadata(IHttpContextAccessor httpContextAccessor) {
    _tenantInfo = httpContextAccessor.HttpContext?.Features.Get<TenantInfo>();
    // ArgumentNullException.ThrowIfNull(_tenantInfo);
  }

  public TenantMetadata(TenantInfo tenantInfo) {
    _tenantInfo = tenantInfo;
    ArgumentNullException.ThrowIfNull(_tenantInfo);
  }

  public TenantInfo TenantInfo => _tenantInfo;
}

public class DesignTimeTenantMetadata : ITenantMetadata {
  public TenantInfo TenantInfo { get; } = new(
    Name: string.Empty
  );
}
