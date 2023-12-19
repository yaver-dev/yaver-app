using System.Text.Json;

using Grpc.Core;

namespace Yaver.App;

/// <summary>
/// Provides extension methods for the <see cref="CallOptions"/> class to add request metadata, audit metadata, tenant metadata, and Yaver metadata.
/// </summary>
public static class RpcCallOptionsExtensions {
  /// <summary>
  /// Extends the CallOptions class with a method to add request metadata.
  /// </summary>
  /// <param name="callOptions">The CallOptions instance.</param>
  /// <param name="metadata">The request metadata.</param>
  /// <returns>The updated CallOptions instance.</returns>
  public static CallOptions WithRequestMetadata(
    this CallOptions callOptions,
    IRequestMetadata metadata
  ) {
    if (metadata.RequestInfo == null) return callOptions;
    var headers = callOptions.Headers ?? [];
    headers.Add("x-request-metadata", JsonSerializer.Serialize(metadata.RequestInfo));
    return callOptions.WithHeaders(headers);
  }

  /// <summary>
  /// Represents options for making a call.
  /// </summary>
  public static CallOptions WithAuditMetadata(
    this CallOptions callOptions,
    IAuditMetadata metadata
  ) {
    if (metadata.AuditInfo == null) return callOptions;
    var headers = callOptions.Headers ?? [];
    headers.Add("x-audit-metadata", JsonSerializer.Serialize(metadata.AuditInfo));
    return callOptions.WithHeaders(headers);
  }

  /// <summary>
  /// Represents the options for a gRPC call.
  /// </summary>
  public static CallOptions WithTenantMetadata(
    this CallOptions callOptions,
    ITenantMetadata metadata
  ) {
    if (metadata.TenantInfo == null) return callOptions;
    var headers = callOptions.Headers ?? [];
    headers.Add("x-tenant-metadata", JsonSerializer.Serialize(metadata.TenantInfo));
    return callOptions.WithHeaders(headers);
  }

  /// <summary>
  /// Represents the options for making a call.
  /// </summary>
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
