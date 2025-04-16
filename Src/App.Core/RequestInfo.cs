using Microsoft.AspNetCore.Http;

namespace Yaver.App;

/// <summary>
///   Implements the <see cref="IRequestMetadata" /> interface to provide access to the request information.
/// </summary>

// Represents the information about a request.
public record RequestInfo(
  string CorrelationId,
  string RequestIp,
  string UserAgent,
  string AcceptLanguage
);

// Provides access to the request information.
/// <summary>
///   Represents the context of a Yaver request.
/// </summary>
public interface IRequestMetadata {
  // Gets the information about the incoming HTTP request.
  /// <summary>
  ///   Gets the information about the incoming HTTP request.
  /// </summary>
  RequestInfo? RequestInfo { get; }
}

/// <summary>
///   Represents the context for a Yaver API request.
/// </summary>
public class RequestMetadata : IRequestMetadata {
  private readonly RequestInfo? _requestInfo;

  /// <summary>
  ///   Initializes a new instance of the <see cref="RequestMetadata" /> class with the specified
  ///   <see cref="IHttpContextAccessor" />.
  /// </summary>
  /// <param name="httpContextAccessor">
  ///   The <see cref="IHttpContextAccessor" /> to use for accessing the current HTTP
  ///   context.
  /// </param>
  public RequestMetadata(IHttpContextAccessor httpContextAccessor) {
    _requestInfo = httpContextAccessor.HttpContext?.Features.Get<RequestInfo>();
    // if (httpContextAccessor.HttpContext != null) {
    //   ArgumentNullException.ThrowIfNull(_requestInfo);
    // }
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="RequestMetadata" /> class with the specified <paramref name="rInfo" />.
  /// </summary>
  /// <param name="rInfo">The request information.</param>
  public RequestMetadata(RequestInfo rInfo) {
    _requestInfo = rInfo;
    ArgumentNullException.ThrowIfNull(_requestInfo);
  }

  /// <inheritdoc />
  /// <summary>
  ///   Gets the information about the incoming HTTP request.
  /// </summary>
  public RequestInfo RequestInfo => _requestInfo;
}

/// <summary>
/// Represents request metadata specifically for design-time scenarios.
/// This class provides default, empty values for request information,
/// suitable for use when actual HTTP request context is unavailable,
/// such as during design-time operations or unit testing.
/// </summary>
/// <remarks>
/// Implements the <see cref="IRequestMetadata"/> interface by providing
/// a <see cref="RequestInfo"/> property initialized with empty strings.
/// </remarks>
public class DesignTimeRequestMetadata : IRequestMetadata {
  /// <inheritdoc />
  /// <summary>
  ///   Gets the information about the incoming HTTP request.
  /// </summary>
  public RequestInfo RequestInfo { get; } = new(
    CorrelationId: string.Empty,
    RequestIp: string.Empty,
    UserAgent: string.Empty,
    AcceptLanguage: string.Empty
  );
}
