using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Yaver.App;

public class XUserInfoAuthenticationHandler(
                 IOptionsMonitor<AuthenticationSchemeOptions> options,
                 ILoggerFactory logger,
                 UrlEncoder encoder,
                 ISystemClock clock) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock) {

  public const string SchemeName = "X-Userinfo";
  internal const string RoleType = "roles";

  protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
    if (Context
                                    .GetEndpoint()?
                                    .Metadata.OfType<AllowAnonymousAttribute>()
                                    .Any() is null or true) {
      return Task.FromResult(AuthenticateResult.NoResult());
    }

    string xUserInfo = Request.Headers[SchemeName]!;

    if (string.IsNullOrEmpty(xUserInfo))
      return Task.FromResult(AuthenticateResult.Fail($"{SchemeName} header not found"));


    var payload = JwtPayload.Base64UrlDeserialize(xUserInfo) ?? throw new ArgumentException(null, "payload");

    ClaimsIdentity identity = new(
                                                                                                                                    claims: payload.Claims,
                                                                                                                                    authenticationType: SchemeName,
                                                                                                                                    nameType: "name",
                                                                                                                                    roleType: RoleType
                                                                    );

    AuthenticationTicket ticket = new(
                                                                                                    principal: new ClaimsPrincipal(identity),
                                                                                                    properties: new AuthenticationProperties(),
                                                                                                    authenticationScheme: SchemeName
                                    );


    var x = new RequestInfo(
            UserId: Guid.Parse(payload.Sub),
                            AcceptLanguage: Request.Headers["accept-language"].FirstOrDefault() ?? "",
                            RequestId: Request.Headers["x-request-id"].FirstOrDefault() ?? "",
                            UserName: payload.FirstOrDefault(p => p.Key == "preferred_username").Value?.ToString() ?? "",
                            GivenName: payload.FirstOrDefault(p => p.Key == "given_name").Value?.ToString() ?? "",
                            FamilyName: payload.FirstOrDefault(p => p.Key == "family_name").Value?.ToString() ?? "",
                            Roles: ticket.Principal.Claims
                                                                                     .Where(c => c.Type == RoleType)
                                                                                     .Select(c => c.Value).ToList(),
                            Email: payload.FirstOrDefault(p => p.Key == "email").Value?.ToString() ?? "",
                            TenantIdentifier: Request.Headers["x-tenan-id"].FirstOrDefault() ?? ""
                    );

    Context.Features.Set(x);

    var roles = ticket.Principal.Claims
                                                                                     .Where(c => c.Type == RoleType)
                                                                                     .Select(c => c.Value).ToList();

    return Task.FromResult(AuthenticateResult.Success(ticket));
  }

  //--------------------
}