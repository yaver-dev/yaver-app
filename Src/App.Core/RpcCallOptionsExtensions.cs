using System.Text.Json;

using Grpc.Core;

namespace Yaver.App;

public static class RpcCallOptionsExtensions {
  public static CallOptions WithRequestMetadata(
    this CallOptions callOptions,
    IRequestMetadata metadata
  ) {
    if (metadata.RequestInfo == null) return callOptions;
    var headers = callOptions.Headers ?? [];
    headers.Add("x-request-metadata", JsonSerializer.Serialize(metadata.RequestInfo));
    return callOptions.WithHeaders(headers);
  }

  public static CallOptions WithAuditMetadata(
    this CallOptions callOptions,
    IAuditMetadata metadata
  ) {
    if (metadata.AuditInfo == null) return callOptions;
    var headers = callOptions.Headers ?? [];
    headers.Add("x-audit-metadata", JsonSerializer.Serialize(metadata.AuditInfo));
    return callOptions.WithHeaders(headers);
  }

  public static CallOptions WithTenantMetadata(
    this CallOptions callOptions,
    ITenantMetadata metadata
  ) {
    if (metadata.TenantInfo == null) return callOptions;
    var headers = callOptions.Headers ?? [];
    headers.Add("x-tenant-metadata", JsonSerializer.Serialize(metadata.TenantInfo));
    return callOptions.WithHeaders(headers);
  }

  public static CallOptions WithYaverMetadata(
    this CallOptions callOptions,
    CancellationToken cancellationToken = new(),
    IRequestMetadata? requestMetadata = null,
    IAuditMetadata? auditMetadata = null,
    ITenantMetadata? tenantMetadata = null
  ) {
    var headers = callOptions.Headers ?? [];

    if (requestMetadata?.RequestInfo != null) {
      headers.Add("x-request-metadata", JsonSerializer.Serialize(requestMetadata.RequestInfo));
    }

    if (auditMetadata?.AuditInfo != null) {
      headers.Add("x-audit-metadata", JsonSerializer.Serialize(auditMetadata.AuditInfo));
    }

    if (tenantMetadata?.TenantInfo != null) {
      headers.Add("x-tenant-metadata", JsonSerializer.Serialize(tenantMetadata.TenantInfo));
    }

    return headers.Count < 1
      ? callOptions.WithCancellationToken(cancellationToken)
      : callOptions.WithHeaders(headers).WithCancellationToken(cancellationToken);
  }
}
