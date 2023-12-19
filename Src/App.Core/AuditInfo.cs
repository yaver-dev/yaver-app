using Microsoft.AspNetCore.Http;

namespace Yaver.App;

public record AuditInfo(
  Guid UserId,
  string UserName,
  string Email,
  string GivenName,
  string FamilyName,
  List<string> Roles
);

public interface IAuditMetadata {
  AuditInfo? AuditInfo { get; }
}

public class AuditMetadata : IAuditMetadata {
  private readonly AuditInfo? _auditInfo;

  public AuditMetadata(IHttpContextAccessor httpContextAccessor) {
    _auditInfo = httpContextAccessor.HttpContext?.Features.Get<AuditInfo>();
    // if (httpContextAccessor.HttpContext != null) {
    //   ArgumentNullException.ThrowIfNull(_auditInfo);
    // }
  }

  public AuditMetadata(AuditInfo info) {
    _auditInfo = info;
    ArgumentNullException.ThrowIfNull(_auditInfo);
  }

  public AuditInfo AuditInfo => _auditInfo;
}

public class DesignTimeAuditMetadata : IAuditMetadata {
  public AuditInfo AuditInfo { get; } = new(
    UserId: Guid.Empty,
    UserName: string.Empty,
    Email: string.Empty,
    GivenName: string.Empty,
    FamilyName: string.Empty,
    Roles: []
  );
}
