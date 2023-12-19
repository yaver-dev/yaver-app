using Microsoft.AspNetCore.Http;

namespace Yaver.App;

/// <summary>
/// Represents audit information for a user.
/// </summary>
public record AuditInfo(
  Guid UserId,
  string UserName,
  string Email,
  string GivenName,
  string FamilyName,
  List<string> Roles
);

/// <summary>
/// Represents metadata for auditing purposes.
/// </summary>
public interface IAuditMetadata {
  /// <summary>
  /// Gets the audit information for the object.
  /// </summary>
  AuditInfo? AuditInfo { get; }
}

/// <summary>
/// Represents metadata for auditing purposes.
/// </summary>
public class AuditMetadata : IAuditMetadata {
  private readonly AuditInfo? _auditInfo;

  /// <summary>
  /// Initializes a new instance of the <see cref="AuditMetadata"/> class using the provided <see cref="IHttpContextAccessor"/>.
  /// </summary>
  /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> used to access the current HTTP context.</param>
  public AuditMetadata(IHttpContextAccessor httpContextAccessor) {
    _auditInfo = httpContextAccessor.HttpContext?.Features.Get<AuditInfo>();
    // if (httpContextAccessor.HttpContext != null) {
    //   ArgumentNullException.ThrowIfNull(_auditInfo);
    // }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="AuditMetadata"/> class using the provided <see cref="AuditInfo"/>.
  /// </summary>
  /// <param name="info">The <see cref="AuditInfo"/> object containing the audit information.</param>
  public AuditMetadata(AuditInfo info) {
    _auditInfo = info;
    ArgumentNullException.ThrowIfNull(_auditInfo);
  }

  /// <summary>
  /// Gets the audit information associated with this metadata.
  /// </summary>
  public AuditInfo AuditInfo => _auditInfo;
}

/// <summary>
/// Represents metadata for design-time audit information.
/// </summary>
public class DesignTimeAuditMetadata : IAuditMetadata {
  /// <summary>
  /// Represents the audit information for a user.
  /// </summary>
  public AuditInfo AuditInfo { get; } = new(
    UserId: Guid.Empty,
    UserName: string.Empty,
    Email: string.Empty,
    GivenName: string.Empty,
    FamilyName: string.Empty,
    Roles: []
  );
}
