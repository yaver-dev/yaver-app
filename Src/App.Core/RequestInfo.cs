using Microsoft.AspNetCore.Http;

namespace Yaver.App;

/// <summary>
///   Implements the <see cref="IYaverContext" /> interface to provide access to the request information.
/// </summary>

// Represents the information about a request.
public record RequestInfo(
  Guid UserId,
  string AcceptLanguage,
  string RequestId,
  string UserName,
  string Email,
  string GivenName,
  string FamilyName,
  List<string> Roles,
  string Tenant
);

// Provides access to the request information.
/// <summary>
///   Represents the context of a Yaver request.
/// </summary>
public interface IYaverContext {
  // Gets the information about the incoming HTTP request.
  /// <summary>
  ///   Gets the information about the incoming HTTP request.
  /// </summary>
  RequestInfo RequestInfo { get; }
}

// Implements the IYaverContext interface to provide access to the request information.
//   public class YaverContext : IYaverContext
//   {
//     private readonly RequestInfo? _requestInfo;

//     // Initializes a new instance of the YaverContext class.
//     public YaverContext(IHttpContextAccessor httpContextAccessor, RequestInfo rInfo)
//     {
//       _requestInfo = rInfo;
//       RequestInfo = httpContextAccessor?.HttpContext?.Features.Get<RequestInfo>();
//     }

//     // Gets the information about the incoming HTTP request.
//     public RequestInfo RequestInfo { get; init; }
//   }
// }

/// <summary>
///   Represents the context for a Yaver API request.
/// </summary>
public class YaverContext : IYaverContext {
  private readonly RequestInfo? _requestInfo;

  /// <summary>
  ///   Initializes a new instance of the <see cref="YaverContext" /> class with the specified
  ///   <see cref="IHttpContextAccessor" />.
  /// </summary>
  /// <param name="httpContextAccessor">
  ///   The <see cref="IHttpContextAccessor" /> to use for accessing the current HTTP
  ///   context.
  /// </param>
  public YaverContext(IHttpContextAccessor httpContextAccessor) {
    _requestInfo = httpContextAccessor?.HttpContext?.Features.Get<RequestInfo>();
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="YaverContext" /> class with the specified <paramref name="rInfo" />.
  /// </summary>
  /// <param name="rInfo">The request information.</param>
  public YaverContext(RequestInfo rInfo) {
    _requestInfo = rInfo;
  }

  /// <inheritdoc />
  /// <summary>
  ///   Gets the information about the incoming HTTP request.
  /// </summary>
  public RequestInfo RequestInfo => _requestInfo;
}

public class DesignTimeYaverContext : IYaverContext {
  /// <inheritdoc />
  /// <summary>
  ///   Gets the information about the incoming HTTP request.
  /// </summary>
  public RequestInfo RequestInfo { get; } = new(
    UserId: Guid.Empty,
    AcceptLanguage: string.Empty,
    RequestId: string.Empty,
    UserName: string.Empty,
    Email: string.Empty,
    GivenName: string.Empty,
    FamilyName: string.Empty,
    Roles: [],
    Tenant: string.Empty
  );
}
