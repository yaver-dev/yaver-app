using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Yaver.App;

/// <summary>
///   Authentication handler for authenticating the user's credentials based on the provided X-User-Info header.
/// </summary>
public class UserInfoAuthenticationHandler(
  IOptionsMonitor<AuthenticationSchemeOptions> options,
  ILoggerFactory logger,
  UrlEncoder encoder,
  ISystemClock clock
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock) {
  public const string SchemaName = "UserInfo";
  internal const string RoleType = "roles";

  /// <summary>
  ///   Authenticates the user's credentials based on the provided X-User-Info header.
  /// </summary>
  /// <returns>A task that represents the asynchronous operation. The task result contains the authentication result.</returns>
  protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
    if (Context
          .GetEndpoint()?
          .Metadata.OfType<AllowAnonymousAttribute>()
          .Any() is null or true) {
      return Task.FromResult(AuthenticateResult.NoResult());
    }

    string xUserInfo = Request.Headers[$"X-{SchemaName}"]!;

    if (string.IsNullOrEmpty(xUserInfo)) {
      return Task.FromResult(AuthenticateResult.Fail($"{SchemaName} header not found"));
    }


    var payload = JwtPayload.Base64UrlDeserialize(xUserInfo) ?? throw new ArgumentException(null, "payload");

    ClaimsIdentity identity = new(
      payload.Claims,
      SchemaName,
      "name",
      RoleType);

    AuthenticationTicket ticket = new(
      new ClaimsPrincipal(identity),
      new AuthenticationProperties(),
      SchemaName);


    var requestInfo = new RequestInfo(
      Guid.Parse(payload.Sub),
      Request.Headers["accept-language"].FirstOrDefault() ?? "",
      Request.Headers["x-request-id"].FirstOrDefault() ?? "",
      payload.FirstOrDefault(p => p.Key == "preferred_username").Value?.ToString() ?? "",
      GivenName: payload.FirstOrDefault(p => p.Key == "given_name").Value?.ToString() ?? "",
      FamilyName: payload.FirstOrDefault(p => p.Key == "family_name").Value?.ToString() ?? "",
      Roles: ticket.Principal.Claims
        .Where(c => c.Type == RoleType)
        .Select(c => c.Value).ToList(),
      Email: payload.FirstOrDefault(p => p.Key == "email").Value?.ToString() ?? "",
      Tenant: Request.Headers["tenant"].FirstOrDefault() ?? ""
    );

    Context.Features.Set(requestInfo);

    // var roles = ticket.Principal.Claims
    //   .Where(c => c.Type == RoleType)
    //   .Select(c => c.Value)
    //   .ToList();

    return Task.FromResult(AuthenticateResult.Success(ticket));
  }
}
