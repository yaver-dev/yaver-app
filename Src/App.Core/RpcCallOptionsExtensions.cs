using System.Text.Json;

using Grpc.Core;

namespace Yaver.App;

/// <summary>
///   Provides extension methods for gRPC call options used in Yaver.
/// </summary>
public static class RpcCallOptionsExtensions {
  /// <summary>
  ///   Sets the context for the gRPC call options.
  /// </summary>
  /// <param name="callOptions">The gRPC call options.</param>
  /// <param name="context">The Yaver context.</param>
  /// <param name="ct">The cancellation token.</param>
  /// <returns>The updated gRPC call options.</returns>
  public static CallOptions SetYaverContext(
    this CallOptions callOptions,
    IYaverContext context,
    CancellationToken ct
  ) {
    return callOptions
      .WithHeaders(new Metadata { { "x-yaver-context", JsonSerializer.Serialize(context.RequestInfo) } })
      .WithCancellationToken(ct);
  }

  public static CallOptions SetTenantContext(
    this CallOptions callOptions,
    ITenantContext context
  ) {
    return callOptions
      .WithHeaders(new Metadata { { "x-tenant-context", JsonSerializer.Serialize(context.TenantInfo) } });
  }
}
