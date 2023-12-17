using Microsoft.AspNetCore.Http;

namespace Yaver.App;

public record TenantInfo(
  // Guid Id,
  string Name
);

public interface ITenantContext {
  TenantInfo TenantInfo { get; }
}

public class TenantContext : ITenantContext {
  private readonly TenantInfo? _tenantInfo;

  public TenantContext(IHttpContextAccessor httpContextAccessor) {
    _tenantInfo = httpContextAccessor?.HttpContext?.Features.Get<TenantInfo>();
  }

  public TenantContext(TenantInfo tenantInfo) {
    _tenantInfo = tenantInfo;
  }

  public TenantInfo TenantInfo => _tenantInfo;
}

public class DesignTimeTenantContext : ITenantContext {
  public TenantInfo TenantInfo { get; } = new(
    // Id: Guid.Empty,
    Name: string.Empty
  );
}
