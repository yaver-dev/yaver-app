// ReSharper disable once CheckNamespace

namespace Yaver.App;

/// <summary>
///   Represents an HTTP feature that provides the tenant context for the current request.
/// </summary>
public record TenantHttpFeature(TenantContext TenantContext);

/// <summary>
///   Represents the context of a tenant, including the tenant identifier, language, user ID, and correlation ID.
/// </summary>
public record TenantContext(string TenantIdentifier, string Language, Guid UserId, Guid CorrelationId);
