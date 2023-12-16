using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using Api;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

using Yaver.App;

var builder = WebApplication.CreateSlimBuilder(args);
builder.AddYaverLogger();

builder.Services.Configure<JsonOptions>(o =>
  o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

builder.Services.AddScoped<IYaverContext, YaverContext>();
builder.Services.AddLocalization();

builder.Services
  .AddFastEndpoints(o => { o.IncludeAbstractValidators = true; })
  .AddAuthorization()
  .AddAuthentication(UserInfoAuthenticationHandler.SchemaName)
  .AddScheme<AuthenticationSchemeOptions, UserInfoAuthenticationHandler>(
    UserInfoAuthenticationHandler.SchemaName,
    null
  );


var supportedCultures = new[] { new CultureInfo("tr-TR"), new CultureInfo("en-UK") };


var app = builder.Build();
app.UseRequestLocalization(
       new RequestLocalizationOptions
       {
         DefaultRequestCulture = new("en-UK"),
         SupportedCultures = supportedCultures,
         SupportedUICultures = supportedCultures
       });

app
  .UseYaverExceptionHandler(logStructuredException: true)
  .UseAuthentication()
  .UseAuthorization()
  .UseFastEndpoints(c =>
  {
    c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    c.Endpoints.Configurator = ep =>
    {
      //TODO remove before stable release
      ep.PreProcessor<LocalizationProcessor>(Order.Before);
      ep.PreProcessor<MyRequestLogger>(Order.Before);
      ep.PostProcessor<MyResponseLogger>(Order.After);
    };
  });

app.MapRpcHandlers(
  "Admin.ServiceBase",
  app.Configuration.GetSection("Services").GetValue<string>("ADMIN"));




app.Run();


namespace Api
{
  public class LocalizationProcessor : IGlobalPreProcessor
  {
    public async Task PreProcessAsync(IPreProcessorContext ctx, CancellationToken ct)
    {
      await Task.CompletedTask;

      var cultureKey = ctx.HttpContext.Request.Headers["Accept-Language"];
      Console.WriteLine("cultureKey:------------------" + cultureKey);
      if (!string.IsNullOrEmpty(cultureKey))
      {
        // if (DoesCultureExist(cultureKey)) {
        var culture = new CultureInfo(cultureKey);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        // }
      }
    }
  }

  public class MyResponseLogger : IGlobalPostProcessor
  {
    public async Task PostProcessAsync(IPostProcessorContext ctx, CancellationToken ct)
    {
      await Task.CompletedTask;


      Log.Information(
          $"request:{ctx.Request} response: {ctx.Response}");
    }
  }
  public class MyRequestLogger : IGlobalPreProcessor
  {
    public async Task PreProcessAsync(IPreProcessorContext ctx, CancellationToken ct)
    {
      await Task.CompletedTask;
      // var logger = ctx.HttpContext.Resolve<ILogger>();

      Log.Information(
          $"request:{ctx.Request.GetType().FullName} path: {ctx.HttpContext.Request.Path}");

      // var logger = ctx.RequestServices.GetRequiredService<ILogger>();
      // logger.LogInformation($"request:{req?.GetType().FullName} path: {ctx.Request.Path}");

      var userInfo = ctx.HttpContext.Request.Headers["X-UserId"].FirstOrDefault();
      // if (userInfo == null) {
      // 	failures.Add(new("MissingHeaders", "The [X-UserId] header needs to be set!"));
      // 	await ctx.Response.SendErrorsAsync(failures, cancellation: ct); //sending response here
      // 	return;
      // }


      Console.WriteLine("Headers:------------------");
      Console.WriteLine(JsonSerializer.Serialize(ctx.HttpContext.Request.Headers, new JsonSerializerOptions { WriteIndented = true }));
      Console.WriteLine("------------------");
      Console.WriteLine(
        $"roles: {JsonSerializer.Serialize(ctx.HttpContext.User?.Claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList())}");
      Console.WriteLine("------------------");
      Console.WriteLine($"request:{ctx.HttpContext.Request?.GetType().FullName} path: {ctx.HttpContext.Request.Path}");
      Console.WriteLine("------------------");
    }
  }
}
