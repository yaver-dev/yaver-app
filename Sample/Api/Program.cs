using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using Api;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Yaver.App;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddYaverConfiguration();

builder.Services.Configure<JsonOptions>(o =>
  o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase
);

builder.Services.AddScoped<IRequestMetadata, RequestMetadata>();
builder.Services.AddScoped<IAuditMetadata, AuditMetadata>();
builder.Services.AddScoped<ITenantMetadata, TenantMetadata>();

builder.Services.AddLocalization();

builder.Services
  .AddFastEndpoints(o => { o.IncludeAbstractValidators = true; })
  .AddAuthorization()
  // user info authentication
  .AddAuthentication(UserInfoAuthenticationHandler.SchemaName)
  .AddScheme<AuthenticationSchemeOptions, UserInfoAuthenticationHandler>(UserInfoAuthenticationHandler.SchemaName, null);
// jwt authentication
// .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// .AddScheme<JwtBearerOptions, JwtAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, options => {
//   options.Audience = "http://auth.bla.com/realms/x";
//   options.Authority = "http://auth.bla.com";
//   options.RequireHttpsMetadata = true;
//
//   var cert = "___CERT_STRING_CONTENT___";
//
//   // options.SaveToken = false;
//   options.TokenValidationParameters = new TokenValidationParameters {
//     ValidateIssuer = true,
//     ValidateAudience = false,
//     ValidateLifetime = false,
//     ValidateIssuerSigningKey = false,
//     IssuerSigningKey = new X509SecurityKey(new X509Certificate2(System.Text.Encoding.Unicode.GetBytes(cert))),
//     ValidIssuer = "http://auth.bla.com/realms/x",
//     // ValidAudience = "http://auth.bla.com/realms/x"
//   };
// });

//
// .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// .AddScheme<JwtBearerOptions, JwtAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, options => { });

var supportedCultures = new[] { new CultureInfo("tr-TR"), new CultureInfo("en-UK") };

var app = builder.Build();
app.UseRequestLocalization(
  new RequestLocalizationOptions {
    DefaultRequestCulture = new("en-UK"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
  });

app
  .UseYaverExceptionHandler(logStructuredException: true)
  .UseAuthentication()
  .UseAuthorization()
  .UseFastEndpoints(c => {
    c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    c.Endpoints.Configurator = ep => {
      //TODO remove before stable release
      ep.PreProcessor<Localizer>(Order.Before);
      ep.PreProcessor<RequestLogger>(Order.Before);
      ep.PostProcessor<ResponseLogger>(Order.After);
    };
  });

app.MapRpcHandlers(
  "Admin.ServiceBase",
  app.Configuration.GetSection("Services").GetValue<string>("ADMIN")
);


app.Run();
