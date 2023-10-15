using Microsoft.AspNetCore.Http;

namespace Yaver.App;


/// <summary>
/// Implements the <see cref="IYaverContext"/> interface to provide access to the request information.
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
  string TenantIdentifier);

// Provides access to the request information.
public interface IYaverContext {
  // Gets the information about the incoming HTTP request.
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

public class YaverContext : IYaverContext {
  private readonly RequestInfo? _requestInfo;

  /// <summary>
  /// Initializes a new instance of the <see cref="YaverContext"/> class.
  /// </summary>
  /// <param name="httpContextAccessor">The HTTP context accessor.</param>
  /// <param name="rInfo">The request information.</param>
  public YaverContext(IHttpContextAccessor httpContextAccessor) {

    _requestInfo = httpContextAccessor?.HttpContext?.Features.Get<RequestInfo>();
  }

  public YaverContext(RequestInfo rInfo) {
    _requestInfo = rInfo;

  }

  /// <inheritdoc/>
  /// <summary>
  /// Gets the information about the incoming HTTP request.
  /// </summary>
  public RequestInfo RequestInfo { get => _requestInfo; }
}