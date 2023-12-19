using Microsoft.AspNetCore.Http;

namespace Yaver.App;

/// <summary>
/// Represents information about a tenant.
/// </summary>
public record TenantInfo(
  // Guid Id,
  string Name
);

/// <summary>
/// Represents the metadata for a tenant.
/// </summary>
public interface ITenantMetadata {
  /// <summary>
  /// Gets the information about the tenant.
  /// </summary>
  TenantInfo? TenantInfo { get; }
}

/// <summary>
/// Represents metadata for a tenant.
/// </summary>
public class TenantMetadata : ITenantMetadata {
  private readonly TenantInfo? _tenantInfo;

  /// <summary>
  /// Initializes a new instance of the TenantMetadata class.
  /// </summary>
  /// <param name="httpContextAccessor">The IHttpContextAccessor used to access the current HttpContext.</param>
  public TenantMetadata(IHttpContextAccessor httpContextAccessor) {
    _tenantInfo = httpContextAccessor.HttpContext?.Features.Get<TenantInfo>();
    // ArgumentNullException.ThrowIfNull(_tenantInfo);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="TenantMetadata"/> class.
  /// </summary>
  /// <param name="tenantInfo">The tenant information.</param>
  public TenantMetadata(TenantInfo tenantInfo) {
    _tenantInfo = tenantInfo;
    ArgumentNullException.ThrowIfNull(_tenantInfo);
  }

  /// <summary>
  /// Gets the tenant information.
  /// </summary>
  public TenantInfo TenantInfo => _tenantInfo;
}

/// <summary>
/// Represents the metadata for a tenant during design time.
/// </summary>
public class DesignTimeTenantMetadata : ITenantMetadata {
  /// <summary>
  /// Represents information about a tenant.
  /// </summary>
  public TenantInfo TenantInfo { get; } = new(
    Name: string.Empty
  );
}
