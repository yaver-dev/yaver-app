using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using Api;
using Api.Features.Admin;

using FluentValidation.Results;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Yaver.App;

var builder = WebApplication.CreateBuilder(args);
builder.AddYaverLogger();

builder.Services.Configure<JsonOptions>(o =>
  o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

builder.Services.AddScoped<IYaverContext, YaverContext>();

builder.Services
  .AddFastEndpoints(o => { o.IncludeAbstractValidators = true; })
  .AddAuthorization()
  .AddAuthentication(UserInfoAuthenticationHandler.SchemaName)
  .AddScheme<AuthenticationSchemeOptions, UserInfoAuthenticationHandler>(
    UserInfoAuthenticationHandler.SchemaName,
    null
  );

var app = builder.Build();

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
      ep.PreProcessors(Order.Before, new MyRequestLogger());
      // ep.PreProcessors(Order.Before, new YaverHttpProcessor());
      // ep.PreProcessors(Order.Before, new TenantPreProcessor());
    };
  });

app.MapRpcHandlers(
  "Admin.ServiceBase",
  app.Configuration.GetSection("Services").GetValue<string>("ADMIN"));




app.Run();



namespace Api
{
  public class MyRequestLogger : IGlobalPreProcessor
  {
    public async Task PreProcessAsync(IPreProcessorContext ctx, CancellationToken ct)
    {
      await Task.CompletedTask;
      // var logger = ctx.RequestServices.GetRequiredService<ILogger>();
      // logger.LogInformation($"request:{req?.GetType().FullName} path: {ctx.Request.Path}");

      var userInfo = ctx.HttpContext.Request.Headers["X-UserId"].FirstOrDefault();
      // if (userInfo == null) {
      // 	failures.Add(new("MissingHeaders", "The [X-UserId] header needs to be set!"));
      // 	await ctx.Response.SendErrorsAsync(failures, cancellation: ct); //sending response here
      // 	return;
      // }


      Console.WriteLine("userInfo:------------------");
      Console.WriteLine($"{userInfo}");
      Console.WriteLine("------------------");
      Console.WriteLine(
        $"roles: {JsonSerializer.Serialize(ctx.HttpContext.User?.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList())}");
      Console.WriteLine("------------------");
      Console.WriteLine($"request:{ctx.HttpContext.Request?.GetType().FullName} path: {ctx.HttpContext.Request.Path}");
      Console.WriteLine("------------------");
    }
  }
}
