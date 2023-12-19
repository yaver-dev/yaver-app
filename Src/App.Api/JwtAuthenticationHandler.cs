using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Yaver.App;

public class JwtAuthenticationHandler : JwtBearerHandler {
  public JwtAuthenticationHandler(
    IOptionsMonitor<JwtBearerOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
  ) : base(options, logger, encoder) { }
  
  protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {

    var result = await base.HandleAuthenticateAsync();

    if (!result.Succeeded) return result;
    
    var authorization = Request.Headers.Authorization.ToString();

    var token = authorization["Bearer ".Length..].Trim();

    var handler = new JwtSecurityTokenHandler();
    var jwtSecurityToken = handler.ReadJwtToken(token);

    var requestInfo = new RequestInfo(
      CorrelationId: Request.Headers["x-request-id"].FirstOrDefault() ?? "",
      RequestIp: Request.Headers["x-forwarded-for"].FirstOrDefault() ?? "",
      UserAgent: Request.Headers.UserAgent.ToString(),
      AcceptLanguage: Request.Headers.AcceptLanguage.ToString()
    );

    Context.Features.Set(requestInfo);

    var auditInfo = new AuditInfo(
      UserId: Guid.Parse(jwtSecurityToken.Claims.FirstOrDefault(p => p.Type == "sub")?.Value.ToString() ?? ""),
      UserName: jwtSecurityToken.Claims.FirstOrDefault(p => p.Type == "preferred_username")?.Value.ToString() ?? "",
      GivenName: jwtSecurityToken.Claims.FirstOrDefault(p => p.Type == "given_name")?.Value.ToString() ?? "",
      FamilyName: jwtSecurityToken.Claims.FirstOrDefault(p => p.Type == "family_name")?.Value.ToString() ?? "",
      Roles: jwtSecurityToken.Claims
        .Where(c => c.Type == "roles")
        .Select(c => c.Value)
        .ToList(),
      Email: jwtSecurityToken.Claims.FirstOrDefault(p => p.Type == "email")?.Value.ToString() ?? ""
    );

    Context.Features.Set(auditInfo);

    var tenantInfo = new TenantInfo(
      Name: jwtSecurityToken.Claims.FirstOrDefault(p => p.Type == "tenant")?.Value.ToString() ?? ""
    );

    Context.Features.Set(tenantInfo);
    
    return result;
  }
  
}
